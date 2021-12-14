using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main m_Instance;
    public GameObject m_Control;
    public GameObject m_Target;
    private NPC m_Npc;
    private bool m_bAstar = false;
    private List<Obstacle> m_Obstacles;
    private void Awake()
    {
        m_Instance = this;
    }
    void Start()
    {
        WayPointTerrain wayPointTerrain = new WayPointTerrain();
        wayPointTerrain.Init();
        AStar aStar = new AStar();
        aStar.Init(wayPointTerrain);
        m_Npc = m_Control.GetComponent<NPC>();
        //�W�[��ê���iList
        m_Obstacles = new List<Obstacle>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Obstacle");
        if (gos != null || gos.Length > 0)
        {
            foreach (GameObject go in gos)
            {
                m_Obstacles.Add(go.GetComponent<Obstacle>());
            }
        }
    }
    public List<Obstacle> GetObstacles()
    {
        return m_Obstacles;
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
                //m_Npc.SetTarget(rh.point);    //��W���R���令�o�ӴN�u��Seek
                    
                
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
                //�p�G���Ӯg�u�|������Ncontinue�A���줣�|������~���o�Ӫ��u�Z��
                //�_�h���e�@���I�����u�Z��
                Vector3 sPos = path[i];
                Vector3 cPos = m_Control.transform.position;
                if (Physics.Linecast(cPos, sPos, 1 << LayerMask.NameToLayer("Wall")))
                {
                    continue;
                }
                //���է令���T�����ѡA�٬O�|����
                //Vector3 sPos = path[i];
                //Vector3 leftSPos = sPos - m_Control.transform.right * 1.0f;
                //Vector3 rightSPos = sPos + m_Control.transform.right * 1.0f;
                //Vector3 cPos = m_Control.transform.position;
                //Vector3 leftCPos = cPos - m_Control.transform.right * 1.0f;
                //Vector3 rightCPos = cPos + m_Control.transform.right * 1.0f;
                //if (Physics.Linecast(cPos, sPos, 1 << LayerMask.NameToLayer("Wall")) ||
                //    Physics.Linecast(leftCPos, leftSPos, 1 << LayerMask.NameToLayer("Wall")) ||
                //    Physics.Linecast(rightCPos, rightSPos, 1 << LayerMask.NameToLayer("Wall")))
                //{
                //    continue;
                //}
                //���ե�BoxCast����
                /*if (Physics.BoxCast(cPos,new Vector3(1.0f,1.0f,1.0f),sPos - cPos,Quaternion.identity,100.0f,1 << LayerMask.NameToLayer("Wall"))){
                    continue;
                }*/
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
                sPos.y += 1.0f; //�B�Ť@�I���u
                Vector3 ePos = path[i + 1];
                ePos.y += 1.0f;
                Gizmos.DrawLine(sPos, ePos);

            }

        }
    }
}
