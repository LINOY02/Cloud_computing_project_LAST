namespace Cloud_computing_project_LAST.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNum { get; set; }
        public string ZIPCode { get; set; }

        public UserInfo()
        {

        }

        public UserInfo(string id, string firstName, string lastName, string phoneNumber, string city, string street, string streetNum, string zIPCode)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            City = city;
            Street = street;
            StreetNum = streetNum;
            ZIPCode = zIPCode;
        }
    }
}
