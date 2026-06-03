using HospitalManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.DataAccess.DAOs;

public class PatientDAO
{
    private static PatientDAO? _instance;
    private static readonly object _instanceLock = new object();

    private PatientDAO() { }

    public static PatientDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new PatientDAO();
                }
                return _instance;
            }
        }
    }

    public IQueryable<Patient> GetPatients(HospitalManagementDbContext context)
    {
        return context.Patients.AsQueryable();
    }

    public async Task<Patient?> GetPatientByIdAsync(HospitalManagementDbContext context, int id)
    {
        return await context.Patients.FindAsync(id);
    }

    public async Task AddPatientAsync(HospitalManagementDbContext context, Patient patient)
    {
        context.Patients.Add(patient);
        await context.SaveChangesAsync();
    }

    public async Task UpdatePatientAsync(HospitalManagementDbContext context, Patient patient)
    {
        context.Entry(patient).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeletePatientAsync(HospitalManagementDbContext context, int id)
    {
        var patient = await context.Patients.FindAsync(id);
        if (patient != null)
        {
            context.Patients.Remove(patient);
            await context.SaveChangesAsync();
        }
    }
}
