-- This script updates any current client configurations to now require consent.
update _ids4."Clients" set "RequireConsent" = true;