using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class GenericCatalog : GenericEntity
{
    [Column("name")]
    public required string Name { get; set; }
}