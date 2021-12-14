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
        //增加障礙物進List
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
        if (Input.GetMouseButtonDown(0))    //滑鼠按下左鍵
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);  //打個射線到目標點
            RaycastHit rh;
            if (Physics.Raycast(r,out rh,1000.0f,1 << LayerMask.NameToLayer("Terrain"))) //如果在Terrain地形上
            {
                m_bAstar = AStar.mInstance.PerformAStar(m_Control.transform.position, rh.point);
                m_Npc.m_iCurrentPathPt = 0;
                //m_Npc.SetTarget(rh.point);    //把上兩行刪掉改成這個就只有Seek
                    
                
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
                //如果打個射線會撞到牆就continue，直到不會撞到牆才走這個直線距離
                //否則走前一個點的直線距離
                Vector3 sPos = path[i];
                Vector3 cPos = m_Control.transform.position;
                if (Physics.Linecast(cPos, sPos, 1 << LayerMask.NameToLayer("Wall")))
                {
                    continue;
                }
                //嘗試改成打三條失敗，還是會撞到
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
                //嘗試用BoxCast失敗
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
            List<Vector3> path = AStar.mInstance.GetPath(); //畫出AStar的線
            Gizmos.color = Color.blue;
            int iCount = path.Count - 1;
            int i;
            for (i = 0; i < iCount; i++)
            {
                Vector3 sPos = path[i];
                sPos.y += 1.0f; //浮空一點的線
                Vector3 ePos = path[i + 1];
                ePos.y += 1.0f;
                Gizmos.DrawLine(sPos, ePos);

            }

        }
    }
}
