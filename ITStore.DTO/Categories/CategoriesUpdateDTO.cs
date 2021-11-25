using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ITStore.DTOs.Categories
{
    public class CategoriesUpdateDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
