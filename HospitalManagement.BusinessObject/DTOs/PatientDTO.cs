using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.BusinessObject.DTOs;

public class PatientDTO
{
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100, ErrorMessage = "Full Name must not exceed 100 characters.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Gender is required.")]
    [StringLength(10, ErrorMessage = "Gender must not exceed 10 characters.")]
    public string Gender { get; set; } = null!;

    [Required(ErrorMessage = "Date of Birth is required.")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone Number must be between 10 and 15 digits.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Phone Number must contain only digits.")]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(250, ErrorMessage = "Address must not exceed 250 characters.")]
    public string? Address { get; set; }

    [StringLength(20, ErrorMessage = "Health Insurance Code must not exceed 20 characters.")]
    public string? HealthInsuranceCode { get; set; }
}
