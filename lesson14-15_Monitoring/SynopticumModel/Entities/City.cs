using SynopticumModel.Contract;
using System.ComponentModel.DataAnnotations;

namespace SynopticumModel.Entities;

public class City: EntityBase<int>
{
    [MaxLength(200)]
    public required string Name { get; set; }

    public required Country Country { get; set; }
}
