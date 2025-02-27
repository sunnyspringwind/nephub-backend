using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NepHubAPI.Models;
[Table("Quiziz")]
public class Quiziz
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; } 
    [Required] public required string Question { get; set; }
    [Required] public required string[] Options {get; set;} = new string[4];
    [Required] public required string CorrectAnswer { get; set; }
    [Required] public required string Category { get; set; }
}
