/*
 * uGUI-Hypertext (https://github.com/setchi/uGUI-Hypertext)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/uGUI-Hypertext/blob/master/LICENSE)
 */

using UnityEngine;

namespace Hypertext
{
    public class RegexExample : MonoBehaviour
    {
        [SerializeField] RegexHypertext text = default;

        const string RegexUrl = @"https?://(?:[!-~]+\.)+[!-~]+";
        //const string RegexHashtag = @"[\s^][#＃]\w+";

        void Start()
        {
            text.OnClick(RegexUrl, new Color(0, 0.2235f, 0.898f, 1), url => Application.OpenURL(url));
            //text.OnClick(RegexHashtag, Color.green, hashtag => Debug.Log(hashtag));
        }
    }
}
