using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/SkillStats")]
public class SkillStats : ScriptableObject
{
    // ��������
    public int WeaponAttack;
    public int WeaponDefense;
    public Sprite WeaponSpriteIcon;//ͼ��
    public Sprite WeaponSpriteBg;//����ͼ
    public string WeaponName; //����
    public string WeaponDsc; //����

}
