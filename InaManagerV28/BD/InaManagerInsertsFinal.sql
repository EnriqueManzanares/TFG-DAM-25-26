SET FOREIGN_KEY_CHECKS = 0;

USE InaManager;
-- Limpiamos las tablas antes de la inserción masiva para evitar duplicados y conflictos
DELETE FROM Supertecnicas where id_tecnica > 0;
DELETE FROM Formaciones where id_formacion > 0;
DELETE FROM Mercado where id_anuncio > 0;
DELETE FROM Fichajes where id_fichaje > 0;
DELETE FROM Inversiones where id_inversion > 0;
DELETE FROM Activos_Inversion where id_activo > 0;
DELETE FROM Transacciones where id_transaccion > 0;
DELETE FROM Cuentas_Bancarias where id_cuenta > 0;
DELETE FROM Jugador_Tecnicas where id_jugador > 0 OR id_tecnica > 0;
DELETE FROM Jugadores where id_jugador > 0;
DELETE FROM Equipos where id_equipo > 0;
DELETE FROM Empleados where id_empleado > 0;
DELETE FROM Sponsors where id_sponsor > 0;
DELETE FROM Partidos where id_partido > 0;
DELETE FROM Entrenos where id_entreno > 0;
DELETE FROM Ejercicios where id_ejercicio > 0;
DELETE FROM Partidos_Sponsors where id_partido > 0;


