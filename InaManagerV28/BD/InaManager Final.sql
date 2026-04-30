CREATE DATABASE IF NOT EXISTS InaManager;
USE InaManager;

-- ==========================================
-- 1. LIMPIEZA (Orden inverso a las dependencias)
-- ==========================================
DROP TABLE IF EXISTS Mercado;
DROP TABLE IF EXISTS Fichajes;
DROP TABLE IF EXISTS Transacciones;
DROP TABLE IF EXISTS Cuentas_Bancarias;
DROP TABLE IF EXISTS Partidos_Sponsors;
DROP TABLE IF EXISTS Entrenos;
DROP TABLE IF EXISTS Jugador_Tecnicas;
DROP TABLE IF EXISTS Jugadores;
DROP TABLE IF EXISTS Equipos;
DROP TABLE IF EXISTS Empleados;
DROP TABLE IF EXISTS Formaciones;
DROP TABLE IF EXISTS Supertecnicas;
DROP TABLE IF EXISTS Ejercicios;
DROP TABLE IF EXISTS Sponsors;
DROP TABLE IF EXISTS Partidos;

-- ==========================================
-- 2. CREACIÓN (De independientes a dependientes)
-- ==========================================

-- 2.1 TABLAS MAESTRAS
CREATE TABLE Formaciones (
    id_formacion INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    slot_1  ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_2  ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_3  ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_4  ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_5  ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_6  ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_7  ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_8  ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_9  ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_10 ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    slot_11 ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL
);

CREATE TABLE Supertecnicas (
    id_tecnica INT PRIMARY KEY AUTO_INCREMENT, 
    nombre VARCHAR(100) NOT NULL UNIQUE,
    tipo ENUM('Tiro', 'Regate', 'Defensa', 'Parada', 'Talento') NOT NULL,
    afinidad ENUM('Aire', 'Bosque', 'Fuego', 'Montaña', 'Neutro') NOT NULL,
    especial VARCHAR(50) DEFAULT '-', 
    potencia INT
);

CREATE TABLE Ejercicios (
    id_ejercicio INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL, 
    categoria ENUM('Físico', 'Técnico', 'Táctico', 'Portería', 'Específico') NOT NULL,
    descripcion TEXT
);

CREATE TABLE Sponsors (
    id_sponsor INT AUTO_INCREMENT PRIMARY KEY,
    nombre_empresa VARCHAR(100) NOT NULL,
    sector VARCHAR(50),
    aporte_economico DECIMAL(12, 2) DEFAULT 0.00,
    fecha_inicio DATE,
    fecha_fin DATE,
    url_logo VARCHAR(255)
);

CREATE TABLE Partidos (
    id_partido INT AUTO_INCREMENT PRIMARY KEY,
    rival VARCHAR(100) NOT NULL,
    fecha DATE NOT NULL,
    competicion ENUM('Amistoso', 'Fútbol Frontier', 'Mundial') DEFAULT 'Amistoso',
    goles_local INT DEFAULT 0,
    goles_rival INT DEFAULT 0
);

-- 2.2 TABLA EMPLEADOS
CREATE TABLE Empleados (
    id_empleado INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50),
    email VARCHAR(250),
    username VARCHAR(100),
    password VARCHAR(100),
    telefono INT,
    puesto ENUM('Director', 'Manager', 'Entrenador') NOT NULL,
    especialidad VARCHAR(50), 
    años_experiencia INT DEFAULT 0,
    salario DECIMAL(10, 2) DEFAULT 0.00,
    entrenador_titular BOOLEAN DEFAULT FALSE,
    url_imagen VARCHAR(255),
    id_formacion_activa INT,
    CONSTRAINT fk_empleado_formacion FOREIGN KEY (id_formacion_activa) REFERENCES Formaciones(id_formacion) ON DELETE SET NULL
);

-- 2.3 TABLA EQUIPOS (Actualizada: sin ciudad)
CREATE TABLE Equipos (
    id_equipo INT AUTO_INCREMENT PRIMARY KEY,
    nombre_equipo VARCHAR(100) NOT NULL UNIQUE,
    fk_director INT NOT NULL UNIQUE,
    url_escudo VARCHAR(255),
    CONSTRAINT fk_equipo_director FOREIGN KEY (fk_director) REFERENCES Empleados(id_empleado) ON DELETE RESTRICT
);

-- 2.4 TABLA JUGADORES (Actualizada: id_equipo, clausula, disponibilidad)
CREATE TABLE Jugadores (
    id_jugador INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    apodo VARCHAR(50),
    email VARCHAR(100) UNIQUE,
    telefono INT UNIQUE,
    username VARCHAR(50) UNIQUE, 
    password VARCHAR(255), 
    dorsal INT,
    posicion ENUM('PR','LI','DFCI','DFC','DFCD','LD','CI','MCDI','MCDC','MCDD','CD','MI','MCI','MCC','MCD','MD','II','MCO','ID','EI','DCI','DC','DCD','ED') NOT NULL,
    afinidad ENUM('Aire', 'Bosque', 'Fuego', 'Montaña', 'Neutro'),
    es_titular BOOLEAN DEFAULT FALSE,
    esta_convocado BOOLEAN DEFAULT FALSE,
    url_imagen VARCHAR(255),
    nivel INT DEFAULT 1,
    id_responsable INT,
    id_equipo INT,
    clausula_rescision DECIMAL(20, 2) DEFAULT 0.00,
    esta_disponible BOOLEAN DEFAULT TRUE,
    debe_cambiar_pass BOOLEAN DEFAULT TRUE,
    sueldo DECIMAL(10, 2) DEFAULT 0.00,
    CONSTRAINT fk_jugador_empleado FOREIGN KEY (id_responsable) REFERENCES Empleados(id_empleado) ON DELETE SET NULL,
    CONSTRAINT fk_jugador_equipo FOREIGN KEY (id_equipo) REFERENCES Equipos(id_equipo) ON DELETE SET NULL
);

