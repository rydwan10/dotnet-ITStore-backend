using AutoMapper;
using ITStore.Domain;
using ITStore.DTOs.Wishlists;
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
    public class WishlistsService : BaseService, IWishlistsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public WishlistsService(IMapper mapper, AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = appDbContext;
            _mapper = mapper;
        }
        public async Task<List<WishlistsViewDTO>> GetWishlists()
        {
            var wishlists = await _context.Wishlists.Where(x => x.UsersId == UserId)
                .Include(x => x.Products).ThenInclude(x => x.Inventories)
                .Include(x => x.Products).ThenInclude(x => x.Discounts)
                .Include(x => x.Products).ThenInclude(x => x.Categories)
                .ToListAsync();

            var mappedResult = _mapper.Map<List<WishlistsViewDTO>>(wishlists);

            return mappedResult;
        }

        public async Task<WishlistsViewDTO> CreateWishlists(WishlistsCreateDTO data)
        {
            var newWishlist = _mapper.Map<Wishlists>(data);

            newWishlist.UsersId = UserId;
            newWishlist.CreatedBy(UserId);

            await _context.Wishlists.AddAsync(newWishlist);
            await _context.SaveChangesAsync();

            var result = await _context.Wishlists.Where(x => x.UsersId == UserId && x.Id == newWishlist.Id)
                .Include(x => x.Products).ThenInclude(x => x.Inventories)
                .Include(x => x.Products).ThenInclude(x => x.Discounts)
                .Include(x => x.Products).ThenInclude(x => x.Categories)
                .SingleOrDefaultAsync();
            var mappedResult = _mapper.Map<WishlistsViewDTO>(result);

            return mappedResult;
        }

        public async Task<WishlistsViewDTO> RemoveWishlists(Guid id)
        {
            var wishlist = await _context.Wishlists.Where(x => x.UsersId == UserId && x.Id == id)
                .Include(x => x.Products).ThenInclude(x => x.Inventories)
                .Include(x => x.Products).ThenInclude(x => x.Discounts)
                .Include(x => x.Products).ThenInclude(x => x.Categories)
                .SingleOrDefaultAsync();

            if (wishlist == null) return null;

            wishlist.DeletedBy(UserId);
            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<WishlistsViewDTO>(wishlist);

            return mappedResult;
        }
    }
}
