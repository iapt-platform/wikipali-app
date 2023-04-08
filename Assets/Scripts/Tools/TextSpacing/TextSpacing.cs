using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[AddComponentMenu("UI/Effects/TextSpacing")]
public class TextSpacing : BaseMeshEffect
{
    public float _textSpacing = 1f;
    public float _colSpacing = 1f;
    //public bool _dirtyRefresh = false;
    //bool dirty = false;
    public override void ModifyMesh(VertexHelper vh)
    {

        if (!IsActive() || vh.currentVertCount == 0)
        {
            return;
        }

        Text text = GetComponent<Text>();
        if (text == null)
        {
            Debug.LogError("Missing Text component");
            return;
        }
        if (!text.text.Contains("|-|-|"))
            return;
        //if (_dirtyRefresh && dirty)
        //{
        //    return;
        //}
        //if (_dirtyRefresh)
        //    dirty = true;
        List<UIVertex> vertexs = new List<UIVertex>();
        vh.GetUIVertexStream(vertexs);
        int indexCount = vh.currentIndexCount;
        //todo 目前只支持一个表格
        string[] lineTexts = text.text.Split('\n');
        int lineCount = lineTexts.Length;
        //去除表格的形式
        //string textTest = text.text.Replace("|-", " ").Replace("|", " ");
        //text.text = textTest;
        //表头的列
        int tableStartRowID = 0;
        //表格格式的列|-|-|-|
        int tableFormatRowID = 0;
        //表格最后一列
        int tableEndRowID = 0;
        bool startTable = false;
        int colCount;
        //每列最宽的宽度
        int[] maxColSpace = null;
        //根据每列最宽的宽度设置每列起始位置
        int[] maxColStratID = null;
        //根据每列起始stringID
        int[] maxColStratStrID = null;
        for (int i = 0; i < lineCount; i++)
        {
            if (startTable)
            {
                if (!lineTexts[i].Contains("|"))
                {
                    tableEndRowID = i;
                    break;
                }
                else
                {
                    string[] temp = lineTexts[i].Split('|');
                    //获取最大列宽
                    //???????????????????????
                    //for (int j = 0; j < temp.Length; j++)
                    for (int j = 1; j < temp.Length - 1; j++)
                    {
                        //maxColSpace[j] = Mathf.Max(maxColSpace[j], temp[j].Length);
                        maxColSpace[j - 1] = Mathf.Max(maxColSpace[j - 1], temp[j].Length);
                    }
                }
            }
            if (!startTable && lineTexts[i].Contains("|-|-|"))
            {
                startTable = true;
                tableStartRowID = i - 1;
                tableFormatRowID = i;
                MatchCollection mc = Regex.Matches(lineTexts[i], "-");
                //n = mc.Count;
                //string[] temp = lineTexts[i].Split('|');
                colCount = mc.Count; //temp.Length;
                maxColSpace = new int[colCount];
                maxColStratID = new int[colCount];
                maxColStratStrID = new int[colCount];
                string[] tempLast = lineTexts[i - 1].Split('|');
                //每列最宽的宽度暂时取表头列宽
                //????????????????
                //for (int j = 0; j < tempLast.Length; j++)
                for (int j = 1; j < tempLast.Length - 1; j++)
                {
                    //maxColSpace[j] = tempLast[j].Length;
                    maxColSpace[j - 1] = tempLast[j].Length;
                }
            }

        }
        if (tableEndRowID == 0)
            tableEndRowID = lineCount - 1;
        //根据列宽获取列起始位置
        for (int i = 1; i < maxColSpace.Length; i++)
        {
            maxColStratID[i] = maxColStratID[i - 1] + maxColSpace[i - 1];
        }

        Line[] lines = new Line[lineTexts.Length];

        //根据lines数组中各个元素的长度计算每一行中第一个点的索引，每个字、字母、空母均占6个点
        for (int i = 0; i < lines.Length; i++)
        {
            //除最后一行外，vertexs对于前面几行都有回车符占了6个点
            if (i == 0)
            {
                lines[i] = new Line(0, lineTexts[i].Length + 1);

            }
            else if (i > 0 && i < lines.Length - 1)
            {
                lines[i] = new Line(lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length + 1);
            }

            else
            {
                lines[i] = new Line(lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length);
            }
        }

        UIVertex vt;

        for (int i = 0; i < lines.Length; i++)
        {
            if (i >= tableStartRowID && i < tableEndRowID)
            {
                int startIndex = 0;
                int col = 0;
                List<int> rowStartIdInLine = new List<int>();
                //|和-的位置，用于去掉他们
                List<int> formatIdInLine = new List<int>();
                while (true)
                {

                    int index = lineTexts[i].IndexOf('|', startIndex);
                    if (index != -1)
                        formatIdInLine.Add(index);
                    ++col;
                    startIndex = index + "|".Length;

                    //判断是否是最后一个"|"
                    int nextIndex = lineTexts[i].IndexOf('|', startIndex);
                    if (nextIndex == -1 || index == -1)
                        break;

                    maxColStratStrID[col - 1] = index;
                    rowStartIdInLine.Add(index);

                    if (startIndex >= lineTexts[i].Length)
                        break;
                }
                float[] startX = new float[6];
                float firstX = 0;
                int temp = 0;
                float leftX = 0;
                float longX = 0;
                //第一个字母是|隐藏掉
                for (int j = lines[i].StartVertexIndex; j < lines[i].StartVertexIndex + 6; j++)
                {
                    if (j < 0 || j >= vertexs.Count)
                    {
                        continue;
                    }
                    vt = vertexs[j];
                    vt.position = Vector3.zero;
                    vertexs[j] = vt;
                    //以下注意点与索引的对应关系
                    if (j % 6 <= 2)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
                    }
                    if (j % 6 == 4)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                    }
                }
                for (int j = lines[i].StartVertexIndex + 6; j <= lines[i].EndVertexIndex; j++)
                {
                    if (j < 0 || j >= vertexs.Count)
                    {
                        continue;
                    }

                    //该行的第几个字母
                    int strId = (j - lines[i].StartVertexIndex) / 6;
                    int rowID = rowStartIdInLine.Count - 1;
                    //获取当前char在第几个列中
                    for (int w = 1; w < rowStartIdInLine.Count; w++)
                    {
                        if (strId < rowStartIdInLine[w])
                        {
                            rowID = w - 1;
                            break;
                        }
                    }
                    //对齐的列起始字母位置
                    //if(rowID<0)
                    //    Debug.LogError(rowID);
                    //if (maxColStratID.Length <= rowID)
                    //    Debug.LogError(rowID);
                    int rowStartID = maxColStratID[rowID];
                    //列起始string位置
                    int rowStartStrID = maxColStratStrID[rowID];

                    vt = vertexs[j];
                    //0/5     1
                    //
                    //
                    //4     2/3
                    //-----
                    //|\  |
                    //| \ |
                    //|---|
                    if (temp == 0)
                    {
                        firstX = vt.position.x;
                    }
                    int vertexID = (j - lines[i].StartVertexIndex) % 6;
                    if (vertexID == 0)
                    {
                        leftX = vt.position.x;
                    }
                    else if (vertexID == 1)
                    {
                        longX = vt.position.x - leftX;
                    }
                    float longTemp = 0;
                    if (vertexID == 1 || vertexID == 2 || vertexID == 3)
                    {
                        longTemp = longX;
                    }
                    //隐藏掉|
                    //if (strId == lineTexts[i].Length - 1 || rowStartStrID == strId)
                    if (formatIdInLine.Contains(strId))
                    {
                        longTemp = 0;
                    }
                    if (temp < 6)
                    {
                        startX[temp] = vt.position.x;
                        //Debug.LogError(vt.position.x + "," + vt.position.y);
                        ++temp;
                    }
                    //Debug.LogError("-----------");
                    //vt.position += new Vector3(_textSpacing * ((strId) / 6), 0, 0);
                    //列起始位置+列的字母ID
                    //vt.position = new Vector3(startX[(j - lines[i].StartVertexIndex) % 6] + rowStartID * (_textSpacing), vt.position.y, vt.position.z) + new Vector3((strId - rowStartStrID) * (_textSpacing), 0, 0);
                    vt.position = new Vector3(firstX + longTemp + rowID * _colSpacing + rowStartID * (_textSpacing), vt.position.y, vt.position.z) + new Vector3((strId - rowStartStrID) * (_textSpacing), 0, 0);
                    vertexs[j] = vt;
                    //以下注意点与索引的对应关系
                    if (j % 6 <= 2)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
                    }
                    if (j % 6 == 4)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                    }
                }
            }

            //for (int j = lines[i].StartVertexIndex + 6; j <= lines[i].EndVertexIndex; j++)
            //{
            //    if (j < 0 || j >= vertexs.Count)
            //    {
            //        continue;
            //    }
            //    vt = vertexs[j];
            //    vt.position += new Vector3(_textSpacing * ((j - lines[i].StartVertexIndex) / 6), 0, 0);
            //    vertexs[j] = vt;
            //    //以下注意点与索引的对应关系
            //    if (j % 6 <= 2)
            //    {
            //        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
            //    }
            //    if (j % 6 == 4)
            //    {
            //        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
            //    }
            //}
        }
    }
}
/*
     public override void ModifyMesh(VertexHelper vh)
    {

        if (!IsActive() || vh.currentVertCount == 0)
        {
            return;
        }

        Text text = GetComponent<Text>();
        if (text == null)
        {
            Debug.LogError("Missing Text component");
            return;
        }
        if (!text.text.Contains("|-|-|"))
            return;
        if (dirty)
        {
            return;
        }
        dirty = true;
        List<UIVertex> vertexs = new List<UIVertex>();
        vh.GetUIVertexStream(vertexs);
        int indexCount = vh.currentIndexCount;
        //todo 目前只支持一个表格
        string[] lineTexts = text.text.Split('\n');
        int lineCount = lineTexts.Length;
        //去除表格的形式
        string textTest = text.text.Replace("|-", " ").Replace("|", " ");
        text.text = textTest;
        //表头的列
        int tableStartRowID = 0;
        //表格格式的列|-|-|-|
        int tableFormatRowID = 0;
        //表格最后一列
        int tableEndRowID = 0;
        bool startTable = false;
        int colCount;
        //每列最宽的宽度
        int[] maxColSpace = null;
        //根据每列最宽的宽度设置每列起始位置
        int[] maxColStratID = null;
        //根据每列起始stringID
        int[] maxColStratStrID = null;
        for (int i = 0; i < lineCount; i++)
        {
            if (startTable)
            {
                if (!lineTexts[i].Contains("|"))
                {
                    tableEndRowID = i;
                    break;
                }
                else
                {
                    string[] temp = lineTexts[i].Split('|');
                    //获取最大列宽
                    //???????????????????????
                    //for (int j = 0; j < temp.Length; j++)
                    for (int j = 1; j < temp.Length - 1; j++)
                    {
                        //maxColSpace[j] = Mathf.Max(maxColSpace[j], temp[j].Length);
                        maxColSpace[j - 1] = Mathf.Max(maxColSpace[j - 1], temp[j].Length);
                    }
                }
            }
            if (!startTable && lineTexts[i].Contains("|-|-|"))
            {
                startTable = true;
                tableStartRowID = i - 1;
                tableFormatRowID = i;
                MatchCollection mc = Regex.Matches(lineTexts[i], "-");
                //n = mc.Count;
                //string[] temp = lineTexts[i].Split('|');
                colCount = mc.Count; //temp.Length;
                maxColSpace = new int[colCount];
                maxColStratID = new int[colCount];
                maxColStratStrID = new int[colCount];
                string[] tempLast = lineTexts[i - 1].Split('|');
                //每列最宽的宽度暂时取表头列宽
                //????????????????
                //for (int j = 0; j < tempLast.Length; j++)
                for (int j = 1; j < tempLast.Length - 1; j++)
                {
                    //maxColSpace[j] = tempLast[j].Length;
                    maxColSpace[j - 1] = tempLast[j].Length;
                }
            }

        }
        if (tableEndRowID == 0)
            tableEndRowID = lineCount - 1;
        //根据列宽获取列起始位置
        for (int i = 1; i < maxColSpace.Length; i++)
        {
            maxColStratID[i] = maxColStratID[i - 1] + maxColSpace[i - 1];
        }

        Line[] lines = new Line[lineTexts.Length];

        //根据lines数组中各个元素的长度计算每一行中第一个点的索引，每个字、字母、空母均占6个点
        for (int i = 0; i < lines.Length; i++)
        {
            //除最后一行外，vertexs对于前面几行都有回车符占了6个点
            if (i == 0)
            {
                lines[i] = new Line(0, lineTexts[i].Length + 1);

            }
            else if (i > 0 && i < lines.Length - 1)
            {
                lines[i] = new Line(lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length + 1);
            }

            else
            {
                lines[i] = new Line(lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length);
            }
        }

        UIVertex vt;

        for (int i = 0; i < lines.Length; i++)
        {
            if (i >= tableStartRowID && i <= tableEndRowID)
            {
                int startIndex = 0;
                int col = 0;
                List<int> rowStartIdInLine = new List<int>();
                while (true)
                {

                    int index = lineTexts[i].IndexOf('|', startIndex);

                    ++col;
                    startIndex = index + "|".Length;

                    //判断是否是最后一个"|"
                    int nextIndex = lineTexts[i].IndexOf('|', startIndex);
                    if (nextIndex == -1 || index == -1)
                        break;

                    maxColStratStrID[col - 1] = index;
                    rowStartIdInLine.Add(index);

                    if (startIndex >= lineTexts[i].Length)
                        break;
                }
                float[] startX = new float[6];
                float firstX = 0;
                int temp = 0;
                float leftX = 0;
                float longX = 0;
                for (int j = lines[i].StartVertexIndex + 6; j <= lines[i].EndVertexIndex; j++)
                {
                    if (j < 0 || j >= vertexs.Count)
                    {
                        continue;
                    }

                    //该行的第几个字母
                    int strId = (j - lines[i].StartVertexIndex) / 6;
                    int rowID = rowStartIdInLine.Count - 1;
                    //获取当前char在第几个列中
                    for (int w = 1; w < rowStartIdInLine.Count; w++)
                    {
                        if (strId < rowStartIdInLine[w])
                        {
                            rowID = w - 1;
                            break;
                        }
                    }
                    //对齐的列起始字母位置
                    int rowStartID = maxColStratID[rowID];
                    //列起始string位置
                    int rowStartStrID = maxColStratStrID[rowID];

                    vt = vertexs[j];
                    //0/5     1
                    //
                    //
                    //4     2/3
                    //-----
                    //|\  |
                    //| \ |
                    //|---|
                    if (temp == 0)
                    {
                        firstX = vt.position.x;
                    }
                    int vertexID = (j - lines[i].StartVertexIndex) % 6;
                    if (vertexID == 0)
                    {
                        leftX = vt.position.x;
                    }
                    else if (vertexID == 1)
                    {
                        longX = vt.position.x - leftX;
                    }
                    float longTemp = 0;
                    if (vertexID == 1 || vertexID == 2 || vertexID == 3)
                    {
                        longTemp = longX;
                    }
                    if (temp < 6)
                    {
                        startX[temp] = vt.position.x;
                        Debug.LogError(vt.position.x + "," + vt.position.y);
                        ++temp;
                    }
                    Debug.LogError("-----------");
                    //vt.position += new Vector3(_textSpacing * ((strId) / 6), 0, 0);
                    //列起始位置+列的字母ID
                    //vt.position = new Vector3(startX[(j - lines[i].StartVertexIndex) % 6] + rowStartID * (_textSpacing), vt.position.y, vt.position.z) + new Vector3((strId - rowStartStrID) * (_textSpacing), 0, 0);
                    vt.position = new Vector3(firstX + longTemp + rowStartID * (_textSpacing), vt.position.y, vt.position.z) + new Vector3((strId - rowStartStrID) * (_textSpacing), 0, 0);
                    vertexs[j] = vt;
                    //以下注意点与索引的对应关系
                    if (j % 6 <= 2)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
                    }
                    if (j % 6 == 4)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                    }
                }
            }

            //for (int j = lines[i].StartVertexIndex + 6; j <= lines[i].EndVertexIndex; j++)
            //{
            //    if (j < 0 || j >= vertexs.Count)
            //    {
            //        continue;
            //    }
            //    vt = vertexs[j];
            //    vt.position += new Vector3(_textSpacing * ((j - lines[i].StartVertexIndex) / 6), 0, 0);
            //    vertexs[j] = vt;
            //    //以下注意点与索引的对应关系
            //    if (j % 6 <= 2)
            //    {
            //        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
            //    }
            //    if (j % 6 == 4)
            //    {
            //        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
            //    }
            //}
        }
    }
     */

