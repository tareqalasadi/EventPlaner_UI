using EventPlaner_UI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventPlaner_UI.Models
{
    public class CategoryOfServiceViewModel
    {
        public long ID { get; set; }

        public string CategoryNameEn { get; set; }
        public string CategoryNameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public long ServiceId { get; set; }
        public byte[]? Image { get; set; }
        public IFormFile Images{ get; set; }
        public List<SelectListItem> Services { get; set; } 
    }
    public class CategoryOfService
    {
        public long ID { get; set; }

        public string CategoryNameEn { get; set; }
        public string CategoryNameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public byte[]? Image { get; set; }
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
        [ForeignKey("ServiceId")]
        [JsonIgnore]
        //public ServiceModel Service { get; set; }

        public ICollection<Images> Images { get; set; } = new List<Images>();
    }
 
}

