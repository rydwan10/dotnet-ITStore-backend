using System;
namespace ITStore.Domain
{
    public class Orders : BaseProperties
    {
        public string UsersId { get; set; }
        public decimal Total { get; set; }
        public Guid OrderPaymentsId { get; set; }

        public virtual ApplicationUsers Users { get; set; }
        public virtual OrderPayments OrderPayments { get; set; }
    }
}
