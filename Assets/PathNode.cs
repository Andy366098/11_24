using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathNodeState   //�Ϊ��A�Ӫ�ܦbOpen��CloseList��
{
    NODE_NONE = -1,
    NODE_OPENED,
    NODE_CLOSED
}
public class PathNode       //�ΨӦsAstar��WayPoint�����
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
    
