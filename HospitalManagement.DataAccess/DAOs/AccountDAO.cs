using HospitalManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.DataAccess.DAOs;

public class AccountDAO
{
    private static AccountDAO? _instance;
    private static readonly object _instanceLock = new object();

    private AccountDAO() { }

    public static AccountDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new AccountDAO();
                }
                return _instance;
            }
        }
    }

    public IQueryable<Account> GetAccounts(HospitalManagementDbContext context)
    {
        return context.Accounts.Include(a => a.Role).AsQueryable();
    }

    public async Task<Account?> GetAccountByIdAsync(HospitalManagementDbContext context, int id)
    {
        return await context.Accounts
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.AccountId == id);
    }

    public async Task<Account?> GetAccountByUsernameAsync(HospitalManagementDbContext context, string username)
    {
        return await context.Accounts
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Username == username);
    }

    public async Task AddAccountAsync(HospitalManagementDbContext context, Account account)
    {
        context.Accounts.Add(account);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAccountAsync(HospitalManagementDbContext context, Account account)
    {
        context.Entry(account).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(HospitalManagementDbContext context, int id)
    {
        var account = await context.Accounts.FindAsync(id);
        if (account != null)
        {
            context.Accounts.Remove(account);
            await context.SaveChangesAsync();
        }
    }
}
