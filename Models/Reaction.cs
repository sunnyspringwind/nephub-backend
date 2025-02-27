using System;

namespace NepHubAPI.Models;

public class Reaction
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public required string UserId { get; set; }
    public int EntityId { get; set; }
    public Entity? Entity { get; set; }
}

