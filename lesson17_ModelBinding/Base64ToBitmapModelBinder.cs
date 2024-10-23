using Microsoft.AspNetCore.Mvc.ModelBinding;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using System.Text;


public class Base64ToImageModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        try
        {
            using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body, Encoding.UTF8))
            {
                var base64String = await reader.ReadToEndAsync();

                if (string.IsNullOrEmpty(base64String))
                {
                    return;
                }

                // Convert base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(base64String);

                // Load the image from the byte array
                using (var ms = new MemoryStream(imageBytes))
                {
                    var image = await Image.LoadAsync(ms);
                    bindingContext.Result = ModelBindingResult.Success(image);
                }
            }
        }
        catch (Exception ex)
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"Invalid base64 string: {ex.Message}");
        }

        return;
    }
}
