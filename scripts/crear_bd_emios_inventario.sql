-- ============================================================
-- Script para crear la base de datos de ESCRITURA: emios_inventario
-- en el servidor MySQL/MariaDB (p. ej. el servicio MySQL de Railway).
--
-- CÓMO EJECUTARLO:
--   Con un cliente SQL (MySQL Workbench, DBeaver, mysql CLI, o la
--   consola SQL que ofrezca Railway) conectado con un usuario con
--   privilegios (root) a la instancia.
--
-- IMPORTANTE:
--   * El nombre real debe coincidir con lo que pongas en la variable
--     de entorno EMIOS_INVENTARIO_DB de la app (por defecto es
--     'emios_inventario').
--   * El usuario con el que la app se conecta (MYSQLUSER en Railway)
--     necesita privilegios sobre esta BD para poder migrar/escribir.
--     Si no se los das, la app no podrá crear las tablas.
-- ============================================================

-- 1) Crear la base de datos (idempotente).
CREATE DATABASE IF NOT EXISTS emios_inventario
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

-- 2) Dar todos los privilegios al usuario de la aplicación.
--    Sustituye 'TU_USUARIO_APP' por el valor de ${{MySQL.MYSQLUSER}}.
--    El '@%' cubre la conexión desde cualquier host (red privada de Railway).
GRANT ALL PRIVILEGES ON emios_inventario.* TO 'TU_USUARIO_APP'@'%';

-- 3) (Opcional) Si también vas a usar emios301 en la misma instancia,
--    créala y da SOLO LECTURA al usuario de la app (la app nunca la modifica):
-- CREATE DATABASE IF NOT EXISTS emios301
--   CHARACTER SET utf8mb4
--   COLLATE utf8mb4_unicode_ci;
-- GRANT SELECT ON emios301.* TO 'TU_USUARIO_APP'@'%';

-- 4) Aplicar los cambios.
FLUSH PRIVILEGES;

-- 5) Verificación (debe listar la BD):
--    SHOW DATABASES LIKE 'emios_inventario';
