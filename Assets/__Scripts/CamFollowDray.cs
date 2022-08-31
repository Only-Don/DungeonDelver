using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowDray : MonoBehaviour//让摄像机跟随Dray
{
    static public bool TRANSITIONING = false;

    [Header("Set in Inspector")]
    public InRoom drayInRm;//指定一个InRoom变量drayInRm
    public float transTime = 0.5f;//从一个房间转移到另一个房间的时间

    private Vector3 p0, p1;//p0为旧房间，p1位新房间

    private InRoom inRm;//指定一个InRoom变量inRm
    private float transStart;//开始移动的时间

    private void Awake()//对inRm实例化
    {
        inRm = GetComponent<InRoom>();
    }

    private void Update()//判断主角是否从一个房间移动到另一个房间
    {
        if(TRANSITIONING)
        {
            float u = (Time.time - transStart) / transTime;//当前时间减去开始移动的时间，设置移动速度
            if (u >= 1)
            {
                u = 1;
                TRANSITIONING = false;
            }
            transform.position = (1 - u) * p0 + u * p1;//从p0到p1线性插值，完全移动到p1后停止移动
        }
        else
        {
            if(drayInRm.roomNum != inRm.roomNum)//判断主角的房间与摄像机的房间是否一样，不一样的话便调用TransitionTo
            {
                TransitionTo(drayInRm.roomNum);
            }
        }
    }

    void TransitionTo(Vector2 rm)//记录移动的起始位置和结束位置以及开始移动的时间，并将移动设置为真
    {
        p0 = transform.position;
        inRm.roomNum = rm;
        p1 = transform.position + (Vector3.back * 10);
        transform.position = p0;

        transStart = Time.time;
        TRANSITIONING = true;
    }
}
