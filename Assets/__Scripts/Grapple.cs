using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public enum eMode
    {
        none, gOut, gInMiss, gInHit
    }

    [Header("Set in Inspector")]
    public float grappleSpd = 10;
    public float grappleLength = 7;
    public float grappleInLength = 0.5f;
    public int unsafeTileHealthPenalty = 2;
    public TextAsset mapGrappleable;

    [Header("Set Dynamically")]
    public eMode mode = eMode.none;

    public List<int> grappleTiles;//可能碰撞的游戏对象列表
    public List<int> unsafeTiles;//对Dray不安全的游戏对象列表

    private Dray dray;
    private Rigidbody rigid;
    private Animator anim;
    private Collider drayColld;

    private GameObject grapHead;
    private LineRenderer grapLine;
    private Vector3 p0, p1;
    private int facing;

    private Vector3[] directions = new Vector3[]
    {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down
    };

    private void Awake()
    {
        //获取grappleTiles与unsafeTiles列表
        string gTiles = mapGrappleable.text;
        gTiles = Utils.RemoveLineEndings(gTiles);
        grappleTiles = new List<int>();
        unsafeTiles = new List<int>();
        for (int i=0; i<gTiles.Length; i++)
        {
            switch(gTiles[i])
            {
                case 'S':
                    grappleTiles.Add(i);
                    break;
                case 'X':
                    unsafeTiles.Add(i);
                    break;
            }
        }

        //获取各个将使用的游戏对象
        dray = GetComponent<Dray>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        drayColld = GetComponent<Collider>();

        Transform trans = transform.Find("Grappler");
        grapHead = trans.gameObject;
        grapLine = grapHead.GetComponent<LineRenderer>();
        grapHead.SetActive(false);
    }

    private void Update()
    {
        //无Grappler时无动作
        if (!dray.hasGrappler)
            return;

        switch(mode)
        {
            case eMode.none:
                if(Input.GetKeyDown(KeyCode.K))
                {
                    StartGrapple();
                }
                break;
        }
    }

    /// <summary>
    /// 设置Grappler初始位置并发射
    /// </summary>
    void StartGrapple()
    {
        facing = dray.GetFacing();

        // 阻止与Dray，Dray碰撞的交互
        dray.enabled = false;
        anim.CrossFade("Dray_Attack_" + facing, 0);
        drayColld.enabled = false;
        rigid.velocity = Vector3.zero;

        // 设定Grappler初始位置
        grapHead.SetActive(true);

        p0 = transform.position + (directions[facing] * 0.5f);
        p1 = p0;
        grapHead.transform.position = p1;
        grapHead.transform.rotation = Quaternion.Euler(0, 0, 90 * facing);

        grapLine.positionCount = 2;
        grapLine.SetPosition(0, p0);
        grapLine.SetPosition(1, p1);
        mode = eMode.gOut;// 更新Grappler状态为发出
    }

    private void FixedUpdate()
    {
        switch(mode)
        {
            // 发射后
            case eMode.gOut:
                // 使Grappler运动
                p1 += directions[facing] * grappleSpd * Time.fixedDeltaTime;
                grapHead.transform.position = p1;
                grapLine.SetPosition(1, p1);

                // 确认Grappler是否撞击或消失
                int tileNum = TileCamera.GET_MAP(p1.x, p1.y);
                if(grappleTiles.IndexOf(tileNum) != -1)
                {
                    // 撞击可抓取tile
                    mode = eMode.gInHit;
                    break;
                }
                if((p1-p0).magnitude >= grappleLength)
                {
                    // 到末尾而未碰到物体则消失
                    mode = eMode.gInMiss;
                }
                break;

            // 消失后
            case eMode.gInMiss:
                p1 -= directions[facing] * 2 * grappleSpd * Time.fixedDeltaTime;
                if(Vector3.Dot((p1-p0), directions[facing]) > 0)
                {
                    //若仍在Dray前方，则双倍速度返回
                    grapHead.transform.position = p1;
                    grapLine.SetPosition(1, p1);
                }
                else
                {
                    //到Dray处后消失
                    StopGrapple();
                }
                break;

            // 碰到物体后
            case eMode.gInHit:
                float dist = grappleInLength + grappleSpd * Time.fixedDeltaTime;// Dray与Grappler的最小距离
                if(dist>(p1-p0).magnitude)
                {
                    // 若Dray与Grappler间小于最小距离，则删除Grappler且不再移动
                    p0 = p1 - (directions[facing] * grappleInLength);
                    transform.position = p0;
                    StopGrapple();
                    break;
                }
                // 否则将Dray拖到Grappler旁
                p0 += directions[facing] * grappleSpd * Time.fixedDeltaTime;
                transform.position = p0;
                grapLine.SetPosition(0, p0);
                grapHead.transform.position = p1;
                break;
        }
    }
    /// <summary>
    /// 在Grappler消失后的后续处理
    /// </summary>
    void StopGrapple()
    {
        // 恢复Dray交互
        dray.enabled = true;
        drayColld.enabled = true;

        // 检查Dray位置，若在不安全位置则扣血并回到进入的门口
        int tileNum = TileCamera.GET_MAP(p0.x, p0.y);
        if(mode == eMode.gInHit && unsafeTiles.IndexOf(tileNum) != -1)
        {
            dray.ResetInRoom(unsafeTileHealthPenalty);
        }

        grapHead.SetActive(false);

        mode = eMode.none;
    }
    /// <summary>
    /// 进行Grappler碰撞Enemy时的交互
    /// </summary>
    /// <param name="colld">被碰撞物体，这里取Enemy</param>
    private void OnTriggerEnter(Collider colld)
    {
        Enemy e = colld.GetComponent<Enemy>();
        if (e == null)
            return;

        mode = eMode.gInMiss;
    }
}
