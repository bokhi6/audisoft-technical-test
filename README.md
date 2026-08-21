# Prueba Técnica Angular — AudiSoft Consulting SAS

**Desarrollado por: Anthony Daza**

Aplicación web full-stack para la gestión CRUD de Estudiantes, Profesores y Notas, desarrollada según los requisitos de la prueba técnica de AudiSoft Consulting SAS.

## Stack

- **Backend**: .NET 10 en Clean Architecture (Domain / Application / Infrastructure / WebApi)
- **Frontend**: Angular (standalone components) + Angular Material + Tailwind CSS
- **Base de datos**: SQL Server (LocalDB en desarrollo local, contenedor en Docker) vía EF Core Code-First
- **Alertas**: SweetAlert2
- **Calidad**: ESLint (`angular-eslint`) en el frontend, analizadores de .NET + `dotnet format` en el backend, pruebas unitarias (xUnit + Moq)

## Requisitos que cubre

- API REST completa (GET paginado, GET por id, POST, PUT, DELETE) para Estudiante, Profesor y Nota, con FKs de Nota hacia Estudiante y Profesor.
- Mensajes de confirmación en cada acción de creación, edición y eliminación.
- Paginación funcional en las 3 tablas del frontend.
- No se puede eliminar un Estudiante o Profesor con una Nota asociada: se muestra una alerta clara en vez de un error técnico (HTTP 409 + doble validación en backend).
- Ortografía y tildes cuidadas en toda la interfaz.

## Arquitectura del backend

```
backend/
  src/
    AudiSoft.Domain/          Entidades y excepciones de negocio (sin dependencias externas)
    AudiSoft.Application/     DTOs, interfaces de repositorio/servicio, reglas de negocio
    AudiSoft.Infrastructure/  DbContext EF Core, configuraciones, repositorios, migraciones
    AudiSoft.WebApi/          Controllers REST, middleware de errores, Swagger, CORS
  tests/
    AudiSoft.Application.Tests/  Pruebas unitarias (xUnit + Moq)
```

Regla de dependencia: `Domain ← Application ← Infrastructure ← WebApi` (WebApi es el composition root).

## Cómo correrlo

### Opción A — Docker (recomendada, todo en un comando)

Requiere [Docker Desktop](https://www.docker.com/products/docker-desktop/).

```bash
docker compose up --build
```

Esto levanta 3 contenedores: SQL Server, backend (aplica las migraciones automáticamente al arrancar) y frontend (servido con nginx).

- Frontend: http://localhost:4200
- Backend / Swagger: http://localhost:5080/swagger

### Opción B — Local (sin Docker)

**Prerrequisitos**: .NET SDK 10, Node.js 20+, SQL Server LocalDB, Angular CLI (`npm install -g @angular/cli`).

Backend:

```bash
cd backend
dotnet restore
dotnet run --project src/AudiSoft.WebApi
```

La base de datos y las migraciones se aplican automáticamente al arrancar. API en http://localhost:5080, Swagger en http://localhost:5080/swagger.

Frontend:

```bash
cd frontend
npm install
ng serve
```

App en http://localhost:4200.

## Pruebas y linting

```bash
# Backend: pruebas unitarias
cd backend
dotnet test

# Backend: formato y analizadores
dotnet format
dotnet build

# Frontend: lint
cd frontend
ng lint
```

## Entregables

- Código fuente (este repositorio / carpeta comprimida .ZIP adjunta)
- `entregables/script_base_datos.sql` — script SQL generado desde las migraciones de EF Core
- `entregables/Documento_Instalacion.docx` — documento de instalación con capturas de la aplicación
