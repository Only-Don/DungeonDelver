using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private GameObject sword;
    private Dray dray;
    // Start is called before the first frame update
    void Start()
    {
        sword = transform.Find("Sword").gameObject;    //������Sword����Ϸ����
        dray = transform.parent.GetComponent<Dray>();  //��Dray�฽�ӵ�����Ϸ�����ʵ������������λ�ù�����Dray��λ��
        sword.SetActive(false);                        //ֹͣʹ�ý�������Ⱦ����ײ�����еȸ�����ɾ����Ϸ����
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);  //ʹ��ָ��Dray����Եķ���ʹ��Χ��z����ת90*facing�ȡ�
        sword.SetActive(dray.mode == Dray.eMode.attack);                //Updateʱ���Dray���ڹ���ģʽ�����������á�
    }
}