-- 2.5 MERCADO DE FICHAJES
CREATE TABLE Mercado (
    id_anuncio INT AUTO_INCREMENT PRIMARY KEY,
    id_jugador INT NOT NULL,
    id_equipo INT NOT NULL,
    precio DECIMAL(20, 2) NOT NULL,
    fecha_fin DATE NOT NULL,
    estado ENUM('disponible', 'acabado', 'comprado') DEFAULT 'disponible',
    CONSTRAINT fk_mercado_jugador FOREIGN KEY (id_jugador) REFERENCES Jugadores(id_jugador) ON DELETE CASCADE,
    CONSTRAINT fk_mercado_equipo FOREIGN KEY (id_equipo) REFERENCES Equipos(id_equipo) ON DELETE CASCADE
);

-- 2.6 TABLAS ECONÓMICAS
CREATE TABLE Cuentas_Bancarias (
    id_cuenta INT AUTO_INCREMENT PRIMARY KEY,
    iban VARCHAR(34) NOT NULL UNIQUE,
    id_jugador INT NULL,
    id_empleado INT NULL,
    saldo_actual DECIMAL(20, 2) DEFAULT 0.00,
    moneda VARCHAR(3) DEFAULT 'EUR',
    CONSTRAINT fk_cuenta_jugador FOREIGN KEY (id_jugador) REFERENCES Jugadores(id_jugador) ON DELETE CASCADE,
    CONSTRAINT fk_cuenta_empleado FOREIGN KEY (id_empleado) REFERENCES Empleados(id_empleado) ON DELETE CASCADE,
    CONSTRAINT chk_propietario_unico CHECK ((id_jugador IS NOT NULL AND id_empleado IS NULL) OR (id_jugador IS NULL AND id_empleado IS NOT NULL))
);

CREATE TABLE Transacciones (
    id_transaccion INT AUTO_INCREMENT PRIMARY KEY,
    id_cuenta_origen INT,
    id_cuenta_destino INT,
    monto DECIMAL(20, 2) NOT NULL,
    tipo ENUM('Fichaje', 'Salario', 'Sponsor', 'Premio') NOT NULL,
    concepto VARCHAR(255),
    fecha_operacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    id_jugador_relacionado INT,
    CONSTRAINT fk_t_origen FOREIGN KEY (id_cuenta_origen) REFERENCES Cuentas_Bancarias(id_cuenta),
    CONSTRAINT fk_t_destino FOREIGN KEY (id_cuenta_destino) REFERENCES Cuentas_Bancarias(id_cuenta),
    CONSTRAINT fk_t_jugador FOREIGN KEY (id_jugador_relacionado) REFERENCES Jugadores(id_jugador)
);

CREATE TABLE Fichajes (
    id_fichaje INT AUTO_INCREMENT PRIMARY KEY,
    fk_jugador INT NOT NULL,
    fk_equipo_anterior INT NULL,
    fk_equipo_destino INT NOT NULL,
    fk_transaccion INT UNIQUE,
    precio_final DECIMAL(20, 2),
    fecha_fichaje DATE NOT NULL,
    fecha_incorporacion DATE NOT NULL,
    estado_fichaje ENUM('Pendiente', 'Completado') DEFAULT 'Pendiente',
    CONSTRAINT fk_f_jugador FOREIGN KEY (fk_jugador) REFERENCES Jugadores(id_jugador),
    CONSTRAINT fk_f_equipo_ant FOREIGN KEY (fk_equipo_anterior) REFERENCES Equipos(id_equipo),
    CONSTRAINT fk_f_equipo_dest FOREIGN KEY (fk_equipo_destino) REFERENCES Equipos(id_equipo),
    CONSTRAINT fk_f_transaccion FOREIGN KEY (fk_transaccion) REFERENCES Transacciones(id_transaccion)
);

-- 2.6 OTRAS RELACIONES
CREATE TABLE Jugador_Tecnicas (
    id_jugador INT NOT NULL,
    id_tecnica INT NOT NULL,
    PRIMARY KEY (id_jugador, id_tecnica),
    FOREIGN KEY (id_jugador) REFERENCES Jugadores(id_jugador) ON DELETE CASCADE,
    FOREIGN KEY (id_tecnica) REFERENCES Supertecnicas(id_tecnica) ON DELETE CASCADE
);

CREATE TABLE Entrenos (
    id_entreno INT AUTO_INCREMENT PRIMARY KEY,
    id_jugador INT NOT NULL,
    id_ejercicio INT NOT NULL,
    id_empleado INT,
    fecha DATE NOT NULL,
    comentarios TEXT, 
    completado BOOLEAN DEFAULT FALSE,
    CONSTRAINT fk_entreno_jugador FOREIGN KEY (id_jugador) REFERENCES Jugadores(id_jugador) ON DELETE CASCADE,
    CONSTRAINT fk_entreno_ejercicio FOREIGN KEY (id_ejercicio) REFERENCES Ejercicios(id_ejercicio) ON DELETE CASCADE,
    CONSTRAINT fk_entreno_empleado FOREIGN KEY (id_empleado) REFERENCES Empleados(id_empleado) ON DELETE SET NULL
);

CREATE TABLE Partidos_Sponsors (
    id_partido INT NOT NULL,
    id_sponsor INT NOT NULL,
    PRIMARY KEY (id_partido, id_sponsor),
    FOREIGN KEY (id_partido) REFERENCES Partidos(id_partido) ON DELETE CASCADE,
    FOREIGN KEY (id_sponsor) REFERENCES Sponsors(id_sponsor) ON DELETE CASCADE
);

-- ==========================================
-- 3. PROCESOS ALMACENADOS
-- ==========================================

