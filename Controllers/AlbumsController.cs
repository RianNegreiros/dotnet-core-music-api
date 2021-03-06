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
    protected readonly IConfiguration Configuration;
    private DataContext _dataContext;

    public AlbumsController(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        Configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Album album)
    {
        var imageUrl = await FileHelper.UploadImage(album.Image, Configuration);
        album.ImageUrl = imageUrl;
        await _dataContext.Albums.AddAsync(album);
        await _dataContext.SaveChangesAsync();
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAlbums(int? pageNumber, int? pageSize)
    {
        int currentPageNumber = pageNumber ?? 1;
        int currentPageSize = pageSize ?? 5;
        var albums = await (from album in _dataContext.Albums
            select new
            {
                Id = album.Id,
                Name = album.Name,
                ImageUrl = album.ImageUrl
            }).ToListAsync();
        return Ok(albums.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
    }
    
}