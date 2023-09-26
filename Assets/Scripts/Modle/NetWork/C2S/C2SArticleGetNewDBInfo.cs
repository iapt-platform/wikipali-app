using CI.HttpClient;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class C2SArticleGetNewDBInfo
{
    //获取所有版本风格
    public static void GetChannelData(string view = "public", string update_at = "", int limit = 30, int offset = 0)
    {
        HttpClient client = new HttpClient();

        //ProgressSlider.value = 100;
        //client.Get(new System.Uri("https://staging.wikipali.org/api/v2/channel-io?view=public&limit=30&offset=0"), HttpCompletionOption.StreamResponseContent, (r) =>
        client.Get(new System.Uri(string.Format(@"https://staging.wikipali.org/api/v2/channel-io?view={0}&limit={1}&offset={2}", view, limit, offset)),
            HttpCompletionOption.StreamResponseContent, (r) =>
        {
            //RightText.text = "Download: " + r.PercentageComplete.ToString() + "%";
            //ProgressSlider.value = 100 - r.PercentageComplete;
            byte[] responseData = r.ReadAsByteArray();
            string json = Encoding.Default.GetString(responseData);
            Debug.LogError(json);
        });
    }












}
