﻿using Google.GData.Spreadsheets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

[CreateAssetMenu(menuName = "Localization/Localization Updater")]
[ExecuteInEditMode]
public class LocalizationUpdater : ScriptableObject
{
    private const string KeyIdentifier = "key";
    private const string CharsFileSuffix = "Chars.txt";
    private const string AllCharsFile = "AllChars.txt";
    private const string NonAsianCharsFile = "NonAsianChars.txt";

    [SerializeField]
    private LanguagesData languagesData;

    [SerializeField]
    private string spreadsheetId;
    [SerializeField]
    private int subsheetCount;

    //The feed likes to convert the first row to lowercase so the second row on the first subsheet has a buffer for that
    [SerializeField]
    private string idNameKey;

    [SerializeField]
    private string languagesPath;
    [SerializeField]
    private string charsPath;
    [SerializeField]
    private string reportFile;

    [SerializeField]
    [Multiline]
    private string ignoreChars;

    public void updateLanguages()
    {
        var languages = new Dictionary<string, SerializedNestedStrings>();
        SerializedNestedStrings englishData = null;

        var sheetTitles = new List<string>();

        var missingValues = new Dictionary<string, Dictionary<string, int>>();

        for (int i = 1; i <= subsheetCount; i++)
        {
            var sheet = GDocService.GetSpreadsheet(spreadsheetId, i);
            var sheetTitle = sheet.Title.Text;
            sheetTitles.Add(sheetTitle);

            // Ran only at start of loop, but necessary here so we don't have to read the first sheet twice
            if (i == 1)
            {
                languages = generateLanguageDict(sheet);
                englishData = languages.FirstOrDefault().Value;
                foreach (var language in languages)
                {
                    missingValues[language.Key] = new Dictionary<string, int>();
                }
            }

            // Missing values structure is initially populated with every language and sheet title set to 0 missing values
            foreach (var language in languages)
            {
                missingValues[language.Key][sheetTitle] = 0;
            }

            foreach (ListEntry row in sheet.Entries)
            {
                string rowKey = "";
                foreach (ListEntry.Custom element in row.Elements)
                {
                    if (element.LocalName.Equals(KeyIdentifier))
                        rowKey = element.Value;
                    else if (languages.ContainsKey(element.LocalName))
                    {
                        var languageData = languages[element.LocalName];

                        if (!string.IsNullOrEmpty(element.Value))
                        {
                            var cleansedEntry = cleanseEntry(element.Value);

                            if (checkEntryIntegrity(languageData, rowKey, cleansedEntry, englishData))
                                languageData[rowKey] = cleansedEntry;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(englishData[rowKey]))
                                missingValues[element.LocalName][sheetTitle]++;
                        }
                    }
                }
            }
        }

        string fullLanguagesPath = Path.Combine(Application.dataPath, languagesPath);
        foreach (var languageData in languages)
        {
            string name = getLanguageIdName(languageData.Value);
            File.WriteAllText(Path.Combine(fullLanguagesPath, name), languageData.Value.ToString());

            var metaRecordedStatus = languageData.Value["meta.recorded"];
            if (string.IsNullOrEmpty(metaRecordedStatus) || !metaRecordedStatus.Equals("Y", System.StringComparison.OrdinalIgnoreCase))
                Debug.LogWarning($"Language {languageData.Key} does not have metadata recorded in google sheets");
        }

        // Format missing text report
        var missingValuesLanguageReports = missingValues
            //.Where(language => language.Value.Any(sheet => sheet.Value > 0))    // Select from languages who have missing values whatsoever
            .Select(language => language.Key + ": " + language.Value.Sum(sheet => sheet.Value) + " - "  // Sum up all missing values in a language
                + string.Join(", ", language.Value   // Then list out each subsheet in that language and its amount of missing values
                    .Where(sheet => sheet.Value > 0)    // Exclude any sheets with no missing values
                    .Select(sheet => sheet.Key + ": " + sheet.Value.ToString())));

        // Write to log
        var reportText = "Push this file with any localization updates.\n\n";
        reportText += $"Last pulled:\n{System.DateTime.Now.ToString()}\n";
        reportText += "\nThis is the order the sheets were found in. If github tries to change them, rearrange the cells so they match this and update language content again.\n"
            + string.Join("\n", sheetTitles) + "\n";
        reportText += "\nHow many values are missing translations from each language (doesn't count non-game pages such as Steam Store):\n"
            + string.Join("\n", missingValuesLanguageReports) + "\n";
        File.WriteAllText(Path.Combine(Application.dataPath, reportFile), reportText);

