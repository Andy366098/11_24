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
        LoadWP();//讀取文字檔裡的路徑點鄰居資料

    }
    public void ClearAStarInfo()    //清除前一次Astar的資料
    {
        foreach (PathNode node in mNodeList)
        {
            node.mParent = null;
            node.mfF = 0.0f;
            node.mfG = 0.0f;
            node.mfH = 0.0f;
            node.pathNodeState = PathNodeState.NODE_NONE;
        }
    }
    public PathNode GetNodeFromPosition(Vector3 pos)
    {
        PathNode rnode = null;
        PathNode node;
        int iC = mNodeList.Count;
        float max = 100000.0f;
        if (mNodeList != null)
        {
            for (int i = 0; i < iC; i++)
            {
                node = mNodeList[i];
                Vector3 vec = node.mPos - pos;  //路徑點到目標點的向量
                vec.y = 0.0f;   //濾掉y
                float mag = vec.magnitude;  //得到向量長度
                if (mag < max)  //比較得出最靠近的路徑點
                {
                    max = mag;
                    rnode = node;
                }
            }
        }
        return rnode;
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
            //Debug.Log(s);
            lineIndex++;    //行的索引+1
            string[] ss = s.Split(','); //用字串裡的,分隔=>每一行結尾也要有逗號否則辨識會出問題
            foreach(string a in ss)
            {
                Debug.Log(a);
            }
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
            int iIndex = 2;     //由下一段字開始
            for (int i = 0; i < numNeibors; i++)
            {
                string sName = ss[iIndex + i];  //拿到鄰居的名字
                //Debug.Log(sName);
                for (int j = 0; j < mNodeList.Count; j++)   //用For迴圈遍歷陣列來找到其鄰居
                {
                    //Debug.Log(j);
                    //Debug.Log(mNodeList[j].mGo.name);
                    if (mNodeList[j].mGo.name.Equals(sName))
                    {
                        pCurrent.neibors.Add(mNodeList[j]);
                        //Debug.Log(mNodeList[j].mGo.name);
                        break;
                    }
                }
            }
        }

    }
}
