using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int damage = 1;//造成的伤害为1
    public bool knockback = true;//被击退
}
