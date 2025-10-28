namespace GenericRepoWebApi.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        //Foreign Key
        public int ProductId { get; set; }
        public Product Prodcut { get; set; } = null!;

    }
}
