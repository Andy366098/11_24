using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveWP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");
        StreamWriter sw = new StreamWriter("Assets/aaa.txt");//�g��ƶiaaa.txt����r��
        for(int i = 0;i < gos.Length; i++)
        {
            WP wp = gos[i].GetComponent<WP>();  //�������W��WP��Script
            sw.Write($"{wp.name},{wp.neibors.Count},");
            for(int j = 0;j < wp.neibors.Count; j++)
            {
                sw.Write($"{wp.neibors[j].name},");
            }
            sw.WriteLine();
        }
        sw.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
