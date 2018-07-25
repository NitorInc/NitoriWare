using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CreditsSplitLines : MonoBehaviour
{
    [SerializeField]
    private string lastText;
    [SerializeField]
    private Text[] textColumns;
    [SerializeField]
    [Multiline]
    private string contents;
	
	void Update ()
    {
		if (contents != lastText && textColumns?.Length > 0)
        {
            var lines = contents.Split('\n');
            int lineIndex = 0;
            while (lineIndex < lines.Length)
            {
                foreach (var column in textColumns)
                {
                    if (lineIndex < textColumns.Length)
                        column.text = lines[lineIndex];
                    else
                        column.text += "\n" + lines[lineIndex];
                    lineIndex++;
                    if (lineIndex >= lines.Length)
                        break;
                }
            }
            lastText = contents;
        }
	}
}
