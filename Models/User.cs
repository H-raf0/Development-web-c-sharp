using Microsoft.AspNetCore.Identity;
public enum Role
{
    ADMIN,
    USER
}

public class User
{
    private static int IdCounter = 0;
    public int Id { get; private set; }
    public string Pseudo { get; set; } = string.Empty;
    public Role UserRole { get; set; } = Role.USER;
    // Make password property mappable by EF (keep setter private)
    public string Password { get; set; } = string.Empty;

    public User(string pseudo, string password, Role userRole)
    {
        Pseudo = pseudo;
        Password = password;
        UserRole = userRole;
        Id = ++IdCounter;
    }

    // Parameterless constructor required by EF Core when it can't use constructor binding
    protected User() { }

    // Verifies the provided password against the stored one


    public bool VerifyPassword(string password)
    {
        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(this, this.Password, password);
        if (result == PasswordVerificationResult.Success)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void UpdatePassword(string currentPassword, string newPassword)
    {
        if (string.IsNullOrEmpty(newPassword))
            throw new ArgumentException("Le nouveau mot de passe ne peut pas être vide");

        if (!VerifyPassword(currentPassword))
            throw new InvalidOperationException("Mot de passe actuel incorrect");

        Password = newPassword;
    }
}