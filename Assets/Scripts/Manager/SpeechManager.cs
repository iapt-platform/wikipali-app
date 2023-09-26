using AeLa.EasyFeedback;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
//using UnityEditor.PackageManager.UI;
//using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class SpeechManager : MonoBehaviour
{
    public TextAsset pali2My;
    public TextAsset pali2Tlg;
    private SpeechManager() { }
    private static SpeechManager manager = null;
    //��̬�������� 
    public static SpeechManager Instance()
    {
        if (manager == null)
        {
            manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SpeechManager>();
        }
        return manager;
    }
    //�ʶ�����
    List<ReadTextInfo> paliList = new List<ReadTextInfo>();
    List<ReadTextInfo> transList = new List<ReadTextInfo>();

    public class ReadTextInfo
    {
        public ReadTextInfo(string _sentence, int _offsetIndex, int _textID)//, bool isTransString)
        {

            sentence = _sentence;
            //ֻȥ��paliԭ�ĵ�����
            //if (!isTransString)
            //    if (SettingManager.Instance().GetPaliRemoveBracket() == 1)
            //        sentence = MarkdownText.RemoveBracket(sentence);
            offsetIndex = _offsetIndex;
            textID = _textID;
            sentenceReplace = _sentence;
        }
        public string sentence;
        public int offsetIndex;
        public int textID;//��ָ�����textUI������
        public string sentenceReplace;//paliת��������

    }
    bool isTrans;


    List<AudioClip> paliACList = new List<AudioClip>();
    List<AudioClip> transACList = new List<AudioClip>();
    class SpeechWordBoundary
    {
        public SpeechWordBoundary(SpeechSynthesisWordBoundaryEventArgs _e)
        {
            e = _e;
        }
        public SpeechSynthesisWordBoundaryEventArgs e;
        public int reTextOffset;
    }
    List<List<SpeechWordBoundary>> paliWordBoundaryList = new List<List<SpeechWordBoundary>>();
    List<List<SpeechWordBoundary>> transWordBoundaryList = new List<List<SpeechWordBoundary>>();
    Coroutine coroutinePali;
    Coroutine coroutineTrans;

    AudioSource aus;
    public void ClearContentList()
    {
        paliList.Clear();
        transList.Clear();
    }
    //todo �����е㲥�Ű�ť��δ������
    bool CheckIsSame(List<ReadTextInfo> _paliList, List<ReadTextInfo> _transList, bool _isTrans)
    {
        if (isTrans != _isTrans)
            return false;
        if (paliList == null)
            return false;
        if (paliList.Count != _paliList.Count)
            return false;
        //ֻ���ǰ��������
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
        //����Ƿ��ظ�����
        if (CheckIsSame(_paliList, _transList, _isTrans))//���ظ�����
        {
            isStartPlay = true;
            isStartHighLight = true;
            return;
        }
        ssmlTextLength = 0;


        paliACList.Clear();
        paliACList.InsertRange(0, new AudioClip[_paliList.Count]);

        paliWordBoundaryList.Clear();
        paliWordBoundaryList.InsertRange(0, new List<SpeechWordBoundary>[_paliList.Count]);

        //����������Ҫ���
        DeepCopyList(paliList, _paliList);
        transACList.Clear();
        transWordBoundaryList.Clear();
        if (_isTrans)
        {
            transACList.InsertRange(0, new AudioClip[_transList.Count]);
            transWordBoundaryList.InsertRange(0, new List<SpeechWordBoundary>[_transList.Count]);
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
        isStartHighLight = false;
        //waitTime = 0;


        //IEnumerator enumeratorP = CoroutineFuncPali();
        //coroutinePali = StartCoroutine(enumeratorP);
        if (isTrans)
        {
            //������ʾpaliԭ��ѡ��
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
        isStartHighLight = false;
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
            paliList[i].sentenceReplace = SpeechGeneration.Instance().ReplaceWord(paliList[i].sentence);
            SpeekPaliTask(paliList[i].sentenceReplace, i);
            //paliACList.Add(SpeechGeneration.Instance().SpeekPali(SpeechGeneration.Instance().ReplaceWord(paliList[i])));
            //++loadPaliId;
            //if (i == 0)
            //    isStartPlay = true;
        }
        //yield return null;
    }
    int ssmlTextLength;
    public void SpeekPaliTask(string word, int id)
    {
        if (!SpeechGeneration.Instance().CheckKey())
            return;
        var config = SpeechConfig.FromSubscription(SpeechGeneration.SPEECH_KEY, SpeechGeneration.SPEECH_REGION);
        string voice = SpeechGeneration.Instance().GetVoice();
        config.SpeechSynthesisVoiceName = voice;
        using (var synthsizer = new SpeechSynthesizer(config, null))
        {
            //todo:��ʡazure��������������Ϊ0ʱ������ssml������ͨread text
            int speed = (int)SettingManager.Instance().GetPaliVoiceSpeed();
            string text = word;
            if (speed != 0)
            {
                //text = @"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='" + SpeechGeneration.Instance().GetLanguage() + "'><voice name='" + voice + "'><prosody rate='" + speed.ToString() + "%'>" + word + "</prosody></voice></speak>";
                text = @"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='" + SpeechGeneration.Instance().GetLanguage() + "'><voice name='" + voice + "'><prosody rate='" + speed.ToString() + "%'>";
                ssmlTextLength = text.Length;
                text += word + "</prosody></voice></speak>";
            }
            //var task = SynthesisToSpeakerPaliAsync(config, text, id);
            var task = SynthesisToSpeakerPaliAsync(config, text, id, speed);
            Task.Delay(1).ContinueWith(t => { task.RunSynchronously(); });
        }
    }
    private List<SpeechWordBoundary> ReComputeWordBoundaryTextOffset(List<SpeechWordBoundary> eL, bool isHaveSpeed)
    {
        if (isHaveSpeed)
        {
            eL[0].reTextOffset = (int)eL[0].e.TextOffset;
            // eL[1].reTextOffset = (int)eL[0].e.TextOffset + (int)eL[0].e.WordLength;
            for (int i = 1; i < eL.Count; i++)
            {
                eL[i].reTextOffset = eL[i - 1].reTextOffset + (int)eL[i - 1].e.WordLength + 1;
            }
        }
        else
        {
            for (int i = 0; i < eL.Count; i++)
            {
                eL[i].reTextOffset = (int)eL[i].e.TextOffset;
            }
        }

        return eL;
    }
    public /*static*/ async Task SynthesisToSpeakerPaliAsync(SpeechConfig config, string text, int id, int speed)
    {
        // Creates a speech synthesizer using the default speaker as audio output.
        //�������δ��룬��ֱ�Ӳ��ţ�������unity��a
        //using (var synthesizer = new SpeechSynthesizer(config))
        using (var synthesizer = new SpeechSynthesizer(config, null))
        {

            paliWordBoundaryList[id] = new List<SpeechWordBoundary>();
            synthesizer.WordBoundary += (s, e) =>
            {

                //�����ssml���¼���textoffset
                paliWordBoundaryList[id].Add(new SpeechWordBoundary(e));

            };

            //using (var result = await synthesizer.SpeakSsmlAsync(text))

            using (var result = await (speed == 0 ? synthesizer.SpeakTextAsync(text) : synthesizer.SpeakSsmlAsync(text)))
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
                    {
                        isStartPlay = true;
                        isStartHighLight = true;
                    }
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
        if (!SpeechGeneration.Instance().CheckKey())
            return;
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
        //todo �жϷ������ʲô����

        using (var synthesizer = new SpeechSynthesizer(config, null))
        {
            //var wordBoundaryList = [];
            //synthesizer.wordBoundary = function(s, e) {
            //    window.console.log(e);
            //    wordBoundaryList.push(e);
            //};
            transWordBoundaryList[id] = new List<SpeechWordBoundary>();
            synthesizer.WordBoundary += (s, e) =>
            {
                // The unit of e.AudioOffset is tick (1 tick = 100 nanoseconds), divide by 10,000 to convert to milliseconds.
                //Debug.LogError($"Word boundary event received. Audio offset: " +
                //    $"{(e.AudioOffset + 5000) / 10000}ms, text offset: {e.TextOffset}, word length: {e.WordLength}.");
                transWordBoundaryList[id].Add(new SpeechWordBoundary(e));
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
                    {
                        isStartPlay = true;
                        isStartHighLight = true;
                    }
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
            isStartHighLight = true;
            waitTime = 0;
        }
        yield return null;
    }


    bool isStartPlay = false;
    bool isStartHighLight = false;
    float waitTime = 0;
    int curPaliID;
    int curTransID;
    int temp = 0;
    //�ȵ���һ����Ƶ�������˿�ʼ����
    //��ȡ��Ƶʱ�䣬�ȴ���ʱ����󲥷���һ���������û������͵ȴ�5s�ٲ��ţ�ѭ����ȡ��ֱ���������
    private void Update()
    {
        if (isStartHighLight)
        {
            //ÿ9ִ֡��һ�θ���
            if (temp % 9 == 0)
            {
                if (isTrans)
                {
                    //������ʾpaliԭ��ѡ��
                    bool transContent = SettingManager.Instance().GetTransContent() == 1;
                    if (transContent)
                    {
                        bool currPali = true;
                        if (curPaliID > curTransID)
                        {
                            currPali = true;
                            HighLightWordPali();
                        }
                        else
                        {
                            HighLightWordTrans();
                            currPali = false;
                        }
                    }
                    else
                    {
                        HighLightWordTrans();
                    }
                }
                else
                {
                    HighLightWordPali();
                }
                temp = 0;
            }
            ++temp;

        }
        if (isStartPlay)
        {
            if (waitTime <= 0)
            {
                if (isTrans)
                {
                    //������ʾpaliԭ��ѡ��
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
                        }
                    }
                    else//����ʾpaliԭ��ֻ�����벿�־Ϳ�����
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
        //���������ٵĸ���
        int speed = (int)SettingManager.Instance().GetPaliVoiceSpeed();
        paliWordBoundaryList[highLightPaliID] = ReComputeWordBoundaryTextOffset(paliWordBoundaryList[highLightPaliID], speed != 0);

        ++curPaliID;
        if (!isTrans && curPaliID >= paliList.Count)
        {
            isStartPlay = false;
            waitTime = 0;
            return;
        }
        float clipTime = clip.length;//����
        waitTime += clipTime + 1;
    }
    public void TransUpdateFunc()
    {
        //�����paliԭ���٣��ȶ�������
        //������ʾpaliԭ��ѡ��
        bool transContent = SettingManager.Instance().GetTransContent() == 1;
        if (transContent)
        {
            if (curTransID >= transACList.Count)
            {
                if (curPaliID >= paliACList.Count)
                {

                    isStartPlay = false;
                    waitTime = 0;
                    return;
                }
                else
                {
                    ++curTransID;
                    return;
                }
            }
        }
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
            if (transContent)
            {
                if (curPaliID >= paliACList.Count)
                {
                    isStartPlay = false;
                    waitTime = 0;
                    return;
                }
            }
            else
            {
                isStartPlay = false;
                waitTime = 0;
                return;
            }

        }
        float clipTime = clip.length;//����
        waitTime += clipTime + 1;
    }

    int highLightTransID = 0;
    int highLightPaliID = 0;
    //���ָ�������1
    //UI��ʾ��������TEXT���Ķ������õ��Ƿ�ɢ��TEXT����
    //�ڻ�ȡ����ʱ����ÿ�����ӵ�ƫ�ƣ�������,����ʱֱ�Ӹ�������text��λ�ã�����������

    //����ѭ����������ÿִ֡�У�ÿ50msִ��һ��
    void HighLightWordTrans()
    {
        //���������һ�����Ӿ�ֹͣ����,��ԭ����
        if (!isStartPlay)
        {
            if (!aus.isPlaying)//transACList[highLightTransID].length <= aus.time)
            {
                isStartHighLight = false;
                ArticleManager.Instance().articleView.RestoreTextHighLight();
                return;
            }
        }
        //Debug.LogError("ausTime:" + aus.time);
        //Debug.LogError("ausTimeP:" + (int)(aus.time * 1000000));
        //Debug.LogError("curTransID:" + highLightTransID);
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
            if (transWordBoundaryList[highLightTransID][i].e.AudioOffset < playTime)
            // if (dur < playTime)
            {
                //Debug.LogError("dur:" + dur);

                curwordBoundary = transWordBoundaryList[highLightTransID][i].e;
            }
            else
                break;

        }
        if (curwordBoundary != null)
        {
            //Debug.LogError(curwordBoundary.Text);
            ReadTextInfo info = transList[highLightTransID];
            ArticleManager.Instance().articleView.SetTextHighLight(info.textID, info.offsetIndex + (int)curwordBoundary.TextOffset/*+15*/, (int)curwordBoundary.WordLength);

        }


    }

    void HighLightWordPali()
    {
        //���������һ�����Ӿ�ֹͣ����,��ԭ����
        if (!isStartPlay)
        {
            if (!aus.isPlaying)
            {
                isStartHighLight = false;
                ArticleManager.Instance().articleView.RestoreTextHighLight();
                return;
            }
        }

        //Debug.LogError("ausTime:" + aus.time);
        //Debug.LogError("ausTimeP:" + (int)(aus.time * 1000000));
        //Debug.LogError("curPalisID:" + highLightPaliID);
        float playTime = aus.time * 10000000f;
        int c = paliWordBoundaryList[highLightPaliID].Count;
        int curID = 0;
        SpeechWordBoundary curwordBoundary = null;
        float dur = 0;
        for (int i = 0; i < c; i++)
        {

            if (paliWordBoundaryList[highLightPaliID][i].e.AudioOffset < playTime)
            // if (dur < playTime)
            {
                //Debug.LogError("dur:" + dur);
                curwordBoundary = paliWordBoundaryList[highLightPaliID][i];
            }
            else
                break;

        }
        if (curwordBoundary != null)
        {
            //Debug.LogError(curwordBoundary.Text);
            ReadTextInfo info = paliList[highLightPaliID];
            string[] replaceArr = info.sentenceReplace.Split(' ');
            string[] orignArr = info.sentence.Split(' ');
            int iID = 0;
            int rID = 0;
            int rSpaceID = 0;
            for (int i = 0; i < replaceArr.Length; i++)
            {
                rSpaceID = i;
                rID += replaceArr[i].Length + 1;
                if (curwordBoundary.reTextOffset - ssmlTextLength < rID)
                {
                    //Debug.LogError("curwordBoundary.TextOffset" + curwordBoundary.e.Text);
                   // Debug.LogError("curwordBoundary.TextOffset" + curwordBoundary.reTextOffset);
                    //Debug.LogError("ssmlTextLength" + ssmlTextLength);
                   // Debug.LogError(".TextOffset - ssmlTextLength" + (curwordBoundary.reTextOffset - ssmlTextLength));
                    break;
                }
                iID += orignArr[i].Length + 1;
            }
            //Debug.LogError("TextOffset:" + curwordBoundary.TextOffset);
            //Debug.LogError("rSpaceID:" + rSpaceID);
            //Debug.LogError("iID:" + iID);
            //Debug.LogError("Length:" + orignArr[rSpaceID].Length);
            ArticleManager.Instance().articleView.SetTextHighLight(info.textID,
                info.offsetIndex + iID, orignArr[rSpaceID].Length);

        }

    }
    //todo:
    //--0.ssml�ĸ�������ƫ��offset,0%�ٶȲ���ssml
    //--1:�ڶ��ε㷢�����û����ʶ�
    //--2.pali�뷭���Ϸ�������
    //--3.key��Ϊ�����ȡ
    //--4.ʥ��Ŀ¼����ӳ��
    //--5.ɱ��app�����ϴ��Ķ�λ��
    //--bug:
    //--a.pali�뷭���Ϸ���������������ʱ�����������¶�pali
    //?--b.<b></b>html������������һ��//��Ϊ�ո���</b>���
    //?--c.������ʱ�����ȵ�һƪ���£�����λ�ò���
    //����
    //1.�����Ķ�/ȥ�������ڵ�����
    //2.���µ������ݿ�
    //3.����1.1�汾
}
