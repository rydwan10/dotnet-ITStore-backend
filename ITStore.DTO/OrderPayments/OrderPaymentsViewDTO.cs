using ITStore.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ITStore.Shared.Enums;

namespace ITStore.DTOs.OrderPayments
{
    public class OrderPaymentsViewDTO
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public EnumProviders Provider { get; set; }
        public string ProviderName {
            get
            {
                try
                {
                    return EnumFunction.GetAttribute<DisplayAttribute>(Provider).Name;
                }
                catch (Exception)
                {
                    return "Enum display name is not found";
                }
            } 
        }

    }
}
