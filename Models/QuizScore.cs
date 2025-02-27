using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NepHubAPI.Models;

public class QuizScore
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int ScoreId { get; set; }
    public required string UserId { get; set; }
    public AppUser? User { get; set; }
    public required int QuizId { get; set; }
    public Quiziz? Quiziz { get; set; }
    public int Score { get; set; }
    public DateTime ScoredDate { get; set; }
}
