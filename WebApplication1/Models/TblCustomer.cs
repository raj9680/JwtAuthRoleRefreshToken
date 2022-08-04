using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication.Models
{
    public partial class TblCustomer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? CreditLimit { get; set; }
    }
}
