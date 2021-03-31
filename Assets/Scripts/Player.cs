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
    public List<Image> stateImageList=new List<Image>();//用来显示状态的图片列表
    public Animator effectAnimator;//动效控制器
    public Text hpText;//血条数值文本
    public Slider hpSlider;//血条图片滑动条
    public GameObject shieldImg;//护盾图片
    public Text shieldText;//护盾值文本
    public Text energyText;//能量值文本
    #endregion
    #region 动画相关
    public Animator animController;//动画控制器
    #endregion
    public void GetShield(int shield)//获得护盾
    {
        //如果拥有背水一战状态，则无法获得护甲
        if (CheckState(Value.ValueType.背水一战))
        {
            return;
        }
        this.shield += CheckState(Value.ValueType.重伤) ? (int)(0.7 * shield) : shield;

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

    public bool CheckState(Value.ValueType stateType) //状态检测
    {
        foreach (var state in stateList)
        {
            if (state.type==stateType&&state.value>0)
            {
                if (state.type==Value.ValueType.起势)
                {
                    state.value--;
                }
                return true;
            }
        }
        return false;
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
        if (shield>0)
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

    public void Recover(int value) //回血
    {
        hp += value;
        if (hp>maxHp)
        {
            hp = maxHp;
        }
    }
    private void Awake()
    {
        Instance = this;
        shieldImg.SetActive(false);
    }

    void InitState() //初始化状态
    {
        foreach (var img in stateImageList)
        {
            img.enabled = false;
        }
    }
    
    void Start()
    {
        InitEnergy(); //初始化能量
        InitState();
    }

    void Update()
    {
        UpdateUIState();//更新UI状态
    }
}
