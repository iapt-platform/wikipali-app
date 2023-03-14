using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteAlways]
public class MarkdownText
{
    public bool link = true;
    public bool emoji = true;
    public enum StyleMode { None, Preview, Replace }
    public static StyleMode style = StyleMode.Preview;

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

    private static readonly Regex superlinkRegex = new Regex("(\\[.*\\])(\\(\\S+\\))|((http:\\/\\/|https:\\/\\/|www\\.)([A-Z0-9.-:]{1,})\\.[0-9A-Z?;~&#=\\-_\\.\\/]{2,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    private static readonly Regex emojiRegex = new Regex(":[\\w|-]+:");
    private static readonly Regex boldRegex = new Regex("\\*\\*(.*)\\*\\*");
    private static readonly Regex itallicRegex = new Regex("\\*(.*)\\*");
    private static readonly Regex strikeRegex = new Regex("\\~\\~(.*)\\~\\~");
    private static readonly Regex underlineRegex = new Regex("__([^_]*)__");
    //todo
    public static string PreprocessText(string text)
    {
        // LINK
        //if (link)
        //{
        //    MatchCollection matches = superlinkRegex.Matches(text);
        //    foreach (Match match in matches)
        //    {
        //        // 0th index is full match
        //        // 1st index hyperlink name
        //        // 2nd index is url
        //        string name = match.Groups[1].Value;
        //        string link = match.Groups[2].Value;
        //        // If it's a plain link
        //        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        //        {
        //            text = text.Replace(match.Value, ShortLink(match.Value));
        //        }
        //        // Otherwise it's a hyperlink
        //        else
        //        {
        //            name = name.Trim('[', ']');
        //            link = link.Trim('(', ')');
        //            text = text.Replace(match.Value, $"<#{ColorUtility.ToHtmlStringRGB(Color.blue)}><u><link=\"{link}\">{name}</link></u></color>");
        //        }
        //    }
        //}

        //if (emoji)
        //{
        //    // EMOJI
        //    MatchCollection matches = emojiRegex.Matches(text);
        //    foreach (Match match in matches)
        //    {
        //        var emojiName = match.Value;
        //        // remove ':' from beginning and end
        //        emojiName = emojiName.Remove(0, 1);
        //        emojiName = emojiName.Remove(emojiName.Length - 1, 1);

        //        // Make sure sprite actually exists
        //        //if (EmojiExists(emojiName))
        //        //{
        //        //    // replace emoji text with sprite tag
        //        //    text = text.Replace(match.Value, $"<sprite name=\"{emojiName}\">");
        //        //}
        //    }
        //}

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