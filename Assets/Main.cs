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
        if (Input.GetMouseButtonDown(0))    //�ƹ����U����
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);  //���Ӯg�u��ؼ��I
            RaycastHit rh;
            if (Physics.Raycast(r,out rh,1000.0f,1 << LayerMask.NameToLayer("Terrain")))
            {
                //��Astar���A���Ȯɵ��ѱ��ݬ�Seek�i���i���`�B�@
                //m_bAstar = AStar.mInstance.PerformAStar(m_Control.transform.position, rh.point);
                //m_Npc.m_iCurrentPathPt = 0;
                
                m_Npc.SetTarget(rh.point);
                    
                
            }
        }
        //if (m_bAstar)   //�p�GAstar���\
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
