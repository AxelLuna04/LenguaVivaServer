# 🌎 LenguaViva — Servidor

Servidor del sistema de gestión para **LenguaViva**, una escuela de idiomas. Expone servicios **WCF** que usa la aplicación de escritorio [LenguaVivaCliente](https://github.com/AxelLuna04/LenguaVivaCliente) para administrar alumnos, profesores, cursos, horarios, inscripciones y pagos.

## ✨ Funcionalidades

- **Usuarios:** registro, búsqueda y edición de administradores, profesores y alumnos; inicio de sesión para administradores y profesores. Las contraseñas se guardan con **hash SHA-256 y salt**.
- **Cursos e idiomas:** alta, edición y eliminación de cursos; búsqueda por nombre, idioma, nivel o profesor; asignación de horarios.
- **Inscripciones y pagos:** inscribir alumnos a cursos, registrar pagos y **enviar el recibo en PDF por correo**.
- **Reportes:** profesores activos, alumnos por curso, horarios de profesores e ingresos.

## 🧰 Tecnologías

| Área | Herramientas |
|---|---|
| Plataforma | C#, .NET Framework 4.8 |
| Servicios | WCF con `netTcpBinding` |
| Datos | SQL Server con Entity Framework 6 (Database First) |
| Pruebas | NUnit y Moq, con pruebas unitarias y de integración |

## 🗂️ Estructura

```
LenguaVivaServidor/
├── Program.cs          # Arranca el host de WCF
├── ServicioContrato/   # Contratos (IGestionUsuarios, IGestionCursos, IGestionAlumnos, IGestionReportes) e implementación
├── AccesoDatos/        # Modelo de Entity Framework (ModeloDB.edmx) y DAOs
└── Pruebas/            # Pruebas unitarias y de integración (NUnit)
```

## ⚙️ Cómo correrlo

### Prerrequisitos

- Windows con Visual Studio 2022 y la carga de trabajo de escritorio .NET
- SQL Server (Express funciona) con la base de datos `LenguaVivaDB`

### Pasos

1. Clona el repositorio y abre `LenguaVivaServidor.sln` en Visual Studio.
2. En `App.config`, cambia la cadena de conexión `LenguaVivaDBEntities`: pon el nombre de tu instancia de SQL Server en `data source` y el usuario y contraseña de tu base de datos.
3. Restaura los paquetes NuGet y compila la solución.
4. Ejecuta el proyecto. El servicio queda escuchando en `net.tcp://localhost:8000/ServiceImplementation`.
5. Abre el [cliente](https://github.com/AxelLuna04/LenguaVivaCliente) para usar el sistema.

> ⚠️ No subas a GitHub la cadena de conexión con tu usuario y contraseña reales.

## 🧪 Pruebas

Abre el **Explorador de pruebas** de Visual Studio y ejecuta el proyecto `Pruebas`. Las pruebas de integración necesitan la base de datos configurada.

<!--
## 👤 Mi participación
Este fue un proyecto en equipo. Describe aquí qué partes desarrollaste tú.
-->
