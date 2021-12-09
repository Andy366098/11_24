using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject m_Control;
    public GameObject m_Target;
    private NPC m_Npc;
    private bool m_bAstar = false;
    void Start()
    {
        WayPointTerrain wayPointTerrain = new WayPointTerrain();
        wayPointTerrain.Init();
        AStar aStar = new AStar();
        aStar.Init(wayPointTerrain);
        m_Npc = m_Control.GetComponent<NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))    //滑鼠按下左鍵
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);  //打個射線到目標點
            RaycastHit rh;
            if (Physics.Raycast(r,out rh,1000.0f,1 << LayerMask.NameToLayer("Terrain"))) //如果在Terrain地形上
            {
                m_bAstar = AStar.mInstance.PerformAStar(m_Control.transform.position, rh.point);
                m_Npc.m_iCurrentPathPt = 0;
                //m_Npc.SetTarget(rh.point);    //這行是改成只有Seek
                    
                
            }
        }
        //m_Npc.SetTarget(m_Target.transform.position); //這行是改成Seek某個物件

        if (m_bAstar)   //如果Astar成功
        {
            List<Vector3> path = AStar.mInstance.GetPath();
            int iFinal = path.Count - 1;
            int i;
            for (i = iFinal; i >= m_Npc.m_iCurrentPathPt; i--)
            {
                Vector3 sPos = path[i];
                m_Npc.m_iCurrentPathPt = i;
                m_Npc.SetTarget(sPos);
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (m_bAstar)
        {
            List<Vector3> path = AStar.mInstance.GetPath(); //畫出AStar的線
            Gizmos.color = Color.blue;
            int iCount = path.Count - 1;
            int i;
            for (i = 0; i < iCount; i++)
            {
                Vector3 sPos = path[i];
                sPos.y += 1.0f;
                Vector3 ePos = path[i + 1];
                ePos.y += 1.0f;
                Gizmos.DrawLine(sPos, ePos);

            }

        }
    }
}
