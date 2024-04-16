using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapons", menuName = "Weapon/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    // ��������
    public int WeapoNnumber;
    public int WeaponAttack;
    public int WeaponDefense;
    public Sprite WeaponSpriteIcon;//ͼ��
    public Sprite WeaponSpriteBg;//����ͼ
    public string WeaponName; //����
    public string WeaponDsc; //����

    public GameObject WeaponObj;//����ʵ��
}
