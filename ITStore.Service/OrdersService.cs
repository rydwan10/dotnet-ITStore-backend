using AutoMapper;
using ITStore.Domain;
using ITStore.DTOs.OrderItems;
using ITStore.DTOs.Orders;
using ITStore.DTOs.ShippingAddresses;
using ITStore.DTOs.Transactions;
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
    public class OrdersService : IOrdersService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public OrdersService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<TransactionsViewDTO> CreateOrder(Guid userId, TransactionsCreateDTO data)
        {
            var orderPayment = _mapper.Map<OrderPayments>(data.Orders.OrderPayments);
            orderPayment.CreatedBy(userId);
            await _context.OrderPayments.AddAsync(orderPayment);
            await _context.SaveChangesAsync();

            var order = _mapper.Map<Orders>(data.Orders);
            order.UsersId = userId.ToString();
            order.OrderPaymentsId = orderPayment.Id;
            order.CreatedBy(userId);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var orderItems = _mapper.Map<List<OrderItems>>(data.OrderItems);
            foreach (var item in orderItems)
            {
                item.OrdersId = order.Id;
                item.CreatedBy(userId);
            }
            await _context.OrderItems.AddRangeAsync(orderItems);
            await _context.SaveChangesAsync();

            var shippingAddress = _mapper.Map<ShippingAddresses>(data.ShippingAddresses);
            shippingAddress.OrdersId = order.Id;
            shippingAddress.CreatedBy(userId);
            await _context.ShippingAddresses.AddAsync(shippingAddress);

            await _context.SaveChangesAsync();

            var resultOrder = await _context.Orders
                                            .Include(x => x.OrderPayments)
                                            .Include(x => x.Users)
                                            .SingleOrDefaultAsync(x => x.Id == order.Id);

            var mappedResultOrder = _mapper.Map<OrdersViewDTO>(resultOrder);

            var resultOrderItems = await _context.OrderItems.Where(x => x.OrdersId == order.Id)
                                                 .Include(y => y.Products)
                                                 .ThenInclude(x => x.Inventories)
                                                 .Include(y => y.Products)
                                                 .ThenInclude(x => x.Discounts)
                                                 .Include(y => y.Products)
                                                 .ThenInclude(x => x.Categories)
                                                 .ToListAsync();
            var mappedResultOrderItems = _mapper.Map<List<OrderItemsViewDTO>>(resultOrderItems);

            var resultShippingAddress = await _context.ShippingAddresses.SingleOrDefaultAsync(x => x.OrdersId == order.Id);
            var mappedResultShippingAddress = _mapper.Map<ShippingAddressesViewDTO>(resultShippingAddress);

            return new TransactionsViewDTO
            {
                Orders = mappedResultOrder,
                OrderItems = mappedResultOrderItems,
                ShippingAddresses = mappedResultShippingAddress
            };
        }

        public async Task<List<TransactionsViewDTO>> GetAllOrders(Guid userId)
        {
            var transactionList = new List<TransactionsViewDTO>();

            var resultOrder = await _context.Orders
                                            .Include(x => x.OrderPayments)
                                            .Include(x => x.Users)
                                            .Where(x => x.UsersId == userId.ToString())
                                            .ToListAsync();

            var mappedResultOrder = _mapper.Map<List<OrdersViewDTO>>(resultOrder);

            foreach (var order in mappedResultOrder)
            {
                var resultOrderItems = await _context.OrderItems.Where(x => x.OrdersId == order.Id)
                                                     .Include(y => y.Products)
                                                     .ThenInclude(x => x.Inventories)
                                                     .Include(y => y.Products)
                                                     .ThenInclude(x => x.Discounts)
                                                     .Include(y => y.Products)
                                                     .ThenInclude(x => x.Categories)
                                                     .ToListAsync();

                var mappedResultOrderItems = _mapper.Map<List<OrderItemsViewDTO>>(resultOrderItems);

                var resultShippingAddress = await _context.ShippingAddresses.SingleOrDefaultAsync(x => x.OrdersId == order.Id);
                var mappedResultShippingAddress = _mapper.Map<ShippingAddressesViewDTO>(resultShippingAddress);

                var transaction = new TransactionsViewDTO
                {
                    Orders = order,
                    OrderItems = mappedResultOrderItems,
                    ShippingAddresses = mappedResultShippingAddress
                };

                transactionList.Add(transaction);
            }




            return transactionList;
        }

        public Task<TransactionsViewDTO> GetOrdersById(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
