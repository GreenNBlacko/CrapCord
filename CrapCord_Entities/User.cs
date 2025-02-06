namespace CrapCord_Entities;

public class User {
    public int ID { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public List<Token> Tokens { get; set; }
}