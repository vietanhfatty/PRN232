using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public interface IRoleRepository
{
    IQueryable<Role> GetRoles();
    Task<Role?> GetRoleByIdAsync(int id);
}