-- Inserción masiva de datos adaptada (ID, Nombre, Tipo, Afinidad, Especial, Potencia)
INSERT INTO Supertecnicas (id_tecnica, nombre, tipo, afinidad, especial, potencia) VALUES
(1, 'Tiro Fantasma', 'Tiro', 'Bosque', '-', 175),
(2, 'Bola de Fango', 'Tiro', 'Montaña', '-', 175),
(3, 'Trayectoria Perfecta', 'Tiro', 'Bosque', '-', 175),
(4, 'Disparo Rodante', 'Tiro', 'Fuego', '-', 175),
(5, 'Ataque Cóndor', 'Tiro', 'Aire', '-', 175),
(6, 'Remate Tarzán', 'Tiro', 'Montaña', '-', 175),
(7, 'Granada Doble', 'Tiro', 'Fuego', '-', 210),
(8, 'Peces Voladores', 'Tiro', 'Aire', 'Tiro Largo', 175),
(9, 'Lecho de Rosas', 'Tiro', 'Bosque', '-', 175),
(10, 'Pase Cruzado', 'Tiro', 'Aire', '-', 175),
(11, 'Remate Misil', 'Tiro', 'Fuego', 'Tiro Largo', 210),
(12, 'Balon Rodante', 'Tiro', 'Montaña', '-', 175),
(13, 'Tiro Cegador', 'Tiro', 'Bosque', '-', 210),
(14, 'Remate Seguridad', 'Tiro', 'Montaña', '-', 210),
(15, 'Bomba Saltarina', 'Tiro', 'Fuego', '-', 175),
(16, 'Tiro Sónico', 'Tiro', 'Aire', '-', 210),
(17, 'Balon de Plasma', 'Tiro', 'Bosque', 'Tiro Largo', 175),
(18, 'Flecha de Cupido', 'Tiro', 'Bosque', 'Tiro Largo', 210),
(19, 'Cabezazo Cohete', 'Tiro', 'Fuego', '-', 210),
(20, 'Megalodón', 'Tiro', 'Aire', '-', 210),
(21, 'Zapatazo Dual', 'Tiro', 'Fuego', '-', 210),
(22, 'Ataque de Paladín', 'Tiro', 'Montaña', '-', 210),
(23, 'Tiro Radiocontrol', 'Tiro', 'Bosque', 'Tiro Largo', 210),
(24, 'Cañonazo Pedregoso', 'Tiro', 'Fuego', '-', 175),
(25, 'Tiro Espectral', 'Tiro', 'Bosque', '-', 210),
(26, 'Balon Iceberg', 'Tiro', 'Aire', '-', 255),
(27, 'Tirachinas', 'Tiro', 'Bosque', '-', 312),
(28, 'Doble Hélice', 'Tiro', 'Bosque', '-', 210),
(29, 'Remate en V', 'Tiro', 'Fuego', '-', 210),
(30, 'Remate del Águila', 'Tiro', 'Aire', 'Tiro Largo', 210),
(31, 'Llamarada Atómica', 'Tiro', 'Fuego', 'Contraataque', 255),
(32, 'Fuego Supremo', 'Tiro', 'Fuego', '-', 210),
(33, 'Ciclón Blanco', 'Tiro', 'Aire', 'Contraataque', 210),
(34, 'Estrella Oscura', 'Tiro', 'Bosque', '-', 255),
(35, 'Golpe de Samba', 'Tiro', 'Fuego', 'Tiro Largo', 210),
(36, 'Remate Celestial', 'Tiro', 'Aire', 'Tiro Largo', 255),
(37, 'Caída Planetaria', 'Tiro', 'Montaña', '-', 210),
(38, 'Triple Amenaza', 'Tiro', 'Fuego', '-', 370),
(39, 'Rayo Oscuro', 'Tiro', 'Bosque', '-', 255),
(40, 'Acelerón', 'Regate', 'Fuego', '-', 175),
(41, 'Torbellino Dragón', 'Regate', 'Aire', '-', 175),
(42, 'Giro de Mono', 'Regate', 'Montaña', '-', 175),
(43, 'Aro-Alehop', 'Regate', 'Aire', '-', 454),
(44, 'Rematerialización', 'Regate', 'Bosque', '-', 210),
(45, 'Patada Canguro', 'Regate', 'Montaña', '-', 175),
(46, 'Barrena Voladora', 'Regate', 'Aire', '-', 210),
(47, 'Tacón Infernal', 'Regate', 'Fuego', '-', 210),
(48, 'Finta Bumerán', 'Regate', 'Aire', '-', 175),
(49, 'Balón Galgo', 'Regate', 'Bosque', '-', 175),
(50, 'Regate Aurora', 'Regate', 'Aire', '-', 255),
(51, 'Mina Terrestre', 'Regate', 'Montaña', '-', 210),
(52, 'Ataque de Garza', 'Regate', 'Aire', '-', 210),
(53, 'Salto Explosivo', 'Regate', 'Fuego', '-', 210),
(54, 'Truco Balonazo', 'Regate', 'Bosque', '-', 175),
(55, 'Regate Multiple', 'Regate', 'Aire', '-', 175),
(56, 'Carrera a 3 Piernas', 'Regate', 'Fuego', '-', 210),
(57, 'Cruce Explosivo', 'Regate', 'Fuego', '-', 255),
(58, 'Engaño Torero', 'Regate', 'Fuego', '-', 175),
(59, 'Entrada Tormentosa', 'Regate', 'Aire', '-', 255),
(60, 'Himno de Atenea', 'Regate', 'Bosque', '-', 255),
(61, 'Entrada Tormentosa Plus', 'Regate', 'Aire', '-', 255),
(62, 'Pantalla Acuática', 'Regate', 'Aire', '-', 210),
(63, 'Patín Aéreo', 'Regate', 'Aire', '-', 255),
(64, 'Pisotón de Sumo', 'Defensa', 'Montaña', '-', 175),
(65, 'Mirada Ladrona', 'Defensa', 'Bosque', '-', 175),
(66, 'Ataque Afilado', 'Defensa', 'Bosque', '-', 175),
(67, 'Presa Fractal', 'Defensa', 'Aire', '-', 255),
(68, 'Paisaje Helado', 'Defensa', 'Aire', '-', 210),
(69, 'Superpisotón de Sumo', 'Defensa', 'Montaña', 'Bloqueo', 210),
(70, 'Estela Ígnea', 'Defensa', 'Fuego', '-', 210),
(71, 'Picadora', 'Defensa', 'Bosque', '-', 210),
(72, 'Bola Falsa', 'Defensa', 'Montaña', '-', 175),
(73, 'Bobina Eléctrica', 'Defensa', 'Aire', '-', 210),
(74, 'Carga de Elefantes', 'Defensa', 'Montaña', '-', 255),
(75, 'Dientes de Roca', 'Defensa', 'Montaña', '-', 210),
(76, 'Pinzas Cortantes', 'Defensa', 'Bosque', '-', 210),
(77, 'Magnetismo', 'Defensa', 'Bosque', '-', 175),
(78, 'Barreno de Fuego', 'Defensa', 'Fuego', '-', 210),
(79, 'Zona de Seguridad', 'Defensa', 'Aire', '-', 175),
(80, 'Entrada Rodante', 'Defensa', 'Montaña', '-', 210),
(81, 'Obstrucción Eléctrica', 'Defensa', 'Bosque', 'Bloqueo', 175),
(82, 'Segadora', 'Defensa', 'Bosque', '-', 210),
(83, 'Campo Minado', 'Defensa', 'Montaña', 'Bloqueo', 210),
(84, 'Proyectil de Ozono', 'Defensa', 'Aire', 'Bloqueo', 210),
(85, 'Estrella Fugaz', 'Defensa', 'Fuego', 'Bloqueo', 210),
(86, 'Voltereta Circense', 'Defensa', 'Bosque', '-', 210),
(87, 'Emboscada Defensiva', 'Defensa', 'Montaña', '-', 312),
(88, 'Maza Rocosa', 'Defensa', 'Montaña', '-', 454),
(89, 'Corte Diabólico', 'Defensa', 'Bosque', 'Bloqueo', 255),
(90, 'Escudo Corporal', 'Defensa', 'Fuego', '-', 312),
(91, 'Ilusión Deslumbrante', 'Defensa', 'Bosque', '-', 312),
(92, 'Despeje de Leñador', 'Parada', 'Montaña', 'Despeje', 210),
(93, 'Parada en Pirueta', 'Parada', 'Aire', '-', 175),
(94, 'Captura Excelente', 'Parada', 'Montaña', '-', 175),
(95, 'Despeje Cohete', 'Parada', 'Fuego', 'Despeje', 210),
(96, 'Torbellino', 'Parada', 'Aire', '-', 175),
(97, 'Tormenta de Pétalos', 'Parada', 'Bosque', '-', 175),
(98, 'Gran Barrera de Coral', 'Parada', 'Aire', '-', 210),
(99, 'Espada Defensora', 'Parada', 'Bosque', '-', 210),
(100, 'Ancla de Navío', 'Parada', 'Montaña', 'Despeje', 175),
(101, 'Agujero Negro', 'Parada', 'Bosque', '-', 210),
(102, 'Presa de Sombras', 'Parada', 'Bosque', '-', 175),
(103, 'Rayo de Puño', 'Parada', 'Fuego', 'Despeje', 255),
(104, 'Tormenta del Desierto', 'Parada', 'Aire', '-', 210),
(105, 'Parada de Capoeira', 'Parada', 'Bosque', '-', 210),
(106, 'Martillo Defensor', 'Parada', 'Montaña', '-', 210),
(107, 'Escudo Protector', 'Parada', 'Bosque', '-', 210),
(108, 'Rompetiros', 'Parada', 'Montaña', 'Despeje', 210),
(109, 'Araña Gigante', 'Parada', 'Bosque', '-', 210),
(110, 'Barrera de Cristal', 'Parada', 'Aire', '-', 210),
(111, 'Despeje Medialuna', 'Parada', 'Aire', 'Despeje', 255),
(112, 'Bloqueo Doble', 'Parada', 'Bosque', '-', 210),
(113, 'Puños Voladores', 'Parada', 'Fuego', 'Despeje', 255),
(114, 'Corte de Arena', 'Parada', 'Montaña', '-', 255),
(115, 'Puente sin Final', 'Parada', 'Bosque', '-', 255),
(116, 'Centro de Gravedad', 'Parada', 'Montaña', '-', 255),
(117, 'Colmillo de Pantera', 'Parada', 'Fuego', '-', 312),
(118, 'Millon de Manos', 'Parada', 'Bosque', 'Despeje', 255),
(119, 'Aplastamiento', 'Defensa', 'Montaña', '-', 210),
(120, 'Lanzallamas', 'Defensa', 'Fuego', '-', 175),
(121, 'Baile de Llamas', 'Regate', 'Fuego', '-', 255),
(122, 'Ataque Sombrío', 'Regate', 'Bosque', '-', 210),
(123, 'Telaraña', 'Defensa', 'Bosque', '-', 210),
(124, 'Fortaleza Nocturna', 'Defensa', 'Bosque', 'Bloqueo', 312),
(125, 'Robo Rápido', 'Defensa', 'Aire', '-', 175),
(126, 'A Todo Vapor', 'Regate', 'Aire', '-', 175),
(127, 'Remolino Acuático', 'Regate', 'Aire', '-', 175),
(128, 'Chut Ancestral', 'Tiro', 'Montaña', '-', 210),
(129, 'Llamarada Infernal', 'Tiro', 'Fuego', '-', 210),
(130, 'Chut Congelante', 'Tiro', 'Aire', '-', 175),
(131, 'Cascabel', 'Tiro', 'Bosque', '-', 175),
(132, 'Vaselina', 'Tiro', 'Neutro', '-', 115),
(133, 'Tiro con Efecto', 'Tiro', 'Neutro', '-', 115),
(134, 'Tiro con Rebote', 'Tiro', 'Neutro', '-', 115),
(135, 'Tiro Potente', 'Tiro', 'Neutro', '-', 115),
(136, 'Tiro Rasante', 'Tiro', 'Neutro', '-', 115),
(137, 'Chut de 100 Toques', 'Tiro', 'Montaña', '-', 175),
(138, 'Chut Granada', 'Tiro', 'Fuego', '-', 175),
(139, 'Tiro Giratorio', 'Tiro', 'Aire', '-', 175),
(140, 'Remate Múltiple', 'Tiro', 'Bosque', '-', 175),
(141, 'Remate Espiral', 'Tiro', 'Aire', '-', 175),
(142, 'Disparo con Rebotes', 'Tiro', 'Montaña', '-', 210),
(143, 'Cabezazo Kung-Fu', 'Tiro', 'Bosque', '-', 175),
(144, 'Flecha Divina', 'Tiro', 'Aire', '-', 210),
(145, 'Tornado Inverso', 'Tiro', 'Fuego', 'Contraataque', 210),
(146, 'Tornado Oscuro', 'Tiro', 'Bosque', 'Contraataque', 210),
(147, 'Arcoíris Luminoso', 'Tiro', 'Aire', 'Tiro Largo', 175),
(148, 'Bomba Acrobática', 'Tiro', 'Fuego', '-', 210),
(149, 'Ventisca de Fuego', 'Tiro', 'Fuego', '-', 312),
(150, 'Remate Triple', 'Tiro', 'Aire', 'Tiro Largo', 312),
(151, 'Fuego Cruzado', 'Tiro', 'Fuego', '-', 370),
(152, 'Torbellino Trampolín', 'Tiro', 'Aire', '-', 312),
(153, 'Tiro Espejismo', 'Tiro', 'Bosque', '-', 210),
(154, 'Mandíbulas Dobles', 'Tiro', 'Fuego', '-', 210),
(155, 'Carga Negativa', 'Tiro', 'Fuego', 'Contraataque', 210),
(156, 'Lluvia de Azufre', 'Tiro', 'Fuego', '-', 255),
(157, 'Leopardo de la Ventisca', 'Tiro', 'Aire', '-', 255),
(158, 'Tiro Rotativo', 'Tiro', 'Montaña', '-', 175),
(159, 'Engranajes Aceleradores', 'Tiro', 'Fuego', '-', 210),
(160, 'Doble Trallazo Planetario', 'Tiro', 'Fuego', 'Contraataque', 255),
(161, 'Trueno de Primavera', 'Tiro', 'Bosque', '-', 370),
(162, 'Espada Solar', 'Tiro', 'Fuego', '-', 210),
(163, 'Destello de Fuerza', 'Tiro', 'Aire', '-', 210),
(164, 'Tiro Rotatorio', 'Tiro', 'Montaña', '-', 175),
(165, 'Tiro Tramposo', 'Tiro', 'Bosque', '-', 255),
(166, 'Tiro de Francotirador', 'Tiro', 'Fuego', '-', 175),
(167, 'Desfile de Pinguinos', 'Tiro', 'Bosque', '-', 312),
(168, 'Tornado de Fuego Meteoro', 'Tiro', 'Fuego', '-', 312),
(169, 'Remate Bahamut', 'Tiro', 'Fuego', '-', 255),
(170, 'Trueno Celestial', 'Tiro', 'Aire', '-', 210),
(171, 'Entrada Cegadora', 'Regate', 'Neutro', '-', 115),
(172, 'Finta de Pie', 'Regate', 'Neutro', '-', 115),
(173, 'Taconazo', 'Regate', 'Neutro', '-', 115),
(174, 'Cañonazo', 'Regate', 'Neutro', '-', 115),
(175, 'Amago y Esprint', 'Regate', 'Neutro', '-', 115),
(176, 'Paso Engañoso', 'Regate', 'Neutro', '-', 115),
(177, 'Zigzag Chispeante', 'Regate', 'Aire', '-', 175),
(178, 'Superarmadillo', 'Regate', 'Montaña', '-', 175),
(179, 'Imagen Residual', 'Regate', 'Bosque', '-', 210),
(180, 'Regate Topo', 'Regate', 'Montaña', '-', 175),
(181, 'Regate Espejismo', 'Regate', 'Bosque', '-', 210),
(182, 'Pared Solitaria', 'Regate', 'Fuego', '-', 210),
(183, 'Aikido', 'Regate', 'Aire', '-', 255),
(184, 'Lluvia de Meteoros', 'Regate', 'Fuego', '-', 255),
(185, 'Vuelo de Ícaro', 'Regate', 'Aire', '-', 255),
(186, 'Superelástico', 'Regate', 'Bosque', '-', 210),
(187, 'Ultraluna', 'Regate', 'Aire', '-', 255),
(188, 'Círculo de Hielo', 'Regate', 'Aire', '-', 210),
(189, 'Túnel Tornado', 'Regate', 'Aire', '-', 255),
(190, 'Avance Chispeante', 'Regate', 'Fuego', '-', 210),
(191, 'Carrera Afanosa', 'Regate', 'Fuego', '-', 175),
(192, 'Resplandor Cegador', 'Regate', 'Aire', '-', 175),
(193, 'Cataratas del Niágara', 'Regate', 'Aire', '-', 210),
(194, 'Mirada Asesina', 'Regate', 'Bosque', '-', 255),
(195, 'Luz del Mariscal', 'Regate', 'Bosque', '-', 312),
(196, 'Regate Disimulado', 'Regate', 'Neutro', '-', 115),
(197, 'Carga de Hombro', 'Regate', 'Neutro', '-', 115),
(198, 'Parada en Arco', 'Parada', 'Neutro', '-', 115),
(199, 'Jugarreta', 'Regate', 'Neutro', '-', 115),
(201, 'Barrido Defensivo', 'Defensa', 'Bosque', '-', 175),
(202, 'Giro Bobina', 'Defensa', 'Aire', '-', 175),
(203, 'Sismo', 'Defensa', 'Montaña', '-', 175),
(204, 'Flecha Huracán', 'Defensa', 'Aire', '-', 210),
(205, 'Corte Giratorio', 'Defensa', 'Aire', 'Bloqueo', 175),
(206, 'Megaterremoto', 'Defensa', 'Montaña', '-', 255),
(207, 'Corte Volcánico', 'Defensa', 'Fuego', 'Bloqueo', 210),
(208, 'Ciclón Doble', 'Defensa', 'Aire', '-', 255),
(209, 'Campo Torbellino', 'Defensa', 'Bosque', 'Bloqueo', 210),
(210, 'Ángel de Nieve', 'Defensa', 'Aire', '-', 210),
(211, 'Jaula de Piedra', 'Defensa', 'Montaña', '-', 210),
(212, 'Zigzag de Fuego', 'Defensa', 'Fuego', '-', 210),
(213, 'Gran Torbellino', 'Defensa', 'Aire', '-', 210),
(214, 'Fuerza Centrífuga', 'Defensa', 'Montaña', '-', 175),
(215, 'Salto Dimensional', 'Defensa', 'Bosque', '-', 255),
(216, 'Acrobacia Robótica', 'Defensa', 'Aire', '-', 312),
(217, 'Tortuga Danzarina', 'Defensa', 'Fuego', '-', 210),
(218, 'Fórmula Matemática', 'Defensa', 'Bosque', '-', 175),
(219, 'Submarinismo Salvaje', 'Defensa', 'Aire', '-', 210),
(220, 'El Fuerte', 'Defensa', 'Montaña', '-', 312),
(221, 'Maremoto Umibozu', 'Defensa', 'Aire', '-', 255),
(222, 'Despeje de Puño', 'Parada', 'Neutro', 'Despeje', 115),
(223, 'Parada Poderosa', 'Parada', 'Neutro', '-', 115),
(224, 'Parada de Mano Firme', 'Parada', 'Neutro', '-', 115),
(225, 'Escudo de Fuerza', 'Parada', 'Fuego', 'Despeje', 210),
(226, 'Campo de Fuerza', 'Parada', 'Fuego', '-', 175),
(227, 'Bloqueo Firme', 'Parada', 'Bosque', '-', 210),
(228, 'Escudo de Fuerza Total', 'Parada', 'Fuego', 'Despeje', 255),
(229, 'Combustión', 'Parada', 'Fuego', '-', 210),
(230, 'Bloque de Hielo', 'Parada', 'Aire', '-', 210),
(231, 'Golpes de Luz', 'Parada', 'Aire', '-', 210),
(232, 'Rechace de Fuego', 'Parada', 'Fuego', 'Despeje', 210),
(233, 'Guardia del Coliseo', 'Parada', 'Fuego', 'Despeje', 210),
(234, 'Malla Eléctrica', 'Parada', 'Aire', '-', 210),
(235, 'Despeje de Fuerza', 'Parada', 'Fuego', '-', 175),
(236, 'Mordisco de Serpiente', 'Parada', 'Bosque', '-', 255),
(237, 'Giroparada', 'Parada', 'Fuego', '-', 210),
(238, 'Gracia Glacial', 'Parada', 'Aire', '-', 210),
(239, 'Extramundo', 'Parada', 'Bosque', '-', 255),
(240, 'Gota de la Quietud', 'Parada', 'Aire', '-', 175),
(241, 'Duna Gravitacional', 'Parada', 'Montaña', '-', 210),
(242, 'Pincelada Silenciosa', 'Parada', 'Aire', '-', 210),
(243, 'Escudo Real', 'Parada', 'Bosque', 'Despeje', 312),
(244, 'Mano Celestial del Tigre', 'Parada', 'Montaña', '-', 312),
(245, 'Abrazo Divino', 'Parada', 'Aire', '-', 370),
(246, 'Deflectores Divinos', 'Parada', 'Bosque', '-', 370),
(247, 'Trampolín Relámpago', 'Tiro', 'Fuego', 'Contraataque', 255),
(248, 'Remate Combinado', 'Tiro', 'Fuego', '-', 255),
(249, 'Tornado de Fuego', 'Tiro', 'Fuego', '-', 210),
(250, 'Superrelámpago', 'Tiro', 'Fuego', 'Contraataque', 255),
(251, 'Remate Dragón', 'Tiro', 'Bosque', '-', 175),
(252, 'Triangulo Letal', 'Tiro', 'Bosque', '-', 255),
(253, 'Triangulo Z', 'Tiro', 'Bosque', '-', 255),
(254, 'Bateo Total', 'Tiro', 'Montaña', 'Contraataque', 210),
(255, 'Sabiduría Divina', 'Tiro', 'Aire', '-', 255),
(256, 'Tornado Dragón', 'Tiro', 'Fuego', '-', 255),
(257, 'Pájaro de Fuego', 'Tiro', 'Aire', 'Tiro Largo', 210),
(258, 'Ruptura Relámpago', 'Tiro', 'Aire', '-', 370),
(259, 'Pinguino Emperador nº2', 'Tiro', 'Bosque', '-', 312),
(260, 'Ventisca Eterna', 'Tiro', 'Aire', '-', 210),
(261, 'Tormenta de Fuego', 'Tiro', 'Fuego', '-', 255),
(262, 'Remate Tsunami', 'Tiro', 'Aire', 'Tiro Largo', 175),
(263, 'Triángulo Letal 2', 'Tiro', 'Bosque', '-', 312),
(264, 'Astrorremate', 'Tiro', 'Bosque', '-', 210),
(265, 'Cañón de Meteoritos', 'Tiro', 'Fuego', 'Contraataque', 255),
(266, 'Aullido de Lobo', 'Tiro', 'Aire', 'Tiro Largo', 255),
(267, 'Pinguino Emperador nº1', 'Tiro', 'Bosque', '-', 312),
(268, 'Lanza de Odín', 'Tiro', 'Montaña', 'Tiro Largo', 210),
(269, 'Tripegaso', 'Tiro', 'Aire', '-', 312),
(270, 'Baile de Mariposas', 'Tiro', 'Bosque', 'Contraataque', 210),
(271, 'Remate Gafas', 'Tiro', 'Bosque', '-', 175),
(272, 'Remate Combinado F', 'Tiro', 'Bosque', '-', 312),
(273, 'Remate Guiverno', 'Tiro', 'Bosque', '-', 210),
(274, 'Ventisca Guiverno', 'Tiro', 'Aire', '-', 255),
(275, 'Fénix Oscuro', 'Tiro', 'Bosque', '-', 370),
(276, 'Cabezazo Megatón', 'Tiro', 'Montaña', 'Contraataque', 255),
(277, 'Excalibur', 'Tiro', 'Aire', 'Tiro Largo', 210),
(278, 'Gran Tifón', 'Tiro', 'Aire', '-', 210),
(279, 'Tormenta del Tigre', 'Tiro', 'Fuego', '-', 312),
(280, 'Megadragón', 'Tiro', 'Bosque', '-', 255),
(281, 'Círculo de Espadas', 'Tiro', 'Fuego', 'Tiro Largo', 255),
(282, 'Espada de Odín', 'Tiro', 'Aire', '-', 255),
(283, 'Huracán', 'Tiro', 'Aire', '-', 312),
(284, 'Torbellino de Fuego', 'Tiro', 'Fuego', '-', 312),
(285, 'Cañón X', 'Tiro', 'Fuego', '-', 255),
(286, 'Lanza Letal', 'Tiro', 'Fuego', 'Tiro Largo', 210),
(287, 'Tiro Vendaval', 'Tiro', 'Aire', '-', 175),
(288, 'Proyectil Letal', 'Tiro', 'Bosque', '-', 210),
(289, 'Salto Incandescente', 'Tiro', 'Fuego', 'Contraataque', 175),
(291, 'Triángulo ZZ', 'Tiro', 'Aire', '-', 312),
(292, 'Pentagrama', 'Tiro', 'Bosque', '-', 175),
(293, 'Agujón Letal', 'Tiro', 'Bosque', '-', 255),
(295, 'Ceniza Negra', 'Tiro', 'Bosque', '-', 255),
(296, 'Remate Rebotado', 'Tiro', 'Fuego', '-', 210),
(297, 'Disparo Doble', 'Tiro', 'Aire', 'Tiro Largo', 210),
(298, 'Rizo de Dragón', 'Tiro', 'Fuego', '-', 255),
(299, 'Superremate Rebotado', 'Tiro', 'Bosque', '-', 255),
(300, 'Giro Atómico', 'Tiro', 'Fuego', '-', 255),
(301, 'Futuro Negativo', 'Tiro', 'Montaña', '-', 312),
(302, 'Tiro Amasado', 'Tiro', 'Montaña', 'Tiro Largo', 255),
(303, 'Entrada Huracán', 'Defensa', 'Aire', '-', 210),
(304, 'Espejismo de Balón', 'Regate', 'Bosque', '-', 210),
(305, 'Entrada de Llamas', 'Defensa', 'Fuego', '-', 210),
(306, 'Confusión', 'Regate', 'Montaña', '-', 175),
(307, 'Hora Celestial', 'Regate', 'Aire', '-', 312),
(308, 'Acelerrelámpago', 'Regate', 'Fuego', '-', 255),
(309, 'Cruz del Sur', 'Defensa', 'Aire', '-', 255),
(310, 'Coz 3', 'Defensa', 'Fuego', '-', 370),
(311, 'Balón Angelical', 'Regate', 'Aire', '-', 255),
(312, 'Balón Diabólico', 'Regate', 'Bosque', '-', 255),
(313, 'Mirada al Futuro', 'Defensa', 'Montaña', '-', 175),
(314, 'Arrancada', 'Regate', 'Aire', '-', 210),
(315, 'Visto y no Visto', 'Regate', 'Montaña', '-', 210),
(316, 'Brisa Deslizante', 'Regate', 'Aire', '-', 210),
(317, 'Esferas Luminosas', 'Regate', 'Bosque', '-', 255),
(318, 'Dragón Ascendente', 'Regate', 'Bosque', '-', 255),
(319, 'Guardia Acrobática', 'Regate', 'Aire', '-', 210),
(320, 'Despliegue de Señuelos', 'Regate', 'Fuego', '-', 312),
(321, 'El Muro', 'Defensa', 'Montaña', 'Bloqueo', 210),
(322, 'Manos Fantasmagóricas', 'Defensa', 'Bosque', '-', 210),
(323, 'Corro de la Patata', 'Defensa', 'Bosque', '-', 255),
(324, 'Defensa Múltiple', 'Defensa', 'Bosque', '-', 210),
(325, 'Torre Inexpugnable', 'Defensa', 'Aire', 'Bloqueo', 210),
(326, 'Flash de Fotones', 'Defensa', 'Aire', '-', 210),
(327, 'Gravitación', 'Defensa', 'Montaña', 'Bloqueo', 255),
(328, 'Zona Sigma', 'Defensa', 'Bosque', '-', 312),
(329, 'Cinto Astral', 'Defensa', 'Aire', 'Bloqueo', 255),
(330, 'Cortafuegos', 'Defensa', 'Fuego', '-', 210),
(331, 'Rompehielo', 'Defensa', 'Aire', '-', 210),
(332, 'Torre Perfecta', 'Defensa', 'Aire', '-', 312),
(333, 'Golpe de Vacío', 'Defensa', 'Aire', 'Bloqueo', 255),
(334, 'La Montaña', 'Defensa', 'Montaña', 'Bloqueo', 255),
(335, 'La Niebla', 'Defensa', 'Bosque', '-', 210),
(336, 'Red de Caza', 'Defensa', 'Bosque', 'Bloqueo', 210),
(338, 'Defensa Propulsada', 'Defensa', 'Fuego', 'Bloqueo', 255),
(339, 'Portal Dimensional', 'Defensa', 'Bosque', '-', 210),
(340, 'Luz Cegadora', 'Defensa', 'Aire', '-', 210),
(341, 'La Gran Muralla', 'Defensa', 'Montaña', 'Bloqueo', 210),
(342, 'Niebla Mística', 'Defensa', 'Bosque', 'Bloqueo', 255),
(343, 'Muralla de Atlantis', 'Defensa', 'Montaña', 'Bloqueo', 255),
(344, 'Grumo Pegapasta', 'Defensa', 'Bosque', 'Bloqueo', 210),
(345, 'Mano Celestial', 'Parada', 'Montaña', '-', 210),
(346, 'Deslizamiento', 'Regate', 'Aire', '-', 210),
(347, 'Muralla Tsunami', 'Parada', 'Aire', 'Despeje', 255),
(348, 'Mano Mágica', 'Parada', 'Montaña', '-', 312),
(349, 'Muralla Infinita', 'Parada', 'Montaña', '-', 312),
(350, 'Manos Infinitas', 'Parada', 'Bosque', '-', 312),
(351, 'Agujero de Gusano', 'Parada', 'Bosque', '-', 255),
(352, 'Constelación Estelar', 'Parada', 'Aire', '-', 255),
(353, 'Superpuño Invencible', 'Parada', 'Fuego', 'Despeje', 255),
(356, 'Mano Ultradimensional', 'Parada', 'Montaña', '-', 255),
(357, 'Puño de Furia', 'Parada', 'Fuego', '-', 255),
(358, 'Parada Ardiente', 'Parada', 'Fuego', '-', 175),
(359, 'Mano Celestial V', 'Parada', 'Aire', '-', 312),
(361, 'Técnica Kung-Fu', 'Regate', 'Bosque', 'Contraataque', 175),
(362, 'Supertrampolín Relámpago', 'Tiro', 'Aire', '-', 312),
(363, 'Rayo de Ganímedes', 'Tiro', 'Bosque', 'Contraataque', 210),
(364, 'Disparo Cósmico', 'Tiro', 'Bosque', '-', 255),
(365, 'Remate de Gaia', 'Tiro', 'Montaña', '-', 312),
(366, 'Pinguino Espacial', 'Tiro', 'Aire', '-', 370),
(367, 'Tiro Torre de Osaka', 'Tiro', 'Bosque', '-', 210),
(368, 'Remate del Tigre', 'Tiro', 'Fuego', 'Tiro Largo', 210),
(370, 'Bestia del Trueno', 'Tiro', 'Aire', '-', 255),
(371, 'Cañón de Agua', 'Tiro', 'Aire', '-', 210),
(372, 'Remate Pegaso', 'Tiro', 'Aire', '-', 255),
(373, 'Remate Unicornio', 'Tiro', 'Bosque', '-', 312),
(374, 'Cañón Celestial', 'Tiro', 'Aire', '-', 255),
(375, 'Diluvio Letal', 'Tiro', 'Aire', '-', 210),
(376, 'Pinguino Emperador 7', 'Tiro', 'Aire', 'Tiro Largo', 312),
(377, 'Tiro Balista', 'Tiro', 'Bosque', 'Tiro Largo', 210),
(378, 'Tiro Sobrenatural', 'Tiro', 'Bosque', '-', 255),
(379, 'Tiro de Arcabuz', 'Tiro', 'Bosque', 'Tiro Largo', 210),
(380, 'Viento Celestial', 'Tiro', 'Aire', '-', 255),
(381, 'Prima Donna', 'Regate', 'Aire', '-', 210),
(382, 'Danza del Viento', 'Regate', 'Aire', '-', 255),
(384, 'Pasos Aéreos', 'Regate', 'Aire', '-', 210),
(385, 'Ritmo Agresivo', 'Regate', 'Bosque', '-', 255),
(386, 'Brisa Encabritada', 'Regate', 'Aire', '-', 255),
(389, 'Ciclón', 'Defensa', 'Aire', '-', 210),
(392, 'Muro de Hierro', 'Defensa', 'Montaña', 'Bloqueo', 255),
(393, 'Subida a los Cielos', 'Defensa', 'Aire', '-', 255),
(394, 'Caída a los Infiernos', 'Defensa', 'Bosque', '-', 255),
(395, 'Robo Fantástico', 'Defensa', 'Aire', '-', 210),
(397, 'Muralla de las Montañas', 'Defensa', 'Montaña', 'Bloqueo', 210),
(398, 'Despeje de Fuego', 'Parada', 'Fuego', 'Despeje', 175),
(399, 'Espiral de Distorsión', 'Parada', 'Aire', '-', 175),
(400, 'Defensa Triple', 'Parada', 'Montaña', '-', 255),
(401, 'Muralla Gigante', 'Parada', 'Montaña', '-', 255),
(402, 'Despeje Explosivo', 'Parada', 'Fuego', '-', 210),
(404, 'Destrozataladros', 'Parada', 'Fuego', 'Despeje', 312),
(405, 'Muralla de Escudos', 'Parada', 'Bosque', 'Despeje', 175),
(406, 'Mano Celestial X', 'Parada', 'Fuego', '-', 312),
(407, 'Zona Sagrada', 'Parada', 'Aire', '-', 312),
(408, 'El Olvido', 'Parada', 'Bosque', '-', 312),
(409, 'Barrera de Gaia', 'Parada', 'Montaña', 'Despeje', 210),
(410, 'Manos Liberadas', 'Parada', 'Bosque', '-', 255),
(412, 'Parada en Plancha', 'Parada', 'Bosque', 'Despeje', 210),
(413, 'Bimano Celestial', 'Parada', 'Aire', '-', 312),
(414, 'Mano de Pinguinos', 'Parada', 'Bosque', '-', 255),
(415, 'Remate Caótico', 'Tiro', 'Fuego', '-', 370),
(416, 'Gran Lobo', 'Tiro', 'Bosque', '-', 312),
(417, 'Tiro a Reacción', 'Tiro', 'Aire', 'Tiro Largo', 370),
(418, 'Pinguino Emperador nº3', 'Tiro', 'Bosque', '-', 370),
(419, 'Mangual Letal', 'Tiro', 'Fuego', '-', 370),
(420, 'Tornado de Fuego DD', 'Tiro', 'Fuego', '-', 312),
(421, 'Ataque Omega', 'Tiro', 'Aire', '-', 370),
(422, 'Cañonazo de Fragmentos', 'Tiro', 'Montaña', 'Tiro Largo', 312),
(423, 'Golpe Cataclismo', 'Tiro', 'Montaña', '-', 255),
(424, 'La Aurora', 'Tiro', 'Aire', '-', 312),
(425, 'Supernova', 'Tiro', 'Aire', '-', 370),
(426, 'Big Bang', 'Tiro', 'Fuego', '-', 370),
(427, 'La Tierra', 'Tiro', 'Montaña', '-', 370),
(428, 'Cometa Legendario', 'Tiro', 'Fuego', '-', 370),
(430, 'Muro Dimensional', 'Parada', 'Bosque', 'Despeje', 312),
(431, 'Mano Diabólica', 'Parada', 'Bosque', '-', 370),
(432, 'Mano Espiritual', 'Parada', 'Fuego', '-', 370),
(433, 'Parada Celestial', 'Parada', 'Montaña', '-', 370),
(434, 'Disparo Sagrado', 'Tiro', 'Aire', '-', 312),
(435, 'Zero Magnum', 'Tiro', 'Neutro', 'Contraataque', 312),
(436, 'Fénix', 'Tiro', 'Fuego', '-', 312),
(437, 'Descenso Estelar', 'Tiro', 'Aire', '-', 312),
(438, 'Fuego Total', 'Tiro', 'Fuego', 'Tiro Largo', 370),
(439, 'Impulso Equipo Definitivo', 'Tiro', 'Neutro', '-', 370),
(440, 'Furia de los Elemenos', 'Tiro', 'Fuego', '-', 312),
(441, 'Pinguino Emperador X', 'Tiro', 'Fuego', '-', 312),
(442, 'Voltaje dual', 'Parada', 'Fuego', '-', 370),
(443, 'Mano Omega', 'Parada', 'Montaña', '-', 370);

