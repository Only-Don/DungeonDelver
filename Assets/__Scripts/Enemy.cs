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
    public float knockbackSpeed = 10;//�����ٶ�
    public float knockbackDuration = 0.25f;//���˳���ʱ��
    public float invincibleDuration = 0.5f;//�޵г���ʱ��
    public GameObject[] randomItemDrops;//�����Ʒ����
    public GameObject guaranteedItemDrop = null;//�ض������ģ������õ���Ķ���

    [Header("Set Dynamically: Enemy")]
    public float health;//����ֵ
    public bool invincible = false;//�������޵�ֵ
    public bool knockback = false;//�����ͻ���ֵ

    private float invincibleDone = 0;
    private float knockbackDone = 0;
    private Vector3 knockbackVel;//����һ����ά����

    protected Animator anim;//������
    protected Rigidbody rigid;//������
    protected SpriteRenderer sRend;//������Ⱦ

    protected virtual void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        sRend = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()//ÿ֡���������õĺ���
    {
        if (invincible && Time.time > invincibleDone)//��֤������֮��һ��ʱ�����޵�
            invincible = false;//�޵�ʧЧ
        sRend.color = invincible ? Color.red : Color.white;//����޵У���ɫ�����ǣ����ǰ�ɫ
        if (knockback)
        {
            rigid.velocity = knockbackVel;//���壨���ˣ��ٶȱ�������˵��ٶ�
            if (Time.time < knockbackDone)
                return;
        }

        anim.speed = 1;//�ָ���ʼ�ٶ��ٶ�
        knockback = false;//���˺�ʱ���õĺ���
    }

    public virtual void OnTriggerEnter(Collider colld)
    {
        if (invincible)
            return;//����޵У������иú���
        DamageEffect dEf = colld.gameObject.GetComponent<DamageEffect>();
        if (dEf == null)
            return;//���û���˺�ֵ������ǽ����ײ���������иú���
        Enemy e = colld.gameObject.GetComponent<Enemy>();
        if (e != null)
            return;

        health -= dEf.damage;
        if (health <= 0)
            Die();

        invincible = true;//�޵�
        invincibleDone = Time.time + invincibleDuration;//�޵�ʱ�����

        if (dEf.knockback)//����
        {
            Vector3 delta = transform.position - colld.transform.root.position;
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))//�жϻ��˷���
            {
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else
            {
                delta.x = 0;
                delta.y = (delta.y > 0) ? 1 : -1;
            }

            knockbackVel = delta * knockbackSpeed;//�������ٶȵ��ڷ���*�����˵��ٶ�
            rigid.velocity = knockbackVel;//����������ٶ�

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
            go.transform.position = transform.position;//������Ʒ��λ���������ĵط�
        }
        else if(randomItemDrops.Length>0)
        {
            int n = Random.Range(0, randomItemDrops.Length);//������Ʒ�ı��012
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
