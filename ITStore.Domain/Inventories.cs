using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.Domain
{
    public class Inventories : BaseProperties
    {
        public int Quantity { get; set; }
    }
}
