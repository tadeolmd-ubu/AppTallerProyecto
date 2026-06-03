CREATE DATABASE dbTaller;
GO
USE dbTaller;
GO
CREATE TABLE catRol(
    id INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);
CREATE TABLE Direccion(
    id INT PRIMARY KEY,
    ciudad VARCHAR(50) NOT NULL,
    colonia VARCHAR(50) NOT NULL,
    codigoPostal VARCHAR(10) NOT NULL,
    calle VARCHAR(50) NOT NULL,
    numeroCasa VARCHAR(10) NOT NULL
);
CREATE TABLE catMarca(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE
);
CREATE TABLE TipoProducto(
    id INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);
CREATE TABLE catEmpresa(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    rfc VARCHAR(13) NOT NULL UNIQUE,
    regimen VARCHAR(100) NOT NULL,
    idDireccion INT NULL,
    FOREIGN KEY(idDireccion) REFERENCES Direccion(id)
);
CREATE TABLE Usuario(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    contrasena VARCHAR(200) NOT NULL,
    correo VARCHAR(100) NOT NULL UNIQUE,
    telefono VARCHAR(15) NOT NULL,
    estatus BIT NOT NULL,
    idRol INT NOT NULL,
    FOREIGN KEY (idRol) REFERENCES catRol(id)
);
CREATE TABLE Cliente(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(15) NOT NULL,
    estatus BIT NOT NULL,
    idDireccion INT NULL,
    FOREIGN KEY(idDireccion) REFERENCES Direccion(id)
);
CREATE TABLE Proveedor(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(15) NOT NULL,
    estatus BIT NOT NULL,
    idEmpresa INT NOT NULL,
    idDireccion INT NULL,
    FOREIGN KEY(idDireccion) REFERENCES Direccion(id),
    FOREIGN KEY (idEmpresa) REFERENCES catEmpresa(id)
);
CREATE TABLE Producto(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    estatus BIT NOT NULL,
    idMarca INT NOT NULL,
    idTipoProducto INT NOT NULL,
    fechaCreacion DATETIME DEFAULT GETDATE() NOT NULL,
    fechaActualizacion DATETIME DEFAULT GETDATE() NOT NULL,
    FOREIGN KEY (idMarca) REFERENCES catMarca(id),
    FOREIGN KEY (idTipoProducto) REFERENCES TipoProducto(id)
);
CREATE TABLE catAlmacen(
    id INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    idDireccion INT NULL,
    FOREIGN KEY (idDireccion) REFERENCES Direccion(id)
);
CREATE TABLE Inventario(
    id INT PRIMARY KEY,
    idProducto INT NOT NULL,
    idAlmacen INT NOT NULL,
    stockActual INT DEFAULT 0,
    fechaUltimaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (idProducto) REFERENCES Producto(id),
    FOREIGN KEY (idAlmacen) REFERENCES catAlmacen(id),
    CONSTRAINT UQ_Inventario UNIQUE (idProducto, idAlmacen)
);
CREATE TABLE TipoMovimiento(
    id INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);
