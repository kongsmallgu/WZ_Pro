using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/SkillStats")]
public class SkillStats : ScriptableObject
{
    // 技能属性
    public int WeaponAttack;
    public int WeaponDefense;
    public Sprite WeaponSpriteIcon;//图标
    public Sprite WeaponSpriteBg;//背景图
    public string WeaponName; //名字
    public string WeaponDsc; //描述

}
