using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrogameTraitsSeparateCommands : MicrogameTraits
{
    public string commandKeySuffix;
    public override string localizedCommand => TextHelper.getLocalizedText("microgame." + microgameId + ".command" + commandKeySuffix, command);
}

