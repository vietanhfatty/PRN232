using HospitalManagement.DataAccess.DAOs;
using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly HospitalManagementDbContext _context;

    public AppointmentRepository(HospitalManagementDbContext context)
    {
        _context = context;
    }

    public IQueryable<Appointment> GetAppointments()
    {
        return AppointmentDAO.Instance.GetAppointments(_context);
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(int id)
    {
        return await AppointmentDAO.Instance.GetAppointmentByIdAsync(_context, id);
    }

    public async Task AddAppointmentAsync(Appointment appointment)
    {
        await AppointmentDAO.Instance.AddAppointmentAsync(_context, appointment);
    }

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        await AppointmentDAO.Instance.UpdateAppointmentAsync(_context, appointment);
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        await AppointmentDAO.Instance.DeleteAppointmentAsync(_context, id);
    }
}
