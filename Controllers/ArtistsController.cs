using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using musicApi.Models;

namespace MusicApi.Controllers;

[Route("api/[controller]")]
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

    [HttpGet]
    public async Task<IActionResult> GetArtist()
    {
        var artists = await (from artist in _dataContext.Artists
            select new
            {
                Id = artist.Id,
                Name = artist.Name,
                ImageUrl = artist.ImageUrl
            }).ToListAsync();
        return Ok(artists);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> ArtistDetails(int artistId)
    {
        var artistDetails = await _dataContext.Artists.Where(a => a.Id == artistId).Include(a => a.Songs).ToListAsync();
        return Ok(artistDetails);
    }
}