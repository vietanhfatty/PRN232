using HospitalManagement.DataAccess.DAOs;
using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public class PatientRepository : IPatientRepository
{
    private readonly HospitalManagementDbContext _context;

    public PatientRepository(HospitalManagementDbContext context)
    {
        _context = context;
    }

    public IQueryable<Patient> GetPatients()
    {
        return PatientDAO.Instance.GetPatients(_context);
    }

    public async Task<Patient?> GetPatientByIdAsync(int id)
    {
        return await PatientDAO.Instance.GetPatientByIdAsync(_context, id);
    }

    public async Task AddPatientAsync(Patient patient)
    {
        await PatientDAO.Instance.AddPatientAsync(_context, patient);
    }

    public async Task UpdatePatientAsync(Patient patient)
    {
        await PatientDAO.Instance.UpdatePatientAsync(_context, patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        await PatientDAO.Instance.DeletePatientAsync(_context, id);
    }
}
