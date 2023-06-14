namespace IdentityApi.Settings;

internal sealed class JwtSetting
{
    public string Issuer { get; set; } = null!;
    public string LifetimeInMinutes { get; set; } = null!;
    public string Key { get; set; } = null!;
}