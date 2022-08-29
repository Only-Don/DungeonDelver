using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour, IFacingMover, IKeyMaster
{
<<<<<<< HEAD
    public enum eMode
=======
    public enum eMode //������ö�٣������������ٺͲ�ѯDray��״̬
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    {
        idle, move, attack, transition, knockback
    }

<<<<<<< HEAD
    [Header("Set in Inspector")]
    public float    speed = 5;  //�ƶ��ٶ�
    public float attackDuration = 0.25f;    //�����ĳ�������
    public float transitionDelay = 0.5f;
=======
    //public��������ֱ����unity�༭���е�Inspector��༭
    [Header("Set in Inspector")]
    public float    speed = 5;  //�ƶ��ٶ�
    public float attackDuration = 0.25f;    //�����ĳ�������
    public float transitionDelay = 0.5f;   
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    public float attackDelay = 0.5f;    //���������ļ��
    public int   maxHealth = 10;
    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float invincibleDuration = 0.5f;

    [Header("Set Dynamically")]
    public int      dirHeld = -1;   //��ǰ����ָʾ����
    public int      facing = 1;     //Dray����Եķ���
    public eMode    mode = eMode.idle;//Ĭ��Ϊ��ֹ
    public int      numKeys = 0;
    public bool     invincible = false;
    public bool     hasGrappler = false;
    public Vector3  lastSafeLoc;
    public int      lastSafeFacing;

    [SerializeField]
    private int _health;

<<<<<<< HEAD
    public int health
=======
    public int health  //�������ǵ�������������inspector���޸�
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    {
        get { return _health; }
        set { _health = value; }
    }

<<<<<<< HEAD
    private float   timeAtkDone = 0;
    private float   timeAtkNext = 0;
=======
    private float   timeAtkDone = 0;     //��������Ӧ����ɵ�ʱ��
    private float   timeAtkNext = 0;     //Dray�ܹ��ٴη��𹥻���ʱ����
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    private float   transitionDone = 0;
    private Vector2 transitionPos;
    private float   knockbackDone = 0;
    private float   invincibleDone = 0;
    private Vector3 knockbackVel;

    private SpriteRenderer sRend;
    private Rigidbody rigid;
    private Animator anim;
    private InRoom inRm;

<<<<<<< HEAD
    private Vector3[] directions = new Vector3[]
=======
    private Vector3[] directions = new Vector3[]  //����һ���µ��������飬dirHeld����ֵ��Ӧ����Ĳ�ͬԪ�أ��������������Ĳ�ͬ����
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down
    };

<<<<<<< HEAD
    private KeyCode[] keys = new KeyCode[]
=======
    private KeyCode[] keys = new KeyCode[] //����һ���µİ������飬����������ͬ���������ɵ�����ÿ�������
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    {
        KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S
    };

    private void Awake()
    {
<<<<<<< HEAD
        sRend = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody>(); //�������
        anim = GetComponent<Animator>();//���붯��
        inRm = GetComponent<InRoom>();
        health = maxHealth;
=======
        sRend = GetComponent<SpriteRenderer>();  //������Ⱦ��
        rigid = GetComponent<Rigidbody>();       //�������
        anim = GetComponent<Animator>();         //���붯��
        inRm = GetComponent<InRoom>();
        health = maxHealth;                      //����ʱ���Ѫ��
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
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
<<<<<<< HEAD
            if (Input.GetKey(keys[i]))
=======
            if (Input.GetKey(keys[i]))  //ʹ�õ���ǰ��İ�������
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
                dirHeld = i;    //���Ű����ı��ƶ�����
        }
        
        if(Input.GetKeyDown(KeyCode.J) && Time.time >= timeAtkNext) //��ס����������ʱ�������һ�οɹ���ʱ��ʱ����
        {
<<<<<<< HEAD
            mode = eMode.attack;//ģʽתΪ����ģʽ
            timeAtkDone = Time.time + attackDuration;
            timeAtkNext = Time.time + attackDelay;
=======
            mode = eMode.attack;                       //ģʽתΪ����ģʽ
            timeAtkDone = Time.time + attackDuration;  //���ú�ʱֹͣ��������
            timeAtkNext = Time.time + attackDelay;     //���ú�ʱ���ٴβ��Ź�������
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
        }

        if(Time.time >= timeAtkDone)    //��ʱ����ڹ������ʱ��ʱ�ָ���ֹ
        {
            mode = eMode.idle;
        }

<<<<<<< HEAD
        if(mode != eMode.attack)//��ģʽ��Ϊ����ģʽ����ʼѡ����ȷģʽ
=======
        if(mode != eMode.attack)//��ģʽ��Ϊ����ģʽ��������Ƿ�ס�ƶ�����idle��move֮�����ѡ��
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
        {
            if(dirHeld == -1)
            {
                mode = eMode.idle;//Ĭ��ģʽ��ֹ
            }
            else
            {
<<<<<<< HEAD
                facing = dirHeld;//����Է����Ϊ�������򣬽����ƶ�ģʽ
=======
                facing = dirHeld;//ȷ����Է������ƶ�����һ��
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
                mode = eMode.move;
            }
        }

<<<<<<< HEAD
        Vector3 vel = Vector3.zero;
        switch (mode)   //�ֱ�Ϊ��ͬģʽѡ���Ӧ����
        {
            case eMode.attack:
                anim.CrossFade("Dray_Attack_" + facing, 0);
                anim.speed = 0;
                break;
            case eMode.idle:
                anim.CrossFade("Dray_Walk_" + facing, 0);
                anim.speed = 0;
=======
        //ִ��modeѡ���ģʽ��������ȷ����
        Vector3 vel = Vector3.zero;
        switch (mode) 
        {
            case eMode.attack:
                anim.CrossFade("Dray_Attack_" + facing, 0);
                anim.speed = 0; //���빥��������ͣ��ԭ�أ�ֱ��������������ʱ��Է���Ҳ�ǲ����
                break;
            case eMode.idle:
                anim.CrossFade("Dray_Walk_" + facing, 0);
                anim.speed = 0; //����ʱ������ֹ�������泯���򲻱�
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
                break;
            case eMode.move:
                vel = directions[dirHeld];
                anim.CrossFade("Dray_Walk_" + facing, 0);
<<<<<<< HEAD
                anim.speed = 1;
=======
                anim.speed = 1; //��1֡���ٶȲ���
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
                break;
        }

        rigid.velocity = vel * speed;   //�����˶��ٶ�ʸ������vel�����ٶȣ�Vector3.right = (1,0,0)���Դ����ƣ�
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

        health -= dEf.damage;
        if (health <= 0)
            Die();
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

<<<<<<< HEAD
    void OnTriggerEnter(Collider colld)
    {
        PickUp pup = colld.GetComponent<PickUp>();
        if (pup == null)
            return;

        switch(pup.itemType)
=======
    //PickUp�ű����
    void OnTriggerEnter(Collider colld)
    {
        PickUp pup = colld.GetComponent<PickUp>();  //�����ײ����Ϸ����û�и���PickUp�ű�����˷�������ʱ�����κβ�����
        if (pup == null)
            return;

        switch(pup.itemType)  //����ʰȡ�������ͻ�ò�ͬЧ��
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
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

<<<<<<< HEAD
        Destroy(colld.gameObject);
=======
        Destroy(colld.gameObject);  //ʰȡ��ݻٵ���
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
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
