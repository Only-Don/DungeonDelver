using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeletos : Enemy, IFacingMover
{
    [Header("Set in Inspector: Skeletos")]
    public int speed = 2;//骷髅的速度
    public float timeThinkMin = 1f;
    public float timeThinkMax = 4f;//随机变向的时间范围
    [Header("Set Dynamically: Skeletos")]
    public int facing = 0;
    public float timeNextDecision = 0;

    private InRoom inRm;

    protected override void Awake()//从开始就创建一个骷髅敌人
    {
        base.Awake();
        inRm = GetComponent<InRoom>();//创建一个InRoom
    }

    override protected void Update()
    {
        base.Update();
        if (knockback) return;//如果被击退，不运行该函数

        if(Time.time >= timeNextDecision)
        {
            DecideDirection();
        }
        rigid.velocity = directions[facing] * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        int lastFacing = facing;
        while(facing == lastFacing)
            DecideDirection();
    }

    void DecideDirection()//骷髅随机方向
    {
        facing = Random.Range(0, 4);//0123中随机的方向
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
    }

    public int GetFacing()//接口，函数
    {
        return facing;
    }

    public bool moving
    {
        get { return true; }//是否在移动
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float gridMult
    {
        get { return inRm.gridMult; }
    }

    public Vector2 roomPos//在房间的位置
    {
        get { return inRm.roomPos; }
        set { inRm.roomPos = value; }
    }

    public Vector2 roomNum//在几号房间
    {
        get { return inRm.roomNum; }
        set { inRm.roomNum = value; }
    }

    public Vector2 GetRoomPosOnGrid(float mult=-1)//与网格对齐
    {
        return inRm.GetRoomPosOnGrid(mult);
    }
}
