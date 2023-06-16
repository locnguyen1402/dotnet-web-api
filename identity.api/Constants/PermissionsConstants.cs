namespace IdentityApi.Constants;

public class PermissionsConstants
{
    #region"User"
    public const string GET_USER = nameof(GET_USER);
    public const string GET_USERS = nameof(GET_USERS);
    public const string CREATE_USER = nameof(CREATE_USER);
    public const string UPDATE_USER = nameof(UPDATE_USER);
    public const string DELETE_USER = nameof(DELETE_USER);
    #endregion

    #region"Staff"
    public const string GET_STAFFS = nameof(GET_STAFFS);
    public const string GET_STAFF = nameof(GET_STAFF);
    public const string CREATE_STAFF = nameof(CREATE_STAFF);
    public const string UPDATE_STAFF = nameof(UPDATE_STAFF);
    public const string DELETE_STAFF = nameof(DELETE_STAFF);
    #endregion

    public static List<Permission> GetPermissionList()
    {
        var perms = new List<Permission>();

        var fields = typeof(PermissionsConstants).GetFields().Where(f => f.GetValue(typeof(PermissionsConstants)) != null).ToList();

        foreach (var prop in fields)
        {
            perms.Add(
                new(prop.Name, prop.GetValue(typeof(PermissionsConstants))!.ToString()!)
            );
        }


        return perms;
    }
}