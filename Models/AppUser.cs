using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace NepHubAPI.Models;

public class AppUser : IdentityUser
{
    public string? Bio { get; set; }
    public string? ImageUrl {get; set;}
    public List<QuizScore> QuizScores { get; set; } = new List<QuizScore>();
}
