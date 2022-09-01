using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileSwap
{
    public int          tileNum;    //Ҫ�滻������ͼ��ı��
    public GameObject   swapPrefab; //������Ԥ��
    public GameObject   guaranteedItemDrop;
    public int          overrideTileNum = -1;//�滻��Ĭ��ֵ
}
/// <summary>
/// TileCamera�ฺ������ʹ洢DelverTiles.pngͼ�������е�Sprite����ȡDelverData.txt����ȷ����Щͼ���λ�á�
/// </summary>
public class TileCamera : MonoBehaviour
{
    static private int          W, H; //��ͼ���ܳ����ܿ�
    static private int[,]       MAP;//��ͼ����
    static public Sprite[]      SPRITES;//��������
    static public Transform     TILE_ANCHOR;
    static public Tile[,]       TILES;//��ש����
    static public string        COLLISIONS;//��¼��ײ���ݵ��ַ���

    [Header("Set in Inspector")]    //��Ϊ�������������
    public TextAsset            mapData;        //��ͼԭʼ����
    public Texture2D            mapTiles;       //��ͼԭʼͼ��
    public TextAsset            mapCollisions;  //��ͼ��ײԤ��
    public Tile                 tilePrefab;     //��ͼԤ����ͼ
    public int                  defaultTileNum; //��Ҫ�����ĵ�ͼ����
    public List<TileSwap>       tileSwaps;      //�����л����б�

    private Dictionary<int, TileSwap> tileSwapDict;//�����������ֵ�
    private Transform enemyAnchor, itemAnchor;

    private void Awake()
    {
        COLLISIONS = Utils.RemoveLineEndings(mapCollisions.text); //����Util����������¼����
        PrepareTileSwapDict();//�����б��е���Ŀ������ӵ��ֵ�
        LoadMap();
    }

    public void LoadMap()
    {
        //����TILE_ANCHOR��Ϊ����Tiles�ĸ�Ԫ�ء�ͬʱTILE��ANCHORҲ��ê������á�
        GameObject go = new GameObject("TILE_ANCHOR");
        TILE_ANCHOR = go.transform;
        //��mapTiles��������Sprite��
        SPRITES = Resources.LoadAll<Sprite>(mapTiles.name);

        //��ȡ��ͼ����
        string[] lines = mapData.text.Split('\n');  //��ͼ���ݽ���\n���ָ�Ϊ����
        H = lines.Length;
        string[] tileNums = lines[0].Split(' ');    //��ͼ���ݵ�һ�н����ո�ָ�Ϊһ�����ĵ�ͼ�飬ͬʱ��ɶ�tileNum����Ĺ���
        W = tileNums.Length;

        System.Globalization.NumberStyles hexNum;   //ȷ�������ַ����������������ʽ������֮���int.Parseת��
        //���ڸó�����Ҫ���ڶ��ַ�ƴд���ʽ������hexNum�����С�
        hexNum = System.Globalization.NumberStyles.HexNumber;
        MAP = new int[W, H];
        for(int j=0; j<H; j++)
        {
            tileNums = lines[j].Split(' ');
            for(int i=0; i<W; i++)
            {
                if(tileNums[i] == ".." || tileNums[i] == "..��\r")
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
    /// һ������������ͼ������Tiles
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
                    Tile ti = Instantiate<Tile>(tilePrefab); //��tilePrefab��ΪTileʵ�����������ڿ�¡���壩�������䴫�ݸ��ֲ�����ti��
                    ti.transform.SetParent(TILE_ANCHOR);//��TILE_ANCHOR��Ϊti�ĸ�����
                    ti.SetTile(i, j);//������ͼ
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
        int tY = Mathf.RoundToInt(y - 0.25f);   //��ʱ����ͼ������ʾ�����ϰ��������ڸ�ͼ����������Ȼ���ڿ���״̬��
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
