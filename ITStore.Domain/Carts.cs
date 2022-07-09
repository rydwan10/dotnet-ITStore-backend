using System;

namespace ITStore.Domain
{
    public class Carts : BaseProperties
    {
        public Guid UsersId { get; set; }
        public Guid ProductsId { get; set; }
        public int Quantity { get; set; }

        public virtual ApplicationUsers Users { get; set; }
        public virtual Products Products { get; set; }
    }
}
