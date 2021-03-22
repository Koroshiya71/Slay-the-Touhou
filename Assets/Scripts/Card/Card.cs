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

    public Transform outLook;//外观的位置
    #region 基本属性
    //展示用卡牌的游戏物体
    private GameObject showGo;
    //是否已经满足残心条件
    public bool canXin;
    //是否已经使用过了
    public bool hasUsed;
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

    public Vector3 selectPos;
    public void OnPointerDown()
    {

        if (CardManager.Instance.isChoosing)//如果正在选择卡牌
        {
            CardManager.Instance.chosenCardList.Add(this.cardData);       
        }
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

    public void OnPointerEnter()
    {
        Debug.Log("Enter" );
        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
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
        outLook.transform.position = new Vector3(outLook.transform.position.x, outLook.transform.position.y + 0.5f);

    }

    public void OnPointerExit()
    {
        Debug.Log("exit");

        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
        {
            return;
        }


        showGo.SetActive(false);
        CardManager.Instance.hasShow = false;
        outLook.transform.position = new Vector3(outLook.transform.position.x, outLook.transform.position.y - 0.5f);
    }

    public  void OnPointerUp()
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
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -2)//当鼠标拖拽卡牌到一定位置以上才算进行了卡牌的使用
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
        cardData = CardData.Clone(data);
        valueDic = new Dictionary<Value.ValueType, int>();
        canXin = false;
        hasUsed = false;
        cardData.keepChangeInBattle = false;
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

    public void UpdateCardState() //更新卡牌状态
    {
        switch (cardData.cardID)
        {
            case "0004"://二刀的心得
                if (BattleManager.Instance.hasCanXin)
                {
                    cardData.cost -= 1;
                    InitCard(cardData);
                }
                break;
        }
    }
    public void InitDes()//根据卡牌效果字典初始化描述文本
    {
        switch (cardData.cardID)
        {
            case "0004"://二刀的心得
                cardData.des = "获得一层二刀的心得，如果上回合触发过残心，费用-1";
                return;
            case "0008"://紫电一闪
                cardData.des = "本场对战中，你获得流转状态（你的残心效果改为立即触发，而不是回合结束时触发）\n无何有";
                return;
            case "0009"://狱界剑「二百由旬之一闪」
                cardData.des = "获得一个额外的回合。在额外的回合，只能使用体术牌。\n无何有";
                break;
        }
        
        //如果有伤害KEY的情况
        if (valueDic.ContainsKey(Value.ValueType.伤害))
        {
            cardData.des += "造成"+valueDic[Value.ValueType.伤害]+"点伤害";
            if (cardData.times>1)
            {
                cardData.des += cardData.times + "次";
            }
        }
        //如果有护甲KEY的情况
        if (valueDic.ContainsKey(Value.ValueType.护甲))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "获得" + valueDic[Value.ValueType.护甲] + "点护甲";
            if (cardData.times > 1)
            {
                cardData.des += cardData.times + "次";
            }
        }
        //如果有二刀流Key的情况下
        if (valueDic.ContainsKey(Value.ValueType.二刀流))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "本回合获得二刀流状态";
        }
        //如果有残心的情况下
        if (cardData.canXinList.Count>0)
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }

            cardData.des += "残心：";
            foreach (var canXin in cardData.canXinList)
            {
                if (!canXin.IsTurnEnd&& cardData.canXinList.IndexOf(canXin) == 0)
                {
                    cardData.des += "在下个回合开始时,";
                }
                if (cardData.canXinList.IndexOf(canXin) > 0)
                {
                    cardData.des += ",";
                }
                switch (canXin.CanXinValue.type)
                {
                    case Value.ValueType.伤害:
                        cardData.des += "造成"+canXin.CanXinValue.value+"点伤害";
                        break;
                    case Value.ValueType.护甲:
                        cardData.des += "获得" + canXin.CanXinValue.value + "点护甲";
                        break;
                    case Value.ValueType.回费:
                        cardData.des += "获得" + canXin.CanXinValue.value + "点能量";
                        break;
                }
            }
            
        }
        //如果有连斩的情况下
        if (cardData.comboList.Count > 0)
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }

            foreach (var combo in cardData.comboList)
            {
                cardData.des += "连斩"+combo.comboNum+"：";
                switch (combo.comboValue.type)
                {
                    case Value.ValueType.伤害:
                        cardData.des += "造成" + combo.comboValue.value + "点伤害";
                        break;
                    case Value.ValueType.护甲:
                        cardData.des += "获得" + combo.comboValue.value + "点护甲";
                        break;
                    case Value.ValueType.回费:
                        cardData.des += "获得" + combo.comboValue.value + "点能量";
                        break;
                }
            }
        }
    }
    private void Start()
    {
        showGo = CardManager.Instance.showCard;
    }

    
}
