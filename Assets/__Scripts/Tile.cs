using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tile�����ڴ�TileCamera����һ�����������Ի�֪��ʾ�ĸ�ͼ�顣
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
    /// �÷���������ȡ��ש(Tile)��ͼ��������
    /// ���û�и�<paramref name="eTileNum"/>�����κβ�����
    /// ��<paramref name="eTileNum"/>����TileCamera.GET_MAP()�ж�ȡĬ��ͼ��������
    /// <param name="eX"></param>
    /// <param name="eY"></param>
    /// <param name="eTileNum"></param>
    public void SetTile( int eX, int eY, int eTileNum = -1)
    {
        x = eX;
        y = eY;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3") + "x" + y.ToString("D3");    //�涨�����ʽ��D��ʾ���Ϊʮ��������3��ʾ����ʹ��3���ַ���

        if(eTileNum == -1)
        {
            eTileNum = TileCamera.GET_MAP(x, y);
        }
        else
        {
            TileCamera.SET_MAP(x, y, eTileNum);
        }
        tileNum = eTileNum;
        GetComponent<SpriteRenderer>().sprite = TileCamera.SPRITES[tileNum];    //һ��TileCamera.SPRITES���ڣ���Ϊ��ǰTile�����ʵ��ľ���(Sprite)��

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
