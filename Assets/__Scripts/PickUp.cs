using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
<<<<<<< HEAD
    public enum eType
    {
        key, health, grappler
    }

=======
    public enum eType  //枚举可拾取的物品类型
    {
        key, health, grappler
    }
    /// <summary>
    /// 
    /// </summary>
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    public static float COLLIDER_DELAY = 0.5f;

    [Header("Set in Inspector")]
    public eType itemType;

<<<<<<< HEAD
=======
    //Awake和Active方法使PickUp的对撞机失效0.5秒
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    private void Awake()
    {
        GetComponent<Collider>().enabled = false;
        Invoke("Activate", COLLIDER_DELAY);
    }

    void Activate()
    {
        GetComponent<Collider>().enabled = true;
    }
}