        Debug.Log("Language content updated");
	}

    public void updateCharsFiles()
    {
        string fullLanguagesPath = Path.Combine(Application.dataPath, languagesPath);
        string fullCharsPath = Path.Combine(Application.dataPath, charsPath);

        //Language files
        var languageFiles = languagesData.languages.Select(a => Path.Combine(fullLanguagesPath, a.getFileName())).Distinct();
        foreach (var languageFile in languageFiles)
        {
            string language = Path.GetFileName(languageFile);
            File.WriteAllText(Path.Combine(fullCharsPath, language + CharsFileSuffix), getUniqueCharString(File.ReadAllLines(languageFile)));
        }
        Debug.Log("Language chars updated");

        //All chars
        var allChars = languageFiles.Select(a => getUniqueCharString(File.ReadAllLines(a))).SelectMany(a => a).Distinct().ToArray();
        File.WriteAllText(Path.Combine(fullCharsPath, AllCharsFile), new string(allChars));
        Debug.Log(AllCharsFile + " updated");

        //Non-Asian chars
        languageFiles = languagesData.languages.Where(a => !a.isAsian).Select(a => Path.Combine(fullLanguagesPath, a.getFileName())).Distinct();
        allChars = languageFiles.Select(a => getUniqueCharString(File.ReadAllLines(a))).SelectMany(a => a).Distinct().ToArray();
        File.WriteAllText(Path.Combine(fullCharsPath, NonAsianCharsFile), new string(allChars));
        Debug.Log(NonAsianCharsFile + " updated");
    }

    static string getUniqueCharString(string[] languageLines)
    {
        var str = new string((from line in languageLines
                              where line.Contains("=")
                              select line.Substring(line.IndexOf('=') + 1))
                .SelectMany(a => a).Distinct().ToArray());
        return str;
    }

    string cleanseEntry(string value)
    {
        value = Regex.Replace(value, @"^\n*", "");  //Remove leading line breaks
        value = Regex.Replace(value, @"\n*$", "");  //Remove trailing line breaks
        value = Regex.Replace(value, @"^ *", "");   //Remove leading whitespace
        value = Regex.Replace(value, @" *$", "");   //Remove trailing whitespace
        value = value.Replace("\n", "\\n");         //Format line breaks
        return value;
    }

    bool checkEntryIntegrity(SerializedNestedStrings languageData, string key, string value, SerializedNestedStrings englishData)
    {
        if (englishData != null && englishData != languageData)
        {
            // Check parameter counts
            var paramCount = 0;
            while (true)
            {
                if (!value.Contains("{" + paramCount.ToString() + "}"))
                    break;
                paramCount++;
            }
            var englishText = englishData[key];
            if (englishText != null)
            {
                var englishParamCount = 0;
                while (true)
                {
                    if (!englishText.Contains("{" + englishParamCount.ToString() + "}"))
                        break;
                    englishParamCount++;
                }
                if (paramCount != englishParamCount)
                    Debug.LogWarning($"Language {getLanguageIdName(languageData)} has an inconsistent parameter count in key {key}");
            }
        }

        return true;
    }

    public void checkFontChars()
    {
        string fullLanguagesPath = Path.Combine(Application.dataPath, languagesPath);
        string fullCharsPath = Path.Combine(Application.dataPath, charsPath);
        var errorStrings = new List<string>();
        foreach (var language in LanguagesData.instance.languages)
        {
            var filePath = Path.Combine(fullLanguagesPath, language.getFileName());
            var fontDict = SerializedNestedStrings.deserialize(File.ReadAllText(filePath)).getSubData("meta.font").subData;
            foreach (var fontKVPair in fontDict)
            {
                if (LocalizationManager.parseFontCompabilityString(language, fontKVPair.Value.value))
                {
                    // Font is marked as compatible
                    var font = LanguagesData.instance.languageTMPFonts.FirstOrDefault(a => a.idName.Equals(fontKVPair.Key));
                    if (font == null || font.fontAsset == null)
                        continue;
                    var charString = File.ReadAllText(Path.Combine(fullCharsPath, language.getFileName() + "Chars.txt"));
                    charString = string.Join("", charString.Distinct());
                    List<char> currentChars = charString.ToCharArray().ToList();
                    currentChars.Add('a');

                    // Check fonts AND fallbacks
                    var fallbackList = new List<TMP_FontAsset>();
                    fallbackList.Add(font.fontAsset);                           // Current language
                    fallbackList.AddRange(font.fontAsset.fallbackFontAssets);   // language's fallbacks
                    fallbackList.AddRange(TMP_Settings.fallbackFontAssets);     // Global fallbacks
                    fallbackList = fallbackList.Distinct().ToList();

                    foreach (var fontAsset in fallbackList)
                    {
                        var missingChars = new List<char>();
                        // NO idea why but the hasCharacters() function seems to just be true all the time, so also check for null/any

                        missingChars = currentChars.Where(a => !fontAsset.characterDictionary.ContainsKey((int)a)).ToList();

                        if (missingChars != null && missingChars.Any())
                        {
                            currentChars = missingChars;
                        }
                        else
                        {
                            currentChars = null;
                            break;
                        }
                    }
                    if (currentChars != null)
                    {
                        currentChars = currentChars.Except(ignoreChars.ToCharArray()).ToList();
                        if (currentChars.Any())
                            errorStrings.Add($"{font.fontAsset.name} is missing {language.getFileName()} character(s)  " +
                            $"{string.Join("", currentChars)}");
                    }
                }
            }
        }
        errorStrings.Sort();
        foreach (var errorString in errorStrings)
        {
            Debug.LogWarning(errorString);
        }

        Debug.Log("Font character analysis complete");
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
