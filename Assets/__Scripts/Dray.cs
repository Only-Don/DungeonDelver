using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour, IFacingMover, IKeyMaster
{
    public enum eMode //动画的枚举，可以用来跟踪和查询Dray的状态
    {
        idle, move, attack, transition, knockback
    }

    //public变量可以直接在unity编辑器中的Inspector里编辑
    [Header("Set in Inspector")]
    public float    speed = 5;  //移动速度
    public float attackDuration = 0.25f;    //攻击的持续秒数
    public float transitionDelay = 0.5f;   //房间转换间隔
    public float attackDelay = 0.5f;    //攻击动作的间隔
    public int   maxHealth = 10;
    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float invincibleDuration = 0.5f;

    [Header("Set Dynamically")]
    public int      dirHeld = -1;   //当前按键指示方向
    public int      facing = 1;     //Dray所面对的方向
    public eMode    mode = eMode.idle;//默认为静止
    public int      numKeys = 0;
    public bool     invincible = false;
    public bool     hasGrappler = false;
    public Vector3  lastSafeLoc;
    public int      lastSafeFacing;

    [SerializeField]
    private int _health;
    public int health  //创建主角的生命，可以在inspector里修改
    {
        get { return _health; }
        set { _health = value; }
    }

    private float   timeAtkDone = 0;     //攻击动画应该完成的时间
    private float   timeAtkNext = 0;     //Dray能够再次发起攻击的时间间隔
    private float   transitionDone = 0;
    private Vector2 transitionPos;
    private float   knockbackDone = 0;
    private float   invincibleDone = 0;
    private Vector3 knockbackVel;

    private SpriteRenderer sRend;
    private Rigidbody rigid;
    private Animator anim;
    private InRoom inRm;

    private Vector3[] directions = new Vector3[]  //产生一个新的向量数组，dirHeld的数值对应数组的不同元素，便于引用向量的不同方向
    {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down
    };

    private KeyCode[] keys = new KeyCode[] //产生一个新的按键数组，和向量数组同理，用于轻松的引用每个方向键
    {
        KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S
    };

    private void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();  //载入渲染器
        rigid = GetComponent<Rigidbody>();       //载入刚体
        anim = GetComponent<Animator>();         //载入动画
        inRm = GetComponent<InRoom>();
        health = maxHealth;                      //出生时最大血量
        lastSafeLoc = transform.position;
        lastSafeFacing = facing;
    }
    private void Update()
    {
        if (invincible && Time.time > invincibleDone)
            invincible = false;
        sRend.color = invincible ? Color.red : Color.white;
        if (mode == eMode.knockback)
        {
            rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone)
                return;
        }

        if(mode == eMode.transition)
        {
            rigid.velocity = Vector3.zero;
            anim.speed = 0;
            roomPos = transitionPos;
            if (Time.time < transitionDone)
                return;
            mode = eMode.idle;
        }

        dirHeld = -1;
        for(int i=0; i<4; i++)
        {
            if (Input.GetKey(keys[i]))  //使用的是前面的按键数组
                dirHeld = i;    //随着按键改变移动方向
        }
        
        if(Input.GetKeyDown(KeyCode.J) && Time.time >= timeAtkNext) //按住攻击键并且时间大于下一次可攻击时间时发动
        {
            mode = eMode.attack;                       //模式转为攻击模式
            timeAtkDone = Time.time + attackDuration;  //设置何时停止攻击动画
            timeAtkNext = Time.time + attackDelay;     //设置何时能再次播放攻击动画
        }

        if(Time.time >= timeAtkDone)    //当时间大于攻击完成时间时恢复静止
        {
            mode = eMode.idle;
        }


        if(mode != eMode.attack)//若模式不为攻击模式，则根据是否按住移动键在idle和move之间进行选择
        {
            if(dirHeld == -1)
            {
                mode = eMode.idle;//默认模式静止
            }
            else
            {
                facing = dirHeld;//确保面对方向与移动方向一致
                mode = eMode.move;
            }
        }

        //执行mode选择的模式，播放正确动画
        Vector3 vel = Vector3.zero;
        switch (mode) 
        {
            case eMode.attack:
                anim.CrossFade("Dray_Attack_" + facing, 0);
                anim.speed = 0; //进入攻击动画后停在原地，直到攻击结束，此时面对方向也是不变的
                break;
            case eMode.idle:
                anim.CrossFade("Dray_Walk_" + facing, 0);
                anim.speed = 0; //待机时动画静止，并且面朝方向不变
                break;
            case eMode.move:
                vel = directions[dirHeld];
                anim.CrossFade("Dray_Walk_" + facing, 0);
                anim.speed = 1; //以1帧的速度播放
                break;
        }

        rigid.velocity = vel * speed;   //刚体运动速度矢量等于vel乘以速度（Vector3.right = (1,0,0)，以此类推）
    }

    void LateUpdate()
    {
        Vector2 rPos = GetRoomPosOnGrid(0.5f);

        int doorNum;
        for(doorNum=0; doorNum<4; doorNum++)
        {
            if(rPos == InRoom.DOORS[doorNum])
            {
                break;
            }
        }

        if (doorNum > 3 || doorNum != facing)
            return;

        Vector2 rm = roomNum;
        switch(doorNum)
        {
            case 0:
                rm.x += 1;
                break;
            case 1:
                rm.y += 1;
                break;
            case 2:
                rm.x -= 1;
                break;
            case 3:
                rm.y -= 1;
                break;
        }

        if(rm.x>=0 && rm.x<=InRoom.MAX_RM_X)
        {
            if(rm.y>=0 && rm.y<=InRoom.MAX_RM_Y)
            {
                roomNum = rm;
                transitionPos = InRoom.DOORS[(doorNum + 2) % 4];
                roomPos = transitionPos;
                lastSafeLoc = transform.position;
                lastSafeFacing = facing;
                mode = eMode.transition;
                transitionDone = Time.time + transitionDelay;
            }
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (invincible)
            return;
        DamageEffect dEf = coll.gameObject.GetComponent<DamageEffect>();
        if (dEf == null)
            return;

        if (health <= 0)
            Die();
        health -= dEf.damage;
        invincible = true;
        invincibleDone = Time.time + invincibleDuration;

        if(dEf.knockback)
        {
            Vector3 delta = transform.position - coll.transform.position;
            if(Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            {
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else
            {
                delta.x = 0;
                delta.y = (delta.y > 0) ? 1 : -1;
            }

            knockbackVel = delta * knockbackSpeed;
            rigid.velocity = knockbackVel;

            mode = eMode.knockback;
            knockbackDone = Time.time + knockbackDuration;
        }
    }

    //PickUp脚本相关
    void OnTriggerEnter(Collider colld)
    {
        PickUp pup = colld.GetComponent<PickUp>();  //如果碰撞的游戏对象没有附加PickUp脚本，则此方法返回时不做任何操作。
        if (pup == null)
            return;

        switch(pup.itemType)  //根据拾取道具类型获得不同效果
        {
            case PickUp.eType.health:
                health = Mathf.Min(health + 2, maxHealth);
                break;
            case PickUp.eType.key:
                keyCount++;
                break;
            case PickUp.eType.grappler:
                hasGrappler = true;
                break;
        }

        Destroy(colld.gameObject);  //拾取后摧毁道具
    }

    public void ResetInRoom(int healthLoss = 0)
    {
        transform.position = lastSafeLoc;
        facing = lastSafeFacing;

        health -= healthLoss;

        invincible = true;
        invincibleDone = Time.time + invincibleDuration;
    }

    public int GetFacing()
    {
        return facing;
    }

    public bool moving
    {
        get
        {
            return (mode == eMode.move);
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float gridMult
    {
        get
        {
            return inRm.gridMult;
        }
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

    public int keyCount
    {
        get { return numKeys; }
        set { numKeys = value; }
    }

    public void TakeDamage(DamageEffect dEf, Vector3 Dpos)
    {
        if (invincible)
            return;
        if (dEf == null)
            return;

        if (health <= 0)
            Die();
        health -= dEf.damage;
        invincible = true;
        invincibleDone = Time.time + invincibleDuration;

        if (dEf.knockback)
        {
            Vector3 delta = transform.position - this.transform.position;
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            {
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else
            {
                delta.x = 0;
                delta.y = (delta.y > 0) ? 1 : -1;
            }

            knockbackVel = delta * knockbackSpeed;
            rigid.velocity = knockbackVel;

            mode = eMode.knockback;
            knockbackDone = Time.time + knockbackDuration;
        }
    }
    void Die()
    {
        Destroy(gameObject);
        Application.Quit();
    }
}
