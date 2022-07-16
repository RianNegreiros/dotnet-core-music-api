using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.Helpers;
using musicApi.Models;

namespace MusicApi.Controllers;

[Route("api/[controller")]
[ApiController]
public class SongsController : ControllerBase
{
    private DataContext _dataContext;

    public SongsController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Song song)
    {
        var imageUrl = await FileHelper.UploadImage(song.Image);
        song.ImageUrl = imageUrl;
        var audioUrl = await FileHelper.UploadFile(song.AudioFile);
        song.AudioUrl = audioUrl;
        song.UploadedDate = DateTime.Now;
        await _dataContext.Songs.AddAsync(song);
        await _dataContext.SaveChangesAsync();
        return StatusCode(StatusCodes.Status201Created);
    }
}