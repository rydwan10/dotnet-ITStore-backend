using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static ITStore.Shared.Enums;

namespace ITStore.DTOs.Orders;

public class UpdateStatusOrderDTO
{
    [Required]
    public List<Guid> OrderIds { get; set; }
    [Required]
    public EnumOrderStatuses OrderStatus { get; set; }
}