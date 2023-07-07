using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    List<string> paliList = new List<string>();
    List<string> transList = new List<string>();
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
        paliACList.InsertRange(0, new AudioClip[_paliList.Count]);

        //引用类型需要深复制
        DeepCopyList(paliList, _paliList);
        transACList.Clear();
        if (_isTrans)
        {
            transACList.InsertRange(0, new AudioClip[_transList.Count]);
            DeepCopyList(transList, _transList);
        }
        //paliList = _paliList;
        //transList = _transList;
        isTrans = _isTrans;
        aus = _aus;
        curPaliID = 0;
        curTransID = 0;
        loadPaliId = 0;
        loadTransId = 0;
        isStartPlay = false;
        waitTime = 0;
        CoroutineFuncPali();
        //IEnumerator enumeratorP = CoroutineFuncPali();
        //coroutinePali = StartCoroutine(enumeratorP);
        if (isTrans)
        {
            IEnumerator enumeratorT = CoroutineFuncTrans();
            coroutineTrans = StartCoroutine(enumeratorT);
        }
    }
    void DeepCopyList(List<string> origin, List<string> copy)
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
        float t = Time.timeSinceLevelLoad;
        for (int i = 0; i < paliList.Count; i++)
        {
            //Debug.LogError(Time.timeSinceLevelLoad - t);
            SpeekPaliTask(SpeechGeneration.Instance().ReplaceWord(paliList[i]), i);
            //paliACList.Add(SpeechGeneration.Instance().SpeekPali(SpeechGeneration.Instance().ReplaceWord(paliList[i])));
            //++loadPaliId;
            //if (i == 0)
            //    isStartPlay = true;
        }
        //yield return null;
    }
    public void SpeekPaliTask(string word, int id)
    {
        var config = SpeechConfig.FromSubscription("82563fdf180f434aaf8c8f14171bae6c", "eastus");
        string voice = SpeechGeneration.Instance().GetVoice();
        config.SpeechSynthesisVoiceName = voice;
        using (var synthsizer = new SpeechSynthesizer(config, null))
        {
            int speed = (int)SettingManager.Instance().GetPaliVoiceSpeed();
            string text = @"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='" + SpeechGeneration.Instance().GetLanguage() + "'><voice name='" + voice + "'><prosody rate='" + speed.ToString() + "%'>" + word + "</prosody></voice></speak>";
            //var result = synthsizer.SpeakSsmlAsync(text).Result;
            //Task<SpeechSynthesisResult> task = synthsizer.SpeakSsmlAsync(text);
            //StartCoroutine(CheckSynthesizer(task, config, synthsizer, id));
            var task = SynthesisToSpeakerAsync(config, text, id);
            Task.Delay(1).ContinueWith(t=> { task.RunSynchronously(); });

            //task.Wait();
            //task.Start();
            //var task = Task.Run(async () => {

            //    using (var synthesizer = new SpeechSynthesizer(config, null))
            //    {
            //        using (var result = await synthesizer.SpeakSsmlAsync(text))
            //        {
            //            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            //            {
            //                var sampleCount = result.AudioData.Length / 2;
            //                var audioData = new float[sampleCount];
            //                for (var i = 0; i < sampleCount; ++i)
            //                {
            //                    audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0F;

            //                }
            //                // The default output audio format is 16K 16bit mono
            //                var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
            //                audioClip.SetData(audioData, 0);
            //                //paliACList[id] = audioClip;
            //                //if (id == 0)
            //                //    isStartPlay = true;
            //                paliACList[0] = audioClip;

            //                    isStartPlay = true;
            //                ++loadPaliId;
            //            }
            //            else if (result.Reason == ResultReason.Canceled)
            //            {
            //                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
            //                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

            //                if (cancellation.Reason == CancellationReason.Error)
            //                {
            //                    //Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
            //                    //Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
            //                    //Console.WriteLine($"CANCELED: Did you update the subscription info?");
            //                }
            //            }
            //        }
            //        // }
            //    }


            //});
            //  task.Wait();

        }
    }
    public /*static*/ async Task SynthesisToSpeakerAsync(SpeechConfig config, string text, int id)
    {
        // Creates a speech synthesizer using the default speaker as audio output.
        //如果用这段代码，就直接播放，不会走unity的sudio
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
    //private void CheckSynthesizer(Task<SpeechSynthesisResult> task,
    //    SpeechConfig config,
    //    SpeechSynthesizer synthesizer, int id)
    //{
    //   // yield return new WaitUntil(() => task.IsCompleted);

    //    var result = task.Result;

    //    // Checks result.
    //    string newMessage = string.Empty;
    //    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
    //    {
    //        // Since native playback is not yet supported on Unity yet (currently
    //        // only supported on Windows/Linux Desktop),
    //        // use the Unity API to play audio here as a short term solution.
    //        // Native playback support will be added in the future release.

    //        System.Threading.Tasks.Task copyTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
    //        {
    //            var sampleCount = result.AudioData.Length / 2;
    //            var audioData = new float[sampleCount];
    //            for (var i = 0; i < sampleCount; ++i)
    //            {
    //                audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8
    //                        | result.AudioData[i * 2]) / 32768.0F;
    //            }

    //            // The default output audio format is 16K 16bit mono
    //            var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
    //            audioClip.SetData(audioData, 0);
    //            paliACList[id] = audioClip;
    //            if (id == 0)
    //                isStartPlay = true;
    //            ++loadPaliId;
    //        });

    //   //     yield return new WaitUntil(() => copyTask.IsCompleted);

    //        newMessage = "Speech synthesis succeeded!";
    //    }
    //    else if (result.Reason == ResultReason.Canceled)
    //    {
    //        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
    //        newMessage = $"CANCELED:\nReason=[{cancellation.Reason}]\n" +
    //                     $"ErrorDetails=[{cancellation.ErrorDetails}]\n" + 
    //                     "Did you update the subscription info?";
    //    }

    //    //message = newMessage;
    //    //waitingForSpeak = false;
    //   // synthesizer.Dispose();
    //}
    //private IEnumerator CheckSynthesizer(Task<SpeechSynthesisResult> task,
    //SpeechConfig config,
    //SpeechSynthesizer synthesizer, int id)
    //{
    //    yield return new WaitUntil(() => task.IsCompleted);

    //    var result = task.Result;

    //    string newMessage = string.Empty;
    //    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
    //    {
    //        var sampleCount = result.AudioData.Length / 2;
    //        var audioData = new float[sampleCount];
    //        for (var i = 0; i < sampleCount; ++i)
    //        {
    //            audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8
    //                    | result.AudioData[i * 2]) / 32768.0F;
    //        }

    //        // The default output audio format is 16K 16bit mono
    //        var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount,
    //                1, 16000, false);
    //        audioClip.SetData(audioData, 0);
    //        paliACList[id] = audioClip;
    //        if (id == 0)
    //            isStartPlay = true;
    //        ++loadPaliId;
    //        newMessage = "Speech synthesis succeeded!";

    //    }
    //    else if (result.Reason == ResultReason.Canceled)
    //    {
    //        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
    //        newMessage = $"CANCELED:\nReason=[{cancellation.Reason}]\n" +
    //                     $"ErrorDetails=[{cancellation.ErrorDetails}]\n" +
    //                     "Did you update the subscription info?";
    //    }
    //    synthesizer.Dispose();
    //}
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
                if (curPaliID >= paliACList.Count || paliACList[curPaliID] == null)
                {
                    waitTime += 1;
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
