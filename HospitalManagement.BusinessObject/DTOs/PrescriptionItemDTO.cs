using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.BusinessObject.DTOs;

public class PrescriptionItemDTO
{
    [Required(ErrorMessage = "Medicine ID is required.")]
    public int MedicineId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Dosage details are required.")]
    [StringLength(250, ErrorMessage = "Dosage must not exceed 250 characters.")]
    public string Dosage { get; set; } = null!;
}
