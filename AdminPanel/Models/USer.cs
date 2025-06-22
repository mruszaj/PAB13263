using ApiRest.Models;

public class User
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;

    public List<Order> Orders { get; set; } = new();
}
