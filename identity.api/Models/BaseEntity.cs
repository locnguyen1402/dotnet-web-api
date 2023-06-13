using IdentityApi.Models.IModels;

namespace IdentityApi.Models;
public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; }
}