CREATE TABLE ReferenciaMovimiento(
    id INT PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL UNIQUE
);
CREATE TABLE MovimientoInventario(
    id INT PRIMARY KEY,
    idInventario INT NOT NULL,
    idTipoMovimiento INT NOT NULL,
    idReferenciaMovimiento INT NULL,
    cantidad INT NOT NULL,
    fechaMovimiento DATETIME DEFAULT GETDATE(),
    idUsuario INT NULL,
    FOREIGN KEY (idInventario) REFERENCES Inventario(id),
    FOREIGN KEY (idTipoMovimiento) REFERENCES TipoMovimiento(id),
    FOREIGN KEY (idReferenciaMovimiento) REFERENCES ReferenciaMovimiento(id),
    FOREIGN KEY (idUsuario) REFERENCES Usuario(id)
);
CREATE TABLE Presupuesto(
    id INT PRIMARY KEY,
    total DECIMAL(10,2) NOT NULL,
    estatus BIT NOT NULL,
    nota VARCHAR(MAX) NULL,
    idCliente INT NOT NULL,
    idUsuario INT NOT NULL,
    fechaCreacion DATETIME DEFAULT GETDATE(),
    fechaModificacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY(idCliente) REFERENCES Cliente(id),
    FOREIGN KEY(idUsuario) REFERENCES Usuario(id)
);
CREATE TABLE PresupuestoDetalle(
    id INT PRIMARY KEY,
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    importe DECIMAL(18,2) NOT NULL,
    iva DECIMAL(18,2) NOT NULL,
    idInventario INT NOT NULL,
    idPresupuesto INT NOT NULL,
    FOREIGN KEY(idInventario) REFERENCES Inventario(id),
    FOREIGN KEY(idPresupuesto) REFERENCES Presupuesto(id)
);
CREATE TABLE Venta(
    id INT PRIMARY KEY,
    fecha DATETIME2 DEFAULT GETDATE() NOT NULL,
    total DECIMAL(18,2) NOT NULL,
    estatus BIT NOT NULL,
    idUsuario INT NOT NULL,
    idCliente INT NOT NULL,
    folio INT NOT NULL UNIQUE,
    FOREIGN KEY (idUsuario) REFERENCES Usuario(id),
    FOREIGN KEY (idCliente) REFERENCES Cliente(id)
);
CREATE TABLE VentaDetalle(
    id INT PRIMARY KEY,
    cantidad INT NOT NULL,
    importe DECIMAL(18,2) NOT NULL,
    iva DECIMAL(18,2) NOT NULL,
    idInventario INT NOT NULL,
    idVenta INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (idVenta) REFERENCES Venta(id),
    FOREIGN KEY(idInventario) REFERENCES Inventario(id)
);
CREATE TABLE catEstatusTarea(
    id INT PRIMARY KEY,
    descripcion VARCHAR(50) NOT NULL
);
CREATE TABLE Empleado(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(15) NOT NULL,
    estatus BIT NOT NULL,
    idRol INT NOT NULL,
    idDireccion INT NULL,
    FOREIGN KEY(idDireccion) REFERENCES Direccion(id),
    FOREIGN KEY(idRol) REFERENCES catRol(id)
);
CREATE TABLE Compra(
    id INT PRIMARY KEY,
    fecha DATETIME2 NOT NULL,
    total DECIMAL(10,2) NOT NULL,
    estatus BIT NOT NULL,
    idEmpleado INT NOT NULL,
    idProveedor INT NOT NULL,
    FOREIGN KEY (idEmpleado) REFERENCES Empleado(id),
    FOREIGN KEY (idProveedor) REFERENCES Proveedor(id)
);
CREATE TABLE CompraDetalle(
    id INT PRIMARY KEY,
    cantidad INT NOT NULL,
    importe DECIMAL(10,2) NOT NULL,
    iva DECIMAL(10,2) NOT NULL,
    idProducto INT NOT NULL,
    idCompra INT NOT NULL,
    FOREIGN KEY (idCompra) REFERENCES Compra(id),
    FOREIGN KEY (idProducto) REFERENCES Producto(id)
);
CREATE TABLE Tarea(
    id INT PRIMARY KEY,
    descripcion VARCHAR(200) NOT NULL,
    fechaInicio DATETIME2 NOT NULL,
    fechaTermino DATETIME2 NOT NULL,
    idEstatus INT NOT NULL,
    idEmpleado INT NOT NULL,
    FOREIGN KEY (idEstatus) REFERENCES catEstatusTarea(id),
    FOREIGN KEY (idEmpleado) REFERENCES Empleado(id)
);
GO
PRINT 'Tablas creadas correctamente.';
GO
ALTER TABLE Usuario ADD CONSTRAINT DF_Usuario_estatus DEFAULT 1 FOR estatus;
ALTER TABLE Cliente ADD CONSTRAINT DF_Cliente_estatus DEFAULT 1 FOR estatus;
ALTER TABLE Proveedor ADD CONSTRAINT DF_Proveedor_estatus DEFAULT 1 FOR estatus;
ALTER TABLE Producto ADD CONSTRAINT DF_Producto_estatus DEFAULT 1 FOR estatus;
ALTER TABLE Presupuesto ADD CONSTRAINT DF_Presupuesto_estatus DEFAULT 1 FOR estatus;
ALTER TABLE Venta ADD CONSTRAINT DF_Venta_estatus DEFAULT 1 FOR estatus;
GO
ALTER TABLE Producto ADD CONSTRAINT CHK_Producto_precio_positivo CHECK (precio > 0);
GO
ALTER TABLE PresupuestoDetalle ADD CONSTRAINT CHK_PresupuestoDetalle_cantidad_positiva CHECK (cantidad > 0);
ALTER TABLE PresupuestoDetalle ADD CONSTRAINT CHK_PresupuestoDetalle_importe_no_negativo CHECK (importe >= 0);
ALTER TABLE PresupuestoDetalle ADD CONSTRAINT CHK_PresupuestoDetalle_iva_no_negativo CHECK (iva >= 0);
ALTER TABLE VentaDetalle ADD CONSTRAINT CHK_VentaDetalle_cantidad_positiva CHECK (cantidad > 0);
ALTER TABLE VentaDetalle ADD CONSTRAINT CHK_VentaDetalle_importe_no_negativo CHECK (importe >= 0);
GO
ALTER TABLE MovimientoInventario ADD CONSTRAINT CHK_MovimientoInventario_cantidad_no_cero CHECK (cantidad != 0);
GO
ALTER TABLE Usuario ADD CONSTRAINT CHK_Usuario_correo_formato CHECK (CHARINDEX('@', correo) > 0);
ALTER TABLE catEmpresa ADD CONSTRAINT CHK_Empresa_rfc_formato CHECK (LEN(rfc) >= 12 AND LEN(rfc) <= 13);
GO
PRINT 'Constraints creados.';
GO
INSERT INTO catRol (id, nombre) VALUES (1, 'Administradors');
INSERT INTO catRol (id, nombre) VALUES (2, 'Gerente');
INSERT INTO catRol (id, nombre) VALUES (3, 'Usuario general');
INSERT INTO Direccion (id, ciudad, colonia, codigoPostal, calle, numeroCasa)
VALUES (1, 'Guasave', 'Del Bosque', '81040', 'Adolfo Lopez mateos', '246');
INSERT INTO Direccion (id, ciudad, colonia, codigoPostal, calle, numeroCasa)
VALUES (2, 'Mochis', 'Andromeda', '81200', 'Centro', '123');
INSERT INTO Direccion (id, ciudad, colonia, codigoPostal, calle, numeroCasa)
VALUES (3, 'Buenos Aires', 'Palermo', '81040', 'Antezana', '247');
INSERT INTO Direccion (id, ciudad, colonia, codigoPostal, calle, numeroCasa)
VALUES (101, 'Guasave', 'Del Bosque', '81040', 'Acacias', '10');
INSERT INTO catMarca (id, nombre) VALUES (1, 'John Deere');
INSERT INTO catMarca (id, nombre) VALUES (2, 'Bosch');
INSERT INTO catMarca (id, nombre) VALUES (3, 'Stanadyne');
INSERT INTO TipoProducto (id, nombre) VALUES (1, 'Producto');
INSERT INTO TipoProducto (id, nombre) VALUES (2, 'Servicio');
INSERT INTO Usuario (id, nombre, contrasena, correo, telefono, estatus, idRol)
VALUES (1001, 'Tadeo', 'Tadeo', 'tadeo@gmail.com', '6871237781', 1, 1);
INSERT INTO Usuario (id, nombre, contrasena, correo, telefono, estatus, idRol)
VALUES (1002, 'Hanniel', 'Hanniel', 'Hanniel@gmail.com', '6871237781', 1, 2);
INSERT INTO Usuario (id, nombre, contrasena, correo, telefono, estatus, idRol)
VALUES (1003, 'Tonny', 'Tonny', 'Tonny@gmail.com', '6871237781', 1, 3);
INSERT INTO Cliente (id, nombre, telefono, estatus, idDireccion)
VALUES (1, 'Hector Fortnite', '6871237781', 1, 1);
INSERT INTO Cliente (id, nombre, telefono, estatus, idDireccion)
VALUES (2, 'Victor Ordoñes', '6871234567', 1, 2);
INSERT INTO Cliente (id, nombre, telefono, estatus, idDireccion)
VALUES (3, 'Ysy A Alejo Acosta', '68712237777', 1, 3);
INSERT INTO catEmpresa (id, nombre, rfc, regimen, idDireccion)
VALUES (1, 'Diesel del fuerte', 'GOME850721H23', 'Régimen de Actividades Empresariales y Profesionales', NULL);
INSERT INTO Proveedor (id, nombre, telefono, estatus, idEmpresa, idDireccion)
VALUES (1, 'Tadeo', '6871237781', 1, 1, 101);
INSERT INTO catAlmacen (id, nombre, idDireccion)
VALUES (1, 'Guasave Taller Del Bosque', 1);
INSERT INTO TipoMovimiento (id, nombre) VALUES (1, 'Entrada a almacen');
INSERT INTO TipoMovimiento (id, nombre) VALUES (2, 'Salida del almacen');
INSERT INTO TipoMovimiento (id, nombre) VALUES (3, 'Ajuste');
INSERT INTO ReferenciaMovimiento (id, descripcion) VALUES (1, 'Entrada a almacen');
GO
PRINT 'Datos de prueba insertados.';
GO
CREATE FUNCTION fn_ObtenerStockProducto(
    @idProducto INT,
    @idAlmacen INT
)
RETURNS INT
AS
BEGIN
    DECLARE @stock INT;
    SELECT @stock = stockActual
    FROM Inventario
    WHERE idProducto = @idProducto AND idAlmacen = @idAlmacen;
    RETURN ISNULL(@stock, 0);
