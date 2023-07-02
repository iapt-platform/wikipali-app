using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    private SpeechManager() { }
    private static SpeechManager manager = null;
    //静态工厂方法 
    public static SpeechManager Instance()
    {
        if (manager == null)
        {
            manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SpeechManager>();
        }
        return manager;
    }
    //朗读队列
    List<string> paliList;
    List<string> transList;
    bool isTrans;


    List<AudioClip> paliACList = new List<AudioClip>();
    List<AudioClip> transACList = new List<AudioClip>();
    Coroutine coroutinePali;
    Coroutine coroutineTrans;

    AudioSource aus;
    //todo 播放中点播放按钮，未加载完
    bool CheckIsSame(List<string> _paliList, List<string> _transList, bool _isTrans)
    {
        if (isTrans != _isTrans)
            return false;
        if (paliList == null)
            return false;
        if (paliList.Count != _paliList.Count)
            return false;
        //只检查前五条即可
        int checkCount = paliList.Count > 5 ? 5 : paliList.Count;
        for (int i = 0; i < checkCount; i++)
        {
            if (paliList[i] != _paliList[i])
                return false;
        
        }
        if (_isTrans)
        {
            if (transList == null)
                return false;
            if (transList.Count != _transList.Count)
                return false;
            checkCount = transList.Count > 5 ? 5 : transList.Count;
            for (int i = 0; i < checkCount; i++)
            {
                if (transList[i] != _transList[i])
                    return false;
            }
        }
            return true;
    }
    public void ReadArticleSList(List<string> _paliList, List<string> _transList, bool _isTrans, AudioSource _aus)
    {
        //检测是否重复播放
        if (CheckIsSame(_paliList, _transList, _isTrans))//是重复播放
        {

            isStartPlay = true;
            waitTime = 0;
            curPaliID = 0;
            return;
        }


        paliACList.Clear();
        transACList.Clear();
        paliList = _paliList;
        transList = _transList;
        isTrans = _isTrans;
        aus = _aus;
        curPaliID = 0;
        curTransID = 0;
        loadPaliId = 0;
        loadTransId = 0;
        isStartPlay = false;
        waitTime = 0;
        IEnumerator enumeratorP = CoroutineFuncPali();
        coroutinePali = StartCoroutine(enumeratorP);
        if (isTrans)
        {
            IEnumerator enumeratorT = CoroutineFuncTrans();
            coroutineTrans = StartCoroutine(enumeratorT);
        }
    }
    public void StopLoad()
    {
        isStartPlay = false;
        waitTime = 0;
        StopCoroutine(coroutinePali);
        if (isTrans)
            StopCoroutine(coroutineTrans);

    }
    int loadPaliId = 0;
    int loadTransId = 0;
    IEnumerator CoroutineFuncPali()
    {
        for (int i = 0; i < paliList.Count; i++)
        {
            paliACList.Add(SpeechGeneration.Instance().SpeekPali(SpeechGeneration.Instance().ReplaceWord(paliList[i])));
            ++loadPaliId;
            if (i == 0)
                isStartPlay = true;
        }
        yield return null;
    }
    IEnumerator CoroutineFuncTrans()
    {
        for (int i = 0; i < transList.Count; i++)
        {
            transACList.Add(SpeechGeneration.Instance().SpeekCN(transList[i]));
            ++loadTransId;
        }
        yield return null;
    }


    bool isStartPlay = false;
    float waitTime = 0;
    int curPaliID;
    int curTransID;
    //等到第一个音频下载完了开始播放
    //获取音频时间，等待该时间过后播放下一个，如果还没下载完就等待5s再播放，循环获取，直到播放完毕
    private void Update()
    {
        if (isStartPlay)
        {
            if (waitTime <= 0)
            {
                if (curPaliID >= paliACList.Count)
                {
                    waitTime += 5;
                    return;
                }

                AudioClip clip = paliACList[curPaliID];
                aus.clip = clip;
                aus.Play();
                ++curPaliID;
                if (curPaliID >= paliList.Count)
                {
                    isStartPlay = false;
                    waitTime = 0;
                    return;
                }
                float clipTime = clip.length;//秒数
                waitTime += clipTime + 1;

            }
            waitTime -= Time.deltaTime;
        }

    }

}
