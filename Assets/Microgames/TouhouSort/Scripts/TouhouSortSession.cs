public class TouhouSortSession : MicrogameSession
{
    public TouhouSortTraits.CategoryScene  category { get; set; }

    public TouhouSortSession(string microgameId, int difficulty, TouhouSortTraits.CategoryScene category) : base(microgameId, difficulty)
    {
        MicrogameId = microgameId;
        Difficulty = difficulty;
        this.category = category;
    }
}
