using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationDAL.Entities;

public class IndexedUsername
{
    public int Id { get; set; }
    [Required]
    [MaxLength(15)]
    public string Username { get; set; }
    [Required]
    public int UserId { get; set; }

}