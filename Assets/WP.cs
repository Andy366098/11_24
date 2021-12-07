using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> neibors;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        if(neibors != null && neibors.Count > 0)
        {
            Gizmos.color = Color.green;
            foreach (GameObject g in neibors)
            {
                Gizmos.DrawLine(transform.position, g.transform.position);
            }
        }
    }
}
