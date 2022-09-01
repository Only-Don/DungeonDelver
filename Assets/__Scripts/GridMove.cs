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
        if (!mover.moving) return;//如果不移动，不做任何动作
        int facing = mover.GetFacing();

        //如果在一个方向移动，分配到网格
        //首先获取网格位置
        Vector2 rPos = mover.roomPos;
        Vector2 rPosGrid = mover.GetRoomPosOnGrid();

        //移动到网格行
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
