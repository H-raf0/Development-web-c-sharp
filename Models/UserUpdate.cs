public class UserUpdate
{
    public string? NewPseudo { get; set; }
    public Role? NewUserRole { get; set; }
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }

    public UserUpdate(string? newPseudo, Role? newUserRole, string? currentPassword, string? newPassword)
    {
        NewPseudo = newPseudo;
        NewUserRole = newUserRole;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }
}