USE InaManager;

-- ==========================================
-- 1. FORMACIÓN
-- ==========================================
INSERT INTO Formaciones (nombre, slot_1, slot_2, slot_3, slot_4, slot_5, slot_6, slot_7, slot_8, slot_9, slot_10, slot_11)
VALUES ('4-4-2 Royal', 'PR', 'LD', 'DFCD', 'DFCI', 'LI', 'MD', 'MCDD', 'MCDI', 'MI', 'DCD', 'DCI');

-- ==========================================
-- 2. EMPLEADOS (Directores y Staff)
-- ==========================================
INSERT INTO Empleados 
(id_empleado, nombre, apellido, email, username, password, telefono, puesto, especialidad, años_experiencia, salario, url_imagen, id_formacion_activa , entrenador_titular) 
VALUES
(99, 'Ray', 'Dark', 'ray.dark@royalacademy.com', 'DARK', 'DarkPass', 600123456, 'Director', 'Tácticas Prohibidas', 25, 500000.00, '/Images/Staff/ray_dark.png', 1 , true),
(2, 'Celia', 'Hills', 'celia.hills@inazuma.com', 'CELIA', 'CeliaPass', 600987654, 'Manager', 'Análisis', 2, 1200.00, '/Images/Staff/celia_hills.png', NULL , false),
(3, 'Travis', 'Percival', 'coach.royal@academy.com', 'ROYAL', 'RoyalPass', 655443322, 'Entrenador', 'Físico', 10, 5000.00, '/Images/Staff/percival_travis.png', 1 , false),
-- Directores para los nuevos equipos
(10, 'Chester', 'Horse Sr.', 'chester@wild.com', 'chester', 'pass', 610000001, 'Director', 'General', 10, 2000.00, '', NULL, false),
(11, 'Seymour', 'Hillman', 'hillman@raimon.com', 'hillman', 'pass', 610000002, 'Director', 'General', 15, 2500.00, '', NULL, false),
(12, 'Percy', 'Travis', 'travis@raimon.com', 'ptravis', 'pass', 610000003, 'Director', 'General', 12, 2200.00, '', NULL, false),
(13, 'Aquilina', 'Schiller', 'lina@raimon.com', 'lina', 'pass', 610000004, 'Director', 'General', 8, 2800.00, '', NULL, false),
(14, 'Astram', 'Schiller', 'astram@genesis.com', 'astram', 'pass', 610000005, 'Director', 'General', 20, 5000.00, '', NULL, false),
(15, 'Garo', 'Garo', 'garo@ogre.com', 'garo', 'pass', 610000006, 'Director', 'General', 10, 3000.00, '', NULL, false),
(16, 'Ken', 'Saito', 'ken@brain.com', 'ken', 'pass', 610000007, 'Director', 'General', 5, 1500.00, '', NULL, false),
(17, 'Ray', 'Dark 2', 'dark2@zeus.com', 'dark2', 'pass', 610000008, 'Director', 'General', 25, 4000.00, '', NULL, false),
(18, 'Mac', 'Robingo', 'mac@nagumohara.com', 'mac', 'pass', 610000009, 'Director', 'General', 10, 3000.00, '', NULL, false),
(19, 'Mr.', 'Yi', 'yi@seishou.com', 'yi', 'pass', 610000010, 'Director', 'General', 10, 3000.00, '', NULL, false),
(20, 'David', 'Evans', 'david@littlegigantes.com', 'david', 'pass', 610000011, 'Director', 'General', 30, 6000.00, '', NULL, false),
(21, 'Maddie', 'Moonlight', 'maddie@marytimes.com', 'maddie', 'pass', 610000012, 'Director', 'General', 10, 2500.00, '', NULL, false),
(22, 'Hugo', 'Talisman', 'hugo@occult.com', 'hugo', 'pass', 610000013, 'Director', 'General', 5, 2000.00, '', NULL, false),
(23, 'Mr.', 'D', 'mrd@orpheus.com', 'mrd', 'pass', 610000014, 'Director', 'General', 20, 4500.00, '', NULL, false),
(24, 'Robert', 'Froste', 'robert@alpine.com', 'robert', 'pass', 610000015, 'Director', 'General', 15, 2500.00, '', NULL, false)
ON DUPLICATE KEY UPDATE id_empleado=id_empleado;

