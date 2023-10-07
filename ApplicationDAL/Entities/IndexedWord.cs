using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDAL.Entities;

public class IndexedWord
{
    public int Id { get; set; }
    [Required]
    [MaxLength(45)]
    public string Word { get; set; }
    [Required]
    public List<(int, int)> WordCountInPostId { get; set; }
    
}