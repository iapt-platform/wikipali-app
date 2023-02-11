using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArticleController;

public class CommonTool
{
    //返回翻译标题
    //todo：不同语言 标题不同
    public static string GetBookTranslateName(Book book)
    {
        if (string.IsNullOrEmpty(book.translateName))
            return book.toc;
        else
            return book.translateName;
    }
}
