using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using musicApi.Models;

namespace MusicApi.Controllers;

[Route("api/[controller]")]
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

    [HttpGet]
    public async Task<IActionResult> GetAlbums()
    {
        var albums = await (from album in _dataContext.Albums
            select new
            {
                Id = album.Id,
                Name = album.Name,
                ImageUrl = album.ImageUrl
            }).ToListAsync();
        return Ok(albums);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> AlbumDetails(int albumId)
    {
        var albumDetails = await _dataContext.Albums.Where(a => a.Id == albumId).Include(a => a.Songs).ToListAsync();
        return Ok(albumDetails);
    }
}