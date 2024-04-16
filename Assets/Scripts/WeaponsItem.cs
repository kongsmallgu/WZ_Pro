using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsItem : MonoBehaviour
{
    public WeaponStats WeaponStats;

    private int weaponAttack;
    private int weaponDefense;
    private Sprite weaponSpriteIcon;//ͼ��
    private Sprite weaponSpriteBg;//����ͼ
    private string weaponName; //����
    private string weaponDsc; //����
    private GameObject weaponPrefab; //����Ԥ����

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

    //��ӵ���¼�
    public void OnClickSelect() 
    {
        Debug.Log("�ұ�ѡ����" + weaponPrefab.name+ "=========================");
        WeaSkiControl.SwitchWeapon(weaponPrefab);
        //ParameterManage.Instance.currentPlayerWeapon = weaponPrefab;
        ParameterManage.Instance.PlayerWeapon = false;
    }

}
