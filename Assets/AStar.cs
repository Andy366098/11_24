using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar 
{
    List<PathNode> mOpenList;
    List<PathNode> mCloseList;

    List<Vector3> mPathList;
    static public AStar mInstance; //Singleton
    public void Init()
    {
        mOpenList = new List<PathNode>();
        mCloseList = new List<PathNode>();
        mPathList = new List<Vector3>();
        mInstance = this;
    }
    public List<Vector3> GetPath()
    {
        return mPathList;
    }
    private PathNode GetBestNode()  //�o��O��XOpenList��TotalCost�̤p��
    {
        PathNode rn = null;
        float fMax = 10000.0f;
        foreach(PathNode n in mOpenList)
        {
            if (n.mfF < fMax)
            {
                fMax = n.mfF;
                rn = n;
            }
        }
        mOpenList.Remove(rn);
        return rn;
    }
    //�o�̬O���[�J�_�l�I�A�ç�|�g�L��WayPoint�Q�Ψ�Parent���[�JPathList�̡A�̫�[�J���I��_��
    private void BuildPath(Vector3 startPos,Vector3 endPos,PathNode startNode,PathNode endNode)
    {
        mPathList.Clear();

        mPathList.Add(startPos);

        if (startNode == endNode)
        {
            mPathList.Add(startNode.mPos);
        }
        else
        {
            PathNode pathNode = endNode;
            while(pathNode != null)
            {
                mPathList.Insert(1, pathNode.mPos);
                pathNode = pathNode.mParent;
            }
        }
        mPathList.Add(endPos);
    }
    //A*�t��k�Astart��endNode�٨S�令Ū���ɮת����
    public bool PerformAStar(Vector3 startPos,Vector3 endPos)
    {
        PathNode startNode = new PathNode();
        PathNode endNode = new PathNode();
        if(startNode == null || endNode == null)
        {
            Debug.Log("�bAStar�a�ϤW�䤣��`�I");
            return false;
        }else if (startNode == endNode)
        {
            //�إ߸��|
            BuildPath(startPos, endPos, startNode, endNode);
            return true;
        }
        mOpenList.Clear();
        mCloseList.Clear();
        //
        PathNode currentNode;
        PathNode newNode;
        mOpenList.Add(startNode);
        while (mOpenList.Count > 0)
        {
            currentNode = GetBestNode();
            if(currentNode == endNode)
            {
                BuildPath(startPos,endPos,startNode,endNode);
                return true;
            }
            Vector3 distance;
            for(int i = 0; i < currentNode.neibors.Count; i++)
            {
                newNode = currentNode.neibors[i];
                distance = currentNode.mPos - newNode.mPos;
                float newG = newNode.mfG + distance.magnitude;
                //����A*�g�A���o��Continue�u��P�w�줺��
                /*for (int i = 0;i < mOpenList.Count; i++)
                {
                    if (currentNode == mOpenList[i] && currentNode.mfG <= newG)
                        continue;
                }*/
                ///�o���Ѯv�����I���P�A�Ϊ��O���Ѯv���Ѫ��A�Q�ݬݦ����̤��P
                if (newNode.pathNodeState == PathNodeState.NODE_OPENED
                    || newNode.pathNodeState == PathNodeState.NODE_CLOSED)
                {
                    if (newNode.mfG <= newG)
                        continue;
                }
                ///
                newNode.mParent = currentNode;
                newNode.mfG = newG;
                distance = endNode.mPos - newNode.mPos;
                newNode.mfH = distance.magnitude;
                newNode.mfF = newNode.mfG + newNode.mfH;
                newNode.pathNodeState = PathNodeState.NODE_OPENED;
                mOpenList.Add(newNode);
            }
            currentNode.pathNodeState = PathNodeState.NODE_CLOSED;
        }
        return false;
    }
    
}
