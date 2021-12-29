using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ITStore.Shared.Enums;

namespace ITStore.DTOs.OrderPayments
{
    public class OrderPaymentsCreateDTO
    {
        public decimal Amount { get; set; }
        public EnumProviders Provider { get; set; }
    }
}
