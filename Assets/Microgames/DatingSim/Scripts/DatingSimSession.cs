public class DatingSimSession : MicrogameSession
{
    public DatingSimTraits.CharacterScene scene { get; set; }

    public DatingSimSession(string microgameId, int difficulty, DatingSimTraits.CharacterScene scene) : base(microgameId, difficulty)
    {
        MicrogameId = microgameId;
        Difficulty = difficulty;
        this.scene = scene;
    }
}
