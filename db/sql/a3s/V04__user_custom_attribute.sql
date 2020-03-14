--
-- *************************************************
-- Copyright (c) 2019, Grindrod Bank Limited
-- License MIT: https://opensource.org/licenses/MIT
-- **************************************************
--

-- Diff code generated with pgModeler (PostgreSQL Database Modeler)
-- pgModeler version: 0.9.2-beta
-- Diff date: 2020-03-14 15:14:58
-- Source model: identity_server
-- Database: identity_server
-- PostgreSQL version: 11.0

-- [ Diff summary ]
-- Dropped objects: 0
-- Created objects: 2
-- Changed objects: 0
-- Truncated tables: 0

SET search_path=public,pg_catalog,_a3s;
-- ddl-end --

-- [ Created objects ] --
-- object: _a3s.user_custom_attribute | type: TABLE --
-- DROP TABLE IF EXISTS _a3s.user_custom_attribute CASCADE;
CREATE TABLE _a3s.user_custom_attribute (
	id uuid NOT NULL,
	user_id text,
	key text,
	value text,
	CONSTRAINT user_custom_attribute_pk PRIMARY KEY (id),
	CONSTRAINT uk_user_id_key_value UNIQUE (user_id,key,value)

);
-- ddl-end --
COMMENT ON TABLE _a3s.user_custom_attribute IS 'Stores the custom attributes for users';
-- ddl-end --
ALTER TABLE _a3s.user_custom_attribute OWNER TO postgres;
-- ddl-end --



-- [ Created foreign keys ] --
-- object: fk_user_custom_attribute_application_user_user_id | type: CONSTRAINT --
-- ALTER TABLE _a3s.user_custom_attribute DROP CONSTRAINT IF EXISTS fk_user_custom_attribute_application_user_user_id CASCADE;
ALTER TABLE _a3s.user_custom_attribute ADD CONSTRAINT fk_user_custom_attribute_application_user_user_id FOREIGN KEY (user_id)
REFERENCES _a3s.application_user (id) MATCH FULL
ON DELETE NO ACTION ON UPDATE NO ACTION;
-- ddl-end --

