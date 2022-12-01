using System.ComponentModel.DataAnnotations;

namespace EFCoreDemo;

public class Blog
{
    [Key]
    public int Id { get; set; }

    public string? Content { get; set; }
}