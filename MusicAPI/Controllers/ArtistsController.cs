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
using Microsoft.Extensions.Configuration;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext dbcontext;
        private readonly IConfiguration _configuration;

        public ArtistsController(ApiDbContext dbContext)
        {
            dbcontext = dbContext;
            var value = _configuration["laarkeyvault"];
        }

        // GET: api/<ArtistsController>
        [HttpGet]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await (from artist in dbcontext.Artists
                                 select new
                                 {
                                     Id = artist.Id,
                                     ArtistName = artist.Name,
                                     ImageURL = artist.ImageUrl
                                 }).ToListAsync();
            return Ok(artists);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistDetails(int ArtistId)
        {
            var artist = await dbcontext.Artists.Where(a=>a.Id== ArtistId).Include(a => a.Songs).ToListAsync();
            return Ok(artist);
        }

        // POST api/<ArtistsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Artist artist)
        {
            //graba la url de la imagen en Azure en la tabla de base de datos
            artist.ImageUrl = await FileHelper.UploadImage(artist.Image); ;

            //graba los valores en la base de datos
            await dbcontext.Artists.AddAsync(artist);
            await dbcontext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
