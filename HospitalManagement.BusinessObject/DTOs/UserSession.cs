namespace HospitalManagement.BusinessObject.DTOs;

public class UserSession
{
    public int AccountId { get; set; }
    public int? StaffId { get; set; }
    public string Username { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public string? Email { get; set; }
    public string? FullName { get; set; }
}
