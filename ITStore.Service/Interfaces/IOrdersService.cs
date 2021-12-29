using ITStore.DTOs.Orders;
using ITStore.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<TransactionsViewDTO> CreateOrder(Guid userId, TransactionsCreateDTO data);
        Task<List<TransactionsViewDTO>> GetAllOrders(Guid userId);
        Task<TransactionsViewDTO> GetOrdersById(Guid userId, Guid id);
    }
}
