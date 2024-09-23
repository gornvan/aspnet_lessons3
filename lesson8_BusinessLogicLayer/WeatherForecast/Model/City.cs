using System.ComponentModel.DataAnnotations;

namespace SynopticumCore.Model
{
    public class City
    {
        [MaxLength(200)]
        public required string Name { get; set; }

        public required Country Country { get; set; }
    }
}
