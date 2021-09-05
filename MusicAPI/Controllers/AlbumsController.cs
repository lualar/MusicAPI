using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Helpers;
using MusicAPI.Models;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {

        private ApiDbContext dbcontext;

        public AlbumsController(ApiDbContext dbContext)
        {
            dbcontext = dbContext;
        }

        // GET: api/<ArtistsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var albums = await (from album in dbcontext.Albums
                                 select new
                                 {
                                     Id = album.Id,
                                     AlbumsName = album.Name,
                                     ImageURL = album.Image
                                 }).ToListAsync();
            return Ok(albums);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> AlbumDetails(int AlbumId)
        {
            var artist = await dbcontext.Albums.Where(a => a.Id == AlbumId).Include(a => a.Songs).ToListAsync();
            return Ok(artist);
        }

        // POST api/<ArtistsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Album album)
        {
            //graba la url de la imagen en Azure en la tabla de base de datos
            var ImageUrl = await FileHelper.UploadImage(album.Image);
            album.ImageUrl = ImageUrl;

            //graba los valores en la base de datos
            await dbcontext.Albums.AddAsync(album);
            await dbcontext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
