using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WT.IDP.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [BindProperty, Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
