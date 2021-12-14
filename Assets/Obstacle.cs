using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum eState
    {
        NONE = -1,
        OUTSIDE_TEST,
        INSIDE_TEST,
        COL_TEST
    }
    public float m_fRadius;
    [HideInInspector]
    public eState m_eState = eState.NONE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
