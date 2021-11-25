using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.Discounts
{
    public class DiscountsViewDTO
    {
        public Guid Id;
        public string Name;
        public string Description;
        public decimal DiscountPercent;
        public bool IsActive;
    }
}
