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

    public List<int> grappleTiles;//������ײ����Ϸ�����б�
    public List<int> unsafeTiles;//��Dray����ȫ����Ϸ�����б�

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
        //��ȡgrappleTiles��unsafeTiles�б�
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

        //��ȡ������ʹ�õ���Ϸ����
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
        //��Grapplerʱ�޶���
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
    /// ����Grappler��ʼλ�ò�����
    /// </summary>
    void StartGrapple()
    {
        facing = dray.GetFacing();

        // ��ֹ��Dray��Dray��ײ�Ľ���
        dray.enabled = false;
        anim.CrossFade("Dray_Attack_" + facing, 0);
        drayColld.enabled = false;
        rigid.velocity = Vector3.zero;

        // �趨Grappler��ʼλ��
        grapHead.SetActive(true);

        p0 = transform.position + (directions[facing] * 0.5f);
        p1 = p0;
        grapHead.transform.position = p1;
        grapHead.transform.rotation = Quaternion.Euler(0, 0, 90 * facing);

        grapLine.positionCount = 2;
        grapLine.SetPosition(0, p0);
        grapLine.SetPosition(1, p1);
        mode = eMode.gOut;// ����Grappler״̬Ϊ����
    }

    private void FixedUpdate()
    {
        switch(mode)
        {
            // �����
            case eMode.gOut:
                // ʹGrappler�˶�
                p1 += directions[facing] * grappleSpd * Time.fixedDeltaTime;
                grapHead.transform.position = p1;
                grapLine.SetPosition(1, p1);

                // ȷ��Grappler�Ƿ�ײ������ʧ
                int tileNum = TileCamera.GET_MAP(p1.x, p1.y);
                if(grappleTiles.IndexOf(tileNum) != -1)
                {
                    // ײ����ץȡtile
                    mode = eMode.gInHit;
                    break;
                }
                if((p1-p0).magnitude >= grappleLength)
                {
                    // ��ĩβ��δ������������ʧ
                    mode = eMode.gInMiss;
                }
                break;

            // ��ʧ��
            case eMode.gInMiss:
                p1 -= directions[facing] * 2 * grappleSpd * Time.fixedDeltaTime;
                if(Vector3.Dot((p1-p0), directions[facing]) > 0)
                {
                    //������Drayǰ������˫���ٶȷ���
                    grapHead.transform.position = p1;
                    grapLine.SetPosition(1, p1);
                }
                else
                {
                    //��Dray������ʧ
                    StopGrapple();
                }
                break;

            // ���������
            case eMode.gInHit:
                float dist = grappleInLength + grappleSpd * Time.fixedDeltaTime;// Dray��Grappler����С����
                if(dist>(p1-p0).magnitude)
                {
                    // ��Dray��Grappler��С����С���룬��ɾ��Grappler�Ҳ����ƶ�
                    p0 = p1 - (directions[facing] * grappleInLength);
                    transform.position = p0;
                    StopGrapple();
                    break;
                }
                // ����Dray�ϵ�Grappler��
                p0 += directions[facing] * grappleSpd * Time.fixedDeltaTime;
                transform.position = p0;
                grapLine.SetPosition(0, p0);
                grapHead.transform.position = p1;
                break;
        }
    }
    /// <summary>
    /// ��Grappler��ʧ��ĺ�������
    /// </summary>
    void StopGrapple()
    {
        // �ָ�Dray����
        dray.enabled = true;
        drayColld.enabled = true;

        // ���Drayλ�ã����ڲ���ȫλ�����Ѫ���ص�������ſ�
        int tileNum = TileCamera.GET_MAP(p0.x, p0.y);
        if(mode == eMode.gInHit && unsafeTiles.IndexOf(tileNum) != -1)
        {
            dray.ResetInRoom(unsafeTileHealthPenalty);
        }

        grapHead.SetActive(false);

        mode = eMode.none;
    }
    /// <summary>
    /// ����Grappler��ײEnemyʱ�Ľ���
    /// </summary>
    /// <param name="colld">����ײ���壬����ȡEnemy</param>
    private void OnTriggerEnter(Collider colld)
    {
        Enemy e = colld.GetComponent<Enemy>();
        if (e == null)
            return;

        mode = eMode.gInMiss;
    }
}
