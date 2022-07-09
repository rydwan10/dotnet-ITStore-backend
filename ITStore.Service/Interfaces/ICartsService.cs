using ITStore.DTOs.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.Services.Interfaces
{
    public interface ICartsService
    {
        Task<List<CartsViewDTO>> GetCarts();
        Task<CartsViewDTO> AddToCarts(CartsCreateDTO data);
        Task<CartsViewDTO> RemoveFromCarts(Guid id);
    }
}
