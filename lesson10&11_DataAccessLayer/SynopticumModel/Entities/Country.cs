using SynopticumModel.Contract;

namespace SynopticumModel.Entities;

public class Country: EntityBase<int>
{
    public required string Name { get; set; }
}