-- ==========================================
-- 3. EQUIPOS
-- ==========================================
INSERT INTO Equipos (id_equipo, nombre_equipo, fk_director, url_escudo) VALUES 
(1, 'Royal Academy', 99, '/Images/Teams/royal_academy.png'),
(2, 'Raimon FC', 11, '/Images/Teams/raimon.png'),
(3, 'Wild Junior High', 10, '/Images/Teams/wild.png'),
(4, 'Team Ogre', 15, '/Images/Teams/ogres.png'),
(5, 'Zeus', 17, '/Images/Teams/zeus.png'),
(6, 'South Cirrus', 18, '/Images/Teams/soth_cirrus.png'),
(7, 'Polaris', 19, '/Images/Teams/polaris.png'),
(8, 'Little Gigantes', 20, '/Images/Teams/pequenos_gigantes.png'),
(9, 'Mary Times', 21, '/Images/Teams/mary_time.png'),
(10, 'Occult', 22, '/Images/Teams/occult.png'),
(11, 'Orpheus', 23, '/Images/Teams/orfeo.png'),
(12, 'Alpine', 24, '/Images/Teams/alpino.png'),
(13, 'Brainwashing', 16, '/Images/Teams/brain.png'),
(14, 'Genesis', 14, '/Images/Teams/genesis.png'),
(15, 'Fauxshore', 21, '/Images/Teams/fauxshore.png'),
(16, 'Shuriken', 10, '/Images/Teams/shuriken.png')
ON DUPLICATE KEY UPDATE id_equipo=id_equipo;

