using AeLa.EasyFeedback;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.PackageManager.UI;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

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
    List<ReadTextInfo> paliList = new List<ReadTextInfo>();
    List<ReadTextInfo> transList = new List<ReadTextInfo>();

    public class ReadTextInfo
    {
        public ReadTextInfo(string _sentence, int _offsetIndex,int _textID)
        {
            sentence = _sentence;
            offsetIndex = _offsetIndex;
            textID = _textID;
        }
        public string sentence;
        public int offsetIndex;
        public int textID;//拆分隔开的textUI的数量
    }
    bool isTrans;


    List<AudioClip> paliACList = new List<AudioClip>();
    List<AudioClip> transACList = new List<AudioClip>();
    List<List<SpeechSynthesisWordBoundaryEventArgs>> paliWordBoundaryList = new List<List<SpeechSynthesisWordBoundaryEventArgs>>();
    List<List<SpeechSynthesisWordBoundaryEventArgs>> transWordBoundaryList = new List<List<SpeechSynthesisWordBoundaryEventArgs>>();
    Coroutine coroutinePali;
    Coroutine coroutineTrans;

    AudioSource aus;
    public void ClearContentList()
    {
        paliList.Clear();
        transList.Clear();
    }
    //todo 播放中点播放按钮，未加载完
    bool CheckIsSame(List<ReadTextInfo> _paliList, List<ReadTextInfo> _transList, bool _isTrans)
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
            if (paliList[i].sentence != _paliList[i].sentence)
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
                if (transList[i].sentence != _transList[i].sentence)
                    return false;
            }
        }
        return true;
    }
    public void ReadArticleSList(List<ReadTextInfo> _paliList, List<ReadTextInfo> _transList, bool _isTrans, AudioSource _aus)
    {
        waitTime = 0;
        curPaliID = 0;
        curTransID = 0;
        highLightTransID = 0;
        highLightPaliID = 0;
        //检测是否重复播放
        if (CheckIsSame(_paliList, _transList, _isTrans))//是重复播放
        {
            isStartPlay = true;
            return;
        }


        paliACList.Clear();
        paliACList.InsertRange(0, new AudioClip[_paliList.Count]);

        paliWordBoundaryList.Clear();
        paliWordBoundaryList.InsertRange(0, new List<SpeechSynthesisWordBoundaryEventArgs>[_paliList.Count]);

        //引用类型需要深复制
        DeepCopyList(paliList, _paliList);
        transACList.Clear();
        transWordBoundaryList.Clear();
        if (_isTrans)
        {
            transACList.InsertRange(0, new AudioClip[_transList.Count]);
            transWordBoundaryList.InsertRange(0, new List<SpeechSynthesisWordBoundaryEventArgs>[_transList.Count]);
            DeepCopyList(transList, _transList);
        }
        //paliList = _paliList;
        //transList = _transList;
        isTrans = _isTrans;
        aus = _aus;
        //curPaliID = 0;
        //curTransID = 0;
        loadPaliId = 0;
        loadTransId = 0;
        isStartPlay = false;
        //waitTime = 0;


        //IEnumerator enumeratorP = CoroutineFuncPali();
        //coroutinePali = StartCoroutine(enumeratorP);
        if (isTrans)
        {
            //译文显示pali原文选项
            bool transContent = SettingManager.Instance().GetTransContent() == 1;
            if (transContent)
            {
                CoroutineFuncPali();
            }

            CoroutineFuncTrans();

            //IEnumerator enumeratorP = CoroutineFuncPali();
            //coroutinePali = StartCoroutine(enumeratorP);
        }
        else
        {
            CoroutineFuncPali();
        }
    }
    void DeepCopyList(List<ReadTextInfo> origin, List<ReadTextInfo> copy)
    {
        origin.Clear();
        int c = copy.Count;
        for (int i = 0; i < c; i++)
        {
            origin.Add(copy[i]);
        }
    }
    public void StopLoadSpeech()
    {
        isStartPlay = false;
        waitTime = 0;
        if (coroutinePali != null)
            StopCoroutine(coroutinePali);
        if (isTrans)
            if (coroutineTrans != null)
                StopCoroutine(coroutineTrans);

    }
    int loadPaliId = 0;
    int loadTransId = 0;
    void CoroutineFuncPali()
    {
        //float t = Time.timeSinceLevelLoad;
        for (int i = 0; i < paliList.Count; i++)
        {
            //Debug.LogError(Time.timeSinceLevelLoad - t);
            SpeekPaliTask(SpeechGeneration.Instance().ReplaceWord(paliList[i].sentence), i);
            //paliACList.Add(SpeechGeneration.Instance().SpeekPali(SpeechGeneration.Instance().ReplaceWord(paliList[i])));
            //++loadPaliId;
            //if (i == 0)
            //    isStartPlay = true;
        }
        //yield return null;
    }
    public void SpeekPaliTask(string word, int id)
    {
        var config = SpeechConfig.FromSubscription(SpeechGeneration.SPEECH_KEY, SpeechGeneration.SPEECH_REGION);
        string voice = SpeechGeneration.Instance().GetVoice();
        config.SpeechSynthesisVoiceName = voice;
        using (var synthsizer = new SpeechSynthesizer(config, null))
        {
            //todo:节省azure流量，语速设置为0时，不用ssml，用普通read text
            int speed = (int)SettingManager.Instance().GetPaliVoiceSpeed();
            string text = @"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='" + SpeechGeneration.Instance().GetLanguage() + "'><voice name='" + voice + "'><prosody rate='" + speed.ToString() + "%'>" + word + "</prosody></voice></speak>";
            //var result = synthsizer.SpeakSsmlAsync(text).Result;
            //Task<SpeechSynthesisResult> task = synthsizer.SpeakSsmlAsync(text);
            //StartCoroutine(CheckSynthesizer(task, config, synthsizer, id));
            var task = SynthesisToSpeakerPaliAsync(config, text, id);
            Task.Delay(1).ContinueWith(t => { task.RunSynchronously(); });
        }
    }
    public /*static*/ async Task SynthesisToSpeakerPaliAsync(SpeechConfig config, string text, int id)
    {
        // Creates a speech synthesizer using the default speaker as audio output.
        //如果用这段代码，就直接播放，不会走unity的a
        //udio
        //using (var synthesizer = new SpeechSynthesizer(config))
        using (var synthesizer = new SpeechSynthesizer(config, null))
        {
            //while (true)
            //{
            // Receives a text from console input and synthesize it to speaker.
            //Console.WriteLine("Enter some text that you want to speak, or enter empty text to exit.");
            //Console.Write("> ");
            //string text = Console.ReadLine();
            //if (string.IsNullOrEmpty(text))
            //{
            //    break;
            //}
            // Subscribes to word boundary event
            paliWordBoundaryList[id] = new List<SpeechSynthesisWordBoundaryEventArgs>();
            synthesizer.WordBoundary += (s, e) =>
            {
                // The unit of e.AudioOffset is tick (1 tick = 100 nanoseconds), divide by 10,000 to convert to milliseconds.
                //Debug.LogError($"Word boundary event received. Audio offset: " +
                //    $"{(e.AudioOffset + 5000) / 10000}ms, text offset: {e.TextOffset}, word length: {e.WordLength}.");
                paliWordBoundaryList[id].Add(e);

            };

            using (var result = await synthesizer.SpeakSsmlAsync(text))
            {
                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                {
                    var sampleCount = result.AudioData.Length / 2;
                    var audioData = new float[sampleCount];
                    for (var i = 0; i < sampleCount; ++i)
                    {
                        audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0F;

                    }
                    // The default output audio format is 16K 16bit mono
                    var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
                    audioClip.SetData(audioData, 0);
                    paliACList[id] = audioClip;
                    if (id == 0)
                        isStartPlay = true;
                    ++loadPaliId;
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        //Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        //Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        //Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
            }
            // }
        }
    }
    void CoroutineFuncTrans()
    {
        // float t = Time.timeSinceLevelLoad;
        for (int i = 0; i < transList.Count; i++)
        {
            SpeekTransTask(transList[i].sentence, i);
            //Task.Delay(1).ContinueWith(t => { task.RunSynchronously(); });

        }
        //yield return null;
    }
    public void SpeekTransTask(string word, int id)
    {
        var config = SpeechConfig.FromSubscription(SpeechGeneration.SPEECH_KEY, SpeechGeneration.SPEECH_REGION);
        string voice = SpeechGeneration.Instance().GetTransVoice();
        config.SpeechSynthesisVoiceName = voice;
        using (var synthsizer = new SpeechSynthesizer(config, null))
        {
            int speed = (int)SettingManager.Instance().GetPaliVoiceSpeed();
            //string text = @"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='" + SpeechGeneration.Instance().GetLanguage() + "'><voice name='" + voice + "'><prosody rate='" + speed.ToString() + "%'>" + word + "</prosody></voice></speak>";
            //var result = synthsizer.SpeakSsmlAsync(text).Result;
            //Task<SpeechSynthesisResult> task = synthsizer.SpeakSsmlAsync(text);
            //StartCoroutine(CheckSynthesizer(task, config, synthsizer, id));
            var task = SynthesisToSpeakerTransAsync(config, word, id);
            Task.Delay(1).ContinueWith(t => { task.RunSynchronously(); });


        }
    }

    public /*static*/ async Task SynthesisToSpeakerTransAsync(SpeechConfig config, string text, int id)
    {
        //todo 判断翻译的是什么语言

        using (var synthesizer = new SpeechSynthesizer(config, null))
        {
            //var wordBoundaryList = [];
            //synthesizer.wordBoundary = function(s, e) {
            //    window.console.log(e);
            //    wordBoundaryList.push(e);
            //};
            transWordBoundaryList[id] = new List<SpeechSynthesisWordBoundaryEventArgs>();
            synthesizer.WordBoundary += (s, e) =>
            {
                // The unit of e.AudioOffset is tick (1 tick = 100 nanoseconds), divide by 10,000 to convert to milliseconds.
                //Debug.LogError($"Word boundary event received. Audio offset: " +
                //    $"{(e.AudioOffset + 5000) / 10000}ms, text offset: {e.TextOffset}, word length: {e.WordLength}.");
                transWordBoundaryList[id].Add(e);
            };
            using (var result = await synthesizer.SpeakTextAsync(text))
            {
                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                {
                    var sampleCount = result.AudioData.Length / 2;
                    var audioData = new float[sampleCount];
                    for (var i = 0; i < sampleCount; ++i)
                    {
                        audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0F;

                    }
                    // The default output audio format is 16K 16bit mono
                    var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
                    audioClip.SetData(audioData, 0);
                    transACList[id] = audioClip;
                    if (id == 0)
                        isStartPlay = true;
                    ++loadTransId;
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        //Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        //Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        //Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
            }
            // }
        }
    }

    IEnumerator CCoroutineFuncTrans()
    {
        for (int i = 0; i < transList.Count; i++)
        {
            transACList.Add(SpeechGeneration.Instance().SpeekCN(transList[i].sentence));
            ++loadTransId;
            isStartPlay = true;
            waitTime = 0;
        }
        yield return null;
    }


    bool isStartPlay = false;
    float waitTime = 0;
    int curPaliID;
    int curTransID;
    int temp = 0;
    //等到第一个音频下载完了开始播放
    //获取音频时间，等待该时间过后播放下一个，如果还没下载完就等待5s再播放，循环获取，直到播放完毕
    private void Update()
    {
        if (isStartPlay)
        {
            if (isTrans)
            {
                //译文显示pali原文选项
                //bool transContent = SettingManager.Instance().GetTransContent() == 1;
                //if (transContent)
                //{
                //if (curPaliID <= curTransID)
                //{

                //}
                //else
                //{
                //每9帧执行一次高亮
                if (temp % 9 == 0)
                {
                    HighLightWordTrans();
                    temp = 0;
                }
                ++temp;
                // }
                // }
            }
            if (waitTime <= 0)
            {
                if (isTrans)
                {
                    //译文显示pali原文选项
                    bool transContent = SettingManager.Instance().GetTransContent() == 1;
                    if (transContent)
                    {
                        bool currPali = true;
                        if (curPaliID <= curTransID)
                        {
                            currPali = true;
                            PaliUpdateFunc(true);
                        }
                        else
                        {
                            TransUpdateFunc();
                            currPali = false;
                            //每9帧执行一次高亮
                            //if (temp % 9 == 0)
                            //{
                            //    HighLightWordTrans();
                            //    temp = 0;
                            //}
                            //++temp;
                        }


                    }
                    else//不显示pali原文只读翻译部分就可以了
                    {
                        TransUpdateFunc();
                    }
                }
                else
                {
                    PaliUpdateFunc(false);
                }
            }
            waitTime -= Time.deltaTime;
        }

    }
    public void PaliUpdateFunc(bool isTrans)
    {
        if (curPaliID >= paliACList.Count || paliACList[curPaliID] == null)
        {
            waitTime += 1;
            return;
        }

        AudioClip clip = paliACList[curPaliID];
        highLightPaliID = curPaliID;
        aus.clip = clip;
        aus.Play();
        ++curPaliID;
        if (!isTrans && curPaliID >= paliList.Count)
        {
            isStartPlay = false;
            waitTime = 0;
            return;
        }
        float clipTime = clip.length;//秒数
        waitTime += clipTime + 1;
    }
    public void TransUpdateFunc()
    {
        if (curTransID >= transACList.Count || transACList[curTransID] == null)
        {
            waitTime += 1;
            return;
        }
        AudioClip clip = transACList[curTransID];
        highLightTransID = curTransID;
        aus.clip = clip;
        aus.Play();
        ++curTransID;
        if (curTransID >= transList.Count)
        {
            isStartPlay = false;
            waitTime = 0;
            return;
        }
        float clipTime = clip.length;//秒数
        waitTime += clipTime + 1;
    }

    //返回：key
    void ProcessWordBoundaryList()
    {

    }

    void HighLightWordPali()
    {


    }
    int highLightTransID = 0;
    int highLightPaliID = 0;
    //文字高亮方案1
    //UI显示的是所有TEXT，阅读文字用的是分散的TEXT句子
    //在获取句子时计算每个句子的偏移，存下来,高亮时直接高亮所有text的位置，插入高亮语句

    //减少循环次数，不每帧执行，每50ms执行一次
    void HighLightWordTrans()
    {
        Debug.LogError("ausTime:" + aus.time);
        //Debug.LogError("ausTimeP:" + (int)(aus.time * 1000000));
        Debug.LogError("curTransID:" + highLightTransID);
        float playTime = aus.time * 10000000f;
        int c = transWordBoundaryList[highLightTransID].Count;
        int curID = 0;
        SpeechSynthesisWordBoundaryEventArgs curwordBoundary = null;
        float dur = 0;
        for (int i = 0; i < c; i++)
        {
            //dur += (float)transWordBoundaryList[highLightTransID][i].Duration.Milliseconds * 0.001f;
            // if (transWordBoundaryList[].off)
            //50ms = 500000
            if (transWordBoundaryList[highLightTransID][i].AudioOffset < playTime)
            // if (dur < playTime)
            {
                Debug.LogError("dur:" + dur);

                curwordBoundary = transWordBoundaryList[highLightTransID][i];
            }
            else
                break;

        }
        if (curwordBoundary != null)
        {
            Debug.LogError(curwordBoundary.Text);
            ReadTextInfo info = transList[highLightTransID];
            ArticleManager.Instance().articleView.SetTextHighLight(info.textID, info.offsetIndex + (int)curwordBoundary.TextOffset/*+15*/, (int)curwordBoundary.WordLength);

        }


    }
}
