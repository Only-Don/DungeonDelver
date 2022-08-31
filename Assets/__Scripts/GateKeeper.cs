using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : MonoBehaviour
{
    const int lockedR = 95;//每扇可能上锁或者打开门的图块编号
    const int lockedUR = 81;
    const int lockedUL = 80;
    const int lockedL = 100;
    const int lockedDL = 101;
    const int lockedDR = 102;//锁门的tileNums

    const int openR = 48;
    const int openUR = 93;
    const int openUL = 92;
    const int openL = 51;
    const int openDL = 26;
    const int openDR = 27;//开门的tileNums

    private IKeyMaster keys;

    private void Awake()
    {
        keys = GetComponent<IKeyMaster>();
    }

    private void OnCollisionStay(Collision collision)//如果Dray没有钥匙，或者碰撞的不是图块，或者Dray没有面向上锁的门，则该方法返回
    {
        if (keys.keyCount < 1)//没有钥匙，不需要运行
            return;

        Tile ti = collision.gameObject.GetComponent<Tile>();//如果碰撞的不是图块，不需要运行
        if (ti == null) return;

        int facing = keys.GetFacing();//当Dray朝向门时才打开

        Tile ti2;
        switch(ti.tileNum)//判断是否为门图块
        {
            case lockedR:
                if (facing != 0)
                    return;//如果不朝向门，函数返回
                ti.SetTile(ti.x, ti.y, openR);
                break;

            case lockedUR:
                if (facing != 1)
                    return;
                ti.SetTile(ti.x, ti.y, openUR);
                ti2 = TileCamera.TILES[ti.x - 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openUL);
                break;

            case lockedUL:
                if (facing != 1)
                    return;
                ti.SetTile(ti.x, ti.y, openUL);
                ti2 = TileCamera.TILES[ti.x + 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openUR);
                break;

            case lockedL:
                if (facing != 2)
                    return;
                ti.SetTile(ti.x, ti.y, openL);
                break;

            case lockedDL:
                if (facing != 3)
                    return;
                ti.SetTile(ti.x, ti.y, openDL);
                ti2 = TileCamera.TILES[ti.x + 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openDR);
                break;

            case lockedDR:
                if (facing != 3)
                    return;
                ti.SetTile(ti.x, ti.y, openDR);
                ti2 = TileCamera.TILES[ti.x - 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openDL);
                break;

            default:
                return;//返回，防止钥匙数量减少
        }

        keys.keyCount--;
    }
}