DELIMITER $$

-- ==========================================
-- 1. EQUIPOS (NUEVA)
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerTodosEquipos$$
CREATE PROCEDURE sp_ObtenerTodosEquipos()
BEGIN
    SELECT e.*, emp.nombre AS nombre_director, emp.apellido AS apellido_director 
    FROM Equipos e
    INNER JOIN Empleados emp ON e.fk_director = emp.id_empleado;
END$$

DROP PROCEDURE IF EXISTS sp_InsertarEquipo$$
CREATE PROCEDURE sp_InsertarEquipo(
    IN p_nombre VARCHAR(100),
    IN p_fk_director INT,
    IN p_url_escudo VARCHAR(255)
)
BEGIN
    INSERT INTO Equipos (nombre_equipo, fk_director, url_escudo)
    VALUES (p_nombre, p_fk_director, p_url_escudo);
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarEquipo$$
CREATE PROCEDURE sp_ActualizarEquipo(
    IN p_nombre VARCHAR(100),
    IN p_fk_director INT,
    IN p_url_escudo VARCHAR(255),
    IN p_id INT
)
BEGIN
    UPDATE Equipos SET
        nombre_equipo = p_nombre,
        fk_director = p_fk_director,
        url_escudo = p_url_escudo
    WHERE id_equipo = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_EliminarEquipo$$
CREATE PROCEDURE sp_EliminarEquipo(IN p_id INT)
BEGIN
    DELETE FROM Equipos WHERE id_equipo = p_id;
END$$

-- ==========================================
-- 2. CUENTAS BANCARIAS Y TRANSACCIONES (NUEVAS)
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerCuentaPorEmpleado$$
CREATE PROCEDURE sp_ObtenerCuentaPorEmpleado(IN p_id_emp INT)
BEGIN
    SELECT * FROM Cuentas_Bancarias WHERE id_empleado = p_id_emp;
END$$

DROP PROCEDURE IF EXISTS sp_InsertarCuentaBancaria$$
CREATE PROCEDURE sp_InsertarCuentaBancaria(
    IN p_iban VARCHAR(34),
    IN p_id_jugador INT,
    IN p_id_empleado INT,
    IN p_saldo DECIMAL(20,2)
)
BEGIN
    INSERT INTO Cuentas_Bancarias (iban, id_jugador, id_empleado, saldo_actual, moneda)
    VALUES (p_iban, NULLIF(p_id_jugador, 0), NULLIF(p_id_empleado, 0), p_saldo, 'EUR');
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerTransaccionesJugador$$
CREATE PROCEDURE sp_ObtenerTransaccionesJugador(IN p_id_jugador INT)
BEGIN
    SELECT * FROM Transacciones WHERE id_jugador_relacionado = p_id_jugador ORDER BY fecha_operacion DESC;
END$$



-- ==========================================
-- 4. PROCESO DE FICHAJE (LÓGICA DE NEGOCIO)
-- ==========================================

DROP PROCEDURE IF EXISTS sp_RealizarFichaje$$
CREATE PROCEDURE sp_RealizarFichaje(
    IN p_id_jugador INT,
    IN p_id_director_comprador INT,
    IN p_nueva_clausula DECIMAL(20, 2),
    IN p_fecha_inc DATE
)
BEGIN
    DECLARE v_precio DECIMAL(20,2);
    DECLARE v_disponible BOOLEAN;
    DECLARE v_id_vendedor INT;
    DECLARE v_id_equipo_dest INT;
    DECLARE v_cuenta_org INT;
    DECLARE v_cuenta_dest INT;
    DECLARE v_equipo_ant INT;

    -- Obtener datos actuales del jugador
    SELECT clausula_rescision, esta_disponible, id_responsable, id_equipo 
    INTO v_precio, v_disponible, v_id_vendedor, v_equipo_ant
    FROM Jugadores WHERE id_jugador = p_id_jugador;

    -- Validación
    IF v_disponible = FALSE THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El jugador no es transferible.';
    ELSE
        -- Obtener IDs necesarios para la transacción
        SELECT id_equipo INTO v_id_equipo_dest FROM Equipos WHERE fk_director = p_id_director_comprador;
        SELECT id_cuenta INTO v_cuenta_org FROM Cuentas_Bancarias WHERE id_empleado = p_id_director_comprador;
        SELECT id_cuenta INTO v_cuenta_dest FROM Cuentas_Bancarias WHERE id_empleado = v_id_vendedor;

        START TRANSACTION;
            -- 1. Movimiento de dinero
            UPDATE Cuentas_Bancarias SET saldo_actual = saldo_actual - v_precio WHERE id_cuenta = v_cuenta_org;
            UPDATE Cuentas_Bancarias SET saldo_actual = saldo_actual + v_precio WHERE id_cuenta = v_cuenta_dest;
            
            -- 2. Registrar Transacción
            INSERT INTO Transacciones (id_cuenta_origen, id_cuenta_destino, monto, tipo, concepto, id_jugador_relacionado)
            VALUES (v_cuenta_org, v_cuenta_dest, v_precio, 'Fichaje', 'Pago de cláusula', p_id_jugador);
            
            SET @last_t = LAST_INSERT_ID();

            -- 3. Registrar en Histórico de Fichajes
            INSERT INTO Fichajes (fk_jugador, fk_equipo_anterior, fk_equipo_destino, fk_transaccion, precio_final, fecha_fichaje, fecha_incorporacion, estado_fichaje)
            VALUES (p_id_jugador, v_equipo_ant, v_id_equipo_dest, @last_t, v_precio, CURDATE(), p_fecha_inc, 'Pendiente');

            -- 4. Actualizar Jugador (Si la incorporación es hoy, se podría automatizar con el evento que hablamos)
            UPDATE Jugadores SET 
                id_responsable = p_id_director_comprador,
                id_equipo = v_id_equipo_dest,
                clausula_rescision = p_nueva_clausula,
                esta_disponible = FALSE -- Por defecto deja de estar disponible al ser recién fichado
            WHERE id_jugador = p_id_jugador;
        COMMIT;
    END IF;
