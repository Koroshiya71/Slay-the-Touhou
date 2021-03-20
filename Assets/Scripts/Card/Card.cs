using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum CardType//卡牌类型
    {
        体术,//体术
        弹幕,//弹幕
        技能,//技能
        法术,//法术
        防御,//防御
        符卡//符卡
    }

    #region 基本属性
    //展示用卡牌的游戏物体
    private GameObject showGo;

    //是否是用于展示的卡牌
    public bool isShowCard;
    //卡牌数据
    public CardData cardData;
    //效果字典
    public Dictionary<Value.ValueType, int> valueDic = new Dictionary<Value.ValueType, int>();
    #endregion

    #region UI引用
    //卡名文本
    public Text nameText;
    //卡牌描述文本
    public Text desText;
    //能量消耗文本
    public Text costText;
    //卡图
    public Image img;
    #endregion
    
    private void OnMouseDown()
    {
        if (isShowCard)//如果是展示用的卡牌则不进行检测
            return;
        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
        {
            return;
        }
        showGo.SetActive(false);//取消卡牌展示
        CardManager.Instance.hasShow = false;

        CardManager.Instance.selectedCard = this;
    }

    private void OnMouseEnter()
    {
        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
        {
            return;
        }
        if (CardManager.Instance.hasShow)
        {
            return;
        }
        if (isShowCard)//如果是展示用的卡牌则不进行检测
        {
            return;
        }
        showGo.SetActive(true);//展示卡牌
        showGo.GetComponent<Card>().InitCard(cardData);
        CardManager.Instance.hasShow = true;
    }

    private void OnMouseExit()
    {
        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
        {
            return;
        }
        showGo.SetActive(false);
        CardManager.Instance.hasShow = false;

    }

    private void OnMouseUp()
    {
        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
        {
            return;
        }
        if (isShowCard)//如果是展示用的卡牌则不进行检测
            return;
        if (CardManager.Instance.selectedCard==null)
        {
            return;
        }
        if (Input.mousePosition.y>-0.5)//当鼠标拖拽卡牌到一定位置以上才算进行了卡牌的使用
        {
            CardEffectManager.Instance.UseThisCard(CardManager.Instance.selectedCard);
        }
        else
        {
            CardManager.Instance.selectedCard = null;

        }
    }

    public void InitCard(CardData data)//根据CardData初始化卡牌
    {
        valueDic = new Dictionary<Value.ValueType, int>();
        cardData = data;
        costText.text = ""+cardData.cost;
        nameText.text = cardData.name;
        img.sprite = CardManager.Instance.spriteList[cardData.spriteID];

        foreach (var v in cardData.valueList) //初始化卡牌效果字典
        {
            valueDic.Add(v.type, v.value);
        }

        cardData.des = "";
        InitDes();//初始化文本描述
        desText.text = cardData.des;

    }

    public void InitDes()//根据卡牌效果字典初始化描述文本
    {
        if (valueDic.ContainsKey(Value.ValueType.伤害))//如果有DamageKEY的情况
        {
            cardData.des += "造成"+valueDic[Value.ValueType.伤害]+"点伤害";
            if (cardData.times>1)
            {
                cardData.des += cardData.times + "次";
            }
        }
        if (valueDic.ContainsKey(Value.ValueType.护甲))//如果有ShieldKEY的情况
        {
            if (cardData.des!=null)
            {
                cardData.des += "\n";
            }
            cardData.des += "获得" + valueDic[Value.ValueType.护甲] + "点护甲";
            if (cardData.times > 1)
            {
                cardData.des += cardData.times + "次";
            }
        }
    }
    private void Start()
    {
        showGo = CardManager.Instance.showCard;
    }

    
}
