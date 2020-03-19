/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
package za.co.grindrodbank.security.service.accesstokenpermissions;

public interface AccessTokenPermissionsService {
    public Boolean hasPermission(String permission);
}
