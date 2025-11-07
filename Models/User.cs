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
    public string Mdp { get; private set; } = string.Empty;

    public User(string pseudo, string mdp, Role userRole)
    {
        Pseudo = pseudo;
        Mdp = mdp;
        UserRole = userRole;
        Id = ++IdCounter;
    }

    // Parameterless constructor required by EF Core when it can't use constructor binding
    protected User() { }

    // Verifies the provided password against the stored one
    public bool VerifyPassword(string mdp)
    {
        return Mdp == mdp;
    }

}