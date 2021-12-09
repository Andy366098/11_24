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
        if (Input.GetMouseButtonDown(0))    //�ƹ����U����
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);  //���Ӯg�u��ؼ��I
            RaycastHit rh;
            if (Physics.Raycast(r,out rh,1000.0f,1 << LayerMask.NameToLayer("Terrain"))) //�p�G�bTerrain�a�ΤW
            {
                m_bAstar = AStar.mInstance.PerformAStar(m_Control.transform.position, rh.point);
                m_Npc.m_iCurrentPathPt = 0;
                //m_Npc.SetTarget(rh.point);    //�o��O�令�u��Seek
                    
                
            }
        }
        //m_Npc.SetTarget(m_Target.transform.position); //�o��O�令Seek�Y�Ӫ���

        if (m_bAstar)   //�p�GAstar���\
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
            List<Vector3> path = AStar.mInstance.GetPath(); //�e�XAStar���u
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
