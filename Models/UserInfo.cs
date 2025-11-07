public class UserInfo
{
    public string Pseudo { get; set; }
    public string Mdp { get; set; }

    public UserInfo(string pseudo, string mdp)
    {
        Pseudo = pseudo;
        Mdp = mdp;
    }
}