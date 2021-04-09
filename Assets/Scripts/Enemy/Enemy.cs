﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    #region 基本属性
    public string Name;//名字
    public int maxHp;//最大生命值
    public int hp;//当前生命值
    public int shield;//护盾值
    public List<EnemyAction> actionList = new List<EnemyAction>();
    private int actionNo=0;
    public EnemyAction currentEnemyAction;
    public List<Sprite> actionSpriteList = new List<Sprite>();
    public List<Value> stateList = new List<Value>();
    public int actualValue;
    public EnemyData enemyData;//敌人数据

    public List<Image> stateImageList=new List<Image>();//用来显示状态的图片列表

    #endregion

    public Animator animController;//动画控制器
    #region UI引用
    public Text hpText;//血条数值文本
    public Slider hpSlider;//血条图片滑动条
    public GameObject shieldImg;//护盾图片
    public Text shieldText;//护盾值文本
    public GameObject actionImg;//行动类型图示
    public Text actionValueText;//行动数值
    #endregion

    public void GetShield(int value)
    {
        shield +=CheckState(Value.ValueType.重伤)? (int)(0.7*value):value;
    }
    public bool CheckState(Value.ValueType stateType) //状态检测
    {
        foreach (var state in stateList)
        {
            if (state.type == stateType && state.value > 0)
            {
                return true;
            }
        }
        return false;
    }
    //初始化敌人
    public void InitEnemy(EnemyData data)
    {
        enemyData = data;
        maxHp = data.maxHp;
        hp = maxHp;
        shield = data.initShield;
        Name = data.Name;
        foreach (var id in data.ActionIdList)
        {
            foreach (var aData in ActionController.Instance.actionDataList)
            {
                if (id==aData.ActID)
                {
                    EnemyAction newEnemyAction = new EnemyAction();
                    newEnemyAction.data = aData;
                    foreach (var val in aData.valueList)
                    {
                        newEnemyAction.valueDic.Add(val.type,val.value);
                    }
                    actionList.Add(newEnemyAction);
                    break;
                }
            
            }
        }

        if (enemyData.ID==2)
        {
            StateManager.AddStateToEnemy(new Value(){type=Value.ValueType.魂体,value = 1},this);
        }
        EnemyManager.Instance.InGameEnemyList.Add(this);
    }
    void UpdateUIState()//更新UI组件状态
    {
        for (int i = 0; i < stateList.Count; i++)
        {
            stateImageList[i].enabled = true;
            if (StateManager.Instance.stateImgDic.ContainsKey(stateList[i].type))
            {
                stateImageList[i].sprite = StateManager.Instance.stateImgDic[stateList[i].type];
            }
        }

        for (int i = stateList.Count; i < stateImageList.Count; i++)
        {
            stateImageList[i].enabled = false;

        }
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
        if (hp<=0)
        {
            EnemyDie();
        }
        if (currentEnemyAction!=null)
        {
            if (enemyData.ID==7)
            {
                
            }
            switch (currentEnemyAction.data.Type)
            {
                case ActionController.ActionType.Attack:
                    actionValueText.enabled = true;
                    actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Attack];
                    if (enemyData.ID == 7)
                    {
                        actionValueText.enabled = false;
                        actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Unknown];
                    }
                    actualValue = CheckState(Value.ValueType.惊吓)
                        ? Convert.ToInt32(currentEnemyAction.valueDic[Value.ValueType.伤害] * 0.7)
                        : currentEnemyAction.valueDic[Value.ValueType.伤害];
                    actionValueText.text = "" + actualValue;
                    break;
                case ActionController.ActionType.Defend:
                    actionValueText.enabled = true;
                    actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Defend];
                    if (enemyData.ID == 7)
                    {
                        actionValueText.enabled = false;
                        actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Unknown];
                    }
                    actualValue = currentEnemyAction.valueDic[Value.ValueType.护甲];
                    actionValueText.text = "" + actualValue;
                    break;
                case ActionController.ActionType.Buff:
                    actionValueText.enabled = false;
                    actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Buff];
                    if (enemyData.ID == 7)
                    {
                        actionValueText.enabled = false;
                        actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Unknown];
                    }
                    break;
                case ActionController.ActionType.Special:
                    actionValueText.enabled = false;
                    actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Special];
                    if (enemyData.ID == 7)
                    {
                        actionValueText.enabled = false;
                        actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Unknown];
                    }
                    break;
                case ActionController.ActionType.DeBuff:
                    actionValueText.enabled = false;
                    actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.DeBuff];
                    if (enemyData.ID == 7)
                    {
                        actionValueText.enabled = false;
                        actionImg.GetComponent<Image>().sprite = actionSpriteList[(int)ActionController.ActionType.Unknown];
                    }
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
    }
    public void TakeAction()
    {
        ActionController.Instance.TakeAction(currentEnemyAction,this);

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

    public void UpdateCurrentAction()
    {
        if (enemyData.ID==2&&hp>=maxHp/2)//懒惰妖精
        {
            currentEnemyAction = actionList[0];
            return;
        }

        int n = Random.Range(1, 101);//在1到100取值
        int temp = 0;
        for (int i = 0; i < actionList.Count; i++)
        {
            temp += actionList[i].data.actProbability;
            if (n<=temp)
            {
                currentEnemyAction = actionList[i];
                break;
            }
        }
    }

    public void EnemyDie() //敌人生命值归零时触发的方法
    {
        if (this.enemyData.ID==3)//暴躁妖精亡语
        {
            StateManager.AddStateToPlayer(new Value(){type = Value.ValueType.重伤,value = 2});
        }
        EnemyManager.Instance.InGameEnemyList.Remove(this);
        Destroy(gameObject);
    }
    void Update()
    {
        UpdateUIState();
    }
}
