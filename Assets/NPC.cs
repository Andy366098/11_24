using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    AIData m_Data;
    public int m_iCurrentPathPt;    //紀錄目前走到哪個WayPoint

    // Start is called before the first frame update
    void Start()
    {   //初始配置
        m_Data = new AIData();
        m_Data.m_Speed = 0.0f;
        m_Data.m_fArriveRange = 2.0f;
        m_Data.m_fMaxSpeed = 2.5f;
        m_Data.m_fMaxRot = 5.0f;
        m_Data.m_Go = gameObject;
        m_Data.m_vTarget = transform.position;
        m_iCurrentPathPt = -1;
    }

    // Update is called once per frame
    void Update()
    {
        SteeringBehavior.Seek(m_Data);
    }
    public void SetTarget(Vector3 v)    //設置目標點
    {
        m_Data.m_vTarget = v;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2.0f); //畫出目標的線

        if (m_Data != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_Data.m_vTarget, m_Data.m_fArriveRange);  //畫出目標點到達半徑
            
        }
    }
}
