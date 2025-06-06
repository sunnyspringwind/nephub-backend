using System.ComponentModel.DataAnnotations;

namespace NepHubAPI.Dtos.Quiziz;

public record class UpdateQuizizDto
{
    [Required] public required string Question { get; set; }
    [Required] public required string[] Options {get; set;} = new string[4];
    [Required] public required string CorrectAnswer { get; set; }
            [Required] public required string Category { get; set; }

}
