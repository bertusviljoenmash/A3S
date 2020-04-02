--
-- *************************************************
-- Copyright (c) 2020, Grindrod Bank Limited
-- License MIT: https://opensource.org/licenses/MIT
-- **************************************************
--

--
-- Name: DeviceCodes; Type: TABLE; Schema: _ids4; Owner: postgres
--

CREATE TABLE _ids4."DeviceCodes" (
    "UserCode" text NOT NULL PRIMARY KEY,
    "DeviceCode" text NOT NULL,
    "SubjectId" text,
    "ClientId" text NOT NULL,
    "CreationTime" timestamp without time zone NOT NULL,
    "Expiration" timestamp without time zone NOT NULL,
    "Data" text NOT NULL
);

--
-- Name: PersistedGrants; Type: TABLE; Schema: _ids4; Owner: postgres
--

CREATE TABLE _ids4."PersistedGrants" (
    "Key" text NOT NULL PRIMARY KEY,
    "Type" text NOT NULL,
    "SubjectId" text,
    "ClientId" text NOT NULL,
    "CreationTime" timestamp without time zone NOT NULL,
    "Expiration" timestamp without time zone,
    "Data" text NOT NULL
);

--
-- Name: IX_ApiResources_Name; Type: INDEX; Schema: _ids4; Owner: postgres
--

CREATE UNIQUE INDEX "IX_DeviceCodes_DeviceCode" ON _ids4."DeviceCodes" USING btree ("DeviceCode");

--
-- Name: IX_ApiResources_Name; Type: INDEX; Schema: _ids4; Owner: postgres
--

CREATE INDEX "IX_DeviceCodes_Expiration" ON _ids4."DeviceCodes" USING btree ("Expiration");

--
-- Name: IX_ApiResources_Name; Type: INDEX; Schema: _ids4; Owner: postgres
--

CREATE INDEX "IX_PersistedGrants_Expiration" ON _ids4."PersistedGrants" USING btree ("Expiration");

--
-- Name: IX_ApiResources_Name; Type: INDEX; Schema: _ids4; Owner: postgres
--

CREATE INDEX "IX_PersistedGrants_SubjectId_ClientId_Type" ON _ids4."PersistedGrants" USING btree ("SubjectId", "ClientId", "Type");

