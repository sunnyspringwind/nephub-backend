
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NepHubAPI.Models
{
    [Table("Richest")]
    public class Richest () {
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

    //annotations helps in api validation during runtime
    [Required][StringLength(50)] public required string Name { get; set; }

    [Required][Url] public required string Image { get; set; }   

    [Required] [StringLength(50)] public required string Designation {get; set; }

    [Required] public required string NetWorth { get; set; }
}
}
