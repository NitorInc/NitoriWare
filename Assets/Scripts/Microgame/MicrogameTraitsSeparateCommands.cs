using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrogameTraitsSeparateCommands : MicrogameTraits
{
    public string commandKeySuffix;
    public override string GetLocalizedCommand(MicrogameSession session) => TextHelper.getLocalizedText("microgame." + session.MicrogameId + ".command" + commandKeySuffix, command);
}

