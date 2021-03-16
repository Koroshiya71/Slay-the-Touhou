using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region 基本属性
    public string Name;//名字
    public int maxHp;//最大生命值
    public int hp;//当前生命值
    public int shield;//护盾值
    public List<Action> actionList = new List<Action>();
    private int actionNo=0;
    public Action currentAction;
    public List<Sprite> actionSpriteList = new List<Sprite>();
    public int enemyID;

    #endregion

    #region UI引用
    public Text hpText;//血条数值文本
    public Slider hpSlider;//血条图片滑动条
    public GameObject shieldImg;//护盾图片
    public Text shieldText;//护盾值文本
    public GameObject actionImg;//行动类型图示
    public Text actionValueText;//行动数值
    #endregion
    //初始化敌人
    public void InitEnemy()
    {
        actionNo = 0;
        EnemyData data = EnemyManager.Instance.enemyDataList[enemyID];
        maxHp = data.maxHp;
        hp = data.initHp;
        shield = data.initShield;
        Name = data.Name;
        foreach (var id in data.ActionIdList)
        {
            foreach (var a in ActionController.Instance.actionDataList)
            {
                if (id==a.ActID)
                {
                    Action newAction = shieldImg.AddComponent<Action>();
                    newAction.InitAction(a);
                    actionList.Add(newAction);
                    break;
                }
            
            }
        }
        
        EnemyManager.Instance.InGameEnemyList.Add(this);
    }
    void UpdateUIState()//更新UI组件状态
    {
        if (shield > 0)
        {
            shieldImg.SetActive(true);
            shieldText.text = "" + shield;
        }
        else
        {
            shieldImg.SetActive(false);
        }
        hpText.text = hp + "/" + maxHp;
        hpSlider.value = 1.0f * hp / maxHp;
        currentAction = actionList[actionNo];

        if (currentAction!=null)
        {

            switch (currentAction.data.Type)
            {
                case ActionController.ActionType.Attack:
                    actionValueText.enabled = true;
                    actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Attack];
                    actionValueText.text = "" + currentAction.valueDic[Value.ValueType.Damage];
                    break;
                case ActionController.ActionType.Defend:

                    actionValueText.enabled = true;
                    actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Defend];
                    actionValueText.text = "" + currentAction.valueDic[Value.ValueType.Shield];
                    break;
            }
            
        }
    }

    private void OnMouseEnter()
    {
        CardEffectManager.Instance.targetEnemy = this;
    }

    private void OnMouseExit()
    {
        CardEffectManager.Instance.targetEnemy = null;
    }

    void Start()
    {
        InitEnemy();

    }
    public void TakeAction()
    {
        ActionController.Instance.TakeAction(currentAction,this);
        actionNo++;
        if (actionNo==actionList.Count)
        {
            actionNo = 0;
        }

    }
    public void TakeDamage(int damage)//结算受到的伤害
    {
        if (shield >= damage)
        {
            shield -= damage;
        }
        else
        {
            hp -= damage - shield;
            shield = 0;
        }
    }
    void Update()
    {
        UpdateUIState();
    }
}