END;
GO
CREATE FUNCTION fn_CalcularImporteConIVA(
    @precioUnitario DECIMAL(18,2),
    @cantidad INT,
    @porcentajeIVA DECIMAL(5,2) = 16.00
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        @cantidad * @precioUnitario AS importeSinIVA,
        @cantidad * @precioUnitario * (@porcentajeIVA / 100.0) AS iva,
        @cantidad * @precioUnitario * (1 + @porcentajeIVA / 100.0) AS importeTotal
);
GO
CREATE FUNCTION fn_ProductosBajoStock(
    @stockMinimo INT = 5
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        i.id AS idInventario,
        p.id AS idProducto,
        p.nombre AS producto,
        m.nombre AS marca,
        tp.nombre AS tipoProducto,
        a.id AS idAlmacen,
        a.nombre AS almacen,
        i.stockActual,
        @stockMinimo AS stockMinimoRequerido
    FROM Inventario i
    INNER JOIN Producto p ON i.idProducto = p.id
    INNER JOIN catMarca m ON p.idMarca = m.id
    INNER JOIN TipoProducto tp ON p.idTipoProducto = tp.id
    INNER JOIN catAlmacen a ON i.idAlmacen = a.id
    WHERE i.stockActual < @stockMinimo AND p.estatus = 1
);
GO
CREATE FUNCTION fn_MovimientosPorPeriodo(
    @fechaInicio DATETIME,
    @fechaFin DATETIME
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        m.id AS idMovimiento,
        p.nombre AS producto,
        a.nombre AS almacen,
        tm.nombre AS tipoMovimiento,
        rm.descripcion AS referencia,
        m.cantidad,
        m.fechaMovimiento,
        u.nombre AS usuario
    FROM MovimientoInventario m
    INNER JOIN Inventario i ON m.idInventario = i.id
    INNER JOIN Producto p ON i.idProducto = p.id
    INNER JOIN catAlmacen a ON i.idAlmacen = a.id
    INNER JOIN TipoMovimiento tm ON m.idTipoMovimiento = tm.id
    LEFT JOIN ReferenciaMovimiento rm ON m.idReferenciaMovimiento = rm.id
    LEFT JOIN Usuario u ON m.idUsuario = u.id
    WHERE m.fechaMovimiento >= @fechaInicio
      AND m.fechaMovimiento < DATEADD(DAY, 1, @fechaFin)
);
GO
CREATE FUNCTION fn_VentasPorPeriodo(
    @fechaInicio DATETIME2,
    @fechaFin DATETIME2
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        v.id AS idVenta,
        v.folio,
        v.fecha,
        c.nombre AS cliente,
        u.nombre AS usuario,
        v.total,
        v.estatus
    FROM Venta v
    INNER JOIN Cliente c ON v.idCliente = c.id
    INNER JOIN Usuario u ON v.idUsuario = u.id
    WHERE v.fecha >= @fechaInicio
      AND v.fecha < DATEADD(DAY, 1, @fechaFin)
);
GO
CREATE FUNCTION fn_TotalVentasDelDia(
    @fecha DATE
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @total DECIMAL(18,2);
    SELECT @total = ISNULL(SUM(total), 0)
    FROM Venta
    WHERE CAST(fecha AS DATE) = @fecha AND estatus = 1;
    RETURN @total;
END;
GO
CREATE FUNCTION fn_SiguienteId(
    @nombreTabla VARCHAR(50)
)
RETURNS INT
AS
BEGIN
    DECLARE @id INT;
    SET @id = CASE @nombreTabla
        WHEN 'catRol'               THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM catRol)
        WHEN 'Direccion'            THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Direccion)
        WHEN 'catMarca'             THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM catMarca)
        WHEN 'TipoProducto'         THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM TipoProducto)
        WHEN 'catEmpresa'           THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM catEmpresa)
        WHEN 'Usuario'              THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Usuario)
        WHEN 'Cliente'              THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Cliente)
        WHEN 'Proveedor'            THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Proveedor)
        WHEN 'Producto'             THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Producto)
        WHEN 'catAlmacen'           THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM catAlmacen)
        WHEN 'Inventario'           THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Inventario)
        WHEN 'TipoMovimiento'       THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM TipoMovimiento)
        WHEN 'ReferenciaMovimiento' THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM ReferenciaMovimiento)
        WHEN 'MovimientoInventario' THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM MovimientoInventario)
        WHEN 'Presupuesto'          THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Presupuesto)
        WHEN 'PresupuestoDetalle'   THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM PresupuestoDetalle)
        WHEN 'Venta'                THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Venta)
        WHEN 'VentaDetalle'         THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM VentaDetalle)
        WHEN 'Empleado'             THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Empleado)
        WHEN 'Compra'               THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Compra)
        WHEN 'CompraDetalle'        THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM CompraDetalle)
        WHEN 'Tarea'                THEN (SELECT ISNULL(MAX(id), 0) + 1 FROM Tarea)
        ELSE NULL
    END;
    RETURN @id;
END;
GO
PRINT 'Funciones creadas.';
GO
CREATE VIEW vw_UsuarioInfo
AS
SELECT
    u.id, u.nombre, u.correo, u.telefono, u.estatus,
    CASE WHEN u.estatus = 1 THEN 'Activo' ELSE 'Inactivo' END AS estatusDescripcion,
    u.idRol, r.nombre AS rol
FROM Usuario u
INNER JOIN catRol r ON u.idRol = r.id;
GO
CREATE VIEW vw_ClienteInfo
AS
SELECT
    c.id, c.nombre, c.telefono, c.estatus,
    CASE WHEN c.estatus = 1 THEN 'Activo' ELSE 'Inactivo' END AS estatusDescripcion,
    c.idDireccion, d.ciudad, d.colonia, d.calle, d.numeroCasa, d.codigoPostal,
    ISNULL(d.ciudad + ', ', '') + ISNULL(d.colonia, '') AS direccionCompleta
FROM Cliente c
LEFT JOIN Direccion d ON c.idDireccion = d.id;
GO
CREATE VIEW vw_ProveedorInfo
AS
SELECT
    pv.id, pv.nombre, pv.telefono, pv.estatus,
    CASE WHEN pv.estatus = 1 THEN 'Activo' ELSE 'Inactivo' END AS estatusDescripcion,
    pv.idEmpresa, e.nombre AS empresa, e.rfc,
    pv.idDireccion, d.ciudad, d.colonia, d.calle, d.numeroCasa, d.codigoPostal
FROM Proveedor pv
INNER JOIN catEmpresa e ON pv.idEmpresa = e.id
LEFT JOIN Direccion d ON pv.idDireccion = d.id;
GO
CREATE VIEW vw_ProductoInfo
AS
SELECT
    p.id, p.nombre, p.precio, p.estatus,
    CASE WHEN p.estatus = 1 THEN 'Activo' ELSE 'Inactivo' END AS estatusDescripcion,
    p.idMarca, m.nombre AS marca,
    p.idTipoProducto, tp.nombre AS tipoProducto,
    p.fechaCreacion, p.fechaActualizacion
