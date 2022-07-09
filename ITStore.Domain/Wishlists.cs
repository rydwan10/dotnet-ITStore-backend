using System;

namespace ITStore.Domain
{
    public class Wishlists : BaseProperties
    {
        public Guid UsersId { get; set; }
        public Guid ProductsId { get; set; }

        public virtual ApplicationUsers Users { get; set; }
        public virtual Products Products { get; set; }
    }
}
