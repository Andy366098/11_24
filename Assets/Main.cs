using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject m_Control;
    private NPC m_Npc;
    private bool m_bAstar = false;
    void Start()
    {

        AStar aStar = new AStar();
        aStar.Init();
        m_Npc = m_Control.GetComponent<NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))    //滑鼠按下左鍵
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);  //打個射線到目標點
            RaycastHit rh;
            if (Physics.Raycast(r,out rh,1000.0f,1 << LayerMask.NameToLayer("Terrain")))
            {
                //用Astar的，先暫時註解掉看看Seek可不可正常運作
                //m_bAstar = AStar.mInstance.PerformAStar(m_Control.transform.position, rh.point);
                //m_Npc.m_iCurrentPathPt = 0;
                
                m_Npc.SetTarget(rh.point);
                    
                
            }
        }
        //if (m_bAstar)   //如果Astar成功
        //{
        //    List<Vector3> path = AStar.mInstance.GetPath();
        //    int iFinal = path.Count - 1;
        //    int i;
        //    for (i = iFinal; i >= m_Npc.m_iCurrentPathPt; i--)
        //    {
        //        Vector3 sPos = path[i];
        //        Vector3 cPos = m_Control.transform.position;
        //        m_Npc.m_iCurrentPathPt = i;
        //        m_Npc.SetTarget(sPos);
        //        break;
        //    }
        //}
    }
}
