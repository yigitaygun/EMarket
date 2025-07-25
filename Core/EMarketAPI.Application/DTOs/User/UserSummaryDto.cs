using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarketAPI.Application.DTOs.User
{
    public class UserSummaryDto
    {
        //UserSummaryDto: Listeleme veya kısa özet bilgiler için (Id, Email, UserName gibi).
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }

    public class UserDetailDto : UserSummaryDto
    {

        //UserDetailDto: Kullanıcının detay sayfasında, profil bilgilerinde, ek alanlarda (Adres, Kayıt Tarihi, Rol Listesi vs.) kullanılır.
    }
}
