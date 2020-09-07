public class TouhouSortSession : MicrogameSession
{
    public TouhouSortSorter.Category category { get; private set; }

    public TouhouSortSession(string microgameId, int difficulty, TouhouSortSorter.Category category) : base(microgameId, difficulty)
    {
        MicrogameId = microgameId;
        Difficulty = difficulty;
        this.category = category;
    }
}
