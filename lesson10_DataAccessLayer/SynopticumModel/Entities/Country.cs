using SynopticumModel.Contract;

namespace SynopticumModel.Entities;

public class Country: IEntity
{
    public required string Name { get; set; }
}
