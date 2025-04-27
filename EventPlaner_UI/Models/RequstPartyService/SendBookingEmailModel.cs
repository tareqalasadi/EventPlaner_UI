namespace EventPlaner_UI.Models  
{
    public class SendBookingEmailModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public long People { get; set; }
        public string Message { get; set; }
    }
}
