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
<<<<<<< HEAD
        sword = transform.Find("Sword").gameObject;
        dray = transform.parent.GetComponent<Dray>();//������λ�ù�����Dray��λ��
        sword.SetActive(false);//ֹͣʹ�ý�
=======
        sword = transform.Find("Sword").gameObject;    //������Sword����Ϸ����
        dray = transform.parent.GetComponent<Dray>();  //��Dray�฽�ӵ�����Ϸ�����ʵ������������λ�ù�����Dray��λ��
        sword.SetActive(false);                        //ֹͣʹ�ý�������Ⱦ����ײ�����еȸ�����ɾ����Ϸ����
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);//ʹ��ָ��Dray����Եķ���ʹ��Χ��z����ת90*facing�ȡ�
        sword.SetActive(dray.mode == Dray.eMode.attack);
=======
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);  //ʹ��ָ��Dray����Եķ���ʹ��Χ��z����ת90*facing�ȡ�
        sword.SetActive(dray.mode == Dray.eMode.attack);                //Updateʱ���Dray���ڹ���ģʽ�����������á�
>>>>>>> 090f8310ff024ce044089efec6eb7aa71aa16990
    }
}