END$$

-- ==========================================
-- 5. HISTÓRICO DE FICHAJES
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerHistorialFichajes$$
CREATE PROCEDURE sp_ObtenerHistorialFichajes()
BEGIN
    SELECT f.*, j.nombre, j.apellido, e_ant.nombre_equipo AS equipo_origen, e_dest.nombre_equipo AS equipo_destino
    FROM Fichajes f
    INNER JOIN Jugadores j ON f.fk_jugador = j.id_jugador
    LEFT JOIN Equipos e_ant ON f.fk_equipo_anterior = e_ant.id_equipo
    INNER JOIN Equipos e_dest ON f.fk_equipo_destino = e_dest.id_equipo
    ORDER BY f.fecha_fichaje DESC;
END$$
DROP PROCEDURE IF EXISTS sp_BorrarSupertecnica$$
CREATE PROCEDURE sp_BorrarSupertecnica(IN p_id INT)
BEGIN
    DELETE FROM Jugador_Tecnicas WHERE id_tecnica = p_id;
    DELETE FROM Supertecnicas WHERE id_tecnica = p_id;
END$$

-- 1. OBTENER FORMACIONES (Con datos del Entrenador asociado)
DROP PROCEDURE IF EXISTS sp_ObtenerFormaciones$$
CREATE PROCEDURE sp_ObtenerFormaciones()
BEGIN
    SELECT 
        f.*, 
        e.nombre AS entrenador_nombre, 
        e.apellido AS entrenador_apellido, 
        IFNULL(e.url_imagen, '/Images/Empleados/default.png') AS entrenador_foto
    FROM Formaciones f
    LEFT JOIN Empleados e ON f.id_formacion = e.id_formacion_activa;
END$$

-- 2. INSERTAR FORMACION
DROP PROCEDURE IF EXISTS sp_CrearFormacion$$
CREATE PROCEDURE sp_CrearFormacion(
    IN p_nombre VARCHAR(50),
    IN p_s1 VARCHAR(10), IN p_s2 VARCHAR(10), IN p_s3 VARCHAR(10), IN p_s4 VARCHAR(10),
    IN p_s5 VARCHAR(10), IN p_s6 VARCHAR(10), IN p_s7 VARCHAR(10), IN p_s8 VARCHAR(10),
    IN p_s9 VARCHAR(10), IN p_s10 VARCHAR(10), IN p_s11 VARCHAR(10)
)
BEGIN
    INSERT INTO Formaciones (nombre, slot_1, slot_2, slot_3, slot_4, slot_5, slot_6, slot_7, slot_8, slot_9, slot_10, slot_11)
    VALUES (p_nombre, p_s1, p_s2, p_s3, p_s4, p_s5, p_s6, p_s7, p_s8, p_s9, p_s10, p_s11);
END$$

-- 3. ACTUALIZAR FORMACION
DROP PROCEDURE IF EXISTS sp_ActualizarFormacion$$
CREATE PROCEDURE sp_ActualizarFormacion(
    IN p_id INT,
    IN p_nombre VARCHAR(50),
    IN p_s1 VARCHAR(10), IN p_s2 VARCHAR(10), IN p_s3 VARCHAR(10), IN p_s4 VARCHAR(10),
    IN p_s5 VARCHAR(10), IN p_s6 VARCHAR(10), IN p_s7 VARCHAR(10), IN p_s8 VARCHAR(10),
    IN p_s9 VARCHAR(10), IN p_s10 VARCHAR(10), IN p_s11 VARCHAR(10)
)
BEGIN
    UPDATE Formaciones 
    SET nombre = p_nombre,
        slot_1 = p_s1, slot_2 = p_s2, slot_3 = p_s3, slot_4 = p_s4, 
        slot_5 = p_s5, slot_6 = p_s6, slot_7 = p_s7, slot_8 = p_s8, 
        slot_9 = p_s9, slot_10 = p_s10, slot_11 = p_s11
    WHERE id_formacion = p_id;
END$$

-- 4. BORRAR FORMACION
DROP PROCEDURE IF EXISTS sp_BorrarFormacion$$
CREATE PROCEDURE sp_BorrarFormacion(IN p_id INT)
BEGIN
    -- Primero quitamos la referencia en empleados para no romper FK
    UPDATE Empleados SET id_formacion_activa = NULL WHERE id_formacion_activa = p_id;
    DELETE FROM Formaciones WHERE id_formacion = p_id;
END$$


DROP PROCEDURE IF EXISTS sp_AsignarFormacionActiva$$
CREATE PROCEDURE sp_AsignarFormacionActiva(
    IN p_username VARCHAR(100),
    IN p_id_formacion INT
)
BEGIN
    UPDATE Empleados 
    SET id_formacion_activa = p_id_formacion 
    WHERE username = p_username;
END$$

-- EQUIPO:

