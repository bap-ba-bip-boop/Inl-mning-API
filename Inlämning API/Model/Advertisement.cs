using System.ComponentModel.DataAnnotations;

namespace Inlämning_API.Model;

public class Advertisement
{
    public int Id { get; set; }
    [MaxLength(20)]
    public string? Title { get; set; }
    [MaxLength(100)]
    public string? FillerText { get; set; }
}