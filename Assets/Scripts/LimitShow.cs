using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitShow : MonoBehaviour
{
    public GameObject limitshow;
    private GameObject panel;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        panel = limitshow.transform.GetChild(0).gameObject;
        panel.GetComponent<Button>().onClick.AddListener(hideImage);
    }
    public void showImage()
    {

        if (limitshow.activeSelf == false)
        {
            limitshow.SetActive(true);
        }

    }
    public void hideImage()
    {
        if (limitshow.activeSelf == true)
        {
            limitshow.SetActive(false);
        }

    }
}
