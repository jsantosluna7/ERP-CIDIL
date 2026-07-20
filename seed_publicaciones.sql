-- ════════════════════════════════════════════════════════════════
-- Datos de muestra para el módulo de Publicación (CIDIL)
-- ════════════════════════════════════════════════════════════════
-- ANTES DE EJECUTAR: revisa que el usuario_id que usamos (1) exista
-- en tu tabla "usuarios". Si no, corre primero esto y ajusta el ID:
--
--   SELECT id, correo_institucional FROM usuarios LIMIT 5;
--
-- ════════════════════════════════════════════════════════════════

-- 1) Dos anuncios destacados para el carrusel (es_carrusel = true)
INSERT INTO anuncios (titulo, descripcion, imagen_url, usuario_id, es_pasantia, es_carrusel)
VALUES
  ('Apertura del Laboratorio de Robótica 2A',
   'El CIDIL inaugura su laboratorio renovado con nuevos brazos robóticos y estaciones de programación para los estudiantes de Tecnología en Desarrollo de Software.',
   NULL, 1, false, true),
  ('Nuevos sensores IoT llegan al laboratorio',
   'Se incorporaron sensores de temperatura, humedad y movimiento para las prácticas del módulo de Internet de las Cosas.',
   NULL, 1, false, true);

-- 2) Anuncios regulares (noticias del día a día)
INSERT INTO anuncios (titulo, descripcion, imagen_url, usuario_id, es_pasantia, es_carrusel)
VALUES
  ('Mantenimiento programado en Laboratorio de Química',
   'El laboratorio estará cerrado el próximo viernes de 8:00am a 12:00pm por mantenimiento preventivo de los equipos.',
   NULL, 1, false, false),
  ('Jornada de inducción para nuevos estudiantes',
   'Se realizará una jornada de bienvenida para los estudiantes de nuevo ingreso, con recorrido por los 16 laboratorios del CIDIL.',
   NULL, 1, false, false);

-- 3) Pasantías abiertas (es_pasantia = true)
INSERT INTO anuncios (titulo, descripcion, imagen_url, usuario_id, es_pasantia, es_carrusel)
VALUES
  ('Pasantía en mantenimiento de equipos de laboratorio',
   'Buscamos estudiantes o personas externas interesadas en apoyar el mantenimiento preventivo de equipos del CIDIL. Disponibilidad de 10 horas semanales.',
   NULL, 1, true, false),
  ('Pasantía en soporte de inventario y reservas',
   'Apoyo en la gestión del sistema ERP: registro de equipos, control de inventario y atención de solicitudes de reserva de laboratorios.',
   NULL, 1, true, false);

-- 4) Algunos likes y comentarios de muestra sobre el primer anuncio creado
-- (ajusta el anuncio_id si tus IDs autogenerados no empiezan en 1)
INSERT INTO likes (anuncio_id, ip_usuario)
VALUES
  (1, '192.168.1.10'),
  (1, '192.168.1.22'),
  (1, '192.168.1.35');

INSERT INTO comentarios (anuncio_id, usuario, texto)
VALUES
  (1, 'María Pérez', '¡Excelente noticia! Ya quiero ver el laboratorio nuevo.'),
  (1, 'Carlos Jiménez', '¿Habrá horario extendido para los estudiantes de la tarde?');
