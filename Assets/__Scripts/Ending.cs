//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Ending: MonoBehaviour
//{
//    public GameObject Ender;
//    public GameObject End1;
//    public Time CloseTime;
//    public float timeClose = 0;
//    public bool timer = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        Ender.SetActive(false);
//        End1.SetActive(false);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(timer)
//        {
//            if (Time.time >= timeClose)
//            {
//                timer = true;
//                Ender.SetActive(false);
//                End1.SetActive(false);
//            }
//        }
        
//    }

//    public void GoEnd()
//    {
//        timeClose = Time.time + 10;
//        Ender.SetActive(true);
//        End1.SetActive(true);
//        timer = true;
//    }
//}
