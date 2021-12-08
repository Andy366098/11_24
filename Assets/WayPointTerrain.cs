using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WayPointTerrain 
{
    public List<PathNode> mNodeList;
    //還沒加牆壁
    public void Init()
    {
        mNodeList = new List<PathNode>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");//找到所有Tag為WP的物件存入陣列
        foreach(GameObject g in gos)    //初始化每個點的資料存入PathNode陣列
        {
            PathNode p = new PathNode();
            p.mfF = p.mfG = p.mfH = 0.0f;
            p.mParent = null;
            p.neibors = new List<PathNode>();
            p.mPos = g.transform.position;
            p.mGo = g;
            mNodeList.Add(p);
        }
        LoadWP();

    }
    public void LoadWP()
    {
        StreamReader sr = new StreamReader("Assets/aaa.txt");
        if (sr == null)
        {
            sr.Close();
            return;
        }
        
        string sAll = sr.ReadToEnd();
        string[] sLines = sAll.Split('\n');
        int lineIndex = 0;
        while (lineIndex < sLines.Length)
        {
            string s = sLines[lineIndex];   //目前處理的行
            lineIndex++;    //行的索引+1
            string[] ss = s.Split(','); //用字串裡的,分隔
            PathNode pCurrent = null;
            for (int i = 0; i < mNodeList.Count; i++)
            {
                if (mNodeList[i].mGo.name.Equals(ss[0]))    //將檔案裡的點跟儲存的陣列比對
                {
                    pCurrent = mNodeList[i]; //拿到現在處理的點
                    break;
                }
            }
            if (pCurrent == null)
            {
                continue;   //如果都處理完就跳出迴圈
            }
            int numNeibors = int.Parse(ss[1]);  //拿到鄰居數量
            int iIndex = 2;     //由下一個開始
            for (int i = 0; i < numNeibors; i++)
            {
                string sName = ss[iIndex + i];  //拿到鄰居的名字
                for (int j = 0; j < mNodeList.Count; j++)   //用For迴圈遍歷陣列來找到其鄰居
                {
                    if (mNodeList[j].mGo.name.Equals(sName))
                    {
                        pCurrent.neibors.Add(mNodeList[j]); 
                        break;
                    }
                }
            }
        }

    }
}
