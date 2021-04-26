using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionImage : MonoBehaviour
{
    private Enemy thisEnemy;//管理这个状态图标的敌人对象
    private Text actionExplanationText;

    void Start()
    {
        thisEnemy = GetComponentInParent<Enemy>();
        actionExplanationText = MenuEventManager.Instance.actionExplanationText;
    }

    public void OnActionImgPointerEnter() //当鼠标放到行动图标上的回调方法
    {
        actionExplanationText.enabled = true;
        actionExplanationText.text = "";
        actionExplanationText.transform.position = transform.position - new Vector3(0f, -0.5f, 0.0f);

        switch (thisEnemy.currentEnemyAction.data.Type)
        {
            case ActionController.ActionType.Attack:
                actionExplanationText.text = "该敌人将攻击并造成" + thisEnemy.actualValue + "点伤害";
                break;
            case ActionController.ActionType.Defend:
                actionExplanationText.text = "该敌人将获得" + thisEnemy.actualValue + "点护甲";
                if (thisEnemy.enemyData.ID==6)//大妖精群体防御
                {
                    actionExplanationText.text = "该敌人将使所有敌人获得" + thisEnemy.actualValue + "点护甲";
                }
                break;
            case ActionController.ActionType.Buff:
                actionExplanationText.text = "该敌人将获得或给予其他敌人强化效果" ;
                break;
            case ActionController.ActionType.DeBuff:
                actionExplanationText.text = "该敌人将对玩家施加负面效果";
                break;
            case ActionController.ActionType.Special:
                actionExplanationText.text = "该敌人将进行特殊的行动";
                break;
            case ActionController.ActionType.Unknown:
                actionExplanationText.text = "该敌人的行动未知";
                break;
        }
    }

    public void OnActionImgPointerExit() //当鼠标离开行动图标上的回调方法
    {
        actionExplanationText.enabled = false;
    }
    void Update()
    {
        
    }
}
