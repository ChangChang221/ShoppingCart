using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;



namespace ShoppingCart.Models
{
    public class UserEdit
    {
        

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Password { get; set; }
        public UserEdit() { }
        public UserEdit(AppUser appUser)
        {
          
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
    }
}
