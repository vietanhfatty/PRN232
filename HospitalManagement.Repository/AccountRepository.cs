using HospitalManagement.DataAccess.DAOs;
using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly HospitalManagementDbContext _context;

    public AccountRepository(HospitalManagementDbContext context)
    {
        _context = context;
    }

    public IQueryable<Account> GetAccounts()
    {
        return AccountDAO.Instance.GetAccounts(_context);
    }

    public async Task<Account?> GetAccountByIdAsync(int id)
    {
        return await AccountDAO.Instance.GetAccountByIdAsync(_context, id);
    }

    public async Task<Account?> GetAccountByUsernameAsync(string username)
    {
        return await AccountDAO.Instance.GetAccountByUsernameAsync(_context, username);
    }

    public async Task AddAccountAsync(Account account)
    {
        await AccountDAO.Instance.AddAccountAsync(_context, account);
    }

    public async Task UpdateAccountAsync(Account account)
    {
        await AccountDAO.Instance.UpdateAccountAsync(_context, account);
    }

    public async Task DeleteAccountAsync(int id)
    {
        await AccountDAO.Instance.DeleteAccountAsync(_context, id);
    }
}
