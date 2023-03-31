using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//用于获取text里某文字的坐标，用于实现文字中的按钮
public class TextMoveHelper : MonoBehaviour
{

    //public Text textComp;
    //public Canvas canvas;

    //public Text text;

    public Vector3 GetPosAtText(Canvas canvas, Text text, string strFragment)
    {
        int strFragmentIndex = text.text.IndexOf(strFragment);//-1表示不包含strFragment
        Vector3 stringPos = Vector3.zero;
        if (strFragmentIndex > -1)
        {
            Vector3 firstPos = GetPosAtText(canvas, text, strFragmentIndex + 1);
            Vector3 lastPos = GetPosAtText(canvas, text, strFragmentIndex + strFragment.Length);
            stringPos = (firstPos + lastPos) * 0.5f;
        }
        else
        {
            stringPos = GetPosAtText(canvas, text, strFragmentIndex);
        }
        return stringPos;
    }

    /// <summary>
    /// 得到Text中字符的位置；canvas:所在的Canvas，text:需要定位的Text,charIndex:Text中的字符位置
    /// </summary>
    public Vector3 GetPosAtText(Canvas canvas, Text text, int charIndex)
    {
        string textStr = text.text;
        Vector3 charPos = Vector3.zero;
        if (charIndex <= textStr.Length && charIndex > 0)
        {
            TextGenerator textGen = new TextGenerator(textStr.Length);
            Vector2 extents = text.gameObject.GetComponent<RectTransform>().rect.size;
            textGen.Populate(textStr, text.GetGenerationSettings(extents));

            int newLine = textStr.Substring(0, charIndex).Split('\n').Length - 1;
            int whiteSpace = textStr.Substring(0, charIndex).Split(' ').Length - 1;
            int indexOfTextQuad = (charIndex * 4) + (newLine * 4) - 4;
            if (indexOfTextQuad < textGen.vertexCount)
            {
                charPos = (textGen.verts[indexOfTextQuad].position +
                    textGen.verts[indexOfTextQuad + 1].position +
                    textGen.verts[indexOfTextQuad + 2].position +
                    textGen.verts[indexOfTextQuad + 3].position) / 4f;


            }
        }
        charPos /= canvas.scaleFactor;//适应不同分辨率的屏幕
        charPos = text.transform.TransformPoint(charPos);//转换为世界坐标
        return charPos;
    }

    //string a = "跟";
    //string b = "一起";
    //string c = "学习";
    //string d = "学习中文";
    //int i = 0;
    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 10, 100, 80), "Test"))
    //    {
    //        switch (i)
    //        {
    //            case 0:
    //                StartCoroutine(LerpMove(a));
    //                break;
    //            case 1:
    //                StartCoroutine(LerpMove(b));
    //                break;
    //            case 2:
    //                StartCoroutine(LerpMove(c));
    //                break;
    //            case 3:
    //                StartCoroutine(LerpMove(d));
    //                i = -1;
    //                break;
    //        }
    //        i++;

    //    }
    //}
    //IEnumerator LerpMove(string content)
    //{
    //    text.text = content;
    //    text.rectTransform.position = GetPosAtText(canvas, textComp, content);
    //    Vector3 endPos = canvas.transform.TransformPoint(Vector3.zero);
    //    yield return new WaitForSeconds(0.3f);
    //    yield return new WaitUntil(() =>
    //    {
    //        text.rectTransform.position = Vector3.Lerp(text.rectTransform.position, endPos, Time.deltaTime);
    //        return Vector3.Distance(text.rectTransform.position, endPos) < 0.1f;

    //    });

    //}

}