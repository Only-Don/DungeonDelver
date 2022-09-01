using UnityEngine;
using System.Collections;

public class Spiker : Enemy, IFacingMover
{

    public enum eMode { search, attack, retract };

    [Header("Set in Inspector")]
    public float sensorRange = 0.75f;
    public float attackSpeed = 6;
    public float retractSpeed = 3;
    public float radius = 0.4f;
    public float timeWait = 3f;
    public float timeWaitDone = 0;
    public Vector3 lastPos = Vector3.zero;

    [Header("Set Dynamically")]
    public int facing = 0;

    public eMode mode = eMode.search;
    private InRoom inRm;
    private Dray dray;
    private SphereCollider drayColld;
    private Vector3 p0, p1;
    private DamageEffect dEf;

    void Start()
    {
        inRm = GetComponent<InRoom>();

        GameObject go = GameObject.Find("Dray_Walk_0a");
        dray = go.GetComponent<Dray>();
        drayColld = go.GetComponent<SphereCollider>();
        dEf = GetComponent<DamageEffect>();
    }

    override protected void Update()
    {
        base.Update();
        switch (mode)
        {
            case eMode.search:
                // 查找Dray是否在该房间
                if (dray.roomNum != inRm.roomNum) return;

                float moveAmt;
                if (Mathf.Abs(dray.roomPos.x - inRm.roomPos.x) < sensorRange)
                {
                    // 竖直方向攻击
                    moveAmt = dray.roomPos.y - inRm.roomPos.y;
                    if (moveAmt >= 0)
                        facing = 1;
                    else
                        facing = 3;
                    p1 = p0 = transform.position;
                    p1.y += moveAmt;
                    mode = eMode.attack;
                }

                if (Mathf.Abs(dray.roomPos.y - inRm.roomPos.y) < sensorRange)
                {
                    // 水平方向攻击
                    moveAmt = dray.roomPos.x - inRm.roomPos.x;
                    if (moveAmt >= 0)
                        facing = 0;
                    else
                        facing = 2;
                    p1 = p0 = transform.position;
                    p1.x += moveAmt;
                    mode = eMode.attack;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        Vector3 dir, pos, delta;

        switch (mode)
        {
            case eMode.attack:
                if(knockback)
                {
                    mode = eMode.retract;
                    break;
                }
                dir = (p1 - p0).normalized;
                pos = transform.position;
                delta = dir * attackSpeed * Time.fixedDeltaTime;
                if (delta.magnitude > (p1 - pos).magnitude)
                {
                    transform.position = p1;
                    mode = eMode.retract;
                    break;
                }
                transform.position = pos + delta;

                if ((dray.transform.position - transform.position).magnitude < radius + drayColld.radius)
                {
                    dray.TakeDamage(dEf, transform.position);
                }

                if (lastPos != transform.position)
                {
                    lastPos = transform.position;
                    timeWaitDone = Time.time + timeWait;
                    break;
                }
                if (Time.time - timeWaitDone > 0 && lastPos == transform.position)
                    mode = eMode.search;
                break;

            case eMode.retract:
                dir = (p1 - p0).normalized;
                pos = transform.position;
                delta = dir * retractSpeed * Time.fixedDeltaTime;
                if (delta.magnitude > (p0 - pos).magnitude)
                {
                    transform.position = p0;
                    mode = eMode.search;
                    break;
                }
                transform.position = pos - delta;
                if (lastPos != transform.position)
                {
                    lastPos = transform.position;
                    timeWaitDone = Time.time + timeWait;
                    break;
                }
                if (Time.time - timeWaitDone > 0 && lastPos == transform.position)
                    mode = eMode.search;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Tile ti = collision.gameObject.GetComponent<Tile>();
        Collider col = collision.gameObject.GetComponent<Collider>();
        if (ti != null && col != null)
        {
            if(ti.tileNum == 96)
                col.enabled = false;
        }
    }

    public int GetFacing()
    {
        return facing;
    }

    public bool moving
    {
        get { return true; }
    }

    public float GetSpeed()
    {
        return attackSpeed;
    }

    public float gridMult
    {
        get { return inRm.gridMult; }
    }

    public Vector2 roomPos
    {
        get { return inRm.roomPos; }
        set { inRm.roomPos = value; }
    }

    public Vector2 roomNum
    {
        get { return inRm.roomNum; }
        set { inRm.roomNum = value; }
    }

    public Vector2 GetRoomPosOnGrid(float mult = -1)
    {
        return inRm.GetRoomPosOnGrid(mult);
    }
}
