
CREATE SCHEMA exercise AUTHORIZATION postgres;

CREATE SEQUENCE exercise.football_matches_id_seq;

ALTER SEQUENCE  exercise.football_matches_id_seq
    OWNER TO postgres;

CREATE SEQUENCE exercise.football_stats_id_seq;

ALTER SEQUENCE exercise.football_stats_id_seq
    OWNER TO postgres;
    
CREATE SEQUENCE exercise.api_key_types_id_seq;

ALTER SEQUENCE exercise.api_key_types_id_seq
    OWNER TO postgres;
    
CREATE SEQUENCE exercise.api_keys_id_seq;

ALTER SEQUENCE exercise.api_keys_id_seq
    OWNER TO postgres;
    
CREATE TABLE exercise.football_matches
(
    id bigint NOT NULL DEFAULT nextval('exercise.football_matches_id_seq'::regclass),
    division character(2) COLLATE pg_catalog."default",
    date date,
    home_team character varying COLLATE pg_catalog."default" NOT NULL,
    away_team character varying COLLATE pg_catalog."default" NOT NULL,
    ft_home_goals smallint,
    ft_away_goals smallint,
    ht_home_goals smallint,
    ht_away_goals smallint,
    ft_result character(1) COLLATE pg_catalog."default" NOT NULL,
    ht_result character(1) COLLATE pg_catalog."default",
    CONSTRAINT football_matches_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE exercise.football_matches
    OWNER to postgres;

CREATE TABLE exercise.football_stats
(
    id bigint NOT NULL DEFAULT nextval('exercise.football_stats_id_seq'::regclass),
    football_match_fk bigint,
    attendance integer,
    referee text COLLATE pg_catalog."default",
    CONSTRAINT football_stats_pkey PRIMARY KEY (id),
    CONSTRAINT football_match_fkey FOREIGN KEY (football_match_fk)
        REFERENCES exercise.football_matches (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE exercise.football_stats
    OWNER to postgres;
    
CREATE TABLE exercise.api_key_types
(
    id smallint NOT NULL DEFAULT nextval('exercise.api_key_types_id_seq'::regclass),
    role text COLLATE pg_catalog."default",
    CONSTRAINT api_key_types_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE exercise.api_key_types
    OWNER to postgres;
    
CREATE TABLE exercise.api_keys
(
    id bigint NOT NULL DEFAULT nextval('exercise.api_keys_id_seq'::regclass),
    api_key_type_fk bigint,
    key text COLLATE pg_catalog."default",
    CONSTRAINT api_keys_pkey PRIMARY KEY (id),
    CONSTRAINT api_key_type_fkey FOREIGN KEY (api_key_type_fk)
        REFERENCES exercise.api_key_types (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE exercise.api_keys
    OWNER to postgres;
    
INSERT INTO exercise.api_key_types(role) VALUES ('developer');
INSERT INTO exercise.api_key_types(role) VALUES ('customer');

INSERT INTO exercise.api_keys(api_key_type_fk, key) VALUES (1,'1');
INSERT INTO exercise.api_keys(api_key_type_fk, key) VALUES (2,'2');
    
