using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected static Vector3[] directions = new Vector3[]
    {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down
    };

    [Header("Set in Inspector: Enemy")]
    public float maxHealth = 1;
    public float knockbackSpeed = 10;//击退速度
    public float knockbackDuration = 0.25f;//击退持续时间
    public float invincibleDuration = 0.5f;//无敌持续时间
    public GameObject[] randomItemDrops;//随机物品掉落
    public GameObject guaranteedItemDrop = null;//必定会掉落的，会设置掉落的东西

    [Header("Set Dynamically: Enemy")]
    public float health;//健康值
    public bool invincible = false;//布尔型无敌值
    public bool knockback = false;//布尔型击退值

    private float invincibleDone = 0;
    private float knockbackDone = 0;
    private Vector3 knockbackVel;//创建一个三维向量

    protected Animator anim;//动画器
    protected Rigidbody rigid;//刚体类
    protected SpriteRenderer sRend;//妖怪渲染

    protected virtual void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        sRend = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()//每帧动画都调用的函数
    {
        if (invincible && Time.time > invincibleDone)//保证被攻击之后一段时间内无敌
            invincible = false;//无敌失效
        sRend.color = invincible ? Color.red : Color.white;//如果无敌，红色，不是，就是白色
        if (knockback)
        {
            rigid.velocity = knockbackVel;//刚体（敌人）速度被赋予击退的速度
            if (Time.time < knockbackDone)
                return;
        }

        anim.speed = 1;//恢复初始速度速度
        knockback = false;//受伤害时调用的函数
    }

    public virtual void OnTriggerEnter(Collider colld)
    {
        if (invincible)
            return;//如果无敌，不运行该函数
        DamageEffect dEf = colld.gameObject.GetComponent<DamageEffect>();
        if (dEf == null)
            return;//如果没有伤害值（比如墙体碰撞），不运行该函数
        Enemy e = colld.gameObject.GetComponent<Enemy>();
        if (e != null)
            return;

        health -= dEf.damage;
        if (health <= 0)
            Die();

        invincible = true;//无敌
        invincibleDone = Time.time + invincibleDuration;//无敌时间更新

        if (dEf.knockback)//击退
        {
            Vector3 delta = transform.position - colld.transform.root.position;
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))//判断击退方向
            {
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else
            {
                delta.x = 0;
                delta.y = (delta.y > 0) ? 1 : -1;
            }

            knockbackVel = delta * knockbackSpeed;//被击退速度等于方向*被击退的速度
            rigid.velocity = knockbackVel;//赋给刚体的速度

            knockback = true;
            knockbackDone = Time.time + knockbackDuration;
            anim.speed = 0;
        }
    }

    void Die()
    {
        GameObject go;
        if (guaranteedItemDrop != null)
        {
            go = Instantiate<GameObject>(guaranteedItemDrop);
            go.transform.position = transform.position;//掉落物品的位置在死掉的地方
        }
        else if(randomItemDrops.Length>0)
        {
            int n = Random.Range(0, randomItemDrops.Length);//掉落物品的编号012
            GameObject prefab = randomItemDrops[n];
            if (prefab != null)
            {
                go = Instantiate<GameObject>(prefab);
                go.transform.position = transform.position;
            }
        }
        Destroy(gameObject);
    }
}
