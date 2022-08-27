using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private GameObject sword;
    private Dray dray;
    // Start is called before the first frame update
    void Start()
    {
        sword = transform.Find("Sword").gameObject;
        dray = transform.parent.GetComponent<Dray>();//将剑的位置关联到Dray的位置
        sword.SetActive(false);//停止使用剑
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);//使剑指向Dray所面对的方向，使剑围绕z轴旋转90*facing度。
        sword.SetActive(dray.mode == Dray.eMode.attack);
    }
}
