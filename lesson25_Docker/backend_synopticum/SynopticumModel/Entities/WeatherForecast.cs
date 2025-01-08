using SynopticumModel.Contract;
using SynopticumModel.Enums;

namespace SynopticumModel.Entities;

public class WeatherForecast: EntityBase<int>
{
    public required DateOnly? Date { get; set; }

    public required int TemperatureC { get; set; }

    public int TemperatureF
    {
        get
        {
            return 32 + (int)(TemperatureC / 0.5556);
        }
    }

    public required WeatherSummary Summary { get; set; }

    public required City City { get; set; }
}
