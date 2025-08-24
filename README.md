# WheezlyApp

## ¿Qué hay implementado hasta ahora?

¡Hola! 👋 Si estás leyendo esto, probablemente quieras levantar el proyecto. Te cuento que ya tenemos varias cosas funcionando:

### Base de datos y consultas
- Ya está todo el esquema de la base de datos armado en `src/Scripts/DatabaseSchema.sql` 
- Incluye las tablas para autos, compradores, códigos postales y todo el manejo de estados
- Viene con datos de prueba para que puedas empezar a jugar con la app
- Las consultas pedidas están en `Query.sql`

### Scripts de procesamiento de archivos
- El script para procesar archivos .cs y sus tests están en:
  - `ScriptFolders.cs`: Implementación principal
  - `ScriptFolderTests.cs`: Tests unitarios

### Respuestas a preguntas del enunciado

2) What would you do if you had data that doesn't change often but it's used pretty much all
the time? Would it make a difference if you have more than one instance of your
application?
--Usaria cache, si la app tiene mas de una instancia usaria cache distribuido como redis por ejemplo

3) Analyze the following method and make changes to make it better. Explain your
changes.
```csharp
public void UpdateCustomersBalanceByInvoices(List<Invoice> invoices)
{
   foreach (var invoice in invoices)
   {
      var customer = dbContext.Customers.SingleOrDefault(invoice.CustomerId.Value);

      customer.Balance -= invoice.Total;
      dbContext.SaveChanges();
   }
}
```

--lo cambiaria por:
```csharp
public void UpdateCustomersBalanceByInvoices(List<Invoice> invoices)
{
   foreach (var invoice in invoices)
   {
      var customer = dbContext.Customers.SingleOrDefault(invoice.CustomerId.Value);

      customer.Balance -= invoice.Total;
      
   }
   dbContext.SaveChanges();
}
```
//para no hacer el set en la base de datos en cada iteracion

4) Se agrega un OrderController para mostrar el código

## ¿Cómo levantar el proyecto?

### Instalación de herramientas necesarias

1. **.NET 8 SDK**
   - Descargalo de: https://dotnet.microsoft.com/download/dotnet/8.0
   - Verificá la instalación con:
     ```bash
     dotnet --version
     ```

2. **Docker y Docker Compose**
   - Windows/Mac: Instalá Docker Desktop desde https://www.docker.com/products/docker-desktop
   - Linux: Seguí la guía oficial para tu distribución en https://docs.docker.com/engine/install/
   - Verificá las instalaciones con:
     ```bash
     docker --version
     docker-compose --version
     ```

3. **SQL Server Command Line Tools (sqlcmd)**
   - Windows: Descargá "Microsoft Command Line Utilities for SQL Server" desde:
     https://learn.microsoft.com/en-us/sql/tools/sqlcmd/sqlcmd-utility
   - Ubuntu/Debian:
     ```bash
     curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
     curl https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/prod.list | sudo tee /etc/apt/sources.list.d/msprod.list
     sudo apt-get update
     sudo apt-get install mssql-tools unixodbc-dev
     ```

### Pasos para correr la app:

1. Cloná el repo
2. Desde la carpeta raíz del proyecto (donde está el archivo `docker-compose.yml`), ejecutá:
   ```bash
   docker-compose up sqlserver -d
   ```
   Esto levanta SQL Server en el puerto 14334.

3. Una vez que el contenedor esté corriendo, ejecutá el script para crear la base de datos y las tablas:
   ```bash
   sqlcmd -S localhost,14334 -U sa -P YourStrong!Passw0rd -i src/Scripts/DatabaseSchema.sql
   ```
   Si el comando falla, esperá unos segundos a que SQL Server termine de iniciar y volvé a intentar.

4. Actualizá la cadena de conexión en `appsettings.json` si es necesario. La cadena por defecto ya está configurada para el SQL Server en Docker.

5. Ejecutá la aplicación:
   ```bash
   cd src
   dotnet run
   ```

6. ¡Listo! Podés acceder a:
   - Swagger UI: http://localhost:5091/swagger
   - API endpoints: 
     - HTTP: http://localhost:5091
     - HTTPS: https://localhost:7091
   
   La documentación completa de los endpoints está disponible en Swagger.