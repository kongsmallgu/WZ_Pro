using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserList
{
    public List<User> p;
}

//标记可序列化
[System.Serializable]
public class User
{
    public string username; //玩家名字
  /*  public string password;
    public bool isVerfied;//是否进行了实名认证
    public int age; //年龄
    public string agelevel;//年龄层级
    public int remainingduration;//游玩时长存储*/

    public string nickname; //昵称


}