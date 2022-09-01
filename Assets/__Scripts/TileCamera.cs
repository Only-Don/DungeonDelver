using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileSwap
{
    public int          tileNum;    //要替换的特殊图块的编号
    public GameObject   swapPrefab; //交换的预设
    public GameObject   guaranteedItemDrop;
    public int          overrideTileNum = -1;//替换的默认值
}
/// <summary>
/// TileCamera类负责解析和存储DelverTiles.png图像中所有的Sprite并读取DelverData.txt，以确定这些图块的位置。
/// </summary>
public class TileCamera : MonoBehaviour
{
    static private int          W, H; //地图的总长和总宽
    static private int[,]       MAP;//地图数组
    static public Sprite[]      SPRITES;//精灵数组
    static public Transform     TILE_ANCHOR;
    static public Tile[,]       TILES;//地砖数组
    static public string        COLLISIONS;//记录碰撞数据的字符串

    [Header("Set in Inspector")]    //作为检查器属性输入
    public TextAsset            mapData;        //地图原始数据
    public Texture2D            mapTiles;       //地图原始图块
    public TextAsset            mapCollisions;  //地图碰撞预设
    public Tile                 tilePrefab;     //地图预设贴图
    public int                  defaultTileNum; //需要交换的地图块编号
    public List<TileSwap>       tileSwaps;      //可序列化的列表

    private Dictionary<int, TileSwap> tileSwapDict;//方便搜索的字典
    private Transform enemyAnchor, itemAnchor;

    private void Awake()
    {
        COLLISIONS = Utils.RemoveLineEndings(mapCollisions.text); //利用Util方法处理并记录数据
        PrepareTileSwapDict();//迭代列表中的条目，并添加到字典
        LoadMap();
    }

    public void LoadMap()
    {
        //生成TILE_ANCHOR作为所有Tiles的父元素。同时TILE＿ANCHOR也有锚点的作用。
        GameObject go = new GameObject("TILE_ANCHOR");
        TILE_ANCHOR = go.transform;
        //从mapTiles加载所有Sprite。
        SPRITES = Resources.LoadAll<Sprite>(mapTiles.name);

        //读取地图数据
        string[] lines = mapData.text.Split('\n');  //地图数据将被\n来分割为多行
        H = lines.Length;
        string[] tileNums = lines[0].Split(' ');    //地图数据第一行将被空格分割为一个个的地图块，同时完成对tileNum数组的构建
        W = tileNums.Length;

        System.Globalization.NumberStyles hexNum;   //确定数字字符串参数中允许的样式，方便之后的int.Parse转换
        //由于该常量需要用众多字符拼写，故将其放在hexNum常量中。
        hexNum = System.Globalization.NumberStyles.HexNumber;
        MAP = new int[W, H];
        for(int j=0; j<H; j++)
        {
            tileNums = lines[j].Split(' ');
            for(int i=0; i<W; i++)
            {
                if(tileNums[i] == ".." || tileNums[i] == "..、\r")
                {
                    MAP[i, j] = 0;
                }
                else
                {
                    MAP[i, j] = int.Parse(tileNums[i], hexNum);
                }
                CheckTileSwaps(i, j);
            }
        }
        print("Parsed" + SPRITES.Length + " sprites.");
        print("Map size: " + W + " wide by " + H + " high");

        ShowMap();
    }

    /// <summary>
    /// 一次生成整个地图的所有Tiles
    /// </summary>
    void ShowMap()
    {
        TILES = new Tile[W, H];

        for(int j=0; j<H; j++)
        {
            for(int i=0; i<W; i++)
            {
                if(MAP[i,j] != 0)
                {
                    Tile ti = Instantiate<Tile>(tilePrefab); //将tilePrefab作为Tile实例化（类似于克隆物体），并将其传递给局部变量ti。
                    ti.transform.SetParent(TILE_ANCHOR);//将TILE_ANCHOR作为ti的父对象。
                    ti.SetTile(i, j);//进行贴图
                    TILES[i, j] = ti;
                }
            }
        }
    }
    void PrepareTileSwapDict()
    {
        tileSwapDict = new Dictionary<int, TileSwap>();
        foreach(TileSwap ts in tileSwaps)
        {
            tileSwapDict.Add(ts.tileNum, ts);
        }
    }
    void CheckTileSwaps(int i, int j)
    {
        int tNum = GET_MAP(i, j);
        if (!tileSwapDict.ContainsKey(tNum))
            return;
        TileSwap ts = tileSwapDict[tNum];
        if(ts.swapPrefab != null)
        {
            GameObject go = Instantiate(ts.swapPrefab);
            Enemy e = go.GetComponent<Enemy>();
            if(e != null)
            {
                go.transform.SetParent(enemyAnchor);
            }
            else
            {
                go.transform.SetParent(itemAnchor);
            }
            go.transform.position = new Vector3(i, j, 0);
            if(ts.guaranteedItemDrop != null)
            {
                if(e != null)
                {
                    e.guaranteedItemDrop = ts.guaranteedItemDrop;
                }
            }
        }
        if(ts.overrideTileNum == -1)
        {
            SET_MAP(i, j, defaultTileNum);
        }
        else
        {
            SET_MAP(i, j, ts.overrideTileNum);
        }
    }
    static public int GET_MAP(int x, int y)
    {
        if(x<0 || x>=W || y<0 || y>=H)
        {
            return -1;
        }
        return MAP[x, y];
    }
    static public int GET_MAP(float x, float y)
    {
        int tX = Mathf.RoundToInt(x);
        int tY = Mathf.RoundToInt(y - 0.25f);   //此时可在图块外显示主角上半身，并且在该图块上主角仍然处于控制状态。
        return GET_MAP(tX, tY);
    }
    static public void SET_MAP(int x,int y,int tNum)
    {
        if(x<0 || x>=W || y<0 || y>=H)
        {
            return;
        }
        MAP[x, y] = tNum;
    }
}
