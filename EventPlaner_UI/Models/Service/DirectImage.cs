using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlaner_UI.Models
{
    public class DirectImage
    {
        public long ID { get; set; }
        public string ImageNameEn { get; set; }
        public string ImageNameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string Price { get; set; }
        public string TypeService { get; set; }
        public long Count { get; set; }
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

        public long ServiceId { get; set; }
    }
    public class DirectImageVeiwModel
    {
        public long ID { get; set; }
        public string ImageNameEn { get; set; }
        public string ImageNameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string Price { get; set; }
        public string TypeService { get; set; }
        public byte[] Image { get; set; }
        public IFormFile Images { get; set; }
        public List<SelectListItem> Services { get; set; }

        public long ServiceId { get; set; }
    }

}
