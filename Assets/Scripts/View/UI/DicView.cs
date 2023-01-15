using Imdork.SQLite;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 词典面板
/// </summary>
public class DicView : MonoBehaviour
{
    public Button searchBtn;
    public DBManager dbManager;
    public void OnSearchBtnClick()
    {
        Debug.LogError("你单击了Button");
        //SqliteDataReader reader = dbManager.db.SelectOrderASC("bh-paper", "word");
        dbManager.Getdb(db =>
        {
            //查询该账号UID 大于50  对应的 LoginTime 和 User 对应字段数据
            var reader = db.Select("bh-paper"
           );

            //调用SQLite工具  解析对应数据
            Dictionary<string, object>[] pairs = SQLiteTools.GetValues(reader);


        });

    }
    // Start is called before the first frame update
    void Start()
    {
        searchBtn.onClick.AddListener(OnSearchBtnClick);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
