# Reservas de espacios

API de prueba para coordinar salas de reunión y evitar reservas superpuestas. Implementa persistencia, autenticación de integración, idempotencia, cancelaciones y pruebas de concurrencia sobre una base relacional.

[![Build y pruebas](https://github.com/HiroNk11/.NetProyects/actions/workflows/reservas-espacios.yml/badge.svg)](https://github.com/HiroNk11/.NetProyects/actions/workflows/reservas-espacios.yml)

## Qué demuestra

- ASP.NET Core 10 con controladores, modelos de entrada y respuestas HTTP explícitas.
- EF Core y SQLite con migraciones e índices.
- Comprobación de disponibilidad e inserción dentro de una transacción de escritura.
- Reintentos con `Idempotency-Key` sin duplicar reservas.
- Autenticación mediante una credencial privada de integración.
- Errores `ProblemDetails`, paginación y fechas normalizadas a UTC.
- Contrato OpenAPI protegido y verificación continua en GitHub Actions.

Complementa las aplicaciones MVC del repositorio con un backend consumible desde otros sistemas. Es un servicio para una única organización y una única integración de confianza; no implementa usuarios individuales ni separación entre clientes.

## Ejecutar

Requisito: **SDK .NET 10**. Los comandos de ejemplo usan **PowerShell 7**.

Desde la raíz del repositorio:

```powershell
Set-Location WebApi/05-ReservasEspacios
dotnet restore ReservasEspacios.slnx
dotnet test ReservasEspacios.slnx -c Release
$env:ApiAccess__Key = Read-Host 'Clave privada aleatoria de al menos 32 caracteres' -MaskInput
dotnet run --project src/Reservas.Api --launch-profile Reservas
```

Generá la clave en un gestor de contraseñas y pegala cuando se solicite. La aplicación no incluye una clave predeterminada y rechaza una configuración vacía o demasiado corta. No guardes la clave en archivos del repositorio.

Dirección local: `http://localhost:5190`. El perfil activa Development; el uso de HTTP se limita a esta prueba local. En un alojamiento compartido configurá HTTPS y almacenamiento persistente antes de exponer el servicio.

Al iniciar se aplican las migraciones y se crean tres salas: Norte (6 personas), Sur (12) y Auditorio (40). La base queda en `src/Reservas.Api/App_Data/reservas.db`, excluida de Git y de la publicación. No se precargan reservas.

Para usar otra base configurá `ConnectionStrings__Bookings` con una cadena SQLite. Su directorio debe existir. Conservá el archivo entre reinicios; no uses una carpeta temporal de un alojamiento para datos que quieras preservar.

## Recorrido por HTTP

Con la aplicación funcionando, abrí otra terminal PowerShell 7:

```powershell
$key = Read-Host 'La misma clave privada del servidor' -MaskInput
$headers = @{ Authorization = "Bearer $key" }
$base = 'http://localhost:5190'
Invoke-RestMethod "$base/api/rooms" -Headers $headers

$requestKey = [guid]::NewGuid().ToString()
$createHeaders = @{ Authorization = "Bearer $key"; 'Idempotency-Key' = $requestKey }
$start = [DateTimeOffset]::UtcNow.AddDays(1)
$body = @{
    roomId = 1
    title = 'Revisión de planificación'
    attendees = 4
    startsAt = $start.ToString('o')
    endsAt = $start.AddHours(1).ToString('o')
} | ConvertTo-Json

$booking = Invoke-RestMethod "$base/api/bookings" -Method Post -Headers $createHeaders -ContentType 'application/json' -Body $body
# Repetir con la misma clave devuelve la misma reserva.
Invoke-RestMethod "$base/api/bookings" -Method Post -Headers $createHeaders -ContentType 'application/json' -Body $body
Invoke-RestMethod "$base/api/bookings?roomId=1&cancelled=false&page=1&pageSize=10" -Headers $headers
Invoke-RestMethod "$base/api/bookings/$($booking.id)/cancel" -Method Post -Headers $headers
```

Para provocar un conflicto, enviá el mismo horario con una **nueva** clave de idempotencia antes de cancelar. La respuesta será `409`. Después de cancelar, ese horario vuelve a estar disponible.

## Contrato

Todas las rutas excepto `/healthz` requieren `Authorization: Bearer <credencial-privada>`. La credencial es opaca, no un JWT. No se aceptan claves por query string.

| Método y ruta | Resultado |
| --- | --- |
| `GET /healthz` | Liveness del proceso; no verifica la disponibilidad de la base |
| `GET /api/rooms` | Catálogo de salas y capacidades |
| `POST /api/bookings` | `201` y `Location` al crear; `200` al repetir la misma solicitud |
| `GET /api/bookings/{id}` | Reserva persistida o `404` |
| `GET /api/bookings` | Página con `page`, `pageSize`, `total` e `items` |
| `POST /api/bookings/{id}/cancel` | `204`; repetir la cancelación también devuelve `204` |
| `GET /openapi/v1.json` | Documento OpenAPI para importar en un cliente HTTP |

El listado admite `roomId`, `cancelled`, `page` (desde 1) y `pageSize` (1 a 50). Ordena por inicio y luego por identificador. No incluye interfaz Swagger interactiva; al importar OpenAPI configurá la autorización Bearer en tu cliente.

Las entradas inválidas devuelven `400`, la falta de credenciales `401`, los recursos ausentes `404` y las reservas superpuestas o claves reutilizadas con otros datos `409`. Si SQLite agota la espera por un bloqueo de escritura, devuelve `503` con `Retry-After`; el cliente puede reintentar con la misma clave.

## Reglas y consistencia

- El título tiene entre 3 y 120 caracteres; se recortan los espacios exteriores.
- Los asistentes deben ser positivos y no superar la capacidad de la sala.
- El inicio debe estar en el futuro. La duración es positiva y como máximo de 24 horas.
- Las fechas se reciben como ISO 8601 con offset. Se comparan y devuelven en UTC, conservando la precisión de ticks.
- Los intervalos son `[inicio, fin)`: una reserva puede comenzar exactamente cuando termina otra.
- Cancelar conserva el registro y libera el horario. No existe reactivación ni edición en esta versión.
- `Idempotency-Key` debe ser un UUID no vacío. Se guarda con la reserva y no caduca en esta versión.
- Repetir la clave y los mismos datos devuelve el estado actual de la reserva, incluso si está cancelada. Usar esa clave con otros datos devuelve `409`.

SQLite toma un bloqueo de escritura antes de comprobar la clave y la disponibilidad. La transacción comprende esa comprobación y la inserción, evitando que dos conexiones confirmen la misma franja. El índice único de la clave refuerza la idempotencia. La serialización de escrituras limita la capacidad de carga: este diseño es apropiado para una prueba pequeña, no para alta concurrencia.

## Estructura y pruebas

```text
src/Reservas.Api/
  BookingsController.cs  Contrato HTTP y paginación
  BookingService.cs     Reglas, idempotencia y transacción
  Contracts.cs          Entradas y respuestas
  Security/             Autenticación de integración
  Data/                 Modelo persistente y DbContext
  Migrations/           Esquema y catálogo inicial
tests/Reservas.Tests/    Pruebas HTTP con SQLite temporal
```

```powershell
dotnet test ReservasEspacios.slnx -c Release
dotnet publish src/Reservas.Api -c Release -o artifacts/app
```

Las pruebas usan claves aleatorias y una base temporal independiente por caso. Cubren autenticación, configuración sin clave, OpenAPI, persistencia, validación, zonas horarias, superposiciones, límites contiguos, cancelación, filtros, paginación y solicitudes concurrentes con claves iguales y diferentes. También comprueban que el modelo no tenga cambios sin migración.

La [integración continua](../../.github/workflows/reservas-espacios.yml) restaura, compila, prueba y publica un paquete de verificación en Linux. El comando de publicación no despliega recursos.

## Evolución del esquema

Desde esta carpeta:

```powershell
dotnet tool restore
$env:ASPNETCORE_ENVIRONMENT = 'Testing'
dotnet ef migrations add NombreDelCambio --project src/Reservas.Api
Remove-Item Env:ASPNETCORE_ENVIRONMENT
```

El entorno Testing evita aplicar migraciones durante la generación del esquema. En ejecución normal se aplican al inicio; antes de un uso compartido conviene mover ese paso al proceso de entrega, con respaldo y una única instancia responsable.

## Evolución del proyecto

Propuestas pendientes, todavía no implementadas:

- Identidades individuales, permisos por sala y separación entre organizaciones.
- Administración de salas, horarios de apertura y días bloqueados.
- Reservas recurrentes y reprogramación con control de versión.
- Notificaciones mediante procesamiento en segundo plano.
- Métricas, límites de solicitudes y readiness de la base.
- Pruebas de carga y migración a un servidor de base de datos con una estrategia equivalente contra superposiciones.
- Despliegue con HTTPS, rotación de credenciales y respaldos automatizados.

La clave actual permite consultar y cancelar todas las reservas; entregala solamente a la integración de confianza. No hay un despliegue público preparado ni afirmaciones de rendimiento sin mediciones.
