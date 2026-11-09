# ProyectoFinal_ChavesCamposAndres

MatrículaU

Sistema de matrícula universitaria desarrollado con ASP.NET Core MVC.

Requisitos.

Visual Studio 2022 con desarrollo de ASP.NET y web.
SDK de .NET 8.
SQL Server Express LocalDB.

Ejecución.

1. Abrir la solución ProyectoFinal_ChavesCamposAndres en Visual Studio.
2. Restaurar los paquetes NuGet.
3. Establecer ProyectoFinal_ChavesCamposAndres como proyecto de inicio.
4. Abrir Administrar secretos de usuario desde el menú del proyecto.
5. Configurar Administrador:Correo y Administrador:Clave.
6. La contraseña debe tener al menos ocho caracteres e incluir mayúscula, minúscula, número y símbolo.
7. Abrir la Consola del Administrador de paquetes.
8. Seleccionar ProyectoFinal_ChavesCamposAndres como proyecto predeterminado.
9. Ejecutar Update-Database.
10. Iniciar la aplicación con F5.

Acceso.

La cuenta administrativa se crea al iniciar la aplicación, utilizando los valores configurados en los secretos de usuario.

Las cuentas estudiantiles se crean desde la opción Crear cuenta.

Funcionalidades disponibles hasta el momento en este segundo avance.

Registro de estudiantes con selección de carrera.
Inicio y cierre de sesión.
Control de acceso para estudiantes y administradores.
Consulta de carreras disponibles.
Consulta de cursos correspondientes a la carrera del estudiante.
Registro, consulta, modificación y eliminación de carreras.
Registro, consulta, modificación y eliminación de cursos.
Validación de datos obligatorios y códigos duplicados.
Protección de registros que tienen información académica relacionada.

Base de datos.

La base se crea mediante las migraciones incluidas en el proyecto.

Contiene las tablas Estudiantes, Carreras, Cursos, Docentes y Matriculas, además de las tablas de ASP.NET Identity.

Gracias.