-- ==========================================
-- 4. JUGADORES
-- ==========================================
INSERT INTO Jugadores (id_jugador, nombre, apellido, apodo, email, telefono, username, password, dorsal, posicion, afinidad, url_imagen, id_responsable, id_equipo, es_titular, esta_convocado, clausula_rescision, sueldo) VALUES
-- --- ROYAL ACADEMY ---
(1, 'Joseph', 'King', 'King', 'joseph.king@royalacademy.jp', 600000001, 'jking', 'royal', 1, 'PR', 'Fuego', 'Images/Players/joseph_king.png', 3, 1, true, true, 0, 0),
(2, 'Peter', 'Drent', 'Drent', 'peter.drent@royalacademy.jp', 600000002, 'pdrent', 'royal', 2, 'LI', 'Bosque', 'Images/Players/peter_drent.png', 99, 1, true, true, 0, 0),
(3, 'Ben', 'Simmons', 'Simmons', 'ben.simmons@royalacademy.jp', 600000003, 'bsimmons', 'royal', 3, 'DFCI', 'Montaña', 'Images/Players/ben_simmons.png', 99, 1, true, true, 0, 0),
(4, 'Alan', 'Master', 'Master', 'alan.master@royalacademy.jp', 600000004, 'amaster', 'royal', 4, 'DFCD', 'Fuego', 'Images/Players/alan_master.png', 99, 1, true, true, 0, 0),
(5, 'Gus', 'Martin', 'Martin', 'gus.martin@royalacademy.jp', 600000005, 'gmartin', 'royal', 5, 'LD', 'Fuego', 'Images/Players/gus_martin.png', 99, 1, true, true, 0, 0),
(7, 'John', 'Bloom', 'Bloom', 'john.bloom@royalacademy.jp', 600000007, 'jbloom', 'royal', 7, 'MCDC', 'Bosque', 'Images/Players/john_bloom.png', 99, 1, true, true, 0, 0),
(8, 'Steve', 'Grimm', 'Grimm', 'steve.grimm@royalacademy.jp', 600000008, 'sgrimm', 'royal', 8, 'MCI', 'Aire', 'Images/Players/steve_grimm.png', 99, 1, true, true, 0, 0),
(6, 'Herman', 'Waldon', 'Waldon', 'herman.waldon@royalacademy.jp', 600000006, 'hwaldon', 'royal', 6, 'MCDC', 'Aire', 'Images/Players/herman_waldon.png', 99, 1, true, true, 0, 0),
(10, 'Jude', 'Sharp', 'Jude', 'jude.sharp@royalacademy.jp', 600000010, 'jsharp', 'royal', 10, 'MCO', 'Aire', 'Images/Players/jude_sharp.png', 99, 1, true, true, 0, 0),
(9, 'Daniel', 'Hatch', 'Hatch', 'daniel.hatch@royalacademy.jp', 600000009, 'dhatch', 'royal', 9, 'DCI', 'Montaña', 'Images/Players/daniel_hatch.png', 99, 1, true, true, 0, 0),
(11, 'David', 'Samford', 'Samford', 'david.samford@royalacademy.jp', 600000011, 'dsamford', 'royal', 11, 'DCD', 'Bosque', 'Images/Players/david_samford.png', 99, 1, true, true, 0, 0),
(12, 'Bob', 'Carlton', 'Carlton', 'bob.carlton@royalacademy.jp', 600000012, 'bcarlton', 'royal', 12, 'PR', 'Fuego', 'Images/Players/bob_carlton.png', 99, 1, false, true, 0, 0),
(13, 'Jim', 'Lawrenson', 'Lawrenson', 'jim.lawrenson@royalacademy.jp', 600000013, 'jlawrenson', 'royal', 13, 'DFC', 'Aire', 'Images/Players/jim_lawrenson.png', 99, 1, false, true, 0, 0),
(14, 'Barry', 'Pots', 'Pots', 'barry.pots@royalacademy.jp', 600000014, 'bpots', 'royal', 14, 'DFC', 'Bosque', 'Images/Players/barry_pots.png', 99, 1, false, true, 0, 0),
(15, 'Caleb', 'Stonewall', 'Caleb', 'caleb.stonewall@royalacademy.jp', 600000015, 'cstonewall', 'royal', 19, 'MCC', 'Fuego', 'Images/Players/caleb_stonewall.png', 99, 1, false, true, 0, 0),
(16, 'Derek', 'Swing', 'Swing', 'derek.swing@royalacademy.jp', 600000016, 'dswing', 'royal', 16, 'DC', 'Bosque', 'Images/Players/derek_swing.png', 99, 1, false, true, 0, 0),

