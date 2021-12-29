using ITStore.DTOs.OrderItems;
using ITStore.DTOs.OrderPayments;
using ITStore.DTOs.Orders;
using ITStore.DTOs.ShippingAddresses;
using System.Collections.Generic;

namespace ITStore.DTOs.Transactions
{
    public class TransactionsViewDTO
    {
        public OrdersViewDTO Orders { get; set; }
        public List<OrderItemsViewDTO> OrderItems { get; set; }
        public ShippingAddressesViewDTO ShippingAddresses { get; set; }
    }   
}
