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
        Attack,
        Skill,
        Power
    }

    #region 基本属性
    //展示用卡牌的游戏物体
    private GameObject showGo;

    //是否是用于展示的卡牌
    public bool isShowCard;

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
        showGo.SetActive(false);

        CardManager.Instance.selectedCard = this;
    }

    private void OnMouseEnter()
    {
        if (isShowCard)//如果是展示用的卡牌则不进行检测
        {
            return;
        }
        showGo.SetActive(true);//展示卡牌
        showGo.GetComponent<Card>().InitCard(cardData);

    }

    private void OnMouseExit()
    {
        showGo.SetActive(false);
    }

    private void OnMouseUp()
    {
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
        desText.text = cardData.des;
        img.sprite = CardManager.Instance.spriteList[cardData.spriteID];
        
        foreach (var v in cardData.valueList)
        {
            valueDic.Add(v.type,v.value);
        }
    }
    private void Start()
    {
        showGo = CardManager.Instance.showCard;
    }

    
}