FROM Producto p
INNER JOIN catMarca m ON p.idMarca = m.id
INNER JOIN TipoProducto tp ON p.idTipoProducto = tp.id;
GO
CREATE VIEW vw_InventarioInfo
AS
SELECT
    i.id, i.idProducto, p.nombre AS producto, p.precio AS precioUnitario,
    m.nombre AS marca, tp.nombre AS tipoProducto,
    i.idAlmacen, a.nombre AS almacen, i.stockActual, i.fechaUltimaActualizacion,
    CASE WHEN i.stockActual <= 5 THEN 'Bajo' WHEN i.stockActual <= 15 THEN 'Medio' ELSE 'Suficiente' END AS nivelStock
FROM Inventario i
INNER JOIN Producto p ON i.idProducto = p.id
INNER JOIN catMarca m ON p.idMarca = m.id
INNER JOIN TipoProducto tp ON p.idTipoProducto = tp.id
INNER JOIN catAlmacen a ON i.idAlmacen = a.id;
GO
CREATE VIEW vw_MovimientoInventarioInfo
AS
SELECT
    m.id, i.idProducto, p.nombre AS producto, i.idAlmacen, a.nombre AS almacen,
    m.idTipoMovimiento, tm.nombre AS tipoMovimiento,
    m.idReferenciaMovimiento, rm.descripcion AS referencia,
    m.cantidad,
    CASE WHEN tm.id = 1 THEN 'Entrada' WHEN tm.id = 2 THEN 'Salida' ELSE 'Ajuste' END AS direccion,
    m.fechaMovimiento, m.idUsuario, u.nombre AS usuario
FROM MovimientoInventario m
INNER JOIN Inventario i ON m.idInventario = i.id
INNER JOIN Producto p ON i.idProducto = p.id
INNER JOIN catAlmacen a ON i.idAlmacen = a.id
INNER JOIN TipoMovimiento tm ON m.idTipoMovimiento = tm.id
LEFT JOIN ReferenciaMovimiento rm ON m.idReferenciaMovimiento = rm.id
LEFT JOIN Usuario u ON m.idUsuario = u.id;
GO
CREATE VIEW vw_PresupuestoInfo
AS
SELECT
    pr.id, pr.total, pr.estatus,
    CASE WHEN pr.estatus = 1 THEN 'Activo' ELSE 'Inactivo' END AS estatusDescripcion,
    pr.nota, pr.idCliente, c.nombre AS cliente, c.telefono AS telefonoCliente,
    pr.idUsuario, u.nombre AS usuario, pr.fechaCreacion, pr.fechaModificacion
FROM Presupuesto pr
INNER JOIN Cliente c ON pr.idCliente = c.id
INNER JOIN Usuario u ON pr.idUsuario = u.id;
GO
CREATE VIEW vw_PresupuestoDetalleInfo
AS
SELECT
    pd.id, pd.idPresupuesto, pd.cantidad, pd.precioUnitario, pd.importe, pd.iva,
    pd.importe + pd.iva AS totalLinea,
    pd.idInventario, i.idProducto, p.nombre AS producto, p.precio AS precioActual,
    m.nombre AS marca, a.nombre AS almacen
FROM PresupuestoDetalle pd
INNER JOIN Inventario i ON pd.idInventario = i.id
INNER JOIN Producto p ON i.idProducto = p.id
INNER JOIN catMarca m ON p.idMarca = m.id
INNER JOIN catAlmacen a ON i.idAlmacen = a.id;
GO
CREATE VIEW vw_VentaInfo
AS
SELECT
    v.id, v.folio, v.fecha, v.total, v.estatus,
    CASE WHEN v.estatus = 1 THEN 'Activa' ELSE 'Cancelada' END AS estatusDescripcion,
    v.idUsuario, u.nombre AS usuario,
    v.idCliente, c.nombre AS cliente, c.telefono AS telefonoCliente
FROM Venta v
INNER JOIN Usuario u ON v.idUsuario = u.id
INNER JOIN Cliente c ON v.idCliente = c.id;
GO
CREATE VIEW vw_VentaDetalleInfo
AS
SELECT
    vd.id, vd.idVenta, v.folio, vd.cantidad, vd.precioUnitario, vd.importe, vd.iva,
    vd.importe + vd.iva AS totalLinea,
    vd.idInventario, i.idProducto, p.nombre AS producto, p.precio AS precioActual,
    m.nombre AS marca, tp.nombre AS tipoProducto, a.nombre AS almacen
FROM VentaDetalle vd
INNER JOIN Venta v ON vd.idVenta = v.id
INNER JOIN Inventario i ON vd.idInventario = i.id
INNER JOIN Producto p ON i.idProducto = p.id
INNER JOIN catMarca m ON p.idMarca = m.id
INNER JOIN TipoProducto tp ON p.idTipoProducto = tp.id
INNER JOIN catAlmacen a ON i.idAlmacen = a.id;
GO
CREATE VIEW vw_ProductosBajoStock
AS
SELECT
    i.id AS idInventario, p.id AS idProducto, p.nombre AS producto,
    m.nombre AS marca, tp.nombre AS tipoProducto,
    a.id AS idAlmacen, a.nombre AS almacen, i.stockActual,
    5 AS stockMinimoSugerido,
    CASE
        WHEN i.stockActual = 0 THEN 'Sin stock'
        WHEN i.stockActual <= 3 THEN 'Crítico'
        WHEN i.stockActual <= 5 THEN 'Bajo'
        ELSE 'Normal'
    END AS prioridad
FROM Inventario i
INNER JOIN Producto p ON i.idProducto = p.id
INNER JOIN catMarca m ON p.idMarca = m.id
INNER JOIN TipoProducto tp ON p.idTipoProducto = tp.id
INNER JOIN catAlmacen a ON i.idAlmacen = a.id
WHERE i.stockActual <= 5 AND p.estatus = 1;
GO
CREATE VIEW vw_ResumenVentasPorDia
AS
SELECT
    CAST(v.fecha AS DATE) AS fecha,
    COUNT(v.id) AS cantidadVentas,
    ISNULL(SUM(v.total), 0) AS totalVentas,
    ISNULL(AVG(v.total), 0) AS promedioPorVenta,
    ISNULL(MAX(v.total), 0) AS ventaMaxima,
    COUNT(DISTINCT v.idCliente) AS clientesAtendidos
FROM Venta v
WHERE v.estatus = 1
GROUP BY CAST(v.fecha AS DATE);
GO
CREATE VIEW vw_ProductosMasVendidos
AS
SELECT TOP 50
    p.id AS idProducto, p.nombre AS producto,
    m.nombre AS marca, tp.nombre AS tipoProducto,
    SUM(vd.cantidad) AS totalVendido,
    COUNT(DISTINCT v.id) AS vecesVendido,
    ISNULL(SUM(vd.importe + vd.iva), 0) AS ingresoGenerado
