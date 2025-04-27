namespace EventPlaner_UI.Models
{
    public class CustomerRequest
    {
        public long ID { get; set; }
        public string Note { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool WithPhotographer { get; set; }
        public int? GenderPhotographer { get; set; }
        public int CityId { get; set; }
        public int AreaId { get; set; }
        public DateTime StartDate { get; set; }
        public List<string>? FlowersType { get; set; }
        public List<string>? BallonType { get; set; }
        public long? CakeId { get; set; }
        public long? SweetPackagingCount { get; set; }
        public long? WaterPackagingCount { get; set; }
        public string? TypeOfCar { get; set; }
        public string ServiceName { get; set; }
        public byte[]? UplodedFile { get; set; }
        public double TotalPrice { get; set; }
        public long? ImageId { get; set; }
        public Images Image { get; set; }
        public long CategoryOfServiceId { get; set; }
        public CategoryOfService CategoryOfService { get; set; }


    }

}
