using HospitalManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.DataAccess.DAOs;

public class RoleDAO
{
    private static RoleDAO? _instance;
    private static readonly object _instanceLock = new object();

    private RoleDAO() { }

    public static RoleDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new RoleDAO();
                }
                return _instance;
            }
        }
    }

    public IQueryable<Role> GetRoles(HospitalManagementDbContext context)
    {
        return context.Roles.AsQueryable();
    }

    public async Task<Role?> GetRoleByIdAsync(HospitalManagementDbContext context, int id)
    {
        return await context.Roles.FindAsync(id);
    }
}
