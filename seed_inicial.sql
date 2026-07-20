-- ════════════════════════════════════════════════════════════════
-- Seed inicial para base de datos LOCAL recién creada (vacía)
-- Roles + 1 usuario Administrador para poder iniciar sesión
-- ════════════════════════════════════════════════════════════════

-- 1) Roles base del sistema (según app.routes.ts: 1=Admin, 2=Supervisor, 3=Docente, 4=Estudiante)
INSERT INTO roles (rol) VALUES
  ('Administrador'),
  ('Supervisor'),
  ('Docente'),
  ('Estudiante');

-- 2) Usuario administrador de prueba
--    Correo:     admin@cidil.local
--    Contraseña: Admin123!
--    (el hash ya está generado con BCrypt, factor de trabajo 12 — compatible con BCrypt.Net-Next)
INSERT INTO usuarios (
  id_matricula, nombre_usuario, apellido_usuario, correo_institucional,
  contrasena_hash, id_rol, activado, fecha_creacion
) VALUES (
  1, 'Admin', 'CIDIL', 'admin@cidil.local',
  '$2b$12$oasBKS8tbGqnyzF4/Hifs.KCws0lm0C6Rc9tFnm/LYRstSgsaPOB6',
  1, true, now()
);