-- 1. INTERCAMBIAR DOS JUGADORES (Swap completo)
DROP PROCEDURE IF EXISTS sp_IntercambiarJugadores$$
CREATE PROCEDURE sp_IntercambiarJugadores(
    IN p_id1 INT,
    IN p_id2 INT
)
BEGIN
    -- Variables para el Jugador 1
    DECLARE v_pos1 VARCHAR(10);
    DECLARE v_titular1 BOOLEAN;
    DECLARE v_convocado1 BOOLEAN;
    
    -- Variables para el Jugador 2
    DECLARE v_pos2 VARCHAR(10);
    DECLARE v_titular2 BOOLEAN;
    DECLARE v_convocado2 BOOLEAN;

    -- 1. Leer estado actual de ambos
    SELECT posicion, es_titular, esta_convocado 
    INTO v_pos1, v_titular1, v_convocado1
    FROM Jugadores WHERE id_jugador = p_id1;

    SELECT posicion, es_titular, esta_convocado 
    INTO v_pos2, v_titular2, v_convocado2
    FROM Jugadores WHERE id_jugador = p_id2;

    -- 2. Cruzar los datos (Swap)
    -- Jugador 1 recibe los datos del 2
    UPDATE Jugadores 
    SET posicion = v_pos2, 
        es_titular = v_titular2, 
        esta_convocado = v_convocado2
    WHERE id_jugador = p_id1;

    -- Jugador 2 recibe los datos del 1
    UPDATE Jugadores 
    SET posicion = v_pos1, 
        es_titular = v_titular1, 
        esta_convocado = v_convocado1
    WHERE id_jugador = p_id2;
END$$

-- 2. MOVER JUGADOR (Cambio individual)
-- Útil si arrastras un reserva a un hueco vacío de titular
DROP PROCEDURE IF EXISTS sp_ActualizarEstadoJugador$$
CREATE PROCEDURE sp_ActualizarEstadoJugador(
    IN p_id INT,
    IN p_posicion VARCHAR(10),
    IN p_titular BOOLEAN,
    IN p_convocado BOOLEAN
)
BEGIN
    UPDATE Jugadores
    SET posicion = p_posicion,
        es_titular = p_titular,
        esta_convocado = p_convocado
    WHERE id_jugador = p_id;
END$$

-- ==========================================
-- 4. NUEVOS PROCESOS ALMACENADOS (Repositorios)
-- ==========================================

-- ==========================================
-- EMPLEADOS
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ValidarEmpleado$$
CREATE PROCEDURE sp_ValidarEmpleado(
    IN p_username VARCHAR(100),
    IN p_password VARCHAR(100),
    OUT p_es_valido BIT
)
BEGIN
    DECLARE v_count INT;
    SELECT COUNT(*) INTO v_count FROM Empleados WHERE username = p_username AND password = p_password;
    SET p_es_valido = IF(v_count > 0, 1, 0);
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerTodosEmpleados$$
CREATE PROCEDURE sp_ObtenerTodosEmpleados()
BEGIN
    SELECT 
        e.*,
        f.id_formacion AS id_formacion_activa,
        f.nombre AS nombre_formacion,
        f.slot_1, f.slot_2, f.slot_3, f.slot_4, f.slot_5, f.slot_6,
        f.slot_7, f.slot_8, f.slot_9, f.slot_10, f.slot_11
    FROM Empleados e
    LEFT JOIN Formaciones f ON e.id_formacion_activa = f.id_formacion;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerEmpleadoPorId$$
CREATE PROCEDURE sp_ObtenerEmpleadoPorId(IN p_id INT)
BEGIN
    SELECT * FROM Empleados WHERE id_empleado = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerEmpleadoPorUsername$$
CREATE PROCEDURE sp_ObtenerEmpleadoPorUsername(IN p_username VARCHAR(100))
BEGIN
    SELECT * FROM Empleados WHERE username = p_username;
END$$

DROP PROCEDURE IF EXISTS sp_InsertarEmpleado$$
CREATE PROCEDURE sp_InsertarEmpleado(
    IN p_nombre VARCHAR(50),
    IN p_apellido VARCHAR(50),
    IN p_email VARCHAR(250),
    IN p_username VARCHAR(100),
    IN p_password VARCHAR(100),
    IN p_telefono INT,
    IN p_puesto ENUM('Director','Manager','Entrenador'),
    IN p_especialidad VARCHAR(50),
    IN p_experiencia INT,
    IN p_salario DECIMAL(10,2),
    IN p_titular BOOLEAN,
    IN p_url_imagen VARCHAR(255),
    IN p_id_formacion INT
)
BEGIN
    INSERT INTO Empleados (nombre, apellido, email, username, password, telefono, puesto, especialidad, años_experiencia, salario, entrenador_titular, url_imagen, id_formacion_activa)
    VALUES (p_nombre, p_apellido, p_email, p_username, p_password, p_telefono, p_puesto, p_especialidad, p_experiencia, p_salario, p_titular, p_url_imagen, NULLIF(p_id_formacion, 0));
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarEmpleado$$
CREATE PROCEDURE sp_ActualizarEmpleado(
    IN p_nombre VARCHAR(50),
    IN p_apellido VARCHAR(50),
    IN p_email VARCHAR(250),
    IN p_username VARCHAR(100),
    IN p_password VARCHAR(100),
    IN p_telefono INT,
    IN p_puesto ENUM('Director','Manager','Entrenador'),
    IN p_especialidad VARCHAR(50),
    IN p_experiencia INT,
    IN p_salario DECIMAL(10,2),
    IN p_titular BOOLEAN,
    IN p_url_imagen VARCHAR(255),
    IN p_id_formacion INT,
    IN p_id INT
)
BEGIN
    UPDATE Empleados SET
        nombre = p_nombre, apellido = p_apellido, email = p_email,
        username = p_username, password = p_password, telefono = p_telefono,
        puesto = p_puesto, especialidad = p_especialidad, años_experiencia = p_experiencia,
        salario = p_salario, entrenador_titular = p_titular, url_imagen = p_url_imagen,
        id_formacion_activa = NULLIF(p_id_formacion, 0)
    WHERE id_empleado = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_EliminarEmpleado$$
CREATE PROCEDURE sp_EliminarEmpleado(IN p_id INT)
BEGIN
    DELETE FROM Empleados WHERE id_empleado = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerJugadoresPorEmpleado$$
