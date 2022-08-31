using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowDray : MonoBehaviour//�����������Dray
{
    static public bool TRANSITIONING = false;

    [Header("Set in Inspector")]
    public InRoom drayInRm;//ָ��һ��InRoom����drayInRm
    public float transTime = 0.5f;//��һ������ת�Ƶ���һ�������ʱ��

    private Vector3 p0, p1;//p0Ϊ�ɷ��䣬p1λ�·���

    private InRoom inRm;//ָ��һ��InRoom����inRm
    private float transStart;//��ʼ�ƶ���ʱ��

    private void Awake()//��inRmʵ����
    {
        inRm = GetComponent<InRoom>();
    }

    private void Update()//�ж������Ƿ��һ�������ƶ�����һ������
    {
        if(TRANSITIONING)
        {
            float u = (Time.time - transStart) / transTime;//��ǰʱ���ȥ��ʼ�ƶ���ʱ�䣬�����ƶ��ٶ�
            if (u >= 1)
            {
                u = 1;
                TRANSITIONING = false;
            }
            transform.position = (1 - u) * p0 + u * p1;//��p0��p1���Բ�ֵ����ȫ�ƶ���p1��ֹͣ�ƶ�
        }
        else
        {
            if(drayInRm.roomNum != inRm.roomNum)//�ж����ǵķ�����������ķ����Ƿ�һ������һ���Ļ������TransitionTo
            {
                TransitionTo(drayInRm.roomNum);
            }
        }
    }

    void TransitionTo(Vector2 rm)//��¼�ƶ�����ʼλ�úͽ���λ���Լ���ʼ�ƶ���ʱ�䣬�����ƶ�����Ϊ��
    {
        p0 = transform.position;
        inRm.roomNum = rm;
        p1 = transform.position + (Vector3.back * 10);
        transform.position = p0;

        transStart = Time.time;
        TRANSITIONING = true;
    }
}
