# Desplegar en Azure

Destino: **App Service para Windows, .NET 10 y Azure SQL**. SQLite queda para desarrollo local. Necesitás una suscripción, SDK .NET 10 y Azure CLI. Revisá el costo de los recursos antes de crearlos.

## Crear los recursos

1. En el portal, creá un grupo de recursos.
2. Creá una base Azure SQL nueva y su servidor lógico. Habilitá autenticación SQL y guardá las credenciales en tu gestor de contraseñas.
3. Creá una aplicación App Service con publicación **Código**, sistema **Windows** y runtime **.NET 10**. Elegí el plan según tu presupuesto. Usá una sola instancia durante el primer arranque.
4. Activá **Solo HTTPS**.
5. En Propiedades de App Service, copiá las direcciones IP de salida posibles y autorizalas en el firewall del servidor SQL. Actualizalas si cambiás el plan o las direcciones. No es necesario habilitar acceso a todos los servicios de Azure.

No subas el archivo SQLite local. La aplicación aplica migraciones al iniciar: la cuenta SQL debe poder crear y modificar tablas. Para un entorno real, separá posteriormente la identidad de migraciones de una identidad de ejecución con permisos reducidos.

## Configuración privada

En App Service → Configuración → Variables de entorno → Configuración de la aplicación:

| Nombre | Valor |
| --- | --- |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `Database__Provider` | `SqlServer` |
| `Demo__Enabled` | `false` |
| `ConnectionStrings__Helpdesk` | Tu cadena SQL privada |
| `Bootstrap__Enabled` | `true` |
| `Bootstrap__AdminEmail` | Tu correo de acceso |
| `Bootstrap__AdminPassword` | Contraseña única de 12 caracteres o más, con mayúsculas, minúsculas, números y símbolo |

Usá la cadena ADO.NET que ofrece Azure y completá sus valores **solo en Azure**:

```text
Server=tcp:<servidor>.database.windows.net,1433;Initial Catalog=<base>;User ID=<usuario-sql>;Password=<contraseña-sql>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

Si la contraseña SQL contiene separadores, escapala según la sintaxis de SqlClient. Es distinta de la contraseña de acceso a Mesa de ayuda. No guardes credenciales en archivos versionados ni publiques capturas de estas variables.

Para probar el circuito completo, agregá estas parejas opcionales con correos diferentes y contraseñas privadas:

- `Bootstrap__TechnicianEmail` y `Bootstrap__TechnicianPassword`.
- `Bootstrap__RequesterEmail` y `Bootstrap__RequesterPassword`.

Cada pareja requiere ambos valores. El arranque crea roles y cuentas en una transacción, sin tickets ficticios. Si una cuenta ya existe y tiene el rol solicitado, conserva su contraseña; si tiene otro rol, falla sin elevar permisos. Todavía no existe un formulario de alta de usuarios.

## Publicar

Desde la raíz del repositorio, en PowerShell:

```powershell
git pull --ff-only
Set-Location Web/01-MesaAyuda
dotnet test MesaAyuda.slnx -c Release
if ($LASTEXITCODE -ne 0) { throw 'Fallaron las pruebas.' }
./scripts/publicar.ps1
az login
az account set --subscription '<id-de-tu-suscripcion>'
az webapp deploy --resource-group '<grupo>' --name '<nombre-app>' --src-path artifacts/mesa-ayuda.zip --type zip
```

El script publica en una carpeta nueva y comprime su contenido, incluyendo `web.config`. Excluye la base local y los símbolos de depuración. No agregues secretos al paquete. Las cuentas demo solo se crean en Development; no se crean en Production.

## Verificar y cerrar la inicialización

1. Abrí la URL HTTPS de App Service e ingresá con el administrador privado.
2. Si configuraste las otras cuentas, creá un ticket como solicitante, asignalo como administrador y resolvelo como técnico.
3. Cambiá `Bootstrap__Enabled` a `false` y eliminá todas las variables de correo y contraseña `Bootstrap__*`. Conservá la conexión SQL y las demás opciones.
4. Guardá, reiniciá y comprobá que el ticket y los accesos se conservan.

Azure SQL conserva los datos fuera del servidor web. App Service utiliza su ubicación persistente predeterminada para las claves de protección de datos. No uses claves efímeras en producción. Los slots tienen claves independientes y sus intercambios pueden invalidar sesiones.

## Actualizar y diagnosticar

Repetí las pruebas, el script y el comando de despliegue para actualizar. No vuelvas a habilitar Bootstrap salvo para aprovisionar otra cuenta inicial autorizada. Antes de cambiar el esquema, verificá los respaldos y el procedimiento de restauración de Azure SQL. Mantené una sola instancia durante las migraciones; no se incluye reversión automática.

- Falla SQL: revisá firewall, servidor, base y credenciales.
- Falla Bootstrap: revisá parejas de variables y política de contraseñas. Los errores muestran códigos de Identity, sin imprimir contraseñas.
- Error de arranque: usá los registros y Diagnosticar y resolver problemas de App Service. Conservá `Production`.
- Sin técnicos para asignar: verificá que la cuenta técnica se haya creado antes de deshabilitar Bootstrap.

Las pruebas locales cubren SQLite y la generación del esquema SQL Server. La conectividad, los permisos y el comportamiento en tu base Azure se comprueban después del despliegue.

Referencias: [App Service con .NET](https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore), [publicación ZIP](https://learn.microsoft.com/en-us/azure/app-service/deploy-zip), [protección de datos](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/default-settings?view=aspnetcore-10.0).
