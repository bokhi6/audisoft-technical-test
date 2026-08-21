# AudiSoft — Sistema de Gestión Académica

Aplicación web para la gestión de Estudiantes, Profesores y Notas: creación, edición, eliminación y consulta paginada de cada recurso, con validación de integridad referencial entre ellos.

**Desarrollado por:** Anthony Daza

---

## Índice

1. [Características](#características)
2. [Tecnologías](#tecnologías)
3. [Arquitectura](#arquitectura)
4. [Instalación](#instalación)
5. [Uso](#uso)
6. [API](#api)
7. [Pruebas y calidad de código](#pruebas-y-calidad-de-código)
8. [Estructura del proyecto](#estructura-del-proyecto)

---

## Características

- Gestión completa (crear, editar, eliminar, consultar) de Estudiantes, Profesores y Notas.
- Paginación server-side en los tres listados.
- Integridad referencial: no es posible eliminar un Estudiante o Profesor que tenga una Nota asociada; la aplicación muestra una alerta explicando el motivo.
- Notificaciones de éxito y error en cada operación.
- Panel de inicio con conteo de registros por sección.
- Documentación interactiva del API (Swagger / OpenAPI).
- Entorno completamente containerizado: toda la aplicación (base de datos, API y frontend) se levanta con un solo comando.

---

## Tecnologías

**Backend**
- .NET 10 / ASP.NET Core Web API
- Entity Framework Core (Code-First, migraciones)
- SQL Server
- Swagger / Swashbuckle
- xUnit + Moq (pruebas unitarias)

**Frontend**
- Angular (standalone components)
- Angular Material
- Tailwind CSS
- SweetAlert2
- ESLint (angular-eslint)

**Infraestructura**
- Docker / Docker Compose
- nginx (servidor del frontend en producción)

---

## Arquitectura

El backend sigue **Clean Architecture**, organizado en capas con una regla de dependencia estricta: cada capa solo conoce a las que están por debajo de ella.

```
Domain  ←  Application  ←  Infrastructure  ←  WebApi
```

- **Domain**: entidades del negocio (Estudiante, Profesor, Nota) y excepciones propias. No depende de ningún framework.
- **Application**: casos de uso, DTOs, interfaces de repositorio y las reglas de negocio (validaciones, restricciones de eliminación, etc).
- **Infrastructure**: acceso a datos — DbContext de Entity Framework Core, configuraciones del modelo, repositorios y migraciones.
- **WebApi**: controladores REST, middleware de manejo de errores, configuración de Swagger y CORS.

**Integridad referencial**: las llaves foráneas de `Nota` hacia `Estudiante` y `Profesor` usan `DeleteBehavior.Restrict`. Adicionalmente, la capa de aplicación valida antes de eliminar si existen registros relacionados y devuelve un error de negocio (HTTP 409) con un mensaje descriptivo.

El frontend está organizado por *features* (una carpeta por sección funcional), con servicios y modelos compartidos en `core/` y `shared/`.

---

## Instalación

### Con Docker (recomendado)

Requiere [Docker Desktop](https://www.docker.com/products/docker-desktop/).

```bash
docker compose up --build
```

La primera ejecución tarda entre 5 y 10 minutos (descarga de imágenes base y compilación); las siguientes son mucho más rápidas gracias al cache de capas de Docker. El comando levanta tres contenedores — base de datos, API y frontend — y aplica las migraciones automáticamente.

| Servicio | URL |
|---|---|
| Frontend | http://localhost:4200 |
| API | http://localhost:5080 |
| Documentación del API (Swagger) | http://localhost:5080/swagger |

Para detener el entorno: `docker compose down` (agregar `-v` para eliminar también los datos persistidos).

### Sin Docker

**Requisitos**: .NET SDK 10, Node.js 20+, SQL Server (o LocalDB), Angular CLI.

```bash
# Backend
cd backend
dotnet restore
dotnet run --project src/AudiSoft.WebApi

# Frontend (en otra terminal)
cd frontend
npm install
ng serve
```

Las migraciones de base de datos se aplican automáticamente al iniciar el backend.

---

## Uso

La aplicación cuenta con cuatro secciones, accesibles desde la barra de navegación:

- **Inicio**: resumen con el total de registros por sección.
- **Estudiantes** / **Profesores**: listado paginado con la cantidad de notas asociadas a cada registro, y acciones para crear, editar o eliminar.
- **Notas**: listado paginado con el estudiante, el profesor y la calificación (0.0–5.0), indicando si está aprobada o reprobada.

Al eliminar un Estudiante o Profesor con notas asociadas, la aplicación bloquea la acción y explica el motivo en lugar de mostrar un error genérico.

---

## API

URL base: `http://localhost:5080/api`. Las respuestas de error siguen el estándar [ProblemDetails (RFC 7807)](https://www.rfc-editor.org/rfc/rfc7807).

| Recurso | Método | Ruta | Descripción |
|---|---|---|---|
| Estudiantes | `GET` | `/estudiantes?pageNumber=&pageSize=` | Listado paginado |
| | `GET` | `/estudiantes/lista` | Listado simple (id + nombre) |
| | `GET` | `/estudiantes/{id}` | Detalle |
| | `POST` | `/estudiantes` | Crear |
| | `PUT` | `/estudiantes/{id}` | Actualizar |
| | `DELETE` | `/estudiantes/{id}` | Eliminar |
| Profesores | | `/profesores...` | Mismas operaciones que Estudiantes |
| Notas | `GET` | `/notas?pageNumber=&pageSize=` | Listado paginado, incluye nombre de estudiante y profesor |
| | `POST` / `PUT` / `DELETE` | `/notas...` | CRUD estándar |

La especificación completa, con esquemas de request/response, está disponible en Swagger (`/swagger`). También se incluye una colección de Postman lista para importar en `entregables/AudiSoft.postman_collection.json`.

---

## Pruebas y calidad de código

Cada push a `master` ejecuta un pipeline de integración continua (build, análisis estático y pruebas del backend; lint y build del frontend) — ver `.github/workflows/ci.yml`.

```bash
# Backend
cd backend
dotnet test          # pruebas unitarias
dotnet format        # formato de código
dotnet build          # compilación + analizadores estáticos

# Frontend
cd frontend
ng lint               # análisis estático
```

Las pruebas unitarias del backend cubren las reglas de negocio principales: restricción de eliminación por integridad referencial, validaciones de campos y manejo de recursos inexistentes.

---

## Estructura del proyecto

```
.
├── backend/
│   ├── src/
│   │   ├── AudiSoft.Domain/          Entidades y excepciones de negocio
│   │   ├── AudiSoft.Application/     Casos de uso, DTOs, interfaces
│   │   ├── AudiSoft.Infrastructure/  EF Core, repositorios, migraciones
│   │   └── AudiSoft.WebApi/          Controladores, middleware, Swagger
│   ├── tests/
│   │   └── AudiSoft.Application.Tests/
│   └── Dockerfile
├── frontend/
│   ├── src/app/
│   │   ├── core/                     Interceptores y servicios transversales
│   │   ├── shared/                   Modelos, utilidades y servicios comunes
│   │   └── features/                 Módulos por sección (home, estudiantes, profesores, notas)
│   └── Dockerfile
├── entregables/                      Documentación adicional y script de base de datos
└── docker-compose.yml
```
