--
-- *************************************************
-- Copyright (c) 2020, Grindrod Bank Limited
-- License MIT: https://opensource.org/licenses/MIT
-- **************************************************
--

CREATE TABLE _a3s.function_transient
(
    id uuid NOT NULL,
    function_id uuid NOT NULL,
    name text  NOT NULL,
    description text NOT NULL,
    application_id uuid NOT NULL,
    sub_realm_id uuid,
    r_state text NOT NULL,
    changed_by uuid NOT NULL,
    approval_count int NOT NULL,
    action text NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_function_transient PRIMARY KEY (id)
);

ALTER TABLE _a3s.function_transient
    OWNER to postgres;

-- Remove superflous change related columns from the function table.
ALTER TABLE _a3s.function
DROP COLUMN changed_by,
DROP COLUMN sys_period;


CREATE TABLE _a3s.function_permission_transient
(
    id uuid NOT NULL,
    function_id uuid NOT NULL,
    permission_id uuid NOT NULL,
    r_state text NOT NULL,
    changed_by uuid NOT NULL,
    approval_count int NOT NULL,
    action text NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_function_permission_transient PRIMARY KEY (id)
);

-- Remove superflous change related columns from the function permission table.
ALTER TABLE _a3s.function_permission
DROP COLUMN changed_by,
DROP COLUMN sys_period;
