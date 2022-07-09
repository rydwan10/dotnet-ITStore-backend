using AutoMapper;
using ITStore.Domain;
using ITStore.DTOs.Carts;
using ITStore.Persistence;
using ITStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ITStore.Services
{
    public class CartsService : BaseService, ICartsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CartsService(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartsViewDTO> AddToCarts(CartsCreateDTO data)
        {
            var newItem = _mapper.Map<Carts>(data);

            newItem.UsersId = UserId;
            newItem.CreatedBy(UserId);

            await _context.Carts.AddAsync(newItem);
            await _context.SaveChangesAsync();

            var result = await _context.Carts.Where(x => x.UsersId == UserId && x.Id == newItem.Id)
                .Include(x => x.Products).ThenInclude(x => x.Inventories)
                .Include(x => x.Products).ThenInclude(x => x.Categories)
                .Include(x => x.Products).ThenInclude(x => x.Discounts)
                .SingleOrDefaultAsync();

            var mappedResult = _mapper.Map<CartsViewDTO>(result);

            return mappedResult;
        }

        public async Task<List<CartsViewDTO>> GetCarts()
        {
            var result = await _context.Carts.Where(x => x.UsersId == UserId)
                .Include(x => x.Products).ThenInclude(x => x.Inventories)
                .Include(x => x.Products).ThenInclude(x => x.Categories)
                .Include(x => x.Products).ThenInclude(x => x.Discounts)
                .ToListAsync();

            var mappedResult = _mapper.Map<List<CartsViewDTO>>(result);

            return mappedResult;
        }

        public async Task<CartsViewDTO> RemoveFromCarts(Guid id)
        {
            var item = await _context.Carts.Where(x => x.UsersId == UserId && x.Id == id)
               .Include(x => x.Products).ThenInclude(x => x.Inventories)
               .Include(x => x.Products).ThenInclude(x => x.Categories)
               .Include(x => x.Products).ThenInclude(x => x.Discounts)
               .SingleOrDefaultAsync();

            if (item == null) return null;

            item.DeletedBy(UserId);
            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<CartsViewDTO>(item);

            return mappedResult;
        }
    }
}
