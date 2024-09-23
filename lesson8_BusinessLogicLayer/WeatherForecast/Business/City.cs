using System.ComponentModel.DataAnnotations;

namespace Synopticum.Business
{
    public class City
    {
        [MaxLength(200)]
        public required string Name { get; set; }

        public required Country Country { get; set; }
    }
}
