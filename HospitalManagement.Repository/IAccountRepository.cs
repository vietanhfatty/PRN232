using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public interface IAccountRepository
{
    IQueryable<Account> GetAccounts();
    Task<Account?> GetAccountByIdAsync(int id);
    Task<Account?> GetAccountByUsernameAsync(string username);
    Task AddAccountAsync(Account account);
    Task UpdateAccountAsync(Account account);
    Task DeleteAccountAsync(int id);
}
