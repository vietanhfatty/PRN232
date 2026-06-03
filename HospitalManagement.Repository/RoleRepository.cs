using HospitalManagement.DataAccess.DAOs;
using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public class RoleRepository : IRoleRepository
{
    private readonly HospitalManagementDbContext _context;

    public RoleRepository(HospitalManagementDbContext context)
    {
        _context = context;
    }

    public IQueryable<Role> GetRoles()
    {
        return RoleDAO.Instance.GetRoles(_context);
    }

    public async Task<Role?> GetRoleByIdAsync(int id)
    {
        return await RoleDAO.Instance.GetRoleByIdAsync(_context, id);
    }
}
