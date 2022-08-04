using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication.Models
{
    public partial class TblRefreshtoken
    {
        public string UserId { get; set; }
        [Key]
        public string TokenId { get; set; }
        public string RefreshToken { get; set; }
        public bool? IsActive { get; set; }
    }
}
