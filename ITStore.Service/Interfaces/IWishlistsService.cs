using ITStore.DTOs.Wishlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.Services.Interfaces
{
    public interface IWishlistsService
    {
        Task<List<WishlistsViewDTO>> GetWishlists();
        Task<WishlistsViewDTO> CreateWishlists(WishlistsCreateDTO data);
        Task<WishlistsViewDTO> RemoveWishlists(Guid id);
    }
}