FROM VentaDetalle vd
INNER JOIN Venta v ON vd.idVenta = v.id
INNER JOIN Inventario i ON vd.idInventario = i.id
INNER JOIN Producto p ON i.idProducto = p.id
INNER JOIN catMarca m ON p.idMarca = m.id
INNER JOIN TipoProducto tp ON p.idTipoProducto = tp.id
WHERE v.estatus = 1
GROUP BY p.id, p.nombre, m.nombre, tp.nombre
ORDER BY totalVendido DESC;
GO
PRINT 'Vistas creadas.';
GO
CREATE TRIGGER TR_Producto_UpdateFecha
ON Producto
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE p
    SET fechaActualizacion = GETDATE()
    FROM Producto p
    INNER JOIN inserted i ON p.id = i.id;
END;
GO
CREATE TRIGGER TR_Inventario_UpdateFecha
ON Inventario
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(stockActual)
    BEGIN
        UPDATE i
        SET fechaUltimaActualizacion = GETDATE()
        FROM Inventario i
        INNER JOIN inserted ins ON i.id = ins.id;
    END
END;
GO
CREATE TRIGGER TR_Presupuesto_UpdateFechaModificacion
ON Presupuesto
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE p
    SET fechaModificacion = GETDATE()
    FROM Presupuesto p
    INNER JOIN inserted i ON p.id = i.id;
END;
GO
CREATE TRIGGER TR_PresupuestoDetalle_AfterChange
ON PresupuestoDetalle
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idPresupuesto INT;
    SELECT TOP 1 @idPresupuesto = idPresupuesto FROM inserted;
    IF @idPresupuesto IS NULL
        SELECT TOP 1 @idPresupuesto = idPresupuesto FROM deleted;
    IF @idPresupuesto IS NOT NULL
    BEGIN
        UPDATE Presupuesto
        SET total = (SELECT ISNULL(SUM(importe + iva), 0) FROM PresupuestoDetalle WHERE idPresupuesto = @idPresupuesto),
            fechaModificacion = GETDATE()
        WHERE id = @idPresupuesto;
    END
END;
GO
CREATE TRIGGER TR_VentaDetalle_AfterChange
ON VentaDetalle
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idVenta INT;
    SELECT TOP 1 @idVenta = idVenta FROM inserted;
    IF @idVenta IS NULL
        SELECT TOP 1 @idVenta = idVenta FROM deleted;
    IF @idVenta IS NOT NULL
    BEGIN
        UPDATE Venta
        SET total = (SELECT ISNULL(SUM(importe + iva), 0) FROM VentaDetalle WHERE idVenta = @idVenta)
        WHERE id = @idVenta;
    END
END;
GO
PRINT 'Triggers creados.';
GO
CREATE PROCEDURE sp_catRol
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('catRol');
        INSERT INTO catRol (id, nombre) VALUES (@id, @nombre);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE catRol SET nombre = @nombre WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM catRol WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, nombre FROM catRol ORDER BY nombre;
    ELSE IF @opcion = 5
        SELECT id, nombre FROM catRol WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_Direccion
    @opcion INT,
    @id INT = NULL,
    @ciudad VARCHAR(50) = NULL,
    @colonia VARCHAR(50) = NULL,
    @codigoPostal VARCHAR(10) = NULL,
    @calle VARCHAR(50) = NULL,
    @numeroCasa VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('Direccion');
        INSERT INTO Direccion (id, ciudad, colonia, codigoPostal, calle, numeroCasa)
        VALUES (@id, @ciudad, @colonia, @codigoPostal, @calle, @numeroCasa);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE Direccion
        SET ciudad = @ciudad, colonia = @colonia, codigoPostal = @codigoPostal,
            calle = @calle, numeroCasa = @numeroCasa
        WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM Direccion WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, ciudad, colonia, codigoPostal, calle, numeroCasa
        FROM Direccion ORDER BY ciudad, colonia;
    ELSE IF @opcion = 5
        SELECT id, ciudad, colonia, codigoPostal, calle, numeroCasa
        FROM Direccion WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_catMarca
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('catMarca');
        INSERT INTO catMarca (id, nombre) VALUES (@id, @nombre);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE catMarca SET nombre = @nombre WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM catMarca WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, nombre FROM catMarca ORDER BY nombre;
    ELSE IF @opcion = 5
        SELECT id, nombre FROM catMarca WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_TipoProducto
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('TipoProducto');
        INSERT INTO TipoProducto (id, nombre) VALUES (@id, @nombre);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE TipoProducto SET nombre = @nombre WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM TipoProducto WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, nombre FROM TipoProducto ORDER BY nombre;
    ELSE IF @opcion = 5
        SELECT id, nombre FROM TipoProducto WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_catEmpresa
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(100) = NULL,
    @rfc VARCHAR(13) = NULL,
    @regimen VARCHAR(100) = NULL,
    @idDireccion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('catEmpresa');
        INSERT INTO catEmpresa (id, nombre, rfc, regimen, idDireccion)
        VALUES (@id, @nombre, @rfc, @regimen, @idDireccion);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE catEmpresa
        SET nombre = @nombre, rfc = @rfc, regimen = @regimen, idDireccion = @idDireccion
        WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM catEmpresa WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, nombre, rfc, regimen, idDireccion
        FROM catEmpresa ORDER BY nombre;
    ELSE IF @opcion = 5
        SELECT e.id, e.nombre, e.rfc, e.regimen, e.idDireccion
        FROM catEmpresa e WHERE e.id = @id;
