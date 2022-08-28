using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum eType  //ö�ٿ�ʰȡ����Ʒ����
    {
        key, health, grappler
    }
    /// <summary>
    /// 
    /// </summary>
    public static float COLLIDER_DELAY = 0.5f;

    [Header("Set in Inspector")]
    public eType itemType;

    //Awake��Active����ʹPickUp�Ķ�ײ��ʧЧ0.5��
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