-- --- OTROS JUGADORES (RAIMON, ALPINE, ETC) ---
(20, 'Axel', 'Blaze',   'Blaze',   'axel.blaze@raimon.jp',   600000020, 'ablaze',   'raimon', 10, 'DC',   'Fuego',  'Images/Players/axel_blaze.png',    99, 2, false, true, 15000000.00, 8000.00),
(21, 'Mark', 'Evans',   'Mark',    'mark.evans@raimon.jp',   600000021, 'mevans',   'raimon', 1,  'PR',   'Montaña','Images/Players/mark_evans.png',    99, 2, false, true, 12000000.00, 6000.00),
(22, 'Shawn','Froste',  'Froste',  'shawn.froste@raimon.jp', 600000022, 'sfroste',  'raimon', 11, 'DCI',  'Aire',   'Images/Players/shawn_froste.png',  99, 12, false, true, 9000000.00,  5000.00),
(23, 'Tod',  'Lambert', 'Tod',     'tod.lambert@raimon.jp',  600000023, 'tlambert', 'raimon', 5,  'MCDC', 'Montaña','Images/Players/tod_lambert.png',   99, 2, false, true, 7500000.00,  4500.00),
(24, 'Kevin','Dragonfly','Drag',   'kevin.d@raimon.jp',      600000024, 'kdragon',  'raimon', 4,  'LI',   'Bosque', 'Images/Players/kevin_d.png',       99, 2, false, true, 6000000.00,  4000.00),
(27, 'Erik', 'Eagle',   'Eagle',   'erik.eagle@raimon.jp',   600000027, 'eeagle',   'raimon', 9,  'ED',   'Bosque', 'Images/Players/erik_eagle.png',    99, 2, false, true, 8000000.00,  5500.00),
(28, 'Nathan','Swift',  'Swift',   'nathan.swift@raimon.jp', 600000028, 'nswift',   'raimon', 8,  'EI',   'Aire',   'Images/Players/nathan_swift.png',  99, 2, false, true, 4500000.00,  3000.00),
(31, 'Sam',  'Kincaid', 'Sam',     'sam.k@raimon.jp',        600000031, 'skincaid', 'raimon', 17, 'MCO',  'Fuego',  'Images/Players/sam_k.png',         99, 2, false, true, 6500000.00,  4200.00),
(50, 'Adrian', 'Speed', 'Cheetah', 'adrian@wild.jp', 620000050, 'aspeed', 'pass', 11, 'DC', 'Aire', 'Images/Players/adrian_speed.png', 10, 3, false, true, 3000000.00, 2000.00),
(51, 'Axel', 'Blaze', 'Blaze', 'axel2@raimon.jp', 620000051, 'ablaze2', 'pass', 10, 'DC', 'Fuego', 'Images/Players/axel_blaze.png', 11, 2, false, true, 25000000.00, 15000.00),
(52, 'Bash', 'Lancer', 'Baddap', 'bash@ogre.jp', 620000052, 'blancer', 'pass', 10, 'DC', 'Bosque', 'Images/Players/bash_lancer.png', 15, 4, false, true, 18000000.00, 10000.00),
(53, 'Byron', 'Love', 'Aphrodi', 'byron@zeus.jp', 620000053, 'blove', 'pass', 10, 'MCO', 'Aire', 'Images/Players/byron_love.png', 17, 5, false, true, 20000000.00, 12000.00),
(54, 'Cedric', 'Freud', 'Cedric', 'cedric@nagumohara.jp', 620000054, 'cfreud', 'pass', 8, 'MCI', 'Aire', 'Images/Players/cedric_freud.png', 18, 6, false, true, 4000000.00, 2500.00),
(55, 'Darren', 'LaChance', 'Darren', 'darren@raimon.jp', 620000055, 'dlachance', 'pass', 1, 'PR', 'Bosque', 'Images/Players/darren_lachance.png', 12, 15, false, true, 12000000.00, 6000.00),
(56, 'Elliot', 'Ember', 'Haizaki', 'elliot@seishou.jp', 620000056, 'eember', 'pass', 11, 'DC', 'Bosque', 'Images/Players/elliot_ember.png', 19, 7, false, true, 16000000.00, 9000.00),
(57, 'Harper', 'Evans', 'Haru', 'harper@raimon.jp', 620000057, 'hevans', 'pass', 10, 'MCO', 'Montaña', 'Images/Players/harper_evans.png', 11, 2, false, true, 22000000.00, 13000.00),
(58, 'Hector', 'Helio', 'Rococo', 'hector@littlegigantes.jp', 620000058, 'hhelio', 'pass', 1, 'PR', 'Montaña', 'Images/Players/hector_helio.png', 20, 8, false, true, 24000000.00, 14000.00),
(59, 'Hurley', 'Kane', 'Tsunami', 'hurley@marytimes.jp', 620000059, 'hkane', 'pass', 4, 'DFCD', 'Aire', 'Images/Players/hurley_kane.png', 21, 9, false, true, 10000000.00, 5500.00),
(60, 'Johan', 'Tassman', 'Talisman', 'johan@occult.jp', 620000060, 'jtassman', 'pass', 9, 'DC', 'Bosque', 'Images/Players/johan_tassman.png', 22, 10, false, true, 5000000.00, 3000.00),
(61, 'Nathan', 'Jones', 'Mask', 'njones@occult.jp', 620000061, 'njones', 'pass', 1, 'PR', 'Aire', 'Images/Players/nathan_jones.png', 22, 10, false, true, 4500000.00, 2800.00),
(62, 'Nathan', 'Swift', 'Kazemaru', 'nswift2@raimon.jp', 620000062, 'nswift2', 'pass', 2, 'LI', 'Aire', 'Images/Players/nathan_swift.png', 12, 2, false, true, 14000000.00, 7500.00),
(63, 'Paolo', 'Bianchi', 'Fideo', 'paolo@orpheus.jp', 620000063, 'pbianchi', 'pass', 10, 'DC', 'Aire', 'Images/Players/paolo_bianchi.png', 23, 11, false, true, 21000000.00, 12500.00),
(64, 'Shawn', 'Frost', 'Fubuki', 'shawn@alpine.jp', 620000064, 'sfrost', 'pass', 9, 'DCI', 'Aire', 'Images/Players/shawn_frost.png', 24, 12, false, true, 19000000.00, 11000.00),
(65, 'Thomas', 'Feldt', 'Feldt', 'thomas@brain.jp', 620000065, 'tfeldt', 'pass', 1, 'PR', 'Bosque', 'Images/Players/thomas_feldt.png', 16, 13, false, true, 8000000.00, 4500.00),
(66, 'Victor', 'Blade', 'Tsurugi', 'victor@raimon.jp', 620000066, 'vblade', 'pass', 10, 'DC', 'Fuego', 'Images/Players/victor_blade.png', 13, 2, false, true, 17000000.00, 9500.00),
(67, 'Sail', 'Bluesea', 'Otomura', 'sail@marytimes.jp', 620000067, 'sbluesea', 'pass', 7, 'MCD', 'Bosque', 'Images/Players/sail_bluesea.png', 21, 9, false, true, 6000000.00, 3500.00),
(68, 'Xavier', 'Foster', 'Xene', 'xavier.foster@genesis.jp', 620000068, 'xfoster', 'pass', 11, 'DC', 'Fuego', 'Images/Players/xene.png', 14, 14, false, true, 25000000.00, 15000.00)
ON DUPLICATE KEY UPDATE id_jugador=id_jugador;

