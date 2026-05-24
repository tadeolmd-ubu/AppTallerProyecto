-- ============================================================
-- Script completo: dbTaller
-- CREADO A PARTIR DEL HISTORIAL DE CAMBIOS DEL REPO ORIGINAL
-- ============================================================

-- Crear la base de datos
CREATE DATABASE dbTaller;
GO

USE dbTaller;
GO

-- ============================================================
-- 1. TABLAS (esquema final = después de todos los ALTER)
-- ============================================================

-- catRol
CREATE TABLE catRol(
    id INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);

-- Direccion
CREATE TABLE Direccion(
    id INT PRIMARY KEY,
    ciudad VARCHAR(50) NOT NULL,
    colonia VARCHAR(50) NOT NULL,
    codigoPostal VARCHAR(10) NOT NULL,
    calle VARCHAR(50) NOT NULL,
    numeroCasa VARCHAR(10) NOT NULL
);

-- catMarca
CREATE TABLE catMarca(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE
);

-- TipoProducto
CREATE TABLE TipoProducto(
    id INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);

-- catEmpresa
CREATE TABLE catEmpresa(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    rfc VARCHAR(13) NOT NULL UNIQUE,
    regimen VARCHAR(100) NOT NULL,
    idDireccion INT NULL,
    FOREIGN KEY(idDireccion) REFERENCES Direccion(id)
);

-- Usuario
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

-- Cliente
CREATE TABLE Cliente(
    id INT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(15) NOT NULL,
    estatus BIT NOT NULL,
    idDireccion INT NULL,
    FOREIGN KEY(idDireccion) REFERENCES Direccion(id)
);

-- Proveedor
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

-- Producto (con fechaCreacion y fechaActualizacion)
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

-- catAlmacen (idDireccion acepta NULL)
CREATE TABLE catAlmacen(
    id INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    idDireccion INT NULL,
    FOREIGN KEY (idDireccion) REFERENCES Direccion(id)
);

-- Inventario
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

-- TipoMovimiento
CREATE TABLE TipoMovimiento(
    id INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);

-- ReferenciaMovimiento
CREATE TABLE ReferenciaMovimiento(
    id INT PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL UNIQUE
);

-- MovimientoInventario (con idUsuario NULL)
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

-- Presupuesto (con idUsuario, nota, fechas)
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

-- PresupuestoDetalle (con idInventario, precioUnitario)
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

-- Venta (con idUsuario, folio)
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

-- VentaDetalle (con idInventario, precioUnitario)
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

-- ============================================================
-- 2. TABLAS ADICIONALES DEL SCRIPT ORIGINAL (no usadas por la app)
-- ============================================================

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

-- ============================================================
-- 3. TRIGGER: Producto → fechaActualizacion automática
-- ============================================================

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

-- ============================================================
-- 4. DATOS DE PRUEBA
-- ============================================================

-- catRol
INSERT INTO catRol (id, nombre) VALUES (1, 'Administradors');
INSERT INTO catRol (id, nombre) VALUES (2, 'Gerente');
INSERT INTO catRol (id, nombre) VALUES (3, 'Usuario general');

-- Direccion
INSERT INTO Direccion (id, ciudad, colonia, codigoPostal, calle, numeroCasa)
VALUES (1, 'Guasave', 'Del Bosque', '81040', 'Adolfo Lopez mateos', '246');
INSERT INTO Direccion (id, ciudad, colonia, codigoPostal, calle, numeroCasa)
VALUES (2, 'Mochis', 'Andromeda', '81200', 'Centro', '123');
INSERT INTO Direccion (id, ciudad, colonia, codigoPostal, calle, numeroCasa)
VALUES (3, 'Buenos Aires', 'Palermo', '81040', 'Antezana', '247');
INSERT INTO Direccion (id, ciudad, colonia, codigoPostal, calle, numeroCasa)
VALUES (101, 'Guasave', 'Del Bosque', '81040', 'Acacias', '10');

-- catMarca
INSERT INTO catMarca (id, nombre) VALUES (1, 'John Deere');
INSERT INTO catMarca (id, nombre) VALUES (2, 'Bosch');
INSERT INTO catMarca (id, nombre) VALUES (3, 'Stanadyne');

-- TipoProducto
INSERT INTO TipoProducto (id, nombre) VALUES (1, 'Producto');
INSERT INTO TipoProducto (id, nombre) VALUES (2, 'Servicio');

-- Usuario (passwords en texto plano, igual que en la app)
INSERT INTO Usuario (id, nombre, contrasena, correo, telefono, estatus, idRol)
VALUES (1001, 'Tadeo', 'Tadeo', 'tadeo@gmail.com', '6871237781', 1, 1);
INSERT INTO Usuario (id, nombre, contrasena, correo, telefono, estatus, idRol)
VALUES (1002, 'Hanniel', 'Hanniel', 'Hanniel@gmail.com', '6871237781', 1, 2);
INSERT INTO Usuario (id, nombre, contrasena, correo, telefono, estatus, idRol)
VALUES (1003, 'Tonny', 'Tonny', 'Tonny@gmail.com', '6871237781', 1, 3);

-- Cliente
INSERT INTO Cliente (id, nombre, telefono, estatus, idDireccion)
VALUES (1, 'Hector Fortnite', '6871237781', 1, 1);
INSERT INTO Cliente (id, nombre, telefono, estatus, idDireccion)
VALUES (2, 'Victor Ordoñes', '6871234567', 1, 2);
INSERT INTO Cliente (id, nombre, telefono, estatus, idDireccion)
VALUES (3, 'Ysy A Alejo Acosta', '68712237777', 1, 3);

-- catEmpresa
INSERT INTO catEmpresa (id, nombre, rfc, regimen, idDireccion)
VALUES (1, 'Diesel del fuerte', 'GOME850721H23', 'Régimen de Actividades Empresariales y Profesionales', NULL);

-- Proveedor
INSERT INTO Proveedor (id, nombre, telefono, estatus, idEmpresa, idDireccion)
VALUES (1, 'Tadeo', '6871237781', 1, 1, 101);

-- catAlmacen
INSERT INTO catAlmacen (id, nombre, idDireccion)
VALUES (1, 'Guasave Taller Del Bosque', 1);

-- TipoMovimiento
INSERT INTO TipoMovimiento (id, nombre) VALUES (1, 'Entrada a almacen');
INSERT INTO TipoMovimiento (id, nombre) VALUES (2, 'Salida del almacen');
INSERT INTO TipoMovimiento (id, nombre) VALUES (3, 'Ajuste');

-- ReferenciaMovimiento
INSERT INTO ReferenciaMovimiento (id, descripcion) VALUES (1, 'Entrada a almacen');

GO

PRINT 'Base de datos dbTaller creada correctamente.';
PRINT 'Usuarios disponibles:';
PRINT '  Tadeo  (Admin)    - pass: Tadeo';
PRINT '  Hanniel (Gerente) - pass: Hanniel';
PRINT '  Tonny  (Usuario)  - pass: Tonny';
GO
