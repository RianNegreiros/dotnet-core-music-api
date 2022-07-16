using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.Helpers;
using musicApi.Models;

namespace MusicApi.Controllers;

[Route("api/[controller")]
[ApiController]
public class ArtistsController : ControllerBase
{
    private DataContext _dataContext;

    public ArtistsController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Artist artist)
    {
        var imageUrl = await FileHelper.UploadImage(artist.Image);
        artist.ImageUrl = imageUrl;
        await _dataContext.Artists.AddAsync(artist);
        await _dataContext.SaveChangesAsync();
        return StatusCode(StatusCodes.Status201Created);
    }
}