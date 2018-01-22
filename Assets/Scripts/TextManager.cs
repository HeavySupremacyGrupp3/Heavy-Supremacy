using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class TextManager
{
    private const string FILENAME = "Texter.txt";
    private const char LINE_DELIMITER = '^';
    private const char PART_DELIMITER = '_';
    private const char REGION_DELIMITER = '.';
    private static bool fileIsRead = false;
    public static bool IsFileRead
    {
        get { return fileIsRead; }
    }
    private static string[] fileRows;
    private static List<GameText> texts;

    /// <summary>
    /// Reads the file and marshals it into GameText objects
    /// </summary>
    public static void Init()
    {
        if (IsFileRead || texts != null)
            return;

        texts = new List<GameText>();

        using (StreamReader sReader = new StreamReader(FILENAME))
        {
            string file = sReader.ReadToEnd().Replace('\r', ' ').Replace('\n', ' ');
            fileRows = file.Split(LINE_DELIMITER);
            fileIsRead = true;
        }

        foreach (string row in fileRows)
        {
            if (row[0] == '/' && row[1] == '/')
                continue;

            string[] parts = row.Trim().Split(PART_DELIMITER);
            string[] regions = parts[0].Split(REGION_DELIMITER);
            string region = regions[0];
            string subregion = "";
            int idx = 1;
            if (regions.Length > 2)
            {
                subregion = regions[1];
                idx++;
            }
            string name = regions[idx];

            string text = parts[1];

            if (string.IsNullOrEmpty(text))
                continue;

            GameText txt = new GameText(region, name, text, subregion);
            texts.Add(txt);
        }
    }

    /// <summary>
    /// Gets the text associated with the provided regions and name
    /// </summary>
    /// <param name="name">The region, subregion and name of the text, separated by dots ("."). Ex: "Hub.Stats.HappinessText". Case insensitive</param>
    /// <returns>Returns the text associated with the information provided, String.Empty if it cannot be found, or "Text Fetched Too Soon" if file hasn't been read yet.</returns>
    public static string GetText(string name)
    {
        if (!IsFileRead)
            return "Text Fetched Too Soon";

        string[] parts = name.Split(REGION_DELIMITER);
        string region = parts[0];
        string subregion = "";
        int idx = 1;
        if (parts.Length > 2)
        {
            subregion = parts[1];
            idx++;
        }
        string txtName = parts[idx];

        foreach (GameText txt in texts)
        {
            string AssociatedText = txt.Get(region, subregion, txtName);
            if (AssociatedText != string.Empty)
            {
                return AssociatedText;
            }
        }

        return string.Empty;
    }
}

public class GameText
{
    private string Region, SubRegion, Name, Text;

    public GameText(string region, string name, string text, string subregion = "")
    {
        Region = region.ToLower();
        Name = name.ToLower();
        Text = text;
        SubRegion = subregion.ToLower();
    }

    /// <summary>
    /// Get text by region and name
    /// </summary>
    /// <returns>The text requested. Returns String.Empty if it doesn't exist.</returns>
    public string Get(string region, string subregion, string name)
    {
        region = region.ToLower();
        subregion = subregion.ToLower();
        name = name.ToLower();
        
        if (Region == region && SubRegion == subregion && Name == name)
        {
            return Text;
        }

        return string.Empty;
    }
}
