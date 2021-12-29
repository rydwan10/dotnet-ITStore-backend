using ITStore.Domain;
using ITStore.DTOs.OrderPayments;
using ITStore.DTOs.Users;
using ITStore.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using static ITStore.Shared.Enums;

namespace ITStore.DTOs.Orders
{
    public class OrdersViewDTO
    {
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public EnumOrderStatuses Status { get; set; }
        public string StatusDescription
        {
            get
            {
                try
                {
                    return EnumFunction.GetAttribute<DisplayAttribute>(Status).Name;
                }
                catch (Exception)
                {
                    return "Enum display name is not found";
                }
            }
        }

        public OrderPaymentsViewDTO OrderPayments { get; set; }

        public ApplicationUsersViewDTO Users { get; set; }
    }
}
