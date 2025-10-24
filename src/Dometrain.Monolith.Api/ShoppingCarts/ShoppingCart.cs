namespace Dometrain.Monolith.Api.ShoppingCarts;

public class ShoppingCart
{
    public string Pk => StudentId.ToString();
    
    public required Guid StudentId { get; set; }

    public List<Guid> CourseIds { get; set; } = [];
}
