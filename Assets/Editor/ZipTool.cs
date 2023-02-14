
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ZipTool
{
    [MenuItem("Assets/Tools/CompressZIP")]
    static void CompressFile()
    {
        //支持多选
        string[] guids = Selection.assetGUIDs;//获取当前选中的asset的GUID
                                              //for (int i = 0; i < guids.Length; i++)
                                              //{
        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);//通过GUID获取路径
                                                                   //TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);


        //压缩文件
        //CompressFileLZMA(Application.dataPath + "/1.jpg", Application.dataPath + "/2.zip");
        //CompressFileLZMA(Application.dataPath + assetPath.Replace("Assets", ""), Application.dataPath + assetPath.Replace("Assets", "").Replace(".db", "") + ".zip");
        CompressFileLZMA("E:\\UnityProject\\WikiPali\\Assets\\StreamingAssets\\DB\\Sentence.db", "E:\\UnityProject\\WikiPali\\Assets\\StreamingAssets\\DB\\Sentence.zip");
        AssetDatabase.Refresh();

    }
    private static void CompressFileLZMA(string inFile, string outFile)
    {
        SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

        // Write the encoder properties
        coder.WriteCoderProperties(output);

        // Write the decompressed file size.
        output.Write(System.BitConverter.GetBytes(input.Length), 0, 8);

        // Encode the file.
        coder.Code(input, output, input.Length, -1, null);
        output.Flush();
        output.Close();
        input.Close();
    }

    [MenuItem("Assets/Tools/UnCompressZIP")]
    static void DecompressFile()
    {
        //解压文件
        //DecompressFileLZMA("E:\\UnityProject\\WikiPali\\Assets\\StreamingAssets\\Sentence.zip", "E:\\UnityProject\\WikiPali\\Assets\\StreamingAssets\\Sentence.db");
        DecompressFileLZMA("E:\\UnityProject\\WikiPali\\Assets\\StreamingAssets\\DB\\Sentence.zip", "E:\\UnityProject\\WikiPali\\Assets\\StreamingAssets\\DB\\Sentence.db");
        AssetDatabase.Refresh();
    }
    private static void DecompressFileLZMA(string inFile, string outFile)
    {
        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

        // Read the decoder properties
        byte[] properties = new byte[5];
        input.Read(properties, 0, 5);

        // Read in the decompress file size.
        byte[] fileLengthBytes = new byte[8];
        input.Read(fileLengthBytes, 0, 8);
        long fileLength = System.BitConverter.ToInt64(fileLengthBytes, 0);

        // Decompress the file.
        coder.SetDecoderProperties(properties);
        coder.Code(input, output, input.Length, fileLength, null);
        output.Flush();
        output.Close();
        input.Close();
    }
}
