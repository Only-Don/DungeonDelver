using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tile类用于从TileCamera类获得一个整型数，以获知显示哪个图块。
/// </summary>
public class Tile : MonoBehaviour
{
    [Header("Set Dynamically")]
    public int x;
    public int y;
    public int tileNum;

    private BoxCollider bColl;

    private void Awake()
    {
        bColl = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// 该方法用来读取地砖(Tile)的图块数量，
    /// 如果没有给<paramref name="eTileNum"/>传递任何参数，
    /// 则<paramref name="eTileNum"/>将从TileCamera.GET_MAP()中读取默认图块数量。
    /// <param name="eX"></param>
    /// <param name="eY"></param>
    /// <param name="eTileNum"></param>
    public void SetTile( int eX, int eY, int eTileNum = -1)
    {
        x = eX;
        y = eY;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3") + "x" + y.ToString("D3");    //规定输出格式，D表示输出为十进制数，3表示至少使用3个字符。

        if(eTileNum == -1)
        {
            eTileNum = TileCamera.GET_MAP(x, y);
        }
        else
        {
            TileCamera.SET_MAP(x, y, eTileNum);
        }
        tileNum = eTileNum;
        GetComponent<SpriteRenderer>().sprite = TileCamera.SPRITES[tileNum];    //一旦TileCamera.SPRITES存在，则将为当前Tile分配适当的精灵(Sprite)。

        SetCollider();
    }

    void SetCollider()
    {
        bColl.enabled = true;
        char c = TileCamera.COLLISIONS[tileNum];
        switch(c)
        {
            case 'S':
                bColl.center = Vector3.zero;
                bColl.size = Vector3.one;
                break;
            case 'W':
                bColl.center = new Vector3(0, 0.25f, 0);
                bColl.size = new Vector3(1, 0.5f, 1);
                break;
            case 'A':
                bColl.center = new Vector3(-0.25f, 0, 0);
                bColl.size = new Vector3(0.5f, 1, 1);
                break;
            case 'D':
                bColl.center = new Vector3(0.25f, 0, 0);
                bColl.size = new Vector3(0.5f, 1, 1);
                break;

            default:
                bColl.enabled = false;
                break;
        }
    }
    
}