CREATE PROCEDURE sp_ObtenerJugadoresPorEmpleado(IN p_id INT)
BEGIN
    SELECT * FROM Jugadores WHERE id_responsable = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_AsignarJugadorEmpleado$$
CREATE PROCEDURE sp_AsignarJugadorEmpleado(IN p_id_empleado INT, IN p_id_jugador INT)
BEGIN
    UPDATE Jugadores SET id_responsable = p_id_empleado WHERE id_jugador = p_id_jugador;
END$$

DROP PROCEDURE IF EXISTS sp_DesasignarJugadorEmpleado$$
CREATE PROCEDURE sp_DesasignarJugadorEmpleado(IN p_id_empleado INT, IN p_id_jugador INT)
BEGIN
    UPDATE Jugadores SET id_responsable = NULL WHERE id_jugador = p_id_jugador AND id_responsable = p_id_empleado;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerEntrenadorTitular$$
CREATE PROCEDURE sp_ObtenerEntrenadorTitular()
BEGIN
    SELECT 
        e.id_empleado, e.id_formacion_activa,
        f.nombre AS nombre_formacion,
        f.slot_1, f.slot_2, f.slot_3, f.slot_4, f.slot_5, f.slot_6,
        f.slot_7, f.slot_8, f.slot_9, f.slot_10, f.slot_11
    FROM Empleados e
    LEFT JOIN Formaciones f ON e.id_formacion_activa = f.id_formacion
    WHERE e.entrenador_titular = TRUE
    LIMIT 1;
END$$

-- ==========================================
-- JUGADORES
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerTodosJugadores$$
CREATE PROCEDURE sp_ObtenerTodosJugadores()
BEGIN
    SELECT 
        j.*,
        IFNULL(e.url_imagen, '/Images/Empleados/default.png') AS foto_mister
    FROM Jugadores j
    LEFT JOIN Empleados e ON j.id_responsable = e.id_empleado;
END$$

DROP PROCEDURE IF EXISTS sp_ValidarJugador$$
CREATE PROCEDURE sp_ValidarJugador(
    IN p_username VARCHAR(50),
    IN p_password VARCHAR(255),
    OUT p_es_valido BIT
)
BEGIN
    DECLARE v_count INT;
    SELECT COUNT(*) INTO v_count FROM Jugadores WHERE username = p_username AND password = p_password;
    SET p_es_valido = IF(v_count > 0, 1, 0);
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerJugadorPorUsername$$
CREATE PROCEDURE sp_ObtenerJugadorPorUsername(IN p_username VARCHAR(50))
BEGIN
    SELECT * FROM Jugadores WHERE username = p_username;
END$$

DROP PROCEDURE IF EXISTS sp_InsertarJugador$$
CREATE PROCEDURE sp_InsertarJugador(
    IN p_nombre VARCHAR(50),
    IN p_apellido VARCHAR(50),
    IN p_apodo VARCHAR(50),
    IN p_email VARCHAR(100),
    IN p_dorsal INT,
    IN p_posicion VARCHAR(30),
    IN p_afinidad VARCHAR(20),
    IN p_username VARCHAR(50),
    IN p_password VARCHAR(255),
    IN p_url VARCHAR(255),
    IN p_sueldo DECIMAL(10,2)
)
BEGIN
    INSERT INTO Jugadores (nombre, apellido, apodo, email, dorsal, posicion, afinidad, username, password, url_imagen, sueldo)
    VALUES (p_nombre, p_apellido, p_apodo, p_email, p_dorsal, p_posicion, p_afinidad, p_username, p_password, p_url, p_sueldo);
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarJugador$$
CREATE PROCEDURE sp_ActualizarJugador(
    IN p_nombre VARCHAR(50),
    IN p_apellido VARCHAR(50),
    IN p_apodo VARCHAR(50),
    IN p_email VARCHAR(100),
    IN p_dorsal INT,
    IN p_posicion VARCHAR(30),
    IN p_afinidad VARCHAR(20),
    IN p_titular BOOLEAN,
    IN p_convocado BOOLEAN,
    IN p_sueldo DECIMAL(10,2),
    IN p_clausula DECIMAL(20,2),
    IN p_disponible BOOLEAN,
    IN p_id INT
)
BEGIN
    UPDATE Jugadores SET
        nombre = p_nombre, apellido = p_apellido, apodo = p_apodo,
        email = p_email, dorsal = p_dorsal, posicion = p_posicion,
        afinidad = p_afinidad, es_titular = p_titular, esta_convocado = p_convocado,
        sueldo = p_sueldo, clausula_rescision = p_clausula, esta_disponible = p_disponible
    WHERE id_jugador = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_EliminarJugador$$
CREATE PROCEDURE sp_EliminarJugador(IN p_id INT)
BEGIN
    DELETE FROM Jugadores WHERE id_jugador = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerTecnicasDeJugador$$
CREATE PROCEDURE sp_ObtenerTecnicasDeJugador(IN p_id INT)
BEGIN
    SELECT st.* FROM Supertecnicas st
    INNER JOIN Jugador_Tecnicas jt ON st.id_tecnica = jt.id_tecnica
    WHERE jt.id_jugador = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerCatalogoCompleto$$
CREATE PROCEDURE sp_ObtenerCatalogoCompleto()
BEGIN
    SELECT * FROM Supertecnicas ORDER BY nombre;
END$$

DROP PROCEDURE IF EXISTS sp_AsignarTecnicaJugador$$
CREATE PROCEDURE sp_AsignarTecnicaJugador(IN p_id_jugador INT, IN p_id_tecnica INT)
BEGIN
    INSERT IGNORE INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (p_id_jugador, p_id_tecnica);
END$$

