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

-- Crear la tabla alerta con relación a lecturas
CREATE TABLE alerta (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(30),
    lectura_id INT,
    FOREIGN KEY (lectura_id) REFERENCES lecturas(id)
);
