using System.ComponentModel.DataAnnotations;

namespace ApplicationDAL.Entities;

public class Like
{
    public int Id { get; set; }
    [Required]
    public User User { get; set; }
}