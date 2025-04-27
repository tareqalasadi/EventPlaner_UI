using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlaner_UI.Models
{
    public class Images
    {
        public long ID { get; set; }
        public string ImageNameEn { get; set; }
        public string ImageNameAr { get; set; }
        public string Price { get; set; }
        public long? Gender { get; set; }
        public long? CakeId { get; set; }
        public byte[] Image { get; set; }
        public string ImageBase64
        {
            get
            {
                if (Image == null || Image.Length == 0)
                    return string.Empty;
                return Convert.ToBase64String(Image);
            }
        }
        public string VideoPath { get; set; } // New property for storing video path
        public long CategoryOfServiceId { get; set; }
        [ForeignKey("CategoryOfServiceId")]

        public CategoryOfService CategoryOfService { get; set; }

    }

}
