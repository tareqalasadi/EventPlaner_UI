namespace EventPlaner_UI.Models
{
    public class RequstPartyModel
    {
        public long ID { get; set; }
        public long ServiceId { get; set; }

        public string? serviceName { get; set; }
        public string? ThemeColor { get; set; }
        public List<string>? FlowersType { get; set; }
        public string FlowersTypes => FlowersType != null ? string.Join(", ", FlowersType) : string.Empty;
        public string? flowersColor { get; set; }
        public bool? WithZafa { get; set; }
        public bool? WithFreind { get; set; }
        public string? GenderFreind { get; set; }
        public long? CountOfFreind { get; set; }
        public bool? WithPhotographer { get; set; }
        public string? GenderPhotographer { get; set; }
        public long? SweetPackagingCount { get; set; }
        public long? WaterPackagingCount { get; set; }
        public long? Foodcount { get; set; }
        public bool? WithBridalBouquet { get; set; }
        public bool? WithCake { get; set; }
        public bool? WithFood { get; set; }
        public bool? WithSweetPackaging { get; set; }
        public bool? WithFlowers { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public long? CityId { get; set; }
        public string? StartDate { get; set; }
        public string? serviceType { get; set; }
        public string? MainImage { get; set; }
        public string? PlaceParty { get; set; }
        public string? Note { get; set; }
        public List<Additionalservice>? ServiceList { get; set; }

    }
    public class Additionalservice
    {
        public long ServiceId { get; set; }
        public string serviceNameEn { get; set; }
        public string serviceNameAr { get; set; }
        public string? MainImage { get; set; }
        public string Price { get; set; }
        public long? Count { get; set; }
        public byte? uploadFile { get; set; }
    }
}
