# Prueba Técnica Angular — AudiSoft Consulting SAS

**Desarrollado por: Anthony Daza**

Aplicación web full-stack para la gestión CRUD de Estudiantes, Profesores y Notas, desarrollada según los requisitos del PDF de la prueba técnica y las recomendaciones adicionales indicadas por el área de Talento Humano de AudiSoft Consulting SAS (audio de contexto).

## Índice

1. [Qué pide la prueba y qué se entrega](#qué-pide-la-prueba-y-qué-se-entrega)
2. [Stack tecnológico y por qué se eligió cada pieza](#stack-tecnológico-y-por-qué-se-eligió-cada-pieza)
3. [Arquitectura](#arquitectura)
4. [Estructura de carpetas](#estructura-de-carpetas)
5. [Cómo correrlo](#cómo-correrlo)
6. [API REST](#api-rest)
7. [Pruebas y calidad de código](#pruebas-y-calidad-de-código)
8. [Decisiones de diseño](#decisiones-de-diseño)
9. [Qué le falta para un 100/100](#qué-le-falta-para-un-100100)
10. [Entregables](#entregables)

---

## Qué pide la prueba y qué se entrega

**Del PDF de AudiSoft:**

| Pedido | Estado |
|---|---|
| API REST CRUD para Estudiante, Profesor y Nota | ✅ |
| Base de datos con FK de Nota hacia Estudiante y Profesor (constraint) | ✅ |
| Frontend Angular con menú a las 3 secciones, tabla + crear/editar/eliminar | ✅ |
| Documento con instalación y capturas | ✅ (`entregables/Documento_Instalacion.docx`) |
| Carpeta comprimida .ZIP con código y script SQL | ✅ |
| Repositorio de GitHub (opcional) | ✅ |

**Del audio de Talento Humano (recomendaciones adicionales):**

| Pedido | Estado |
|---|---|
| Prueba completa, no básica | ✅ Clean Architecture, no CRUD plano |
| Ortografía y tildes correctas en toda la interfaz | ✅ |
| Mensaje de confirmación en crear/editar/eliminar | ✅ (SweetAlert2) |
| Paginación funcional | ✅ (server-side, 3 por página) |
| No permitir eliminar Estudiante/Profesor con Nota asociada, con alerta clara | ✅ (409 + doble validación) |
| Entrega para el viernes | ✅ |

**Valor agregado, no pedido explícitamente pero sumado para reforzar la entrega:**

- Pantalla de Home con conteos en vivo.
- Rediseño visual completo con Tailwind CSS (más allá del tema por defecto de Angular Material).
- Formularios de Estudiante/Profesor con Nombres y Apellidos separados (UX más rica sin romper el esquema de BD pedido).
- Columna de cantidad de notas por Estudiante y por Profesor.
- Menú de acciones organizado (en vez de botones sueltos) en las 3 tablas.
- Docker + docker-compose (SQL Server + backend + frontend, un solo comando).
- Pruebas unitarias del backend (xUnit + Moq).
- Linting en frontend (ESLint/angular-eslint) y backend (analizadores .NET + `dotnet format`).
- Migraciones de base de datos aplicadas automáticamente al arrancar (no hay que correr comandos manuales).

---

## Stack tecnológico y por qué se eligió cada pieza

### Backend

| Tecnología | Para qué se usa | Por qué esta y no otra |
|---|---|---|
| **.NET 10** | Runtime y framework del backend | Es la versión LTS más reciente disponible en la máquina (no se usó .NET 6, que ya no tiene soporte). |
| **ASP.NET Core Web API** | Exponer los endpoints REST | Estándar de facto para APIs REST en .NET, integración directa con EF Core y Swagger. |
| **Entity Framework Core** | ORM y migraciones | Permite modelar la base de datos desde el código (Code-First) y generar el script SQL pedido por el PDF a partir de las migraciones, sin escribirlo a mano. |
| **SQL Server** (LocalDB en desarrollo, contenedor en Docker) | Base de datos relacional | El PDF permite cualquier motor; se eligió SQL Server porque LocalDB ya estaba disponible en la máquina de desarrollo, sin instalar nada adicional, y porque EF Core + SQL Server es la combinación más madura del ecosistema .NET. |
| **Swashbuckle (Swagger)** | Documentación interactiva del API | El PDF recomienda explícitamente probar el API con una herramienta tipo Postman; Swagger UI cumple esa función sin salir del navegador. |
| **xUnit + Moq** | Pruebas unitarias | xUnit es el framework de pruebas estándar en .NET moderno; Moq permite simular los repositorios y probar las reglas de negocio (Application layer) sin depender de una base de datos real. |
| **Analizadores de .NET + `dotnet format`** | Calidad y estilo de código | Detectan problemas de rendimiento, buenas prácticas y estilo de forma automática en cada build. |

### Frontend

| Tecnología | Para qué se usa | Por qué esta y no otra |
|---|---|---|
| **Angular (última versión, standalone components)** | Framework del frontend | Pedido explícitamente por el PDF de la prueba. Se usa la forma moderna de Angular (sin NgModules, con signals) por ser el estándar actual del framework. |
| **Angular Material** | Componentes funcionales complejos (tabla, paginador, diálogos modales, menús) | Reimplementar un paginador o un sistema de diálogos accesible desde cero no aporta valor a la prueba y consume tiempo; Material los resuelve de forma robusta y accesible. |
| **Tailwind CSS** | Estilos y maquetación visual | Pedido explícitamente por el usuario para un diseño más vistoso y propio, en vez de depender solo del tema por defecto de Material. |
| **SweetAlert2** | Alertas de éxito/error y confirmación de eliminación | Pedido explícitamente; da una experiencia de alerta más pulida y consistente que los componentes nativos del navegador. |
| **RxJS / HttpClient** | Comunicación con el API | Viene integrado en Angular; se usa un interceptor HTTP para centralizar el manejo de errores del backend. |
| **ESLint (`angular-eslint`)** | Calidad de código y accesibilidad | Detectó y permitió corregir 11 problemas reales de accesibilidad (labels sin asociar a su input, uso de `autofocus`) que no se habrían notado a simple vista. |

### DevOps

| Tecnología | Para qué se usa |
|---|---|
| **Docker + Docker Compose** | Levantar todo el stack (SQL Server, backend, frontend) con un solo comando, sin instalar nada más que Docker Desktop. Reproducible en cualquier máquina. |
| **Git** | Control de versiones, con commits incrementales por cada bloque de trabajo completado y verificado. |

---

## Arquitectura

### Backend — Clean Architecture

```
backend/
  src/
    AudiSoft.Domain/          Entidades (Estudiante, Profesor, Nota) y excepciones de negocio.
                               No depende de ningun otro proyecto ni de EF Core.
    AudiSoft.Application/     DTOs, interfaces de repositorio/servicio, y los servicios
                               que contienen las reglas de negocio (validaciones, bloqueo
                               de eliminacion por FK, etc). Depende solo de Domain.
    AudiSoft.Infrastructure/  DbContext de EF Core, configuraciones Fluent API + seed data,
                               implementacion de los repositorios, migraciones.
                               Depende de Application y Domain.
    AudiSoft.WebApi/          Controllers REST, middleware de manejo de errores, Swagger,
                               CORS, Program.cs (composition root: es el unico proyecto
                               que conoce Application e Infrastructure al mismo tiempo).
  tests/
    AudiSoft.Application.Tests/  Pruebas unitarias de los servicios (xUnit + Moq).
```

Regla de dependencia: `Domain ← Application ← Infrastructure ← WebApi`. Ningún proyecto interior conoce a uno exterior — esto permite, por ejemplo, cambiar de SQL Server a otro motor sin tocar una sola línea de `Application` o `Domain`.

**Doble defensa contra el error genérico de SQL al eliminar:**
1. La capa `Application` verifica explícitamente si existen Notas asociadas antes de eliminar un Estudiante/Profesor y lanza una excepción de negocio propia (`ConflictoDeIntegridadException`).
2. Como red de seguridad, el middleware también captura cualquier `DbUpdateException` por violación de llave foránea (error 547 de SQL Server) y la traduce al mismo mensaje amigable — por si en algún escenario de concurrencia la validación anterior no alcanzara a detectarlo.

### Frontend — por features

```
frontend/src/app/
  core/
    interceptors/error.interceptor.ts   Captura errores HTTP del backend y dispara la alerta
    services/notificacion.service.ts    Wrapper de SweetAlert2 (exito, error, confirmar)
  shared/
    models/                             Interfaces TypeScript que reflejan los DTOs del API
    services/crud-base.service.ts       Clase base generica reutilizada por los 3 recursos
    utils/nombre.util.ts                Dividir/unir "Nombres + Apellidos" <-> el campo Nombre unico
  features/
    home/                               Pantalla de inicio con conteos en vivo
    estudiantes/  profesores/  notas/   Cada uno con su *-list (tabla+paginacion) y
                                         *-form-dialog (crear/editar)
```

---

## Estructura de carpetas (raíz del repositorio)

```
Prueba de audisoft/
  backend/              Solucion .NET (Clean Architecture) + Dockerfile
  frontend/             Aplicacion Angular + Dockerfile + nginx.conf
  entregables/           Documento de instalacion, capturas y script SQL
  docker-compose.yml     Orquesta SQL Server + backend + frontend
  README.md              Este archivo
```

---

## Cómo correrlo

### Opción A — Docker (recomendada, todo en un comando)

Requiere [Docker Desktop](https://www.docker.com/products/docker-desktop/).

```bash
docker compose up --build
```

Levanta 3 contenedores: SQL Server, backend (aplica las migraciones automáticamente al arrancar) y frontend (servido con nginx). Probado end-to-end: crea la base de datos desde cero, siembra los datos de ejemplo y sirve la aplicación funcional.

- Frontend: http://localhost:4200
- Backend / Swagger: http://localhost:5080/swagger

Para detener y limpiar: `docker compose down` (agregar `-v` si además se quiere borrar los datos de SQL Server).

### Opción B — Local (sin Docker)

**Prerrequisitos**: .NET SDK 10, Node.js 20+, SQL Server LocalDB, Angular CLI (`npm install -g @angular/cli`).

Backend:

```bash
cd backend
dotnet restore
dotnet run --project src/AudiSoft.WebApi
```

La base de datos y las migraciones se aplican automáticamente al arrancar (no hace falta correr `dotnet ef database update` a mano). API en http://localhost:5080, Swagger en http://localhost:5080/swagger.

Frontend:

```bash
cd frontend
npm install
ng serve
```

App en http://localhost:4200.

---

## API REST

Base URL: `http://localhost:5080/api`. Todas las respuestas de error siguen el formato [ProblemDetails (RFC 7807)](https://www.rfc-editor.org/rfc/rfc7807).

| Recurso | Método | Ruta | Descripción |
|---|---|---|---|
| Estudiantes | GET | `/estudiantes?pageNumber=&pageSize=` | Listado paginado (incluye `cantidadNotas`) |
| | GET | `/estudiantes/lista` | Listado simple id+nombre (para selects) |
| | GET | `/estudiantes/{id}` | Detalle |
| | POST | `/estudiantes` | Crear |
| | PUT | `/estudiantes/{id}` | Actualizar |
| | DELETE | `/estudiantes/{id}` | Eliminar (409 si tiene notas asociadas) |
| Profesores | ... | `/profesores...` | Mismas rutas que Estudiantes |
| Notas | GET | `/notas?pageNumber=&pageSize=` | Listado paginado (incluye nombre de estudiante/profesor) |
| | GET / POST / PUT / DELETE | `/notas...` | CRUD estándar, valida existencia de Estudiante/Profesor y rango 0–5 |

Todos los detalles de request/response están documentados de forma interactiva en Swagger (`/swagger`).

---

## Pruebas y calidad de código

```bash
# Backend: pruebas unitarias (14/14 en verde)
cd backend
dotnet test

# Backend: formato y analizadores (0 advertencias)
dotnet format
dotnet build

# Frontend: lint (0 problemas)
cd frontend
ng lint
```

Las pruebas unitarias del backend cubren específicamente las reglas de negocio más sensibles: bloqueo de eliminación por FK asociada, validaciones (nombre vacío, valor de nota fuera de 0–5), recursos inexistentes (404), y creación exitosa.

---

## Decisiones de diseño

- **Escala de la Nota**: 0.0 a 5.0 (estándar académico), validada en frontend y backend. Es un cambio trivial si se esperaba una escala 0–100.
- **Paginación de 3 por página**: para que los 5 registros de ejemplo de cada tabla se distribuyan en 2 páginas y la paginación quede demostrable con poca data.
- **Nombres/Apellidos en el formulario, un solo campo `Nombre` en la base de datos**: el PDF fija el esquema de Estudiante y Profesor como `id, nombre` — se mantuvo ese esquema exacto en la base de datos, pero el formulario se enriqueció dividiendo la captura en dos campos que se concatenan al guardar.
- **Sin autenticación/login**: no fue pedido por el PDF ni por el audio de recomendaciones, y se priorizó terminar bien lo que sí era requisito dado el plazo. Ver la sección siguiente para el detalle de esta decisión.

---

## Qué le falta para un 100/100

Todo lo pedido explícitamente en el PDF y en el audio de recomendaciones está cubierto. Esta sección es una autoevaluación honesta de lo que le faltaría a esta prueba para ser una aplicación **lista para producción real** (no solo para pasar la evaluación), organizado por categoría:

### Seguridad
- **Autenticación y autorización** (login, JWT o cookies de sesión, roles). Hoy el API es completamente abierto — cualquiera con la URL puede modificar datos. Se decidió no implementarlo por no ser un requisito de la prueba y por el riesgo de tiempo, pero es lo primero que faltaría para un entorno real.
- **Rate limiting** en el API para prevenir abuso.
- **Secretos fuera del código**: la contraseña de SQL Server en `docker-compose.yml` está en texto plano (uso aceptable solo en desarrollo local); en producción iría en un gestor de secretos (Azure Key Vault, AWS Secrets Manager, variables de entorno inyectadas por el orquestador).
- **HTTPS** forzado (hoy corre en HTTP tanto local como en Docker, por simplicidad).

### Testing
- **Pruebas de integración** del backend (contra una base de datos real, ej. con Testcontainers) — las pruebas actuales usan mocks, que validan la lógica de negocio pero no el comportamiento real de EF Core/SQL Server.
- **Pruebas del frontend** (unitarias con Jasmine/Karma o Vitest, y end-to-end con Playwright/Cypress) — hoy el frontend no tiene pruebas automatizadas, solo se verificó manualmente en el navegador.
- **Cobertura de código** medida y reportada (ej. Coverlet + reporte HTML).

### DevOps / CI-CD
- **Pipeline de integración continua** (GitHub Actions) que corra build, lint y tests en cada push/PR — hoy todo se corrió manualmente.
- **Healthchecks** del backend en Docker (`/health` endpoint) para que `docker-compose` pueda esperar a que el API esté realmente listo, no solo que el proceso haya arrancado.
- **Migraciones separadas del arranque en producción**: aplicar migraciones automáticamente al iniciar (como se hizo aquí) es cómodo para desarrollo/demo, pero en un entorno productivo real normalmente se ejecutan como un paso explícito y controlado del pipeline de despliegue.

### Datos y API
- **Auditoría**: fecha de creación/modificación y quién hizo el cambio en cada registro.
- **Borrado lógico (soft delete)** en vez de eliminación física, para poder auditar/recuperar.
- **Búsqueda y filtros** en las tablas (hoy solo hay paginación, no búsqueda por nombre).
- **Ordenamiento** de columnas en las tablas.
- **Versionado del API** (`/api/v1/...`) para poder evolucionar sin romper clientes existentes.

### Frontend / UX
- **Modo oscuro**.
- **Internacionalización** (i18n) si se necesitara soportar más de un idioma.
- **Skeleton loaders / estados de carga** más pulidos mientras llegan los datos (hoy la tabla simplemente aparece vacía un instante).
- **Responsive completo** para móvil (se probó en escritorio; el diseño usa utilidades responsive de Tailwind pero no se validó exhaustivamente en pantallas pequeñas).

### Observabilidad
- **Logging estructurado** (Serilog) con correlación de requests.
- **Métricas y monitoreo** (Application Insights, Prometheus/Grafana, etc.) para un entorno productivo real.

Ninguno de estos puntos es requisito de la prueba técnica — se documentan aquí a propósito, como evidencia de que las decisiones de alcance fueron conscientes y no por desconocimiento.

---

## Entregables

- Código fuente (este repositorio / carpeta comprimida `.ZIP` adjunta)
- `entregables/script_base_datos.sql` — script SQL generado desde las migraciones de EF Core
- `entregables/Documento_Instalacion.docx` — documento de instalación con capturas de la aplicación
- Repositorio de GitHub (opcional, según el PDF)