DROP PROCEDURE IF EXISTS sp_RetirarTecnicaJugador$$
CREATE PROCEDURE sp_RetirarTecnicaJugador(IN p_id_jugador INT, IN p_id_tecnica INT)
BEGIN
    DELETE FROM Jugador_Tecnicas WHERE id_jugador = p_id_jugador AND id_tecnica = p_id_tecnica;
END$$

-- ==========================================
-- EJERCICIOS
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerEjercicios$$
CREATE PROCEDURE sp_ObtenerEjercicios()
BEGIN
    SELECT * FROM Ejercicios ORDER BY nombre;
END$$

DROP PROCEDURE IF EXISTS sp_InsertarEjercicio$$
CREATE PROCEDURE sp_InsertarEjercicio(
    IN p_nombre VARCHAR(100),
    IN p_categoria ENUM('Físico','Técnico','Táctico','Portería','Específico'),
    IN p_descripcion TEXT
)
BEGIN
    INSERT INTO Ejercicios (nombre, categoria, descripcion) VALUES (p_nombre, p_categoria, p_descripcion);
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarEjercicio$$
CREATE PROCEDURE sp_ActualizarEjercicio(
    IN p_nombre VARCHAR(100),
    IN p_categoria ENUM('Físico','Técnico','Táctico','Portería','Específico'),
    IN p_descripcion TEXT,
    IN p_id INT
)
BEGIN
    UPDATE Ejercicios SET nombre = p_nombre, categoria = p_categoria, descripcion = p_descripcion WHERE id_ejercicio = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_EliminarEjercicio$$
CREATE PROCEDURE sp_EliminarEjercicio(IN p_id INT)
BEGIN
    DELETE FROM Ejercicios WHERE id_ejercicio = p_id;
END$$

-- ==========================================
-- ENTRENOS
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerEntrenosAgenda$$
CREATE PROCEDURE sp_ObtenerEntrenosAgenda(IN p_id_jugador INT)
BEGIN
    SELECT 
        e.id_entreno,
        e.fecha,
        ej.nombre AS nombre_ejercicio,
        CONCAT(j.nombre, ' ', j.apellido) AS nombre_jugador,
        IFNULL(CONCAT(emp.nombre, ' ', emp.apellido), 'Sin supervisor') AS nombre_empleado,
        e.comentarios
    FROM Entrenos e
    JOIN Ejercicios ej ON e.id_ejercicio = ej.id_ejercicio
    JOIN Jugadores j ON e.id_jugador = j.id_jugador
    LEFT JOIN Empleados emp ON e.id_empleado = emp.id_empleado
    WHERE (p_id_jugador IS NULL OR e.id_jugador = p_id_jugador)
    ORDER BY e.fecha ASC;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerSelectorJugadores$$
CREATE PROCEDURE sp_ObtenerSelectorJugadores()
BEGIN
    SELECT id_jugador AS id, CONCAT(nombre, ' ', apellido) AS nombre FROM Jugadores ORDER BY nombre;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerSelectorEjercicios$$
CREATE PROCEDURE sp_ObtenerSelectorEjercicios()
BEGIN
    SELECT id_ejercicio AS id, nombre FROM Ejercicios ORDER BY nombre;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerSelectorEmpleados$$
CREATE PROCEDURE sp_ObtenerSelectorEmpleados()
BEGIN
    SELECT id_empleado AS id, CONCAT(nombre, ' ', apellido) AS nombre FROM Empleados ORDER BY nombre;
END$$

DROP PROCEDURE IF EXISTS sp_InsertarEntreno$$
CREATE PROCEDURE sp_InsertarEntreno(
    IN p_id_jugador INT,
    IN p_id_ejercicio INT,
    IN p_id_empleado INT,
    IN p_fecha DATE,
    IN p_comentarios TEXT
)
BEGIN
    INSERT INTO Entrenos (id_jugador, id_ejercicio, id_empleado, fecha, comentarios)
    VALUES (p_id_jugador, p_id_ejercicio, NULLIF(p_id_empleado, 0), p_fecha, p_comentarios);
END$$

DROP PROCEDURE IF EXISTS sp_EliminarEntreno$$
CREATE PROCEDURE sp_EliminarEntreno(IN p_id INT)
BEGIN
    DELETE FROM Entrenos WHERE id_entreno = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarComentariosEntreno$$
CREATE PROCEDURE sp_ActualizarComentariosEntreno(
    IN p_comentarios TEXT,
    IN p_id INT
)
BEGIN
    UPDATE Entrenos SET comentarios = p_comentarios WHERE id_entreno = p_id;
END$$

-- ==========================================
-- PARTIDOS
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerListaPartidos$$
CREATE PROCEDURE sp_ObtenerListaPartidos()
BEGIN
    SELECT id_partido, fecha, rival, competicion, goles_local, goles_rival FROM Partidos ORDER BY fecha DESC;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerAgendaPartidos$$
CREATE PROCEDURE sp_ObtenerAgendaPartidos()
BEGIN
    SELECT id_partido, fecha, rival, competicion, goles_local, goles_rival FROM Partidos ORDER BY fecha DESC;
END$$

DROP PROCEDURE IF EXISTS sp_InsertarPartido$$
CREATE PROCEDURE sp_InsertarPartido(
    IN p_fecha DATE,
    IN p_rival VARCHAR(100),
    IN p_competicion ENUM('Amistoso','Fútbol Frontier','Mundial')
)
BEGIN
    INSERT INTO Partidos (fecha, rival, competicion, goles_local, goles_rival) VALUES (p_fecha, p_rival, p_competicion, 0, 0);
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarDatosPartido$$
CREATE PROCEDURE sp_ActualizarDatosPartido(
    IN p_fecha DATE,
    IN p_rival VARCHAR(100),
    IN p_id INT
)
BEGIN
    UPDATE Partidos SET fecha = p_fecha, rival = p_rival WHERE id_partido = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_EliminarPartido$$
