using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFacingMover//接口的公开声明
{
    int GetFacing();//任何实现IFacingMover的类都有一个公有int类型的GetFacing()方法
    bool moving { get; }//保证属性的实现
    float GetSpeed();
    float gridMult { get; }//允许将InRoom中的gridMult变量传递给其他脚本不需要直接访问InRoom
    Vector2 roomPos { get; set; }//允许任何类将Dray和Skeletos作为IFacingMover调用，不需要访问InRoom
    Vector2 roomNum { get; set; }
    Vector2 GetRoomPosOnGrid (float mult = -1);//如果没有值或者将-1传过去，将查找gridMult属性
}
