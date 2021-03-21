using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 单例
    public static Player Instance;
    #endregion

    #region 基本属性

    public int maxHp;//最大生命值
    public int hp ;//当前生命值
    public int shield;//护盾值
    public int drawCardNum;//每回合抽牌数
    public int energy;//当前能量值
    public int maxEnergy;//能量值上限
    public List<Value> stateList=new List<Value>();//状态列表
    #endregion

    #region UI引用
    public Text hpText;//血条数值文本
    public Slider hpSlider;//血条图片滑动条
    public GameObject shieldImg;//护盾图片
    public Text shieldText;//护盾值文本
    public Text energyText;//能量值文本
    public GameObject doubleBladeText;//二刀文本
    #endregion
    #region 动画相关
    public Animator animController;//动画控制器
    #endregion
    public void GetShield(int shield)//获得护盾
    {
        this.shield += shield;
    }

    public void GetEnergy(int value)//获得能量
    {
        energy += value;
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

    public bool LiuZhuan() //是否处于流转状态
    {
        foreach (var state in stateList)
        {
            if (state.type == Value.ValueType.流转 && state.value > 0)
            {
                return true;
            }
        }
        return false;
    }
    public bool DoubleBlade() //是否处于二刀流状态
    {
        foreach (var state in stateList)
        {
            if (state.type==Value.ValueType.二刀流&&state.value>0)
            {
                return true;
            }
        }
        return false;
    }
    void UpdateUIState()//更新UI组件状态
    {
        if (shield>0)
        {
            shieldImg.SetActive(true); 
            shieldText.text = "" + shield;
        }
        else
        {
            shieldImg.SetActive(false);
        }

        if (DoubleBlade())
        {
            doubleBladeText.SetActive(true);
        }
        else
        {
            doubleBladeText.SetActive(false);

        }

        hpText.text = hp + "/" + maxHp;
        hpSlider.value = 1.0f * hp / maxHp;
        energyText.text = energy + "/" + maxEnergy;
    }

    public void PlayAttackAnim()//播放攻击动画
    {
        animController.SetTrigger("Attack");
        
    }
    public void InitEnergy()//初始化能量
    {
        energy = maxEnergy;
    }
    private void Awake()
    {
        Instance = this;
        shieldImg.SetActive(false);
    }

    
    void Start()
    {
        InitEnergy(); //初始化能量

    }

    void Update()
    {
        UpdateUIState();//更新UI状态
    }
}
