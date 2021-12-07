using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NavMove : MonoBehaviour
{
    NavMeshAgent nma;
    Vector3 vTarget;
    bool bMove = false;
    bool bMoveLerp = false;
    // Use this for initialization
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();

    }

    IEnumerator MoveToEnd(Vector3 ePos)
    {
        bMoveLerp = true;
        while (true)
        {
            Vector3 vDelta = this.transform.position - ePos;
            vDelta.y = 0.0f;    //�ѩ󨤦⪺�ڸ`�I�i�ण�O�]�b�}���A�]����y�o��
            Debug.Log(vDelta);
            if (vDelta.magnitude < 0.01f)   //�p�G�Z���w�����ܤp���d��
            {
                this.transform.position = ePos; //�N�������ʨ�ؼ��I
                break;
            }
            this.transform.position = Vector3.Lerp(this.transform.position, ePos, 0.1f);    //�ót���ʨ�ؼ��I
            yield return 0;
        }

        nma.CompleteOffMeshLink();  //�������|���ʡA�_�h�|�d�b����
        bMoveLerp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rh;
            if (Physics.Raycast(r, out rh, 1000.0f))
            {
                if (rh.collider.gameObject != this.gameObject)
                {
                    bMove = true;
                    vTarget = rh.point;
                    // nma.SetDestination(vTarget);
                    nma.isStopped = false;
                    nma.destination = vTarget;
                    Debug.Log("DDDDDE");
                    return;
                }
            }
        }


        //Debug.Log(nma.isStopped + ":" + nma.remainingDistance);


        if (nma.isStopped == false && nma.remainingDistance > 0.0001f)
        {
            OffMeshLinkData omld = nma.currentOffMeshLinkData;
            if (omld.activated && bMoveLerp == false)
            {
                StartCoroutine(MoveToEnd(omld.endPos));
                //this.transform.position = omld.endPos;
                //nma.CompleteOffMeshLink();
            }
        }
        else
        {
            nma.isStopped = true;
        }


    }
}
