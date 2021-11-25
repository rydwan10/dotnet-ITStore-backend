using System;
using System.ComponentModel.DataAnnotations;

namespace ITStore.Domain
{
    public class BaseProperties
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        [StringLength(1)]
        public string StatusRecord { get; set; }
    }
}
