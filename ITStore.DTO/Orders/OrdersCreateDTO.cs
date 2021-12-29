using ITStore.DTOs.OrderPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.Orders
{
    public class OrdersCreateDTO
    {
        public decimal Total { get; set; }
        public OrderPaymentsCreateDTO OrderPayments { get; set; }

    }
}
