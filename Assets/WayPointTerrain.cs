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
        LoadWP();//Ū����r�ɸ̪����|�I�F�~���

    }
    public void ClearAStarInfo()    //�M���e�@��Astar�����
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
                Vector3 vec = node.mPos - pos;  //���|�I��ؼ��I���V�q
                vec.y = 0.0f;   //�o��y
                float mag = vec.magnitude;  //�o��V�q����
                if (mag < max)  //����o�X�̾a�񪺸��|�I
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
            string s = sLines[lineIndex];   //�ثe�B�z����
            //Debug.Log(s);
            lineIndex++;    //�檺����+1
            string[] ss = s.Split(','); //�Φr��̪�,���j=>�C�@�浲���]�n���r���_�h���ѷ|�X���D
            foreach(string a in ss)
            {
                Debug.Log(a);
            }
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
            int iIndex = 2;     //�ѤU�@�q�r�}�l
            for (int i = 0; i < numNeibors; i++)
            {
                string sName = ss[iIndex + i];  //����F�~���W�r
                //Debug.Log(sName);
                for (int j = 0; j < mNodeList.Count; j++)   //��For�j��M���}�C�ӧ���F�~
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
