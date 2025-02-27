namespace NepHubAPI.Dtos.Quiziz;

public record class ScoreDto
{    public required string UserId {get; set;}

public int QuizId { get; set; }
    public int Score { get; set; }
    public DateTime ScoredDate { get; set; }

}
