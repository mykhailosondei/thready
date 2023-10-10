using System.ComponentModel.DataAnnotations;
namespace ApplicationDAL.Entities;

public class IndexedWord
{
    public int Id { get; set; }
    [Required]
    [MaxLength(45)]
    public string Word { get; set; }
    [Required]
    public List<WordCountInPostId> WordCountInPostId { get; set; }
    
}