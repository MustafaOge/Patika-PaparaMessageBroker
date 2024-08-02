namespace PaparaMessageBroker.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalAmount { get; set; }
    }
}
