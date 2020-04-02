--
-- *************************************************
-- Copyright (c) 2020, Grindrod Bank Limited
-- License MIT: https://opensource.org/licenses/MIT
-- **************************************************
--

-- This script updates any current client configurations to now require consent.
update _ids4."Clients" set "RequireConsent" = true;