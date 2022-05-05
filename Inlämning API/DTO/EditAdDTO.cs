using System.ComponentModel.DataAnnotations;

namespace Inlämning_API.DTO;

public class EditAdDTO
{
    [Required]
    [MaxLength(20)]
    public string? Title { get; set; }
    [Required]
    [MaxLength(100)]
    public string? FillerText { get; set; }
}
