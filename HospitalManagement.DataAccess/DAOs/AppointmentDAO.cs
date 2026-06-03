using HospitalManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.DataAccess.DAOs;

public class AppointmentDAO
{
    private static AppointmentDAO? _instance;
    private static readonly object _instanceLock = new object();

    private AppointmentDAO() { }

    public static AppointmentDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new AppointmentDAO();
                }
                return _instance;
            }
        }
    }

    public IQueryable<Appointment> GetAppointments(HospitalManagementDbContext context)
    {
        return context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .AsQueryable();
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(HospitalManagementDbContext context, int id)
    {
        return await context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);
    }

    public async Task AddAppointmentAsync(HospitalManagementDbContext context, Appointment appointment)
    {
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAppointmentAsync(HospitalManagementDbContext context, Appointment appointment)
    {
        context.Entry(appointment).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAppointmentAsync(HospitalManagementDbContext context, int id)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            context.Appointments.Remove(appointment);
            await context.SaveChangesAsync();
        }
    }
}
