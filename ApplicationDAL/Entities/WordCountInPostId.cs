using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationDAL.Entities;

public class WordCountInPostId
{
    public int Id { get; set; }
    public IndexedWord IndexedWord { get; set; }
    public int WordCount { get; set; }
    public int PostId { get; set; }
}