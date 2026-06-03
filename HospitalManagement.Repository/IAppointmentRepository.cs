using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public interface IAppointmentRepository
{
    IQueryable<Appointment> GetAppointments();
    Task<Appointment?> GetAppointmentByIdAsync(int id);
    Task AddAppointmentAsync(Appointment appointment);
    Task UpdateAppointmentAsync(Appointment appointment);
    Task DeleteAppointmentAsync(int id);
}
