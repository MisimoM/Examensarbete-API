namespace Modules.Users.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
    public string ProfileImage { get; set; }
    public DateTime CreatedAt { get; set; }

    public User(string name, string email, string role, string password)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Role = role;
        Password = password;
        ProfileImage = "https://github.com/MisimoM/Examensarbete-media/blob/main/Global/ProfileImage.png?raw=true";
        CreatedAt = DateTime.UtcNow;
    }

    //Konstruktor för seeding
    public User(Guid id, string name, string email, string role, string password)
    {
        Id = id;
        Name = name;
        Email = email;
        Role = role;
        Password = password;
        ProfileImage = "https://github.com/MisimoM/Examensarbete-media/blob/main/Global/ProfileImage.png?raw=true";
        CreatedAt = DateTime.UtcNow;
    }
}
