using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BL_Crud_Identity.Shared.DTOs.User
{
    public class UserUpdateDto
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name is too long")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name is too long")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street address is required")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Zip code is required")]
        public string ZipCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;
    }
}

