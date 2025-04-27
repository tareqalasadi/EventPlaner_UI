namespace EventPlaner_UI.Models
{
    public class EntityBase
    {
        public long ID { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public long? ModifydBy { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
