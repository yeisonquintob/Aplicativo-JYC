Copiar# Mercancías JyC - Sistema de Gestión de Pedidos y Entregas

Este proyecto consiste en un sistema integral para manejar pedidos y entregas para la empresa Mercancías JyC.

## Estructura del Proyecto

El proyecto está organizado en la carpeta principal "Aplicativo JYC" que contiene tres subcarpetas principales:

1. Backend API: Contiene el proyecto MercanciasJyCAPI.
2. BD: Contiene los scripts de base de datos y una subcarpeta SP con los procedimientos almacenados.
3. Frontend: Contiene el proyecto Angular mercancias-jyc-frontend.

## Requisitos previos

Antes de comenzar, asegúrese de tener instalado lo siguiente:

- Node.js (versión 14.x o superior)
- .NET Core SDK (versión 6.0 o superior)
- SQL Server (versión 2019 o superior)

## Configuración de la Base de Datos

1. Navegue a la carpeta "BD" del proyecto.
2. Ejecute el script "Creacion de Base de datos y tablas.sql" en SQL Server para crear la base de datos y las tablas necesarias.
3. Navegue a la subcarpeta "SP" y ejecute los siguientes procedimientos almacenados en este orden:
   - sp_ValidarFechaEntrega.sql
   - sp_CrearPedido.sql
   - sp_AgregarDetallePedido.sql
   - sp_ProgramarEntrega.sql

## Configuración del Backend

1. Navegue a la carpeta "Backend API/MercanciasJyCAPI".
2. Abra el proyecto en Visual Studio o su IDE preferido.
3. Restaure las dependencias del proyecto.
4. Configure la cadena de conexión a la base de datos en el archivo `appsettings.json`.
5. Ejecute el proyecto.

El backend estará disponible y se puede probar en `https://localhost:[puerto]/swagger/index.html`.

## Configuración del Frontend

1. Navegue a la carpeta "mercancias-jyc-frontend".
2. Abra una terminal en esta ubicación.
3. Ejecute el siguiente comando para instalar las dependencias:
Instalación de npm
Copiar4. Una vez completada la instalación, inicie la aplicación con:
inicio npm
Copiar
La aplicación frontend estará disponible en `http://localhost:4200`.

## Pruebas de la API

Puede probar la API utilizando Swagger UI, que estará disponible en `https://localhost:[puerto]/swagger/index.html` una vez que el backend esté en ejecución. Aquí podrá ver y probar todos los endpoints disponibles para Cliente, Entrega, Producto y Pedido.

### Endpoints disponibles para pruebas:

#### Clientes
- GET /api/Clientes
- POST /api/Clientes
- GET /api/Clientes/{id}
- PUT /api/Clientes/{id}
- DELETE /api/Clientes/{id}

#### Entregas
- GET /api/Entregas
- POST /api/Entregas
- GET /api/Entregas/{id}
- PUT /api/Entregas/{id}
- DELETE /api/Entregas/{id}
- GET /api/Entregas/Pedido/{pedidoId}
- PUT /api/Entregas/{id}/CompletarEntrega

#### Pedidos
- GET /api/Pedidos
- POST /api/Pedidos
- GET /api/Pedidos/{id}
- POST /api/Pedidos/{pedidoId}/DetallePedido
- POST /api/Pedidos/{pedidoId}/ProgramarEntrega

#### Productos
- GET /api/Productos
- POST /api/Productos
- GET /api/Productos/{id}
- PUT /api/Productos/{id}
- DELETE /api/Productos/{id}

Para probar estos endpoints:

1. Inicie el backend de la aplicación.
2. Abra un navegador web y vaya a `https://localhost:[puerto]/swagger/index.html`.
3. En la interfaz de Swagger, verá todos los endpoints listados arriba.
4. Haga clic en el endpoint que desea probar.
5. Haga clic en el botón "Try it out".
6. Si el endpoint requiere un cuerpo de solicitud o parámetros, complete los campos necesarios.
7. Haga clic en "Execute" para enviar la solicitud.
8. Observe la respuesta en la sección "Responses" debajo del botón "Execute".

Nota: Asegúrese de que se hayan realizado las inserciones necesarias en la base de datos para poder probar efectivamente todos los endpoints.


## Documentación adicional


### Lógica de negocio

1. Gestión de Pedidos:
   - Los pedidos están sujetos a reglas específicas de fechas de entrega.
   - Se utiliza el procedimiento almacenado `sp_ValidarFechaEntrega` para asegurar que los pedidos cumplan con estas reglas.
   - El procedimiento `sp_CrearPedido` maneja la creación de nuevos pedidos.

2. Detalles de Pedido:
   - Se utiliza `sp_AgregarDetallePedido` para añadir productos a un pedido.
   - Este procedimiento también actualiza el stock de productos.

3. Programación de Entregas:
   - `sp_ProgramarEntrega` se encarga de crear una nueva entrega y actualizar el estado del pedido.

4. Validaciones:
   - Se implementan validaciones tanto en el backend como en el frontend para asegurar la integridad de los datos.

### Reglas de negocio clave

- Los pedidos para entrega el lunes deben realizarse entre el viernes al mediodía y el sábado al mediodía.
- Los pedidos para entrega el martes deben realizarse entre el sábado después del mediodía y el domingo al mediodía.
- Los pedidos para entrega el miércoles deben realizarse entre el domingo después del mediodía y el lunes al mediodía.
- Los pedidos para entrega el jueves deben realizarse entre el lunes después del mediodía y el martes al mediodía.
- Los pedidos para entrega el viernes deben realizarse entre el martes después del mediodía y el miércoles al mediodía.
- Los pedidos para entrega el sábado deben realizarse entre el miércoles después del mediodía y el jueves al mediodía.
- No se realizan entregas los domingos.

### Aspectos técnicos adicionales

- La API utiliza Swagger para la documentación y pruebas de endpoints.
- Se implementa manejo de errores personalizado tanto en la API como en los procedimientos almacenados.
- El frontend utiliza servicios Angular para comunicarse con la API backend.
- Se implementa validación de formularios en el frontend para mejorar la experiencia del usuario y reducir errores.

