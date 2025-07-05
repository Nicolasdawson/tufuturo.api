using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class GenericEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("isdeleted")]
    public bool IsDeleted { get; set; }
}