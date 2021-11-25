using System;
using static ITStore.Shared.Enums;

namespace ITStore.Domain
{
    public class OrderPayments : BaseProperties
    {
        public decimal Amount { get; set; }
        public EnumProviders Provider { get; set; }
        public EnumPaymentStatuses Status { get; set; }
    }
}
