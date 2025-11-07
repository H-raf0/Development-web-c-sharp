public class UserPublic
{
    public int Id { get; }
    public string Pseudo { get; }
    public Role UserRole { get; }

    public UserPublic(int id, string pseudo, Role userRole)
    {
        Id = id;
        Pseudo = pseudo;
        UserRole = userRole;
    }

    public UserPublic(string pseudo, Role userRole)
        : this(0, pseudo, userRole)
    {
    }
}