using System;
using System.Collections.Generic;
using System.IO;
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
    public class SongsController : ControllerBase
    {
        private ApiDbContext dbcontext;

        public SongsController(ApiDbContext dbContext)
        {
            dbcontext = dbContext;
        }

        // GET: api/<SongsController>
        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;
            var songs = await (from song in dbcontext.Songs
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Image = song.ImageUrl,
                                   AudioUrL = song.AudioUrl
                               }).ToListAsync();  
            return Ok(songs.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        // GET api/<SongsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var song = await dbcontext.Songs.FindAsync(id);

            if (song == null)
                return BadRequest(StatusCodes.Status204NoContent);
            return Ok(song);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var songs = await (from song in dbcontext.Songs
                               where song.IsFeatured == true
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Image = song.ImageUrl,
                                   AudioUrL = song.AudioUrl
                               }).ToListAsync();
            return Ok(songs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await (from song in dbcontext.Songs
                               orderby song.UploadDate descending
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Image = song.ImageUrl,
                                   AudioUrL = song.AudioUrl
                               }).Take(5).ToListAsync();  //only returns 5 records
            return Ok(songs);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string query)
        {
            var songs = await (from song in dbcontext.Songs
                               where song.Title.StartsWith(query)
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Image = song.ImageUrl,
                                   AudioUrL = song.AudioUrl
                               }).Take(5).ToListAsync();  //only returns 5 records
            return Ok(songs);
        }

                // POST api/<SongsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            //graba la url de la imagen en Azure en la tabla de base de datos
            var ImageUrl = await FileHelper.UploadImage(song.Image);
            song.ImageUrl = ImageUrl;

            //graba el audio en Azure
            var AudioUrl = await FileHelper.UploadAudio(song.AudioFile);
            song.AudioUrl = AudioUrl;

            //graba los valores en la base de datos
            song.UploadDate = DateTime.Now;
            await dbcontext.Songs.AddAsync(song);
            await dbcontext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        // POST api/<SongsController>
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Song song)
        //{
        //    await dbcontext.Songs.AddAsync(song);
        //    await dbcontext.SaveChangesAsync();
        //    return StatusCode(StatusCodes.Status201Created);
        //}

        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Song songobj)
        {
            var song = await dbcontext.Songs.FindAsync(id);

            if (song == null)
                return BadRequest("No song found with id: " + id);

            song.Title = songobj.Title;
            song.Duration = songobj.Duration;
            dbcontext.SaveChanges();
            return Ok("Record Updated.");
        }

        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await dbcontext.Songs.FindAsync(id);
            if (song == null)
                return BadRequest("No song found with id: " + id);

            dbcontext.Songs.Remove(song); //remove no tiene opción asoncrónica, para evitar inconsistencias
            await dbcontext.SaveChangesAsync();
            return Ok("Record Deleted.");
        }
    }
}

