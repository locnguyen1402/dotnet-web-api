namespace IdentityApi.Controllers.Requests;

public class AssignRoleClaimsRequest
{
    public string RoleName { get; set; } = null!;
    public string[] Claims { get; set; } = new string[] { };
}