-- ==========================================
-- 5. TÉCNICAS DE JUGADORES
-- ==========================================
-- (Joseph King)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (1, 225), (1, 228), (1, 117), (1, 404);
-- (Bob Carlton)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (12, 225), (12, 120), (12, 235);
-- (Peter Drent)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (2, 201), (2, 202), (2, 123);
-- (Ben Simmons)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (3, 201), (3, 321), (3, 389);
-- (Alan Master)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (4, 201), (4, 207), (4, 212);
-- (Gus Martin)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (5, 201), (5, 121), (5, 235);
-- (Jim Lawrenson)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (13, 201), (13, 205);
-- (Barry Pots)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (14, 304), (14, 123);
-- (Jude Sharp)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (10, 259), (10, 418), (10, 304), (10, 248);
-- (Steve Grimm)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (8, 304), (8, 125), (8, 208), (8, 252);
-- (Herman Waldon)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (6, 304), (6, 10), (6, 252);
-- (John Bloom)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (7, 304), (7, 201), (7, 252);
-- (Caleb Stonewall)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (15, 226), (15, 418), (15, 310);
-- (David Samford)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (11, 259), (11, 267), (11, 366);
-- (Daniel Hatch)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (9, 252), (9, 30), (9, 137);
-- (Derek Swing)
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica) VALUES (16, 7), (16, 24);

