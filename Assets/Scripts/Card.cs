using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum CardType
    {
        Attack,
        Skill,
        Power
    }


    private GameObject showGo ;
    public CardType type;
    public int cardCost;
    public string cardID;
    public Text nameText;
    public Text desText;
    public Text costText;
    public Image img;
    public bool needTarget;
    public bool isShowCard;//是否是用于展示的卡牌
    public Dictionary<Value.ValueType, int> valueDic=new Dictionary<Value.ValueType, int>();//效果字典
    private void OnMouseDown()
    {
        if (isShowCard)//如果是展示用的卡牌则不进行检测
            return;
        CardManager.Instance.selectedCard = this;
    }

    private void OnMouseEnter()
    {
        
        showGo.SetActive(true);
        foreach (var data in CardManager.Instance.cardDataList)
        {
            if (data.cardID==cardID)
            {
                showGo.GetComponent<Card>().InitCard(data);
                break;
            }
        }
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
        if (Input.mousePosition.y>-0.5)
        {
            CardEffectManager.Instance.UseThisCard(CardManager.Instance.selectedCard);
        }
        else
        {
            CardManager.Instance.selectedCard = null;

        }
    }

    public void InitCard(CardData data)
    {
        valueDic = new Dictionary<Value.ValueType, int>();
        type = data.type;
        cardID = data.cardID;
        desText.text = data.des;
        nameText.text = data.name;
        costText.text = ""+data.cost;
        cardCost = data.cost;
        img.sprite = CardManager.Instance.spriteList[data.spriteID];
        needTarget = data.needTarget;
        foreach (var v in data.valueList)
        {
            valueDic.Add(v.type,v.value);
        }
    }
    private void Start()
    {
        showGo = CardManager.Instance.showCard;
    }

    
}
