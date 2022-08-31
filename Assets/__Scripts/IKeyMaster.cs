using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKeyMaster
{
    int keyCount { get; set; }//获取和设置钥匙数量
    int GetFacing();//在Dray类中已经实现GetFacing()
}
