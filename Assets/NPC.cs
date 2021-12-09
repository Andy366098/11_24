using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public AIData m_Data;
    public int m_iCurrentPathPt;    //�����ثe�������WayPoint

    // Start is called before the first frame update
    void Awake()
    {   //��l�t�m
        //m_Data = new AIData();
        m_Data.m_fRadius = 1.0f;
        m_Data.m_Speed = 0.0f;
        m_Data.m_fArriveRange = 0.1f;
        m_Data.m_fDeSpeedRange = 10.0f;
        m_Data.m_fMaxSpeed = 10.0f;
        m_Data.m_fMaxRot = 5.0f;
        m_Data.m_Go = gameObject;
        m_Data.m_vTarget = transform.position;
        m_iCurrentPathPt = -1;
    }

    // Update is called once per frame
    void Update()
    {
        SteeringBehavior.Seek(m_Data);
        SteeringBehavior.Move(m_Data);
    }
    public void SetTarget(Vector3 v)    //�]�m�ؼ��I
    {
        m_Data.m_vTarget = v;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2.0f); //�e�X���V���u

        if (m_Data != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_Data.m_vTarget, m_Data.m_fArriveRange);  //�e�X�ؼ��I��F�b�|
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(m_Data.m_vTarget, m_Data.m_fDeSpeedRange);  //�e�X�ؼ��I��t�b�|
            Gizmos.color = Color.magenta;
            Vector3 target = m_Data.m_vTarget;
            Vector3 go = transform.position;
            go.y = target.y;
            Gizmos.DrawLine(go,target);  //�e�X���|���u
            //�e�X���w
            Gizmos.color = Color.yellow;    
            Vector3 vLeftStart = transform.position - transform.right * m_Data.m_fRadius;
            Vector3 vLeftEnd = vLeftStart + transform.forward * m_Data.m_fProbeLength;
            Gizmos.DrawLine(vLeftStart, vLeftEnd);
            Vector3 vRightStart = transform.position + transform.right * m_Data.m_fRadius;
            Vector3 vRightEnd = vRightStart + transform.forward * m_Data.m_fProbeLength;
            Gizmos.DrawLine(vRightStart, vRightEnd);
            Gizmos.DrawLine(vRightEnd, vLeftEnd);
        }
    }
}
