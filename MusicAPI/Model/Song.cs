using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MusicAPI.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Duration { get; set; }

        public DateTime UploadDate { get; set; }

        public bool IsFeatured { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public string   ImageUrl { get; set;}

        [NotMapped]
        public IFormFile AudioFile{ get; set; }

        public string AudioUrl { get; set; }

        public int ArtistId { get; set; }

        public int? AlbumId { get; set; }


    }
}
