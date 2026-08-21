# AudiSoft — Sistema de Gestión Académica

Aplicación web para la gestión de Estudiantes, Profesores y Notas: creación, edición, eliminación y consulta paginada de cada recurso, con validación de integridad referencial entre ellos.

[![CI](https://github.com/bokhi6/audisoft-technical-test/actions/workflows/ci.yml/badge.svg)](https://github.com/bokhi6/audisoft-technical-test/actions/workflows/ci.yml)

**Desarrollado por:** Anthony Daza

---

## Índice

1. [Características](#características)
2. [Tecnologías](#tecnologías)
3. [Arquitectura](#arquitectura)
4. [Instalación](#instalación)
5. [Uso](#uso)
6. [API](#api)
7. [Integración continua (CI)](#integración-continua)
8. [Pruebas y calidad de código](#pruebas-y-calidad-de-código)
9. [Estructura del proyecto](#estructura-del-proyecto)

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

Hay dos formas de levantar el proyecto: con Docker (todo el entorno en un solo comando, recomendado) o de forma local instalando cada herramienta por separado.

### Opción A — Con Docker (recomendado)

**1. Requisito único**: [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y corriendo.

Verificar que está disponible:

```bash
docker --version
docker compose version
```

**2. Clonar el repositorio** (si aún no se hizo):

```bash
git clone https://github.com/bokhi6/audisoft-technical-test.git
cd audisoft-technical-test
```

**3. Levantar todo el entorno** desde la raíz del proyecto:

```bash
docker compose up --build
```

Este único comando construye y levanta tres contenedores:

| Contenedor | Contiene | Puerto expuesto |
|---|---|---|
| `audisoft-sqlserver` | Motor de base de datos SQL Server | 1433 |
| `audisoft-backend` | API .NET (aplica las migraciones automáticamente al iniciar) | 5080 |
| `audisoft-frontend` | Aplicación Angular compilada, servida con nginx | 4200 |

> ⏱️ **La primera ejecución tarda entre 5 y 10 minutos** (descarga de las imágenes base de SQL Server y del SDK de .NET, y compilación de ambos proyectos). Las siguientes ejecuciones son mucho más rápidas porque Docker reutiliza las capas ya construidas. Si se corre sin `-d`, la terminal muestra el progreso de descarga y compilación en tiempo real.

**4. Confirmar que quedó arriba**: cuando el log deja de imprimir líneas nuevas y se ve `Now listening on: http://+:8080` (backend) y `Application started` en pantalla, el entorno está listo.

| Servicio | URL |
|---|---|
| Aplicación web | http://localhost:4200 |
| API | http://localhost:5080/api |
| Documentación interactiva del API (Swagger) | http://localhost:5080/swagger |

**5. Para detener el entorno**:

```bash
docker compose down
```

Agregar `-v` al final (`docker compose down -v`) si además se quiere eliminar la base de datos persistida y volver a empezar desde cero la próxima vez.

**Problemas comunes**

| Síntoma | Causa probable | Solución |
|---|---|---|
| `port is already allocated` | Otro proceso ya usa el puerto 4200, 5080 o 1433 | Detener ese proceso, o editar los puertos publicados en `docker-compose.yml` |
| El backend no conecta a la base de datos | SQL Server aún no terminó de iniciar | Esperar unos segundos; el backend reintenta automáticamente y `docker-compose.yml` ya incluye un healthcheck que hace esperar al backend |
| Cambios en el código no se reflejan | Las imágenes quedaron cacheadas | Volver a correr con `docker compose up --build` (fuerza la reconstrucción) |

### Opción B — Instalación local (sin Docker)

**Requisitos previos**:

| Herramienta | Versión mínima | Verificar con |
|---|---|---|
| .NET SDK | 10.0 | `dotnet --version` |
| Node.js | 20 LTS | `node --version` |
| npm | (incluido con Node) | `npm --version` |
| SQL Server o SQL Server LocalDB | — | — |
| Angular CLI | última | `ng version` (instalar con `npm install -g @angular/cli` si falta) |

**1. Clonar el repositorio**:

```bash
git clone https://github.com/bokhi6/audisoft-technical-test.git
cd audisoft-technical-test
```

**2. Backend** — desde la carpeta `backend/`:

```bash
cd backend
dotnet restore
dotnet run --project src/AudiSoft.WebApi
```

Al iniciar, el backend crea la base de datos (si no existe) y aplica las migraciones automáticamente — no hace falta ejecutar ningún comando de base de datos por separado. La consola debe mostrar `Now listening on: http://localhost:5080`. Para confirmar que responde:

```bash
curl http://localhost:5080/api/estudiantes?pageNumber=1&pageSize=1
```

Por defecto, el backend usa SQL Server LocalDB (`(localdb)\MSSQLLocalDB`); la cadena de conexión se puede ajustar en `backend/src/AudiSoft.WebApi/appsettings.json` si se quiere apuntar a otra instancia de SQL Server.

**3. Frontend** — desde la carpeta `frontend/`, en otra terminal:

```bash
cd frontend
npm install
ng serve
```

La aplicación queda disponible en http://localhost:4200. El backend debe estar corriendo antes de abrir el frontend para que pueda consumir la API (el CORS ya está configurado para `http://localhost:4200`).

**Problemas comunes**

| Síntoma | Causa probable | Solución |
|---|---|---|
| `Cannot connect to database` | SQL Server / LocalDB no está corriendo | Verificar con `sqllocaldb info`, iniciar con `sqllocaldb start MSSQLLocalDB` |
| Error de CORS en el navegador | El frontend corre en un puerto distinto a 4200 | Ajustar la política de CORS en `backend/src/AudiSoft.WebApi/Program.cs` |
| `ng: command not found` | Angular CLI no está instalado globalmente | `npm install -g @angular/cli` |

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

## Integración continua

Cada push o pull request a `master` dispara automáticamente un pipeline de **GitHub Actions**, definido en [`.github/workflows/ci.yml`](.github/workflows/ci.yml), con dos jobs independientes:

| Job | Pasos |
|---|---|
| **Backend** | `dotnet restore` → `dotnet format --verify-no-changes` (falla si el código no está formateado) → `dotnet build -warnaserror` (falla ante cualquier advertencia de los analizadores) → `dotnet test` (pruebas unitarias) |
| **Frontend** | `npm ci` → `ng lint` → `ng build` |

El resultado de la última ejecución se puede ver en la pestaña [Actions](https://github.com/bokhi6/audisoft-technical-test/actions) del repositorio, o en el badge al inicio de este documento.

---

## Pruebas y calidad de código

### Ejecutar las verificaciones localmente

```bash
# Backend
cd backend
dotnet test           # pruebas unitarias
dotnet format         # aplica formato de código automáticamente
dotnet build           # compilación + analizadores estáticos

# Frontend
cd frontend
ng lint                # análisis estático (ESLint)
ng build                # build de producción
```

Las pruebas unitarias del backend (`AudiSoft.Application.Tests`, xUnit + Moq) cubren las reglas de negocio principales: restricción de eliminación por integridad referencial, validaciones de campos y manejo de recursos inexistentes.

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
