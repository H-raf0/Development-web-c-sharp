public class UserInfo
{
    public string Pseudo { get; set; }
    public string Password { get; set; }

    public UserInfo(string pseudo, string password)
    {
        Pseudo = pseudo;
        Password = password;
    }
}