using Microsoft.AspNetCore.Authorization;

public class Permission
{
    public string Name { get; set; }
    public string? Value { get; set; }

    public Permission(string name, string value)
    {
        Name = name;
        Value = value;
    }
}