using ITStore.DTOs.OrderItems;
using ITStore.DTOs.OrderPayments;
using ITStore.DTOs.Orders;
using ITStore.DTOs.ShippingAddresses;
using System.Collections.Generic;

namespace ITStore.DTOs.Transactions
{
    public class TransactionsCreateDTO
    {
        public OrdersCreateDTO Orders { get; set; }
        public List<OrderItemsCreateDTO> OrderItems { get; set; }
        public ShippingAddressesCreateDTO ShippingAddresses { get; set; }
    }
}
