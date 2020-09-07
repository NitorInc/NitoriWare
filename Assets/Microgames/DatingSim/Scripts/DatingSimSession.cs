public class DatingSimSession : MicrogameSession
{
    public DatingSimCharacters.Character character { get; private set; }

    public DatingSimSession(string microgameId, int difficulty, DatingSimCharacters.Character character) : base(microgameId, difficulty)
    {
        MicrogameId = microgameId;
        Difficulty = difficulty;
        this.character = character;
    }
}
