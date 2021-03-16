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



    public CardType type;
    public int cardCost;
    public string cardID;
    public Text nameText;
    public Text desText;
    public Text costText;
    public Image img;
    public bool needTarget;

    public Dictionary<Value.ValueType, int> valueDic=new Dictionary<Value.ValueType, int>();//Ð§¹û×Öµä
    private void OnMouseDown()
    {
        CardManager.Instance.selectedCard = this;
    }
    private void OnMouseUp()
    {
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
    }

    
}
