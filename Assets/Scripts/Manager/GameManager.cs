﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager manager = null;
    //静态工厂方法 
    public static GameManager Instance()
    {
        if (manager == null)
        {
            manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        }
        return manager;
    }
    public InitView initView;
    public StarGroupDictView dicStarGroup;
    public StarGroupArticleView articleStarGroup;
    public MainView mainView;
    void Awake()
    {
        SettingManager.Instance().InitGame();
        DictManager.Instance().dicStarGroup = dicStarGroup;
        ArticleManager.Instance().articleStarGroup = articleStarGroup;
    }
    bool isStartUnZipDB = false;
    public void StartUnZipDB()
    {
        initView.gameObject.SetActive(true);
        isStartUnZipDB = true;
    }
    public void SetUnZipProgress(float progress)
    {
        initView.SetProgess(progress);
    }
    public void EndUnZipDB()
    {
        initView.gameObject.SetActive(false);
        isStartUnZipDB = false;
        SettingManager.Instance().UnZipFin();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStartUnZipDB)
        {
            int progressFin = ZipManager.Instance().fileProgress[0];

            ulong sizeOfEntry = ZipManager.Instance().sizeOfEntry;
            ulong bwrite = lzma.getBytesWritten();
            float progress = (bwrite/sizeOfEntry);
            //Debug.LogError("s:" + sizeOfEntry);
            //Debug.LogError("b:" + bwrite);
            SetUnZipProgress(progress);
            if (100 == progressFin)
            {
                EndUnZipDB();
            }
        }
    }
}
