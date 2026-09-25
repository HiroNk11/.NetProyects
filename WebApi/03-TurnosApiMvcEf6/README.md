# Turnos API MVC EF6

Proyecto de prueba en evolucion.

API REST de turnos usando ASP.NET Web API 2, .NET Framework 4.8, Entity Framework 6 y SQL Server LocalDB.

## Entidades

- `Paciente`
- `Profesional`
- `Turno`

## Endpoints

| Metodo | Ruta | Descripcion |
| --- | --- | --- |
| GET | `/api/pacientes` | Lista pacientes |
| POST | `/api/pacientes` | Crea un paciente |
| GET | `/api/profesionales` | Lista profesionales |
| POST | `/api/profesionales` | Crea un profesional |
| GET | `/api/turnos` | Lista turnos |
| GET | `/api/turnos/{id}` | Obtiene un turno |
| POST | `/api/turnos` | Agenda un turno |
| POST | `/api/turnos/{id}/cancelar` | Cancela un turno |

## Ejemplo crear paciente

```json
{
  "nombre": "Maria Lopez",
  "documento": "30111222"
}
```

## Ejemplo crear profesional

```json
{
  "nombre": "Dra. Perez",
  "especialidad": "Clinica medica"
}
```

## Ejemplo crear turno

```json
{
  "pacienteId": 1,
  "profesionalId": 1,
  "fechaHora": "2026-09-10T10:00:00",
  "duracionMinutos": 30,
  "motivo": "Consulta inicial"
}
```

## Buenas practicas aplicadas

- DTOs para entrada y salida
- Repositorios con interfaces
- Servicios con reglas de negocio
- Validacion de fecha futura
- Validacion de disponibilidad del profesional
- Cancelacion por cambio de estado
- Entity Framework 6 con relaciones
- Carga de relaciones con `Include`

## Como ejecutar

1. Abrir el proyecto en Visual Studio.
2. Restaurar paquetes NuGet.
3. Ejecutar con IIS Express.
4. Probar endpoints con Postman o Thunder Client.
