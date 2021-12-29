using System;
using System.ComponentModel.DataAnnotations;

namespace ITStore.DTOs.Wishlists
{
    public class WishlistsCreateDTO
    {
        [Required]
        public Guid ProductsId { get; set; }
    }
}
