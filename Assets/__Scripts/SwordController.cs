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
<<<<<<< HEAD
        sword = transform.Find("Sword").gameObject;
        dray = transform.parent.GetComponent<Dray>();//将剑的位置关联到Dray的位置
        sword.SetActive(false);//停止使用剑
=======
        sword = transform.Find("Sword").gameObject;    //引用了Sword子游戏对象
        dray = transform.parent.GetComponent<Dray>();  //将Dray类附加到父游戏对象的实例，即将剑的位置关联到Dray的位置
        sword.SetActive(false);                        //停止使用剑，从渲染、碰撞、运行等各方面删除游戏对象
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);//使剑指向Dray所面对的方向，使剑围绕z轴旋转90*facing度。
        sword.SetActive(dray.mode == Dray.eMode.attack);
=======
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);  //使剑指向Dray所面对的方向，使剑围绕z轴旋转90*facing度。
        sword.SetActive(dray.mode == Dray.eMode.attack);                //Update时如果Dray处于攻击模式，剑将被启用。
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    }
}
