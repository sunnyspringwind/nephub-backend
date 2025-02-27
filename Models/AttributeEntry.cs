using System;

namespace NepHubAPI.Models;

public class AttributeEntry
{
    public int Id { get; set; }
    public required string Key { get; set; } // Attribute name like "Color", "Size", "Age"
    public required string Value { get; set; } // Attribute value
    public int EntityId { get; set; }
    public Entity? Entity { get; set; }
}
