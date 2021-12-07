using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest : MonoBehaviour {

    NavMeshAgent nma;
    Vector3 vTarget;
    bool bMove = false;
    bool bMoveLerp = false;

    NavMeshPath nmp;    //用來存他系統產生的路徑點
	// Use this for initialization
	void Start () {
        nma = GetComponent<NavMeshAgent>();
        nmp = new NavMeshPath();
    }

    IEnumerator MoveToEnd(Vector3 ePos)
    {
        bMoveLerp = true;
        while (true)
        {
            Vector3 vDelta = this.transform.position - ePos;
            vDelta.y = 0.0f;    //由於角色的根節點可能不是設在腳底，因此把y濾掉
            Debug.Log(vDelta);
            if (vDelta.magnitude < 0.01f)   //如果距離已接近到很小的範圍內
            {
                this.transform.position = ePos; //就直接移動到目標點
                break;
            }
            this.transform.position = Vector3.Lerp(this.transform.position, ePos, 0.1f);    //勻速移動到目標點
            yield return 0;
        }

        nma.CompleteOffMeshLink();  //完成路徑移動，否則會卡在那裏
        bMoveLerp = false;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rh;
            if (Physics.Raycast(r, out rh, 1000.0f))
            {
                if (rh.collider.gameObject != this.gameObject)
                {
                    vTarget = rh.point;
                    bMove = nma.CalculatePath(vTarget, nmp);    //用它內建的函式將路徑存在他的List
                    nma.isStopped = false;
                    nma.destination = vTarget;
                    return;
                }
            }
        }
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

    private void OnDrawGizmos()
    {
        if(bMove)
        {
            Vector3 [] vPaths = nmp.corners; //拿出他存的路徑點
            int iCount = vPaths.Length;
            int i;
            if (iCount > 0)
            {
              
                for (i = 0; i < iCount - 1; i++)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(vPaths[i], 0.2f);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(vPaths[i], vPaths[i + 1]);
                }
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(vPaths[i], 0.2f);
            }
        }
    }

    void UpdateByInternal()
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
