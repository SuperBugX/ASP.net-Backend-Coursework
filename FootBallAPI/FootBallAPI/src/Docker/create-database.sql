CREATE DATABASE "matchstats"
    WITH
    OWNER = "postgres"
    TEMPLATE = template0
    ENCODING = 'UTF8'
    LC_COLLATE = 'C'
    LC_CTYPE = 'C'
    TABLESPACE = 'pg_default'
    CONNECTION LIMIT = -1;

ALTER DATABASE "matchstats" SET timezone TO 'UTC';