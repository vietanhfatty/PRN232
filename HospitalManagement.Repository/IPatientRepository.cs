using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public interface IPatientRepository
{
    IQueryable<Patient> GetPatients();
    Task<Patient?> GetPatientByIdAsync(int id);
    Task AddPatientAsync(Patient patient);
    Task UpdatePatientAsync(Patient patient);
    Task DeletePatientAsync(int id);
}
