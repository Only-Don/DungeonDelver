using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeletos : Enemy, IFacingMover
{
    [Header("Set in Inspector: Skeletos")]
    public int speed = 2;//���õ��ٶ�
    public float timeThinkMin = 1f;
    public float timeThinkMax = 4f;//��������ʱ�䷶Χ
    [Header("Set Dynamically: Skeletos")]
    public int facing = 0;
    public float timeNextDecision = 0;

    private InRoom inRm;

    protected override void Awake()//�ӿ�ʼ�ʹ���һ�����õ���
    {
        base.Awake();
        inRm = GetComponent<InRoom>();//����һ��InRoom
    }

    override protected void Update()
    {
        base.Update();
        if (knockback) return;//��������ˣ������иú���

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

    void DecideDirection()//�����������
    {
        facing = Random.Range(0, 4);//0123������ķ���
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
    }

    public int GetFacing()//�ӿڣ�����
    {
        return facing;
    }

    public bool moving
    {
        get { return true; }//�Ƿ����ƶ�
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float gridMult
    {
        get { return inRm.gridMult; }
    }

    public Vector2 roomPos//�ڷ����λ��
    {
        get { return inRm.roomPos; }
        set { inRm.roomPos = value; }
    }

    public Vector2 roomNum//�ڼ��ŷ���
    {
        get { return inRm.roomNum; }
        set { inRm.roomNum = value; }
    }

    public Vector2 GetRoomPosOnGrid(float mult=-1)//���������
    {
        return inRm.GetRoomPosOnGrid(mult);
    }
}
