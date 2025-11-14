public class UserPass
{
    public string Username { get; set; }
    public string Password { get; set; }

    public UserPass(string username, string password)
    {
        Username = username;
        Password = password;
    }
}