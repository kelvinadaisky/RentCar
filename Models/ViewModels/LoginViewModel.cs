using System.ComponentModel.DataAnnotations;

namespace RentCar.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Tc { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
    }

}
