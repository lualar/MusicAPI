using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MusicAPI.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int ArtistId { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public ICollection<Song> Songs{ get; set; }
    }
}
