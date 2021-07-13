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

    public int gold = 0;
    public int Level = 1;
    public List<int> levelExpList = new List<int>();
    public int currentExp;
    public int maxHp; //最大生命值
    public int hp; //当前生命值
    public int shield; //护盾值
    public int drawCardNum; //每回合抽牌数
    public int energy; //当前能量值
    public int maxEnergy; //能量值上限
    public List<Value> stateList = new List<Value>(); //状态列表
    public List<Value> newStateList = new List<Value>(); //刚添加的状态列表

    #endregion

    #region UI引用

    public List<Image> stateImageList = new List<Image>(); //用来显示状态的图片列表
    public List<Text> stateStackTextList = new List<Text>(); //用来显示状态层数的文本列表

    public Animator effectAnimator; //动效控制器
    public Text hpText; //血条数值文本
    public Slider hpSlider; //血条图片滑动条
    public GameObject shieldImg; //护盾图片
    public Text shieldText; //护盾值文本
    public Text energyText; //能量值文本
    public Text goldText; //金币文本
    public Text levelText; //等级文本
    public Text expText; //经验文本

    #endregion

    #region 动画相关

    public Animator animController; //动画控制器

    #endregion

    public void GetExp(int exp) //获得经验并进行升级结算
    {
        currentExp += exp;
        while (currentExp >= levelExpList[Level - 1])
        {
            currentExp -= levelExpList[Level - 1];
            Level++;
        }
    }

    public void GetGold(int value)
    {
        gold += value;
    }

    public void InitLevel()
    {
        Level = 1;
        var n = 20;
        for (var i = 1; i <= 50; i++)
        {
            levelExpList.Add(n);
            n += 20 + i / 5 * 10;
        }
    }

    public void GetShield(int shield) //获得护盾
    {
        //如果拥有背水一战状态，则无法获得护甲
        if (CheckState(Value.ValueType.背水一战)) return;
        //如果拥有遗物武术心得，则进行相关检测
        if (RelicManager.Instance.isWuShu)
        {
            shield = (int) (shield * 1.5);
        }

        this.shield += CheckState(Value.ValueType.重伤) ? (int) (0.7 * shield) : shield;
    }

    public void GetEnergy(int value) //获得能量
    {
        energy += value;
    }

    public void TakeDamage(int damage) //结算受到的伤害
    {
        var cleanAllies = new List<Ally>(); //计算伤害后应该清除的友方单位
        //如果有神隐状态则直接将伤害置为0
        if (CheckState(Value.ValueType.神隐))
        {
            damage = 0;
        }
        //如果有弱肉强食之证，则受到的伤害翻倍
        if (RelicManager.Instance.CheckRelic(11))
        {
            damage *= 2;
        }
        if (CheckState(Value.ValueType.灵体) && CheckState(Value.ValueType.魂体)) damage = (int) (0.7f * damage);
        if (damage <= shield)
        {
            shield -= damage;
            damage = 0;
        }
        else
        {
            shield = 0;
            damage -= shield;
            if (AllyManager.Instance.inGameAlliesList.Count > 0)
                for (var i = 0; i < AllyManager.Instance.inGameAlliesList.Count; i++)
                {
                    var thisAlly = AllyManager.Instance.inGameAlliesList[i];
                    if (damage < thisAlly.currentHp)
                    {
                        thisAlly.currentHp -= damage;
                        damage = 0;
                        break;
                    }
                    else
                    {
                        damage -= thisAlly.currentHp;
                        cleanAllies.Add(thisAlly);
                    }
                }
        }

        foreach (var ally in cleanAllies)
        {
            AllyManager.Instance.inGameAlliesList.Remove(ally);
            Destroy(ally.gameObject);
        }

        if (damage <= 0) return;
        hp -= damage;
    }

    public bool CheckState(Value.ValueType stateType) //状态检测
    {
        foreach (var state in stateList)
            if (state.type == stateType && state.value > 0)
            {
                if (state.type == Value.ValueType.起势) state.value--;
                return true;
            }

        return false;
    }

    private void UpdateUIState() //更新UI组件状态
    {
        for (var i = 0; i < stateList.Count; i++)
        {
            stateImageList[i].enabled = true;
            if (StateManager.Instance.stateImgDic.ContainsKey(stateList[i].type))
            {
                stateImageList[i].sprite = StateManager.Instance.stateImgDic[stateList[i].type];
                if (stateList[i].value > 0)
                {
                    stateStackTextList[i].enabled = true;
                    stateStackTextList[i].text = stateList[i].value.ToString();
                }
            }
        }

        for (var i = stateList.Count; i < stateImageList.Count; i++) stateImageList[i].enabled = false;
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
        energyText.text = energy + "/" + maxEnergy;
        levelText.text = "Level:" + Level;
        goldText.text = "金币:" + gold;
        expText.text = currentExp + "/" + levelExpList[Level - 1];
    }

    public void PlayAttackAnim() //播放攻击动画
    {
        animController.SetTrigger("Attack");
    }

    public void InitEnergy() //初始化能量
    {
        energy = maxEnergy;
    }

    public void Recover(int value) //回血
    {
        hp += value;
        if (hp > maxHp) hp = maxHp;
    }

    private void Awake()
    {
        Instance = this;
        shieldImg.SetActive(false);
    }

    private void InitState() //初始化状态
    {
        foreach (var img in stateImageList) img.enabled = false;
    }

    private void Start()
    {
        InitEnergy(); //初始化能量
        InitState();
        InitLevel();
    }

    private void Update()
    {
        UpdateUIState(); //更新UI状态
    }
}