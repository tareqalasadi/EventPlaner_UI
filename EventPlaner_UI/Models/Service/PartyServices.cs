using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace EventPlaner_UI.Models
{
    public class PartyServices
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string ServiceNameAr { get; set; }

        [Required]
        public string ServiceNameEn { get; set; }

        public string DescriptionEn { get; set; }

        public string DescriptionAr { get; set; }

        public string TypeService { get; set; }

        public byte[] MainImage { get; set; }

        public byte[] ImageListSerialized { get; set; }

        // Flags
        public bool IsPhotographerIncluded { get; set; }
        public bool IsZafaIncluded { get; set; }
        public bool IsSweetCollectionIncluded { get; set; }
        public bool IsFriendAssistanceNeeded { get; set; }
        public bool IsCakeIncluded { get; set; }
        public bool IsFoodIncluded { get; set; }
        public bool IsBridalBouquetIncluded { get; set; }
        public bool IsFlowerArrangementIncluded { get; set; }

        public long ServiceId { get; set; }
        public float Price { get; set; }

        public string MainImageBase64
        {
            get
            {
                if (MainImage == null || MainImage.Length == 0)
                    return string.Empty;
                return Convert.ToBase64String(MainImage);
            }
        }

        [NotMapped]
        public List<string> ImageListBase64
        {
            get
            {
                if (ImageListSerialized == null || ImageListSerialized.Length == 0)
                    return new List<string>();

                var jsonString = System.Text.Encoding.UTF8.GetString(ImageListSerialized);
                var imageList = JsonSerializer.Deserialize<List<byte[]>>(jsonString);

                return imageList.Select(image => Convert.ToBase64String(image)).ToList();
            }
        }
    }

    public class PartyServicesViewModel
    {
        public long Id { get; set; }
        public string ServiceNameAr { get; set; }
        public string ServiceNameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string TypeService { get; set; }
        public IFormFile MainImage { get; set; }
        public List<IFormFile> ImageList { get; set; }
        public long ServiceId { get; set; }

        // Boolean flags
        public bool IsPhotographerIncluded { get; set; }
        public bool IsZafaIncluded { get; set; }
        public bool IsSweetCollectionIncluded { get; set; }
        public bool IsFriendAssistanceNeeded { get; set; }
        public bool IsCakeIncluded { get; set; }
        public bool IsFoodIncluded { get; set; }
        public bool IsBridalBouquetIncluded { get; set; }
        public bool IsFlowerArrangementIncluded { get; set; }

        public float Price { get; set; }

        // List of services for dropdown
        public List<SelectListItem> Services { get; set; }
    }


}
