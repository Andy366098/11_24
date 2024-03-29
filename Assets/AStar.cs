using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar 
{
    WayPointTerrain mTerrain;
    List<PathNode> mOpenList;
    List<PathNode> mCloseList;

    List<Vector3> mPathList;
    static public AStar mInstance; //Singleton
    public void Init(WayPointTerrain terrain)
    {
        mTerrain = terrain;
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
    //A*演算法
    public bool PerformAStar(Vector3 startPos,Vector3 endPos)
    {
        //讀取檔案的資料獲得最靠近的路徑點
        PathNode startNode = mTerrain.GetNodeFromPosition(startPos);    
        PathNode endNode = mTerrain.GetNodeFromPosition(endPos);
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
        mTerrain.ClearAStarInfo();  //清除前一次AStar的資料
        PathNode currentNode;
        PathNode newNode;
        mOpenList.Add(startNode);   //先加入起始點
        while (mOpenList.Count > 0) //如果OpenList裡還有Node
        {
            currentNode = GetBestNode();    //獲得OpenList裡最小Cost的點
            if(currentNode == endNode)  //如果現在的點就是終點就建立路徑
            {
                BuildPath(startPos,endPos,startNode,endNode);
                return true;
            }
            Vector3 distance;
            for(int i = 0; i < currentNode.neibors.Count; i++)
            {
                newNode = currentNode.neibors[i];   //下一步要走的就是鄰居
                distance = currentNode.mPos - newNode.mPos;
                //鄰居新的Cost from Start就是起步的點的CFS加上到鄰居的Travel Cost
                float newG = currentNode.mfG + distance.magnitude;  
                //按照A*寫，但這樣Continue只能判定到內圈，因此跟老師一樣用狀態表示Close
                /*for (int i = 0;i < mOpenList.Count; i++)
                {
                    if (currentNode == mOpenList[i] && currentNode.mfG <= newG)
                        continue;
                }*/
                ///這邊跟老師的有點不同，用的是王老師講解的，想看看有哪裡不同，結果好像一樣
                if (newNode.pathNodeState == PathNodeState.NODE_OPENED
                    || newNode.pathNodeState == PathNodeState.NODE_CLOSED)
                {
                    if (newNode.mfG <= newG)
                        continue;
                }
                ///
                newNode.mParent = currentNode;
                newNode.mfG = newG;     //將Cost from Start設為新的
                distance = endNode.mPos - newNode.mPos;
                newNode.mfH = distance.magnitude;   //Cost to Goal
                newNode.mfF = newNode.mfG + newNode.mfH;
                newNode.pathNodeState = PathNodeState.NODE_OPENED;  //將點的狀態設為OPPENED
                mOpenList.Add(newNode); //並加入Open List
            }
            currentNode.pathNodeState = PathNodeState.NODE_CLOSED;
        }
        return false;
    }
    
}
