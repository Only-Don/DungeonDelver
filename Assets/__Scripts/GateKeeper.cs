using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : MonoBehaviour
{
    const int lockedR = 95;//ÿ�ȿ����������ߴ��ŵ�ͼ����
    const int lockedUR = 81;
    const int lockedUL = 80;
    const int lockedL = 100;
    const int lockedDL = 101;
    const int lockedDR = 102;//���ŵ�tileNums

    const int openR = 48;
    const int openUR = 93;
    const int openUL = 92;
    const int openL = 51;
    const int openDL = 26;
    const int openDR = 27;//���ŵ�tileNums

    private IKeyMaster keys;

    private void Awake()
    {
        keys = GetComponent<IKeyMaster>();
    }

    private void OnCollisionStay(Collision collision)//���Drayû��Կ�ף�������ײ�Ĳ���ͼ�飬����Drayû�������������ţ���÷�������
    {
        if (keys.keyCount < 1)//û��Կ�ף�����Ҫ����
            return;

        Tile ti = collision.gameObject.GetComponent<Tile>();//�����ײ�Ĳ���ͼ�飬����Ҫ����
        if (ti == null) return;

        int facing = keys.GetFacing();//��Dray������ʱ�Ŵ�

        Tile ti2;
        switch(ti.tileNum)//�ж��Ƿ�Ϊ��ͼ��
        {
            case lockedR:
                if (facing != 0)
                    return;//����������ţ���������
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
                return;//���أ���ֹԿ����������
        }

        keys.keyCount--;
    }
}
