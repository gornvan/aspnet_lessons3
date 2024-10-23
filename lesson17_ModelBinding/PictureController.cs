using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    public class NewPicture {
        [ModelBinder(BinderType = typeof(Base64ToImageModelBinder))]
        [FromBody]
        public required Image Image { get; set; }
    }

    [HttpPost()]
    public IActionResult UploadImage(
        NewPicture newImage)
    {
        if (newImage.Image == null)
        {
            return BadRequest("Invalid image data.");
        }

        try
        {
            // Define the path where the file should be saved
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploaded_image.png");

            // Save the image to the specified path as PNG
            newImage.Image.Save(filePath, new PngEncoder());

            return Ok($"Image saved to {filePath}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
