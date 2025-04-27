using System.ComponentModel.DataAnnotations;

namespace EventPlaner_UI.Models
{
    public class Service: EntityBase
    {
        [Required]
        public string ServiceNameEn { get; set; }
        [Required]

        public string ServiceNameAr { get; set; }
        [Required]

        public string DescriptionEn { get; set; }
        [Required]

        public string DescriptionAr { get; set; }

        public byte[]? Image { get; set; }
        public IFormFile Images { get; set; }
        public string ImageBase64
        {
            get
            {
                if (Image == null || Image.Length == 0)
                    return string.Empty;
                return Convert.ToBase64String(Image);
            }
        }
        public long Direct { get; set; }
        public long Active { get; set; }
        public long ComingSoon { get; set; }

    }
    public class ServiceModel : EntityBase
    {
        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public byte[]? Image { get; set; }
        public long Direct { get; set; }


    }
    public class DirectServiec
    {
        public long ID { get; set; }
        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }
    }
}
