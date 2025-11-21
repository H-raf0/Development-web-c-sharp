public class Progression
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Count { get; set; }
    public int Multiplier { get; set; }
    public int BestScore { get; set; }

    // ?
    public Progression(int userId)
    {
        UserId = userId;
        Count = 0;
        Multiplier = 1;
        BestScore = 0;
    }
    /*
    // Relation avec User
    public User? User { get; set; }
    */
}