CREATE PROCEDURE sp_EliminarPartido(IN p_id INT)
BEGIN
    DELETE FROM Partidos WHERE id_partido = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarResultadoPartido$$
CREATE PROCEDURE sp_ActualizarResultadoPartido(
    IN p_gl INT,
    IN p_gv INT,
    IN p_comp ENUM('Amistoso','Fútbol Frontier','Mundial'),
    IN p_id INT
)
BEGIN
    UPDATE Partidos SET goles_local = p_gl, goles_rival = p_gv, competicion = p_comp WHERE id_partido = p_id;
END$$

-- ==========================================
-- SPONSORS
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerSponsors$$
CREATE PROCEDURE sp_ObtenerSponsors()
BEGIN
    SELECT * FROM Sponsors ORDER BY nombre_empresa;
END$$

DROP PROCEDURE IF EXISTS sp_InsertarSponsor$$
CREATE PROCEDURE sp_InsertarSponsor(
    IN p_nombre_empresa VARCHAR(100),
    IN p_sector VARCHAR(50),
    IN p_aporte DECIMAL(12,2),
    IN p_url VARCHAR(255),
    IN p_fecha_inicio DATE,
    IN p_fecha_fin DATE
)
BEGIN
    INSERT INTO Sponsors (nombre_empresa, sector, aporte_economico, url_logo, fecha_inicio, fecha_fin)
    VALUES (p_nombre_empresa, p_sector, p_aporte, p_url, p_fecha_inicio, p_fecha_fin);
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarSponsor$$
CREATE PROCEDURE sp_ActualizarSponsor(
    IN p_nombre_empresa VARCHAR(100),
    IN p_sector VARCHAR(50),
    IN p_aporte DECIMAL(12,2),
    IN p_url VARCHAR(255),
    IN p_fecha_inicio DATE,
    IN p_fecha_fin DATE,
    IN p_id INT
)
BEGIN
    UPDATE Sponsors SET
        nombre_empresa = p_nombre_empresa, sector = p_sector,
        aporte_economico = p_aporte, url_logo = p_url,
        fecha_inicio = p_fecha_inicio, fecha_fin = p_fecha_fin
    WHERE id_sponsor = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_EliminarSponsor$$
CREATE PROCEDURE sp_EliminarSponsor(IN p_id INT)
BEGIN
    DELETE FROM Sponsors WHERE id_sponsor = p_id;
END$$

-- ==========================================
-- SUPERTECNICAS
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerTodasSupertecnicas$$
CREATE PROCEDURE sp_ObtenerTodasSupertecnicas()
BEGIN
    SELECT * FROM Supertecnicas ORDER BY nombre;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerSupertecnicaPorId$$
CREATE PROCEDURE sp_ObtenerSupertecnicaPorId(IN p_id INT)
BEGIN
    SELECT * FROM Supertecnicas WHERE id_tecnica = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_InsertarSupertecnica$$
CREATE PROCEDURE sp_InsertarSupertecnica(
    IN p_nombre VARCHAR(100),
    IN p_tipo ENUM('Tiro','Regate','Defensa','Parada','Talento'),
    IN p_afinidad ENUM('Aire','Bosque','Fuego','Montaña','Neutro'),
    IN p_especial VARCHAR(50),
    IN p_potencia INT
)
BEGIN
    INSERT INTO Supertecnicas (nombre, tipo, afinidad, especial, potencia)
    VALUES (p_nombre, p_tipo, p_afinidad, p_especial, p_potencia);
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarSupertecnica$$
CREATE PROCEDURE sp_ActualizarSupertecnica(
    IN p_nombre VARCHAR(100),
    IN p_tipo ENUM('Tiro','Regate','Defensa','Parada','Talento'),
    IN p_afinidad ENUM('Aire','Bosque','Fuego','Montaña','Neutro'),
    IN p_especial VARCHAR(50),
    IN p_potencia INT,
    IN p_id INT
)
BEGIN
    UPDATE Supertecnicas SET nombre = p_nombre, tipo = p_tipo, afinidad = p_afinidad, especial = p_especial, potencia = p_potencia
    WHERE id_tecnica = p_id;
END$$

-- ==========================================
-- FORMACIONES (adicionales)
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ObtenerFormacionPorId$$
CREATE PROCEDURE sp_ObtenerFormacionPorId(IN p_id INT)
BEGIN
    SELECT * FROM Formaciones WHERE id_formacion = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_ObtenerNombresFormaciones$$
CREATE PROCEDURE sp_ObtenerNombresFormaciones()
BEGIN
    SELECT id_formacion, nombre FROM Formaciones ORDER BY nombre;
END$$

-- ==========================================
-- PLANTILLA
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ActualizarPosicionJugador$$
CREATE PROCEDURE sp_ActualizarPosicionJugador(IN p_posicion VARCHAR(30), IN p_id INT)
BEGIN
    UPDATE Jugadores SET posicion = p_posicion WHERE id_jugador = p_id;
END$$

DROP PROCEDURE IF EXISTS sp_ActualizarDatosBasicosJugador$$
CREATE PROCEDURE sp_ActualizarDatosBasicosJugador(
    IN p_nombre VARCHAR(50),
    IN p_apellido VARCHAR(50),
    IN p_apodo VARCHAR(50),
    IN p_email VARCHAR(100),
    IN p_dorsal INT,
    IN p_posicion VARCHAR(30),
    IN p_afinidad VARCHAR(20),
    IN p_id INT
)
BEGIN
    UPDATE Jugadores SET
        nombre = p_nombre, apellido = p_apellido, apodo = p_apodo,
        email = p_email, dorsal = p_dorsal, posicion = p_posicion, afinidad = p_afinidad
    WHERE id_jugador = p_id;
END$$

DELIMITER ;