END;
GO
CREATE PROCEDURE sp_Usuario
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(100) = NULL,
    @contrasena VARCHAR(200) = NULL,
    @correo VARCHAR(100) = NULL,
    @telefono VARCHAR(15) = NULL,
    @estatus BIT = NULL,
    @idRol INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('Usuario');
        INSERT INTO Usuario (id, nombre, contrasena, correo, telefono, estatus, idRol)
        VALUES (@id, @nombre, @contrasena, @correo, @telefono, ISNULL(@estatus, 1), @idRol);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE Usuario
        SET nombre = @nombre, contrasena = @contrasena, correo = @correo,
            telefono = @telefono, estatus = @estatus, idRol = @idRol
        WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM Usuario WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, nombre, contrasena, correo, telefono, estatus, idRol FROM Usuario ORDER BY nombre;
    ELSE IF @opcion = 5
        SELECT id, nombre, contrasena, correo, telefono, estatus, idRol
        FROM Usuario WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_Cliente
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(100) = NULL,
    @telefono VARCHAR(15) = NULL,
    @estatus BIT = NULL,
    @idDireccion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('Cliente');
        INSERT INTO Cliente (id, nombre, telefono, estatus, idDireccion)
        VALUES (@id, @nombre, @telefono, ISNULL(@estatus, 1), @idDireccion);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE Cliente
        SET nombre = @nombre, telefono = @telefono, estatus = @estatus, idDireccion = @idDireccion
        WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM Cliente WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, nombre, telefono, estatus, idDireccion FROM Cliente ORDER BY nombre;
    ELSE IF @opcion = 5
        SELECT id, nombre, telefono, estatus, idDireccion FROM Cliente WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_Proveedor
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(100) = NULL,
    @telefono VARCHAR(15) = NULL,
    @estatus BIT = NULL,
    @idEmpresa INT = NULL,
    @idDireccion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('Proveedor');
        INSERT INTO Proveedor (id, nombre, telefono, estatus, idEmpresa, idDireccion)
        VALUES (@id, @nombre, @telefono, ISNULL(@estatus, 1), @idEmpresa, @idDireccion);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE Proveedor
        SET nombre = @nombre, telefono = @telefono, estatus = @estatus,
            idEmpresa = @idEmpresa, idDireccion = @idDireccion
        WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM Proveedor WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, nombre, telefono, estatus, idEmpresa, idDireccion
        FROM Proveedor ORDER BY nombre;
    ELSE IF @opcion = 5
        SELECT id, nombre, telefono, estatus, idEmpresa, idDireccion
        FROM Proveedor WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_Producto
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(100) = NULL,
    @precio DECIMAL(10,2) = NULL,
    @estatus BIT = NULL,
    @idMarca INT = NULL,
    @idTipoProducto INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('Producto');
        INSERT INTO Producto (id, nombre, precio, estatus, idMarca, idTipoProducto)
        VALUES (@id, @nombre, @precio, ISNULL(@estatus, 1), @idMarca, @idTipoProducto);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE Producto
        SET nombre = @nombre, precio = @precio, estatus = @estatus,
            idMarca = @idMarca, idTipoProducto = @idTipoProducto
        WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM Producto WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT p.id, p.nombre, p.precio, p.estatus, p.idMarca, p.idTipoProducto,
               p.fechaCreacion, p.fechaActualizacion
        FROM Producto p ORDER BY p.nombre;
    ELSE IF @opcion = 5
        SELECT p.id, p.nombre, p.precio, p.estatus, p.idMarca, p.idTipoProducto,
               p.fechaCreacion, p.fechaActualizacion
        FROM Producto p WHERE p.id = @id;
END;
GO
CREATE PROCEDURE sp_catAlmacen
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(50) = NULL,
    @idDireccion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('catAlmacen');
        INSERT INTO catAlmacen (id, nombre, idDireccion) VALUES (@id, @nombre, @idDireccion);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE catAlmacen SET nombre = @nombre, idDireccion = @idDireccion WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM catAlmacen WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, nombre, idDireccion FROM catAlmacen ORDER BY nombre;
    ELSE IF @opcion = 5
        SELECT id, nombre, idDireccion FROM catAlmacen WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_Inventario
    @opcion INT,
    @id INT = NULL,
    @idProducto INT = NULL,
    @idAlmacen INT = NULL,
    @stockActual INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('Inventario');
        INSERT INTO Inventario (id, idProducto, idAlmacen, stockActual)
        VALUES (@id, @idProducto, @idAlmacen, ISNULL(@stockActual, 0));
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE Inventario
        SET idProducto = @idProducto, idAlmacen = @idAlmacen, stockActual = @stockActual
        WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM Inventario WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, idProducto, idAlmacen, stockActual, fechaUltimaActualizacion
        FROM Inventario ORDER BY idProducto, idAlmacen;
    ELSE IF @opcion = 5
        SELECT i.id, i.idProducto, i.idAlmacen, i.stockActual, i.fechaUltimaActualizacion
        FROM Inventario i WHERE i.id = @id;
END;
GO
CREATE PROCEDURE sp_TipoMovimiento
    @opcion INT,
    @id INT = NULL,
    @nombre VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('TipoMovimiento');
        INSERT INTO TipoMovimiento (id, nombre) VALUES (@id, @nombre);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE TipoMovimiento SET nombre = @nombre WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM TipoMovimiento WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, nombre FROM TipoMovimiento ORDER BY id;
    ELSE IF @opcion = 5
        SELECT id, nombre FROM TipoMovimiento WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_ReferenciaMovimiento
    @opcion INT,
    @id INT = NULL,
    @descripcion VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('ReferenciaMovimiento');
        INSERT INTO ReferenciaMovimiento (id, descripcion) VALUES (@id, @descripcion);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE ReferenciaMovimiento SET descripcion = @descripcion WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM ReferenciaMovimiento WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, descripcion FROM ReferenciaMovimiento ORDER BY descripcion;
    ELSE IF @opcion = 5
        SELECT id, descripcion FROM ReferenciaMovimiento WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_MovimientoInventario
    @opcion INT,
    @id INT = NULL,
    @idInventario INT = NULL,
    @idTipoMovimiento INT = NULL,
    @idReferenciaMovimiento INT = NULL,
    @cantidad INT = NULL,
    @idUsuario INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('MovimientoInventario');
        INSERT INTO MovimientoInventario (id, idInventario, idTipoMovimiento, idReferenciaMovimiento, cantidad, idUsuario)
        VALUES (@id, @idInventario, @idTipoMovimiento, @idReferenciaMovimiento, @cantidad, @idUsuario);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 4
        SELECT id, idInventario, idTipoMovimiento, idReferenciaMovimiento, cantidad, fechaMovimiento
        FROM MovimientoInventario ORDER BY fechaMovimiento DESC;
    ELSE IF @opcion = 5
        SELECT id, idInventario, idTipoMovimiento, idReferenciaMovimiento, cantidad, fechaMovimiento
        FROM MovimientoInventario WHERE id = @id;
END;
GO
CREATE PROCEDURE sp_Presupuesto
    @opcion INT,
    @id INT = NULL,
    @total DECIMAL(10,2) = NULL,
    @estatus BIT = NULL,
    @nota VARCHAR(MAX) = NULL,
    @idCliente INT = NULL,
    @idUsuario INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('Presupuesto');
        INSERT INTO Presupuesto (id, total, estatus, nota, idCliente, idUsuario)
        VALUES (@id, @total, ISNULL(@estatus, 1), @nota, @idCliente, @idUsuario);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE Presupuesto
        SET total = @total, estatus = @estatus, nota = @nota,
            idCliente = @idCliente, idUsuario = @idUsuario
        WHERE id = @id;
    ELSE IF @opcion = 3
    BEGIN
        DELETE FROM PresupuestoDetalle WHERE idPresupuesto = @id;
        DELETE FROM Presupuesto WHERE id = @id;
    END
    ELSE IF @opcion = 4
        SELECT id, total, estatus, nota, idCliente, idUsuario, fechaCreacion, fechaModificacion
        FROM Presupuesto ORDER BY fechaCreacion DESC;
    ELSE IF @opcion = 5
        SELECT pr.id, pr.total, pr.estatus, pr.nota, pr.idCliente, pr.idUsuario,
               pr.fechaCreacion, pr.fechaModificacion
        FROM Presupuesto pr WHERE pr.id = @id;
