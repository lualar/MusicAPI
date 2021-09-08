using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MusicAPI.Data
{
    public static class APIBlobConnection 
    {
        //Get Keys from Vault
        public static string sBlobAccName { get; set; }
        public static string sBlobAccKey { get; set; } 
    }
}
