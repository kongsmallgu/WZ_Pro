using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsItem : MonoBehaviour
{
    public WeaponStats WeaponStats;

    private int weaponAttack;
    private int weaponDefense;
    private Sprite weaponSpriteIcon;//图标
    private Sprite weaponSpriteBg;//背景图
    private string weaponName; //名字
    private string weaponDsc; //描述
    private GameObject weaponPrefab; //武器预制体

    private GameObject WeaponsIcon;
    private GameObject WeaponsTitle;
    private GameObject WeaponsDes;

    private void Start()
    {
        weaponAttack = WeaponStats.WeaponAttack;
        weaponDefense = WeaponStats.WeaponDefense;
        weaponSpriteIcon = WeaponStats.WeaponSpriteIcon;
        weaponSpriteBg = WeaponStats.WeaponSpriteBg;
        weaponName = WeaponStats.WeaponName;
        weaponDsc = WeaponStats.WeaponDsc;
        weaponPrefab = WeaponStats.WeaponObj;

        WeaponsIcon = this.gameObject.transform.GetChild(0).gameObject;
        WeaponsTitle = this.gameObject.transform.GetChild(1).gameObject;
        WeaponsDes = this.gameObject.transform.GetChild(2).gameObject;

        this.gameObject.GetComponent<Image>().sprite = weaponSpriteBg;
        WeaponsIcon.GetComponent<Image>().sprite = weaponSpriteIcon;
        WeaponsTitle.GetComponent<Text>().text = weaponName;
        WeaponsDes.GetComponent<Text>().text = weaponDsc;

    }

    //添加点击事件
    public void OnClickSelect() 
    {
        Debug.Log("我被选中了" + weaponPrefab.name+ "=========================");
        WeaSkiControl.SwitchWeapon(weaponPrefab);
        //ParameterManage.Instance.currentPlayerWeapon = weaponPrefab;
        ParameterManage.Instance.PlayerWeapon = false;
    }

}