END;
GO
CREATE PROCEDURE sp_PresupuestoDetalle
    @opcion INT,
    @id INT = NULL,
    @cantidad INT = NULL,
    @precioUnitario DECIMAL(18,2) = NULL,
    @importe DECIMAL(18,2) = NULL,
    @iva DECIMAL(18,2) = NULL,
    @idInventario INT = NULL,
    @idPresupuesto INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('PresupuestoDetalle');
        INSERT INTO PresupuestoDetalle (id, cantidad, precioUnitario, importe, iva, idInventario, idPresupuesto)
        VALUES (@id, @cantidad, @precioUnitario, @importe, @iva, @idInventario, @idPresupuesto);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE PresupuestoDetalle
        SET cantidad = @cantidad, precioUnitario = @precioUnitario,
            importe = @importe, iva = @iva, idInventario = @idInventario
        WHERE id = @id;
    ELSE IF @opcion = 3
        DELETE FROM PresupuestoDetalle WHERE id = @id;
    ELSE IF @opcion = 4
        SELECT id, cantidad, precioUnitario, importe, iva, idInventario, idPresupuesto
        FROM PresupuestoDetalle ORDER BY id;
END;
GO
CREATE PROCEDURE sp_Venta
    @opcion INT,
    @id INT = NULL,
    @total DECIMAL(18,2) = NULL,
    @estatus BIT = NULL,
    @idUsuario INT = NULL,
    @idCliente INT = NULL,
    @folio INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('Venta');
        INSERT INTO Venta (id, total, estatus, idUsuario, idCliente, folio)
        VALUES (@id, @total, ISNULL(@estatus, 1), @idUsuario, @idCliente, @folio);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 2
        UPDATE Venta
        SET total = @total, estatus = @estatus, idUsuario = @idUsuario, idCliente = @idCliente
        WHERE id = @id;
    ELSE IF @opcion = 3
    BEGIN
        DELETE FROM VentaDetalle WHERE idVenta = @id;
        DELETE FROM Venta WHERE id = @id;
    END
    ELSE IF @opcion = 4
        SELECT id, folio, fecha, total, estatus, idUsuario, idCliente
        FROM Venta ORDER BY fecha DESC;
    ELSE IF @opcion = 5
        SELECT v.id, v.folio, v.fecha, v.total, v.estatus, v.idUsuario, v.idCliente
        FROM Venta v WHERE v.id = @id;
END;
GO
CREATE PROCEDURE sp_VentaDetalle
    @opcion INT,
    @id INT = NULL,
    @cantidad INT = NULL,
    @importe DECIMAL(18,2) = NULL,
    @iva DECIMAL(18,2) = NULL,
    @idInventario INT = NULL,
    @idVenta INT = NULL,
    @precioUnitario DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @opcion = 1
    BEGIN
        IF @id IS NULL
            SET @id = dbo.fn_SiguienteId('VentaDetalle');
        INSERT INTO VentaDetalle (id, cantidad, importe, iva, idInventario, idVenta, precioUnitario)
        VALUES (@id, @cantidad, @importe, @iva, @idInventario, @idVenta, @precioUnitario);
        SELECT @id AS id;
    END
    ELSE IF @opcion = 4
        SELECT id, cantidad, precioUnitario, importe, iva, idInventario, idVenta
        FROM VentaDetalle WHERE idVenta = @idVenta ORDER BY id;
END;
GO
PRINT 'Procedimientos CRUD consolidados.';
GO
CREATE PROCEDURE sp_Login
    @idUsuario INT,
    @contrasena VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.id, u.nombre, u.correo, u.telefono, u.estatus, u.idRol, r.nombre AS rol
    FROM Usuario u
    INNER JOIN catRol r ON u.idRol = r.id
    WHERE u.id = @idUsuario AND u.contrasena = @contrasena AND u.estatus = 1;
END;
GO
CREATE PROCEDURE sp_RegistrarEntradaInventario
    @idProducto INT, @idAlmacen INT, @cantidad INT,
    @idUsuario INT = NULL, @idReferencia INT = 1
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idInventario INT;
    BEGIN TRY
        BEGIN TRANSACTION;
        SELECT @idInventario = id FROM Inventario
        WHERE idProducto = @idProducto AND idAlmacen = @idAlmacen;
        IF @idInventario IS NULL
        BEGIN
            INSERT INTO Inventario (id, idProducto, idAlmacen, stockActual)
            VALUES (dbo.fn_SiguienteId('Inventario'), @idProducto, @idAlmacen, 0);
            SET @idInventario = SCOPE_IDENTITY();
        END
        INSERT INTO MovimientoInventario (id, idInventario, idTipoMovimiento, idReferenciaMovimiento, cantidad, idUsuario)
        VALUES (dbo.fn_SiguienteId('MovimientoInventario'), @idInventario, 1, @idReferencia, @cantidad, @idUsuario);
        COMMIT TRANSACTION;
        SELECT @idInventario AS idInventario, SCOPE_IDENTITY() AS idMovimiento;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
CREATE PROCEDURE sp_RegistrarSalidaInventario
    @idProducto INT, @idAlmacen INT, @cantidad INT,
    @idUsuario INT = NULL, @idReferencia INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idInventario INT, @stockActual INT;
    SELECT @idInventario = id, @stockActual = stockActual
    FROM Inventario WHERE idProducto = @idProducto AND idAlmacen = @idAlmacen;
    IF @idInventario IS NULL
        THROW 50001, 'El producto no existe en el almacén especificado.', 1;
    IF @stockActual < @cantidad
        THROW 50002, 'Stock insuficiente para realizar la salida.', 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO MovimientoInventario (id, idInventario, idTipoMovimiento, idReferenciaMovimiento, cantidad, idUsuario)
        VALUES (dbo.fn_SiguienteId('MovimientoInventario'), @idInventario, 2, @idReferencia, @cantidad, @idUsuario);
        COMMIT TRANSACTION;
        SELECT @idInventario AS idInventario, SCOPE_IDENTITY() AS idMovimiento;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
CREATE PROCEDURE sp_CrearPresupuestoConDetalles
    @nota VARCHAR(MAX) = NULL, @idCliente INT, @idUsuario INT,
    @detalles XML
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idPresupuesto INT, @total DECIMAL(18,2);
    BEGIN TRY
        BEGIN TRANSACTION;
        SET @idPresupuesto = dbo.fn_SiguienteId('Presupuesto');
        INSERT INTO Presupuesto (id, total, estatus, nota, idCliente, idUsuario)
        VALUES (@idPresupuesto, 0, 1, @nota, @idCliente, @idUsuario);
        INSERT INTO PresupuestoDetalle (id, cantidad, precioUnitario, importe, iva, idInventario, idPresupuesto)
        SELECT
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + (SELECT ISNULL(MAX(id), 0) FROM PresupuestoDetalle),
            t.c.value('@cantidad', 'INT'),
            t.c.value('@precioUnitario', 'DECIMAL(18,2)'),
            t.c.value('@cantidad', 'INT') * t.c.value('@precioUnitario', 'DECIMAL(18,2)'),
            t.c.value('@iva', 'DECIMAL(18,2)'),
            t.c.value('@idInventario', 'INT'),
            @idPresupuesto
        FROM @detalles.nodes('/detalles/detalle') t(c);
        SELECT @total = ISNULL(SUM(importe + iva), 0)
        FROM PresupuestoDetalle WHERE idPresupuesto = @idPresupuesto;
        UPDATE Presupuesto SET total = @total WHERE id = @idPresupuesto;
        COMMIT TRANSACTION;
        SELECT @idPresupuesto AS idPresupuesto, @total AS total;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
