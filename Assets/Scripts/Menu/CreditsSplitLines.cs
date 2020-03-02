using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[ExecuteInEditMode]
public class CreditsSplitLines : MonoBehaviour
{
    private string lastText = "";
    [SerializeField]
    private Text[] textColumns;
    [SerializeField]
    private string textFile;
    [SerializeField]
    [Multiline]
    private string contents;

    private void Start()
    {
        if (!string.IsNullOrEmpty(textFile))
            contents = File.ReadAllText(Path.Combine(Application.dataPath, textFile));
        Update();
    }

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
