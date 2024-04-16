using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using System.Collections.Generic;
public class UserManager : MonoBehaviour
{
    public static UserManager Instance;
    private void Awake()
    {
        UserManager.Instance = this;
    }
    //���ݴ洢���
    private string newFilePath = "Users";
    private string filePath;
    //��ȡ����
    public UserList SavePlayerDatas;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        // �־û�·��
        string persistentPath = Application.persistentDataPath;
        filePath = Path.Combine(persistentPath, newFilePath + ".txt");
        if (!File.Exists(filePath))
        {
            TextAsset textAsset = Resources.Load<TextAsset>(newFilePath);
            string fileContent = textAsset.text;

            File.WriteAllText(filePath, fileContent);

            // ֱ�Ӷ�
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            SavePlayerDatas = JsonUtility.FromJson<UserList>(jsonStr);
        }
        else
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();

            SavePlayerDatas = JsonUtility.FromJson<UserList>(jsonStr);
            Debug.Log("��������");
        }
    }

    //��ȡ����
    public User LoadUser(string username)
    {
        if (null == SavePlayerDatas) return null;

        for (int i = 0; i < SavePlayerDatas.p.Count; i++)
        {
            if (username == SavePlayerDatas.p[i].username)
            {
                return SavePlayerDatas.p[i];
            }
        }
        return null;
    }

    //�洢����
    public void SaveUser(User user)
    {
        string persistentPath = Application.persistentDataPath;
        string filePath = Path.Combine(persistentPath, "Users" + ".txt");
        StreamWriter sw = new StreamWriter(filePath);

        UserManager.Instance.SavePlayerDatas.p.Add(user);

        string sData = JsonUtility.ToJson(UserManager.Instance.SavePlayerDatas);
        sw.Write(sData);
        sw.Close();
    }

}
