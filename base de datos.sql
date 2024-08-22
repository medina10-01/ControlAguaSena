-- Eliminar la base de datos si existe y volver a crearla
DROP DATABASE IF EXISTS pecessena;
CREATE DATABASE pecessena;
USE pecessena;

-- Crear la tabla broker
CREATE TABLE broker (
    id INT AUTO_INCREMENT PRIMARY KEY,
    host VARCHAR(50) NOT NULL,
    puerto VARCHAR(50) NOT NULL,
    topico_ox VARCHAR(50) NOT NULL,
    topico_tem VARCHAR(50) NOT NULL
);

-- Crear la tabla lecturas
CREATE TABLE lecturas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    oxigeno DOUBLE NOT NULL,
    temperatura DOUBLE NOT NULL,
    fecha DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Crear la tabla users
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    mail VARCHAR(50) NOT NULL,
    cellPhone VARCHAR(50),
    UserImage VARCHAR(255)
);

-- Crear la tabla lagos
CREATE TABLE lagos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    largo VARCHAR(50) NOT NULL,
    ancho VARCHAR(50) NOT NULL,
    area VARCHAR(50) NOT NULL,
    profundidad VARCHAR(50) NOT NULL,
    PesEstimados VARCHAR(50) NOT NULL
);

-- Crear la tabla cosecha con estado añadido
CREATE TABLE cosecha (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha_inicio DATE NOT NULL,
    densidad_ciembra VARCHAR(50) NOT NULL,
    especie VARCHAR(50) NOT NULL,
    fecha_fin_estimada DATE NOT NULL,
    lago_id INT,
    estado ENUM('Activo', 'Cosechado') NOT NULL,
    FOREIGN KEY (lago_id) REFERENCES lagos(id)
);

-- Crear la tabla fin_cosecha
CREATE TABLE fin_cosecha (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cosecha_id INT NOT NULL,
    produccionKG DOUBLE NOT NULL,
    valor_venta DOUBLE NOT NULL,
    totalProduccion DOUBLE NOT NULL,
    fecha_fin DATE NOT NULL,
    FOREIGN KEY (cosecha_id) REFERENCES cosecha(id)
);


-- Insertar datos en la tabla lecturas
INSERT INTO lecturas (oxigeno, temperatura) VALUES 
(8.5, 22.3),
(7.8, 23.1),
(9.0, 21.7),
(8.3, 22.8),
(7.9, 23.0);

-- Insertar datos en la tabla users
INSERT INTO users (name, mail, cellPhone, UserImage) VALUES 
('Juan Perez', 'juan.perez@example.com', '1234567890', 'juan.png'),
('Maria Lopez', 'maria.lopez@example.com', '0987654321', 'maria.png'),
('Carlos Diaz', 'carlos.diaz@example.com', '1122334455', 'carlos.png'),
('Ana Gomez', 'ana.gomez@example.com', '5566778899', 'ana.png'),
('Luis Martinez', 'luis.martinez@example.com', '6677889900', 'luis.png');

-- Insertar datos en la tabla lagos
INSERT INTO lagos (name, largo, ancho, area, profundidad, PesEstimados) VALUES 
('Lago1', '100', '50', '5000', '10', '1000'),
('Lago2', '120', '60', '7200', '12', '1200'),
('Lago3', '90', '45', '4050', '8', '800'),
('Lago4', '110', '55', '6050', '9', '900'),
('Lago5', '95', '47', '4465', '7', '700');

-- Insertar datos en la tabla cosecha
INSERT INTO cosecha (fecha_inicio, densidad_ciembra, especie, fecha_fin_estimada, lago_id, estado) VALUES 
('2024-01-01', '50 por metro cuadrado', 'Tilapia', '2024-06-01', 1, 'Activo'),
('2024-02-15', '60 por metro cuadrado', 'Carpa', '2024-07-15', 2, 'Activo'),
('2024-03-10', '55 por metro cuadrado', 'Trucha', '2024-08-10', 3, 'Activo'),
('2024-04-05', '70 por metro cuadrado', 'Bagre', '2024-09-05', 4, 'Activo'),
('2024-05-20', '65 por metro cuadrado', 'Pargo', '2024-10-20', 5, 'Activo');
