namespace EventPlaner_UI.Models
{
    public class CartItemViewModel
    {
        public long id { get; set; }
        public string itemId { get; set; }
        public string price { get; set; }
        public string count { get; set; }
        public byte[] image { get; set; }
        public string ImageBase64
        {
            get
            {
                if (image == null || image.Length == 0)
                    return string.Empty;
                return Convert.ToBase64String(image);
            }
        }
        public string name { get; set; }
        public string note { get; set; }
    }
}
