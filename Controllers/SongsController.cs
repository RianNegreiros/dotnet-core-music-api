using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using musicApi.Models;

namespace MusicApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SongsController : ControllerBase
{
    protected readonly IConfiguration Configuration;
    private DataContext _dataContext;

    public SongsController(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        Configuration = configuration;
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Song song)
    {
        var imageUrl = await FileHelper.UploadImage(song.Image, Configuration);
        song.ImageUrl = imageUrl;
        var audioUrl = await FileHelper.UploadFile(song.AudioFile, Configuration);
        song.AudioUrl = audioUrl;
        song.UploadedDate = DateTime.Now;
        await _dataContext.Songs.AddAsync(song);
        await _dataContext.SaveChangesAsync();
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
    {
        int currentPageNumber = pageNumber ?? 1;
        int currentPageSize = pageSize ?? 5;
        
        var songs = await (from song in _dataContext.Songs
            select new
            {
                Id = song.Id,
                Title = song.Title,
                Duration = song.Duration,
                ImageUrl = song.ImageUrl,
                AudioUrl = song.AudioUrl
            }).ToListAsync();
        return Ok(songs.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> FeaturedSongs()
    {
        var songs = await (from song in _dataContext.Songs
            where song.IsFeatured == true
            select new
            {
                Id = song.Id,
                Title = song.Title,
                Duration = song.Duration,
                ImageUrl = song.ImageUrl,
                AudioUrl = song.AudioUrl
            }).ToListAsync();
        return Ok(songs);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> SearchSongs(string query)
    {
        var songs = await (from song in _dataContext.Songs
            where song.Title.StartsWith(query)
            select new
            {
                Id = song.Id,
                Title = song.Title,
                Duration = song.Duration,
                ImageUrl = song.ImageUrl,
                AudioUrl = song.AudioUrl
            }).ToListAsync();
        return Ok(songs);
    }
}