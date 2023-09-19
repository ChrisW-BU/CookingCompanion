namespace RPCC.Data.Models
{
    public class ShoppingListItem
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public int RiId { get; set; }

        public bool HasObtained { get; set; }
    }
}
