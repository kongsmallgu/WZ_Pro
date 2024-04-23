using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏结束界面
public class GameOverUIManager : UIBaseManeger
{
    // 重写基类方法
    public override void ShowUI()
    {
        base.ShowUI(); // 调用基类方法
        Debug.Log("Game Over UI Manager: Showing Game Over UI");
        // 在这里编写显示游戏结束 UI 的逻辑

    }

    public override void HideUI()
    {
        base.HideUI(); // 调用基类方法
        Debug.Log("Game Over UI Manager: Hiding Game Over UI");
        // 在这里编写隐藏游戏结束 UI 的逻辑
    }
}
