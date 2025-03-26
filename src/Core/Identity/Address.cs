using Core.Entities;

namespace E_commerce_Api.Identity
{
    public class Address : Base
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
    
}