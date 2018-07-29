using Google.GData.Spreadsheets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Localization/Localization Updater")]
[ExecuteInEditMode]
public class LocalizationUpdater : ScriptableObject
{
    private const string KeyIdentifier = "key";

    [SerializeField]
    private string spreadsheetId;
    [SerializeField]
    private int subsheetCount;

    //The feed likes to convert the first row to lowercase so the second row on the first subsheet has a buffer for that
    [SerializeField]
    private string idNameKey;

    [SerializeField]
    private string languagesPath;
        
	
    [ContextMenu("Update Sheet")]
	void updateSheet ()
    {
        var languages = new Dictionary<string, SerializedNestedStrings>();
        for (int i = 1; i <= subsheetCount; i++)
        {
            var sheet = GDocService.GetSpreadsheet(spreadsheetId, i);
            if (i == 1)
                languages = generateLanguageDict(sheet);

            foreach (ListEntry row in sheet.Entries)
            {
                string rowKey = "";
                foreach (ListEntry.Custom element in row.Elements)
                {
                    if (element.LocalName.Equals(KeyIdentifier))
                        rowKey = element.Value;
                    else if (languages.ContainsKey(element.LocalName))
                        languages[element.LocalName][rowKey] = element.Value;
                }
            }
        }
        
	}

    //Use the second row sheet buffer to get proper codenames for langauges
    string getLanguageIdName(SerializedNestedStrings languageData)
    {
        return languageData[idNameKey];
    }

    Dictionary<string, SerializedNestedStrings> generateLanguageDict(ListFeed sheet)
    {
        var firstRow = (ListEntry)sheet.Entries.FirstOrDefault();
        var returnDict = new Dictionary<string, SerializedNestedStrings>();
        foreach (ListEntry.Custom element in firstRow.Elements)
        {
            if (element.LocalName.Equals(KeyIdentifier))
                continue;
            returnDict[element.LocalName] = new SerializedNestedStrings();
        }

        return returnDict;
    }

}
