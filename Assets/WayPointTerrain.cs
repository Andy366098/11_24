using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WayPointTerrain 
{
    public List<PathNode> mNodeList;
    //ÁÙ¨S¥[Àð¾À
    public void Init()
    {
        mNodeList = new List<PathNode>();

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

        }

    }
}
