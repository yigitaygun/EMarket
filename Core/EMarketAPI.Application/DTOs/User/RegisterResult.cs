using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarketAPI.Application.DTOs.User
{
    public class RegisterResult
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }=new List<string>();
        public string? Token { get; set; }
    }
}
