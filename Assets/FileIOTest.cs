using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileIOTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StreamWriter sw = new StreamWriter("aaa.txt");
        //sw.WriteLine("AAA");
        //sw.Write("BBB\n");
        //sw.Write("CCC");
        //sw.Close();

        // StreamReader sr = new StreamReader("aaa.txt");
        // string sAll = sr.ReadToEnd();

        // string [] sLines = sAll.Split('\n');
        // foreach(string s in sLines)
        // {
        //     string s2 = s.Trim();
        //     Debug.Log(s2);
        // }
        // sr.Close();

        // File.WriteAllText("bbb.txt", "fhjefjoewfj9f23jfmf");
        // string sAll = File.ReadAllText("bbb.txt");
        // Debug.Log(sAll);

        //int iLevel = 1;
        //float fHp = 100.0f;
        //float fMp = 50.0f;
        //int iLV = 5;
        //FileStream fs = new FileStream("ccc.data", FileMode.Create);
        //BinaryWriter bw = new BinaryWriter(fs);
        //bw.Write(iLevel);
        //bw.Write(fHp);
        //bw.Write(fMp);
        //bw.Write(iLV);
        //bw.Close();
        //fs.Close();

        //FileStream fs2 = new FileStream("ccc.data", FileMode.Open);
        //BinaryReader br = new BinaryReader(fs2);
        //int c = br.ReadInt32();
        //float rHp = br.ReadSingle();
        //float rMp = br.ReadSingle();
        //float ii = br.ReadInt32();
        //Debug.Log(c + ":" + rHp + ":" + rMp  + ":" + ii);
        //br.Close();
        //fs.Close();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
