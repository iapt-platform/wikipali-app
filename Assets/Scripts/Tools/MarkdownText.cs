using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteAlways]
public class MarkdownText
{
    public bool link = true;
    public bool emoji = true;
    public enum StyleMode
    {
        None,
        Preview,//保留markdown符号
        Replace //替换markdown符号
    }
    public static StyleMode style = StyleMode.Replace;

    //private TMP_Text _text;
    //public TMP_Text text
    //{
    //    get
    //    {
    //        if (_text == null)
    //            TryGetComponent(out _text);
    //        return _text;
    //    }
    //}

    //private void OnEnable()
    //{
    //    text.textPreprocessor = this;
    //    text.richText = true;
    //}

    //private void OnDisable()
    //{
    //    text.textPreprocessor = null;
    //}

    //private void OnValidate()
    //{
    //    text.SetAllDirty();
    //    Canvas.ForceUpdateCanvases();
    //}

    private static readonly Regex boldBigRegex = new Regex(@"# (\w+)");
    private static readonly Regex boldRegex2 = new Regex("\\*\\*(.*)\\*\\*");
    private static readonly Regex boldRegex = new Regex(@"\*\*(\w+)\*\*");
    private static readonly Regex itallicRegex = new Regex(@"\*(\w+)\*");
    //private static readonly Regex itallicRegex = new Regex("\\*(.*)\\*");
    //private static readonly Regex strikeRegex = new Regex("\\~\\~(.*)\\~\\~");
    private static readonly Regex strikeRegex = new Regex(@"\\~\\~(\w+)\\~\\~");
    private static readonly Regex underlineRegex = new Regex("__([^_]*)__");
    //todo
    public static string PreprocessText(string text, int textSize = 0)
    {
        //text = text.Replace("|", "   ");
        //text = text.Replace("-", "    ");
        //text = text.Replace("<br>", "\r\n");
        //表格里的换行<br>处理为新的一行表格
        if (text.Contains("<br>") && text.Contains("|-|-|"))
        {
            StringBuilder sb = new StringBuilder();
            string[] lineTexts = text.Split('\n');
            int lc = lineTexts.Length;
            bool startTable = false;
            //表列数
            int colCount = 0;
            for (int i = 0; i < lc; i++)
            {
                bool isAppend = true;
                if (lineTexts[i].Contains("|-|-|"))
                {
                    startTable = true;
                    string[] temp = lineTexts[i].Split('|');
                    colCount = temp.Length - 2;
                }
                if (startTable && lineTexts[i].Contains("<br>"))
                {
                    isAppend = false;
                    lineTexts[i] = lineTexts[i].Replace("<br>", "$");
                    string[] temp = lineTexts[i].Split('|');
                    //string[] temp = lineTexts[i].Split('$');
                    string strFormat = "|";
                    string strFormatLine0 = "|";
                    List<string> brStr = new List<string>();
                    int tempID = 0;
                    for (int j = 1; j < temp.Length - 1; j++)
                    {

                        if (temp[j].Contains("$"))
                        {
                            strFormatLine0 += "{" + tempID + "}" + "|";
                            strFormat += "{" + tempID + "}" + " |";
                            brStr.Add(temp[j]);
                            ++tempID;
                        }
                        else
                        {
                            strFormatLine0 += temp[j] + "|";
                            strFormat += "|";
                        }
                    }
                    //todo 有多列br换行的情况
                    //for (int j = 0; j < brStr.Count; j++)
                    //{
                    //    string[] tempBr = brStr[j].Split('$');
                    //    if (j == 0)
                    //    {

                    //    }
                    //    else
                    //    { 

                    //    }
                    //}
                    //只实现一列换行
                    string[] tempBr = brStr[0].Split('$');
                    for (int x = 0; x < tempBr.Length; x++)
                    {
                        if (x == 0)
                        {
                            string tempBR0 = strFormatLine0.Replace("{" + 0 + "}", tempBr[0]);
                            sb.AppendLine(tempBR0);
                        }
                        else
                        {
                            string tempBRn = strFormat.Replace("{" + 0 + "}", tempBr[x]);
                            sb.AppendLine(tempBRn);
                        }
                    }
                }
                if (startTable && !lineTexts[i].Contains("|"))
                {
                    startTable = false;
                }
                if (isAppend)
                    sb.AppendLine(lineTexts[i]);
            }
            text = sb.ToString();
        }

        //if (style != StyleMode.None)
        {
            MatchCollection matches = boldRegex.Matches(text);
            foreach (Match match in matches)
            {
                switch (style)
                {
                    case StyleMode.Preview:
                        text = text.Replace(match.Value, $"<b>{match.Value}</b>");
                        break;
                    case StyleMode.Replace:
                        var boldText = match.Value;
                        boldText = boldText.Remove(0, 2);
                        boldText = boldText.Remove(boldText.Length - 2, 2);
                        text = text.Replace(match.Value, $"<b>{boldText}</b>");
                        break;
                }
            }
            matches = boldRegex2.Matches(text);
            foreach (Match match in matches)
            {
                switch (style)
                {
                    case StyleMode.Preview:
                        text = text.Replace(match.Value, $"<b>{match.Value}</b>");
                        break;
                    case StyleMode.Replace:
                        var boldText = match.Value;
                        boldText = boldText.Remove(0, 2);
                        boldText = boldText.Remove(boldText.Length - 2, 2);
                        text = text.Replace(match.Value, $"<b>{boldText}</b>");
                        break;
                }
            }
            matches = itallicRegex.Matches(text);
            foreach (Match match in matches)
            {
                switch (style)
                {
                    case StyleMode.Preview:
                        text = text.Replace(match.Value, $"<i>{match.Value}</i>");
                        break;
                    case StyleMode.Replace:
                        var boldText = match.Value;
                        boldText = boldText.Remove(0, 1);
                        boldText = boldText.Remove(boldText.Length - 1, 1);
                        text = text.Replace(match.Value, $"<i>{boldText}</i>");
                        break;
                }
            }
            matches = strikeRegex.Matches(text);
            foreach (Match match in matches)
            {
                switch (style)
                {
                    case StyleMode.Preview:
                        text = text.Replace(match.Value, $"<s>{match.Value}</s>");
                        break;
                    case StyleMode.Replace:
                        var boldText = match.Value;
                        boldText = boldText.Remove(0, 2);
                        boldText = boldText.Remove(boldText.Length - 2, 2);
                        text = text.Replace(match.Value, $"<s>{boldText}</s>");
                        break;
                }
            }

            matches = boldBigRegex.Matches(text);
            int textSizeRes = (textSize == 0) ? 65 : (textSize + 15);
            foreach (Match match in matches)
            {
                switch (style)
                {
                    case StyleMode.Preview:
                        text = text.Replace(match.Value, $"<b><size={textSizeRes}>{match.Value}</size></b>");
                        break;
                    case StyleMode.Replace:
                        var boldText = match.Value;
                        boldText = boldText.Remove(0, 1);
                        //boldText = boldText.Remove(boldText.Length - 2, 2);
                        text = text.Replace(match.Value, $"<b><size={textSizeRes}>{boldText}</size></b>");
                        break;
                }
            }
            matches = underlineRegex.Matches(text);
            foreach (Match match in matches)
            {
                switch (style)
                {
                    case StyleMode.Preview:
                        text = text.Replace(match.Value, $"<u>{match.Value}</u>");
                        break;
                    case StyleMode.Replace:
                        var boldText = match.Value;
                        boldText = boldText.Remove(0, 2);
                        boldText = boldText.Remove(boldText.Length - 2, 2);
                        text = text.Replace(match.Value, $"<u>{boldText}</u>");
                        break;
                }
            }
        }

        return text;
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (PointerIsOverURL(eventData, out int linkIndex))
    //    {
    //        TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
    //        string selectedLink = linkInfo.GetLinkID();
    //        if (!string.IsNullOrEmpty(selectedLink))
    //            Application.OpenURL(selectedLink);
    //    }
    //}

    //public bool PointerIsOverURL(Vector2 screenPosition, Camera camera, out int linkIndex)
    //{
    //    linkIndex = TMP_TextUtilities.FindIntersectingLink(text, screenPosition, camera);
    //    return linkIndex != -1;
    //}
    //public bool PointerIsOverURL(PointerEventData eventData, out int linkIndex) => PointerIsOverURL(eventData.position, eventData.pressEventCamera, out linkIndex);

    //private string ShortLink(in string link, int maxLength = 35)
    //{
    //    string text = link;
    //    // This is definitely the optimal way to do string operations!
    //    // I am a sculptor and strings are my clay! /s
    //    const string www = "www.";
    //    int wwwIndex = text.IndexOf(www, StringComparison.InvariantCulture);
    //    if (wwwIndex >= 0)
    //        text = text.Remove(0, wwwIndex + www.Length);
    //    else
    //    {
    //        const string http = "://";
    //        int httpIndex = text.IndexOf(http, StringComparison.InvariantCulture);
    //        if (httpIndex >= 0)
    //            text = text.Remove(0, httpIndex + http.Length);
    //    }

    //    const string ellipsis = "...";
    //    if (text.Length > maxLength - ellipsis.Length)
    //        text = $"{text.Substring(0, maxLength - ellipsis.Length)}{ellipsis}";
    //    return string.Format($"<#{ColorUtility.ToHtmlStringRGB(Color.blue)}><u><link=\"{link}\">{text}</link></u></color>");
    //}

    //public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    //{
    //    return PointerIsOverURL(sp, eventCamera, out _);
    //}

    //private bool EmojiExists(string name)
    //{
    //    TMP_SpriteAsset spriteAsset = text.spriteAsset;
    //    if (spriteAsset == null)
    //        spriteAsset = TMP_Settings.GetSpriteAsset();
    //    if (spriteAsset == null)
    //        return false;

    //    int spriteIndex = spriteAsset.GetSpriteIndexFromName(name);
    //    if (spriteIndex == -1)
    //    {
    //        foreach (var fallback in spriteAsset.fallbackSpriteAssets)
    //        {
    //            spriteIndex = fallback.GetSpriteIndexFromName(name);
    //            if (spriteIndex != -1)
    //                return true;
    //        }
    //    }
    //    else
    //        return true;

    //    return false;
    //}
}