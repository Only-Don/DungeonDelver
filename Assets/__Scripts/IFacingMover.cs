using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFacingMover//�ӿڵĹ�������
{
    int GetFacing();//�κ�ʵ��IFacingMover���඼��һ������int���͵�GetFacing()����
    bool moving { get; }//��֤���Ե�ʵ��
    float GetSpeed();
    float gridMult { get; }//����InRoom�е�gridMult�������ݸ������ű�����Ҫֱ�ӷ���InRoom
    Vector2 roomPos { get; set; }//�����κ��ཫDray��Skeletos��ΪIFacingMover���ã�����Ҫ����InRoom
    Vector2 roomNum { get; set; }
    Vector2 GetRoomPosOnGrid (float mult = -1);//���û��ֵ���߽�-1����ȥ��������gridMult����
}