-- Técnicas de los nuevos jugadores
INSERT INTO Jugador_Tecnicas (id_jugador, id_tecnica, nivel_tecnica) VALUES
(50, 40, 1), (50, 139, 2), -- Adrian Speed
(51, 249, 3), (51, 261, 2), -- Axel Blaze
(52, 306, 3), -- Bash Lancer
(53, 255, 3), (53, 307, 2), -- Byron Love
(54, 304, 2), -- Cedric Freud
(55, 350, 2), (55, 348, 1), -- Darren LaChance
(56, 259, 3), -- Elliot Ember
(57, 427, 3), (57, 345, 1), -- Harper Evans
(58, 432, 3), (58, 32, 2), -- Hector Helio
(59, 262, 2), (59, 278, 2), -- Hurley Kane
(60, 20, 2), (60, 65, 1), -- Johan Tassman
(61, 223, 2), (61, 399, 1), -- Nathan Jones
(62, 257, 2), (62, 382, 2), -- Nathan Swift
(63, 282, 3), (63, 304, 2), -- Paolo Bianchi
(64, 260, 3), (64, 68, 2), -- Shawn Frost
(65, 73, 2), (65, 226, 2), -- Thomas Feldt
(66, 288, 3), (66, 415, 2), -- Victor Blade
(67, 385, 2), (67, 124, 2), -- Sail Bluesea
(68, 265, 3), (68, 425, 2) -- Xavier Foster
ON DUPLICATE KEY UPDATE nivel_tecnica=VALUES(nivel_tecnica);

-- ==========================================
-- 6. ENTRENAMIENTOS Y SPONSORS
-- ==========================================
INSERT INTO Ejercicios (nombre, categoria, descripcion) VALUES ('Potenciación Oscura', 'Específico', 'Entrenamiento de alta intensidad');
INSERT INTO Entrenos (id_jugador, id_ejercicio, id_empleado, fecha, comentarios) VALUES (16, 1, 3, CURDATE(), 'Cliff está mejorando su velocidad de tiro.');

INSERT INTO Sponsors (nombre_empresa, sector, aporte_economico, fecha_inicio, fecha_fin, url_logo) 
VALUES ('Dark Corp', 'Tecnología', 50000.00, '2024-01-01', '2024-12-31', '\\Images\\Sponsors\\dark_company.png');
INSERT INTO Partidos (rival, fecha, competicion) VALUES ('Raimon', CURDATE(), 'Fútbol Frontier');
INSERT INTO Partidos_Sponsors (id_partido, id_sponsor) VALUES (1, 1);

-- ==========================================
-- 7. MERCADO DE FICHAJES
-- ==========================================
INSERT INTO Mercado (id_jugador, id_equipo, precio, fecha_fin, estado) VALUES
(12, 1, 5000000.00,  DATE_ADD(CURDATE(), INTERVAL 7  DAY), 'disponible'),
(14, 1, 3500000.00,  DATE_ADD(CURDATE(), INTERVAL 5  DAY), 'disponible'),
(20, 2, 12000000.00, DATE_ADD(CURDATE(), INTERVAL 14 DAY), 'disponible'),
(21, 2, 9500000.00,  DATE_ADD(CURDATE(), INTERVAL 10 DAY), 'disponible'),
(22, 12, 7000000.00,  DATE_ADD(CURDATE(), INTERVAL 3  DAY), 'disponible'),
(23, 2, 5800000.00,  DATE_ADD(CURDATE(), INTERVAL 8  DAY), 'disponible'),
(24, 2, 4500000.00,  DATE_ADD(CURDATE(), INTERVAL 2  DAY), 'disponible'),
(27, 2, 6500000.00,  DATE_ADD(CURDATE(), INTERVAL 9  DAY), 'disponible'),
(28, 2, 3200000.00,  DATE_ADD(CURDATE(), INTERVAL 4  DAY), 'disponible'),
(31, 2, 5100000.00,  DATE_ADD(CURDATE(), INTERVAL 15 DAY), 'disponible'),
(50, 3, 3500000.00, DATE_ADD(CURDATE(), INTERVAL 5 DAY), 'disponible'),
(51, 2, 26000000.00, DATE_ADD(CURDATE(), INTERVAL 14 DAY), 'disponible'),
(52, 4, 19000000.00, DATE_ADD(CURDATE(), INTERVAL 10 DAY), 'disponible'),
(53, 5, 21000000.00, DATE_ADD(CURDATE(), INTERVAL 20 DAY), 'disponible'),
(54, 6, 4500000.00, DATE_ADD(CURDATE(), INTERVAL 3 DAY), 'disponible'),
(55, 15, 12500000.00, DATE_ADD(CURDATE(), INTERVAL 8 DAY), 'disponible'),
(56, 7, 16500000.00, DATE_ADD(CURDATE(), INTERVAL 12 DAY), 'disponible'),
(57, 2, 23000000.00, DATE_ADD(CURDATE(), INTERVAL 15 DAY), 'disponible'),
(58, 8, 25000000.00, DATE_ADD(CURDATE(), INTERVAL 30 DAY), 'disponible'),
(59, 9, 10500000.00, DATE_ADD(CURDATE(), INTERVAL 7 DAY), 'disponible'),
(60, 10, 5500000.00, DATE_ADD(CURDATE(), INTERVAL 4 DAY), 'disponible'),
(61, 10, 4800000.00, DATE_ADD(CURDATE(), INTERVAL 6 DAY), 'disponible'),
(62, 2, 14500000.00, DATE_ADD(CURDATE(), INTERVAL 11 DAY), 'disponible'),
(63, 11, 22000000.00, DATE_ADD(CURDATE(), INTERVAL 18 DAY), 'disponible'),
(64, 12, 19500000.00, DATE_ADD(CURDATE(), INTERVAL 14 DAY), 'disponible'),
(65, 13, 8500000.00, DATE_ADD(CURDATE(), INTERVAL 9 DAY), 'disponible'),
(66, 2, 17500000.00, DATE_ADD(CURDATE(), INTERVAL 13 DAY), 'disponible'),
(67, 9, 6500000.00, DATE_ADD(CURDATE(), INTERVAL 5 DAY), 'disponible'),
(68, 14, 25000000.00, DATE_ADD(CURDATE(), INTERVAL 20 DAY), 'disponible');

-- ==========================================
-- 8. CUENTAS BANCARIAS Y TRANSACCIONES
-- ==========================================
INSERT INTO Cuentas_Bancarias (id_cuenta, iban, id_jugador, id_empleado, saldo_actual) VALUES
(1, 'ES9900000000000000000099', NULL, 99, 500000.00),
(2, 'ES1000000000000000000010', 10,   NULL, 15000.00),
(3, 'ES1100000000000000000011', 11,   NULL, 5000.00);

INSERT INTO Transacciones (id_cuenta_origen, id_cuenta_destino, monto, tipo, concepto, id_jugador_relacionado) VALUES
(1, 2, 5000.00,   'Premio',   'Incentivo por rendimiento superior', 10),
(2, 1, 1000.00,   'Premio',   'Devolución de gastos no justificados', 10),
(1, 3, 2500.00,   'Salario',  'Pago de salario mensual', 11),
(NULL, 1, 100000.00, 'Sponsor', 'Inyección de capital Dark Corp', NULL);

-- ==========================================
-- 9. ACTIVOS DE INVERSIÓN
-- ==========================================
INSERT INTO Activos_Inversion (nombre, simbolo, precio_base, volatilidad, icono_emoji, descripcion) VALUES
('INAcoin',               'INA', 45000.00, 0.2000, '🪙', 'Criptomoneda oficial del ecosistema INA. Alta volatilidad y altas recompensas.'),
('Oro',                   'ORO',  1850.00, 0.0500, '🥇', 'Oro en onzas troy. Activo refugio clásico con baja volatilidad.'),
('Plata',                 'PLT',    23.50, 0.0800, '🥈', 'Plata en onzas. Metal industrial y de inversión con volatilidad media.'),
('Acciones Royal Academy','RYA',   120.00, 0.1200, '📈', 'Acciones del club de fútbol Royal Academy. Suben con cada victoria.');

SET FOREIGN_KEY_CHECKS = 1;
