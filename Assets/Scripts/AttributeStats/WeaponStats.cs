using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapons", menuName = "Weapon/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    // ÎäÆ÷ÊôĞÔ
    public int WeapoNnumber;
    public int WeaponAttack;
    public int WeaponDefense;
    public Sprite WeaponSpriteIcon;//Í¼±ê
    public Sprite WeaponSpriteBg;//±³¾°Í¼
    public string WeaponName; //Ãû×Ö
    public string WeaponDsc; //ÃèÊö

    public GameObject WeaponObj;//ÎäÆ÷ÊµÌå
}
