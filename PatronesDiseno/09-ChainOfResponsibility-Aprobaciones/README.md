# 09 - Chain Of Responsibility Aprobaciones

Ejemplo del patron Chain of Responsibility aplicado a aprobaciones de gastos.

## Idea

Cada aprobador decide si puede resolver la solicitud. Si no puede, la pasa al siguiente responsable de la cadena.

## Niveles incluidos

- Supervisor
- Gerente
- Director

## Buenas practicas aplicadas

- Evita condicionales extensos por rango de aprobacion.
- Permite cambiar la cadena sin modificar las solicitudes.
- Cada aprobador conoce solo su responsabilidad.
