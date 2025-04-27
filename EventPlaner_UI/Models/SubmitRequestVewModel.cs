namespace EventPlaner_UI.Models
{
    public class SubmitRequestVewModel
    {
        public long Item_Id { get; set; }
        public string CustomerName { get; set; }
        public long Count { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long CityId { get; set; }
        public long ServiceId { get; set; }
        public string StartDate { get; set; }
        public string Note { get; set; }
        public string ServiceName { get; set; }
        public string Cart_Items { get; set; }
        public string All_items { get; set; }
        public List<DirectImage> AllItemModel { get; set; }
        public List<CartItemViewModel> AllItemCartModel { get; set; }

        public SubmitRequestVewModel()
        {
            AllItemModel = new List<DirectImage>();
            AllItemCartModel = new List<CartItemViewModel>();
        }

        // Convert StartDate to DateTime
        public DateTime? Start_Date()
        {
            if (DateTime.TryParse(StartDate, out DateTime parsedDate))
            {
                return parsedDate;
            }
            return null; // Return null if parsing fails
        }


    }

}
