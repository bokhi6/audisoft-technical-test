IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Estudiantes] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_Estudiantes] PRIMARY KEY ([Id])
);

CREATE TABLE [Profesores] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_Profesores] PRIMARY KEY ([Id])
);

CREATE TABLE [Notas] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(200) NOT NULL,
    [IdEstudiante] int NOT NULL,
    [IdProfesor] int NOT NULL,
    [Valor] decimal(5,2) NOT NULL,
    CONSTRAINT [PK_Notas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notas_Estudiantes_IdEstudiante] FOREIGN KEY ([IdEstudiante]) REFERENCES [Estudiantes] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Notas_Profesores_IdProfesor] FOREIGN KEY ([IdProfesor]) REFERENCES [Profesores] ([Id]) ON DELETE NO ACTION
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre') AND [object_id] = OBJECT_ID(N'[Estudiantes]'))
    SET IDENTITY_INSERT [Estudiantes] ON;
INSERT INTO [Estudiantes] ([Id], [Nombre])
VALUES (1, N'Juan Pérez'),
(2, N'María Gómez'),
(3, N'Carlos Rodríguez'),
(4, N'Ana Martínez'),
(5, N'Luis Fernández');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre') AND [object_id] = OBJECT_ID(N'[Estudiantes]'))
    SET IDENTITY_INSERT [Estudiantes] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre') AND [object_id] = OBJECT_ID(N'[Profesores]'))
    SET IDENTITY_INSERT [Profesores] ON;
INSERT INTO [Profesores] ([Id], [Nombre])
VALUES (1, N'Andrés Torres'),
(2, N'Beatriz Ramírez'),
(3, N'Camilo Vargas'),
(4, N'Diana Castro'),
(5, N'Eduardo Salazar');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre') AND [object_id] = OBJECT_ID(N'[Profesores]'))
    SET IDENTITY_INSERT [Profesores] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IdEstudiante', N'IdProfesor', N'Nombre', N'Valor') AND [object_id] = OBJECT_ID(N'[Notas]'))
    SET IDENTITY_INSERT [Notas] ON;
INSERT INTO [Notas] ([Id], [IdEstudiante], [IdProfesor], [Nombre], [Valor])
VALUES (1, 1, 1, N'Parcial 1', 3.5),
(2, 2, 2, N'Parcial 2', 4.2),
(3, 3, 3, N'Quiz 1', 2.8),
(4, 4, 1, N'Proyecto Final', 4.9),
(5, 5, 2, N'Examen Final', 3.0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IdEstudiante', N'IdProfesor', N'Nombre', N'Valor') AND [object_id] = OBJECT_ID(N'[Notas]'))
    SET IDENTITY_INSERT [Notas] OFF;

CREATE INDEX [IX_Notas_IdEstudiante] ON [Notas] ([IdEstudiante]);

CREATE INDEX [IX_Notas_IdProfesor] ON [Notas] ([IdProfesor]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260821051121_InicialConSeed', N'10.0.11');

COMMIT;
GO

