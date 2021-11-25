using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.Discounts
{
    public class DiscountsCreateDTO
    {
        public string Name;
        public decimal DiscountPercent;
        public bool IsActive;
    }
}
