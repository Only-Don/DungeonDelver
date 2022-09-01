using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    private IFacingMover mover;

    private void Awake()
    {
        mover = GetComponent<IFacingMover>();
    }

    private void FixedUpdate()
    {
        if (!mover.moving) return;//������ƶ��������κζ���
        int facing = mover.GetFacing();

        //�����һ�������ƶ������䵽����
        //���Ȼ�ȡ����λ��
        Vector2 rPos = mover.roomPos;
        Vector2 rPosGrid = mover.GetRoomPosOnGrid();

        //�ƶ���������
        float delta = 0;
        if(facing == 0 || facing == 2)
        {
            delta = rPosGrid.y - rPos.y;
        }
        else
        {
            delta = rPosGrid.x - rPos.x;
        }
        if (delta == 0) return;

        float move = mover.GetSpeed() * Time.fixedDeltaTime;
        move = Mathf.Min(move, Mathf.Abs(delta));
        if (delta < 0)
            move = -move;

        if (facing == 0 || facing == 2)
            rPos.y += move;
        else
            rPos.x += move;

        mover.roomPos = rPos;
    }
}
