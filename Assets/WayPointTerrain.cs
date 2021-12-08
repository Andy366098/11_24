using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WayPointTerrain 
{
    public List<PathNode> mNodeList;
    //�٨S�[���
    public void Init()
    {
        mNodeList = new List<PathNode>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");//���Ҧ�Tag��WP������s�J�}�C
        foreach(GameObject g in gos)    //��l�ƨC���I����Ʀs�JPathNode�}�C
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
            string s = sLines[lineIndex];   //�ثe�B�z����
            lineIndex++;    //�檺����+1
            string[] ss = s.Split(','); //�Φr��̪�,���j
            PathNode pCurrent = null;
            for (int i = 0; i < mNodeList.Count; i++)
            {
                if (mNodeList[i].mGo.name.Equals(ss[0]))    //�N�ɮ׸̪��I���x�s���}�C���
                {
                    pCurrent = mNodeList[i]; //����{�b�B�z���I
                    break;
                }
            }
            if (pCurrent == null)
            {
                continue;   //�p�G���B�z���N���X�j��
            }
            int numNeibors = int.Parse(ss[1]);  //����F�~�ƶq
            int iIndex = 2;     //�ѤU�@�Ӷ}�l
            for (int i = 0; i < numNeibors; i++)
            {
                string sName = ss[iIndex + i];  //����F�~���W�r
                for (int j = 0; j < mNodeList.Count; j++)   //��For�j��M���}�C�ӧ���F�~
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
