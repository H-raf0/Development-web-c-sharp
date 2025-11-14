public class UserPass
{
    public string Pseudo { get; set; }
    public string Password { get; set; }

    public UserPass(string pseudo, string password)
    {
        Pseudo = pseudo;
        Password = password;
    }
}