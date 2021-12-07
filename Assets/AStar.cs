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
    private PathNode GetBestNode()  //這邊是找出OpenList裡TotalCost最小的
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
    //這裡是先加入起始點，並把會經過的WayPoint利用其Parent都加入PathList裡，最後加入終點串起來
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
    //A*演算法，start跟endNode還沒改成讀取檔案的資料
    public bool PerformAStar(Vector3 startPos,Vector3 endPos)
    {
        PathNode startNode = new PathNode();
        PathNode endNode = new PathNode();
        if(startNode == null || endNode == null)
        {
            Debug.Log("在AStar地圖上找不到節點");
            return false;
        }else if (startNode == endNode)
        {
            //建立路徑
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
                //按照A*寫，但這樣Continue只能判定到內圈
                /*for (int i = 0;i < mOpenList.Count; i++)
                {
                    if (currentNode == mOpenList[i] && currentNode.mfG <= newG)
                        continue;
                }*/
                ///這邊跟老師的有點不同，用的是王老師講解的，想看看有哪裡不同
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
