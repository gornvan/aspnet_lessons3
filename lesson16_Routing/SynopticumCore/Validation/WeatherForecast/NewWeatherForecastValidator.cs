using FluentValidation;
using SynopticumCore.Contract.Interfaces.WeatherForecastService;
using SynopticumModel.Enums;

namespace SynopticumCore.Validation.WeatherForecast
{
    public class NewWeatherForecastValidator: AbstractValidator<NewWeatherForecast>
    {
        public NewWeatherForecastValidator() {
            RuleFor(f => f.Summary).Custom(BeTemperaturicallyReasonableSummary);
        }

        private void BeTemperaturicallyReasonableSummary(WeatherSummary summary, ValidationContext<NewWeatherForecast> context)
        {
            var bottomTemperature = -40;

            var expectedMinimumTemperature = (int)context.InstanceToValidate.Summary * 10 + bottomTemperature;
            var expectedMaximumTemperature = ((int)context.InstanceToValidate.Summary + 1) * 10 + bottomTemperature;

            if (
                context.InstanceToValidate.TemperatureC < expectedMinimumTemperature
                || context.InstanceToValidate.TemperatureC > expectedMaximumTemperature
               )
            {
                context.AddFailure("Summary is not temperaturically reasonable!");
            }
        }
    }
}
