using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.Helpers;
using musicApi.Models;

namespace MusicApi.Controllers;

[Route("api/[controller")]
[ApiController]
public class AlbumsController : ControllerBase
{
    private DataContext _dataContext;

    public AlbumsController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Album album)
    {
        var imageUrl = await FileHelper.UploadImage(album.Image);
        album.ImageUrl = imageUrl;
        await _dataContext.Albums.AddAsync(album);
        await _dataContext.SaveChangesAsync();
        return StatusCode(StatusCodes.Status201Created);
    }
}