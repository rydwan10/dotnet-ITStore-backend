namespace ITStore.Domain
{
    public class UserAddresses : BaseProperties
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
    }
}
