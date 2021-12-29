using AutoMapper;
using ITStore.Domain;
using ITStore.DTOs.Carts;
using ITStore.Persistence;
using ITStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.Services
{
    public class CartsService : ICartsService
    {
        public readonly AppDbContext _context;
        public readonly IMapper _mapper;

        public CartsService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartsViewDTO> AddToCarts(CartsCreateDTO data, Guid userId)
        {
            var newItem = _mapper.Map<Carts>(data);

            newItem.UsersId = userId.ToString();
            newItem.CreatedBy(userId);

            await _context.Carts.AddAsync(newItem);
            await _context.SaveChangesAsync();

            var result = await _context.Carts.Where(x => x.UsersId == userId.ToString() && x.Id == newItem.Id)
                .Include(x => x.Products).ThenInclude(x => x.Inventories)
                .Include(x => x.Products).ThenInclude(x => x.Categories)
                .Include(x => x.Products).ThenInclude(x => x.Discounts)
                .SingleOrDefaultAsync();

            var mappedResult = _mapper.Map<CartsViewDTO>(result);

            return mappedResult;
        }

        public async Task<List<CartsViewDTO>> GetCarts(Guid userId)
        {
            var result = await _context.Carts.Where(x => x.UsersId == userId.ToString())
                .Include(x => x.Products).ThenInclude(x => x.Inventories)
                .Include(x => x.Products).ThenInclude(x => x.Categories)
                .Include(x => x.Products).ThenInclude(x => x.Discounts)
                .ToListAsync();

            var mappedResult = _mapper.Map<List<CartsViewDTO>>(result);

            return mappedResult;
        }

        public async Task<CartsViewDTO> RemoveFromCarts(Guid id, Guid userId)
        {
            var item = await _context.Carts.Where(x => x.UsersId == userId.ToString() && x.Id == id)
               .Include(x => x.Products).ThenInclude(x => x.Inventories)
               .Include(x => x.Products).ThenInclude(x => x.Categories)
               .Include(x => x.Products).ThenInclude(x => x.Discounts)
               .SingleOrDefaultAsync();

            if (item == null) return null;

            item.DeletedBy(userId);
            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<CartsViewDTO>(item);

            return mappedResult;
        }
    }
}
