using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NepHubAPI.Models;

public class UpdateRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int ScoreId { get; set; }
    [ForeignKey("User")]
    public int UserId {get; set;}

    public AppUser? RequestBy {get; set;}

    public required string Subject { get; set; }

    public required string Details { get; set; }

    public string? ImageUrl { get; set; }

}
