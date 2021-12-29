using System;
using static ITStore.Shared.Enums;

namespace ITStore.Domain
{
    public class Orders : BaseProperties
    {
        public string UsersId { get; set; }
        public decimal Total { get; set; }
        public Guid OrderPaymentsId { get; set; }
        public EnumOrderStatuses Status { get; set; }

        public virtual ApplicationUsers Users { get; set; }
        public virtual OrderPayments OrderPayments { get; set; }
    }
}
