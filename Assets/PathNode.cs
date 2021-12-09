using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathNodeState   //用狀態來表示在Open或CloseList裡
{
    NODE_NONE = -1,
    NODE_OPENED,
    NODE_CLOSED
}
public class PathNode       //用來存Astar的WayPoint的資料
{
    public GameObject mGo;
    public List<PathNode> neibors;

    public Vector3 mPos;
    public PathNode mParent;
    public float mfG;   //Cost from Start
    public float mfH;   //Cost to Goal
    public float mfF;   //Total Cost
    public PathNodeState pathNodeState;
}
    
