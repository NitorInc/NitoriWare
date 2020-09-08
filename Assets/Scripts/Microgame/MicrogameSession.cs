using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MicrogameSession
{
    public int InstanceId { get; protected set; }

    public string MicrogameId { get; protected set; }
    public int Difficulty { get; protected set; }

    static int InstanceIdCounter = 0;

    public MicrogameSession(string microgameId, int difficulty, bool debugMode = false)
    {
        MicrogameId = microgameId;
        Difficulty = difficulty;
        InstanceId = InstanceIdCounter++;
    }
}