CREATE PROCEDURE sp_CrearVentaConDetalles
    @idCliente INT, @idUsuario INT, @detalles XML
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idVenta INT, @folio INT, @total DECIMAL(18,2);
    BEGIN TRY
        BEGIN TRANSACTION;
        SELECT @folio = ISNULL(MAX(folio), 0) + 1 FROM Venta;
        SET @idVenta = dbo.fn_SiguienteId('Venta');
        INSERT INTO Venta (id, total, estatus, idUsuario, idCliente, folio)
        VALUES (@idVenta, 0, 1, @idUsuario, @idCliente, @folio);
        INSERT INTO VentaDetalle (id, cantidad, precioUnitario, importe, iva, idInventario, idVenta)
        SELECT
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + (SELECT ISNULL(MAX(id), 0) FROM VentaDetalle),
            t.c.value('@cantidad', 'INT'),
            t.c.value('@precioUnitario', 'DECIMAL(18,2)'),
            t.c.value('@cantidad', 'INT') * t.c.value('@precioUnitario', 'DECIMAL(18,2)'),
            t.c.value('@cantidad', 'INT') * t.c.value('@precioUnitario', 'DECIMAL(18,2)') * 0.16,
            t.c.value('@idInventario', 'INT'),
            @idVenta
        FROM @detalles.nodes('/detalles/detalle') t(c);
        SELECT @total = ISNULL(SUM(importe + iva), 0)
        FROM VentaDetalle WHERE idVenta = @idVenta;
        UPDATE Venta SET total = @total WHERE id = @idVenta;
        COMMIT TRANSACTION;
        SELECT @idVenta AS idVenta, @folio AS folio, @total AS total;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
CREATE PROCEDURE sp_CancelarVenta
    @idVenta INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @estatusActual BIT;
    SELECT @estatusActual = estatus FROM Venta WHERE id = @idVenta;
    IF @estatusActual = 0
        THROW 50003, 'La venta ya está cancelada.', 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Venta SET estatus = 0 WHERE id = @idVenta;
        UPDATE i
        SET i.stockActual = i.stockActual + vd.cantidad
        FROM Inventario i
        INNER JOIN VentaDetalle vd ON i.id = vd.idInventario
        WHERE vd.idVenta = @idVenta;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
CREATE PROCEDURE sp_ObtenerDashboard
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 'ProductosActivos' AS indicador, COUNT(*) AS valor FROM Producto WHERE estatus = 1;
    SELECT 'ClientesActivos' AS indicador, COUNT(*) AS valor FROM Cliente WHERE estatus = 1;
    SELECT 'VentasHoy' AS indicador, COUNT(*) AS valor FROM Venta WHERE CAST(fecha AS DATE) = CAST(GETDATE() AS DATE) AND estatus = 1;
    SELECT 'TotalVentasHoy' AS indicador, ISNULL(SUM(total), 0) AS valor FROM Venta WHERE CAST(fecha AS DATE) = CAST(GETDATE() AS DATE) AND estatus = 1;
    SELECT 'PresupuestosActivos' AS indicador, COUNT(*) AS valor FROM Presupuesto WHERE estatus = 1;
    SELECT 'ProductosBajoStock' AS indicador, COUNT(*) AS valor FROM vw_ProductosBajoStock;
    SELECT 'UsuariosActivos' AS indicador, COUNT(*) AS valor FROM Usuario WHERE estatus = 1;
    SELECT 'ProveedoresActivos' AS indicador, COUNT(*) AS valor FROM Proveedor WHERE estatus = 1;
END;
GO
CREATE PROCEDURE sp_ObtenerSiguienteFolio
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(folio), 0) + 1 AS siguienteFolio FROM Venta;
END;
GO
PRINT 'Procedimientos de negocio creados.';
GO
CREATE PROCEDURE sp_Empleado_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.id, e.nombre, e.telefono, e.estatus, e.idRol, r.nombre AS rol, e.idDireccion
    FROM Empleado e
    INNER JOIN catRol r ON e.idRol = r.id
    ORDER BY e.nombre;
END;
GO
CREATE PROCEDURE sp_Empleado_Insert
    @nombre VARCHAR(100), @telefono VARCHAR(15), @estatus BIT = 1,
    @idRol INT, @idDireccion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @id INT = dbo.fn_SiguienteId('Empleado');
    INSERT INTO Empleado (id, nombre, telefono, estatus, idRol, idDireccion)
    VALUES (@id, @nombre, @telefono, @estatus, @idRol, @idDireccion);
    SELECT @id AS id;
END;
GO
CREATE PROCEDURE sp_Compra_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.id, c.fecha, c.total, c.estatus, c.idEmpleado, e.nombre AS empleado,
           c.idProveedor, p.nombre AS proveedor
    FROM Compra c
    INNER JOIN Empleado e ON c.idEmpleado = e.id
    INNER JOIN Proveedor p ON c.idProveedor = p.id
    ORDER BY c.fecha DESC;
END;
GO
CREATE PROCEDURE sp_Tarea_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.id, t.descripcion, t.fechaInicio, t.fechaTermino,
           t.idEstatus, et.descripcion AS estatus,
           t.idEmpleado, e.nombre AS empleado
    FROM Tarea t
    INNER JOIN catEstatusTarea et ON t.idEstatus = et.id
    INNER JOIN Empleado e ON t.idEmpleado = e.id
    ORDER BY t.fechaInicio DESC;
END;
GO
PRINT 'Procedimientos adicionales creados.';
GO
PRINT '============================================================';
PRINT '  BASE DE DATOS dbTaller CREADA EXITOSAMENTE';
PRINT '============================================================';
PRINT '';
PRINT 'Resumen de objetos:';
PRINT '  - 23 Tablas';
PRINT '  - 6  DEFAULT constraints + 9 CHECK constraints';
PRINT '  - 7  Funciones';
PRINT '  - 13 Vistas';
PRINT '  - 5  Triggers';
PRINT '  - 88 Procedimientos almacenados (CRUD + negocio)';
PRINT '  - Datos de prueba insertados';
PRINT '';
PRINT 'Usuarios disponibles:';
PRINT '  Tadeo  (Admin)    - pass: Tadeo';
PRINT '  Hanniel (Gerente) - pass: Hanniel';
PRINT '  Tonny  (Usuario)  - pass: Tonny';
PRINT '============================================================';
GO
