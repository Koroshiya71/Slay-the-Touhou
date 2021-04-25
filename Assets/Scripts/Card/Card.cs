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
        符卡,//符卡
        状态//状态
    }

    public Transform outLook;//外观的位置
    #region 基本属性
    //展示用卡牌的游戏物体
    private GameObject showCardGo;
    private GameObject showSpellCardGo;
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
    public float posY;
    public List<Sprite> cardBackGroundSprites = new List<Sprite>();
    public List<Sprite> spellCardBackGroundSprites = new List<Sprite>();
    #endregion
    #region UI引用
    //卡名文本
    public Text nameText;
    //卡牌描述文本
    public Text desText;
    //能量消耗文本
    public Text costText;
    //能量消耗文本
    public Text typeText;
    //卡图
    public Image img;
    //背景框图
    public Image backGround;
    //机制解释文本
    public Text explanationText;
    #endregion

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CardManager.Instance.selectedCard=null;
        }
    }

    public Vector3 selectPos;
    public void OnPointerDown()
    {

        if (isShowCard)//如果是展示用的卡牌则不进行检测
            return;
        if (MenuEventManager.Instance.isPreviewing&&cardData.type==CardType.符卡)//如果是从额外卡组选择符卡
        {
            CardManager.Instance.GetSpellCard(cardData.cardID);
            MenuEventManager.Instance.ExitDisplayButtonDown();
            return;
        }
        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
        {
            return;
        }

        if (CardManager.Instance.isChoosingFromHand)//如果正在从手牌选择卡牌
        {
            var localPosition = outLook.localPosition;
            posY = localPosition.y;
            if (CardManager.Instance.chosenCardList.Contains(this))
            {
                CardManager.Instance.chosenCardList.Remove(this);
                outLook.localPosition = new Vector3(localPosition.x, posY-15 );
                return;

            }
            CardManager.Instance.chosenCardList.Add(this);

            
            localPosition = new Vector3(localPosition.x, posY+15);
            outLook.localPosition = localPosition;
        }

        if (CardManager.Instance.isAddingCard)//如果正在添加卡牌到牌库
        {
            if (CardManager.Instance.chosenCardList.Contains(this))
            {
                CardManager.Instance.chosenCardList.Remove(this);
                return;

            }
            CardManager.Instance.chosenCardList.Add(this);
        }
        
        showCardGo.SetActive(false);//取消卡牌展示
        CardManager.Instance.hasShow = false;
 
        CardManager.Instance.selectedCard = this;
    }

    public void OnPointerEnter()
    {

        if (CardManager.Instance.isChoosingFromHand)//如果正在选择卡牌则不进行检测
        {
            return;
        }
        if (CardManager.Instance.isAddingCard)//如果正在选择卡牌则不进行检测
        {
            return;
        }
        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
        {
            if (isShowCard)
            {
                explanationText.enabled = true;
                explanationText.text = "";
                InitExplanation();
                transform.localScale *= 1.5f;
                explanationText.transform.position = transform.position+new Vector3(2.7f,7.3f,0);

                if (cardData.type == CardType.符卡)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 600, 0);

                }
                else
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 400,0);
            }
            return;
        }
        if (isShowCard)//如果是展示用的卡牌则不进行检测
        {
            return;
        }

        if (cardData.type!=CardType.符卡)
        {
            CardManager.Instance.hasShow = true;
            showCardGo.SetActive(true);//展示卡牌
            showCardGo.GetComponent<Card>().InitCard(cardData);
            var localPosition = outLook.localPosition;
            posY = localPosition.y;
            localPosition = new Vector3(localPosition.x, posY + 15);
            outLook.localPosition = localPosition;
            explanationText.enabled = true;
            explanationText.text = "";
            explanationText.transform.localPosition = transform.localPosition + new Vector3(350.0f, 200.0f, 0.0f);

            InitExplanation();
        }
        else
        {
            CardManager.Instance.hasShow = true;
            showSpellCardGo.SetActive(true);//展示卡牌
            showSpellCardGo.GetComponent<Card>().InitCard(cardData);
            var localPosition = outLook.localPosition;
            posY = localPosition.y;
            localPosition = new Vector3(localPosition.x, posY + 15);
            outLook.localPosition = localPosition;
            explanationText.enabled = true;
            explanationText.text = "";
            explanationText.transform.localPosition = transform.localPosition + new Vector3(350.0f, 200.0f, 0.0f);

            InitExplanation();
        }
    }

    public void InitExplanation()
    {
        if (desText.text.Contains("残心"))
        {
            explanationText.text += "残心：如果使用这张牌后使能量变为0，则在回合结束触发该效果\n";
        }
        if (desText.text.Contains("连斩"))
        {
            explanationText.text += "连斩X：如果当前回合使用的卡牌数大于X（不计算该牌）则触发该效果\n";
        }

        if (desText.text.Contains("二刀流"))
        {
            explanationText.text += "二刀流：体术牌的伤害减半，但是触发两次\n";

        }
        if (desText.text.Contains("流转"))
        {
            explanationText.text += "流转：残心效果从回合结束后改为立刻触发\n";
        }
        if (desText.text.Contains("惊吓"))
        {
            explanationText.text += "惊吓：造成的伤害减少30%\n";

        }
        if (desText.text.Contains("无何有"))
        {
            explanationText.text += "无何有：打出后移除本场战斗\n";
        }
        if (desText.text.Contains("空无"))
        {
            explanationText.text += "空无：如果此卡在手牌中且未被打出，回合结束时将会被移除战斗\n";
        }
    }
    public void OnPointerExit()
    {

        if (CardManager.Instance.isChoosingFromHand)//如果正在选择卡牌则不进行检测
        {
            return;
        }
        if (CardManager.Instance.isAddingCard)//如果正在选择卡牌则不进行检测
        {
            return;
        }
        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
        {
            if (isShowCard)
            {
                transform.localScale /= 1.5f;
                explanationText.enabled = false;

                if (cardData.type == CardType.符卡)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 600, 0);

                }
                else
                {
                  transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y +400,0);

                }
            }
            return;
        }


        if (cardData.type!=CardType.符卡)
        {
            showCardGo.SetActive(false);
            CardManager.Instance.hasShow = false;
            var localPosition = outLook.localPosition;
            localPosition = new Vector3(localPosition.x, posY);
            outLook.localPosition = localPosition;
            explanationText.enabled = false;
        }
        else
        {
            showSpellCardGo.SetActive(false);
            CardManager.Instance.hasShow = false;
            var localPosition = outLook.localPosition;
            localPosition = new Vector3(localPosition.x, posY);
            outLook.localPosition = localPosition;
            explanationText.enabled = false;

        }
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

        if (CardManager.Instance.isAddingCard)//如果正在选择卡牌则不进行检测
        {
            return;
        }
        if (CardManager.Instance.isChoosingFromHand)
        {
            return;
        }
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -2)//当鼠标拖拽卡牌到一定位置以上才算进行了卡牌的使用
        {
            StartCoroutine(CardEffectManager.Instance.UseThisCard(CardManager.Instance.selectedCard));
        }
        else
        {
            CardManager.Instance.selectedCard = null;

        }
    }

    

    public void InitCard(CardData data)//根据CardData初始化卡牌
    {
        if (data.type!=CardType.符卡)
        {
            if (!cardData.keepChangeInBattle) //如果对卡牌的修改不是可持续的
                foreach (var originalData in CardManager.Instance.CardDataList)
                    if (cardData.cardID == originalData.cardID)
                        cardData=CardData.Clone(originalData);
            cardData = CardData.Clone(data);
            valueDic = new Dictionary<Value.ValueType, int>();
            canXin = false;
            hasUsed = false;
            cardData.keepChangeInBattle = false;
            costText.text = ""+cardData.cost;
            nameText.text = cardData.name;
            typeText.text = "妖梦·" + cardData.type;
            img.sprite = CardManager.Instance.spriteList[cardData.spriteID];
            backGround.sprite = cardBackGroundSprites[(int) cardData.rare];
        }
        else
        {
            cardData = CardData.Clone(data);
            valueDic = new Dictionary<Value.ValueType, int>();
            canXin = false;
            hasUsed = false;
            cardData.keepChangeInBattle = false;
            nameText.text = cardData.name;
            img.sprite = CardManager.Instance.spriteList[cardData.spriteID];
            backGround.sprite = spellCardBackGroundSprites[cardData.cost-1];
        }
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
                cardData.des = "获得2层「二刀流」，如果上回合触发过残心，费用-1";
                return;
            case "0008"://紫电一闪
                cardData.des = "本场对战中，你获得「流转」状态（你的残心效果改为立即触发，而不是回合结束时触发）\n无何有";
                return;
            case "0009"://狱界剑「二百由旬之一闪」
                cardData.des = "获得一个额外的回合。在额外的回合，只能使用体术牌。";
                return;
            case "0010"://冥想
                cardData.des = "抽三张牌，然后选择两张牌洗回牌库，那两张牌获得无何有";
                return;
            case "0013"://居合
                cardData.des += "如果该牌为本回合使用的第一张体术牌，造成"+valueDic[Value.ValueType.伤害]+"点伤害";
                cardData.des += "\n残心：造成"+cardData.canXinList[0].CanXinValue.value+"点伤害";
                return;
            case "0018"://六根清净
                cardData.des = "你的所有牌获得无何有，当你的抽牌堆为空时，抽牌改为随机获得一张妖梦牌\n无何有";

                return;
        }

        //如果有伤害KEY的情况
        if (valueDic.ContainsKey(Value.ValueType.伤害))
        {
            if (cardData.targetType==CardData.TargetType.随机敌人)
            {
                cardData.des += "随机";
            }
            if (cardData.targetType == CardData.TargetType.全部敌人)
            {
                cardData.des += "对所有敌人";
            }
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
            cardData.des += "本回合获得「二刀流」状态";
        }
        //如果有抽牌Key的情况下
        if (valueDic.ContainsKey(Value.ValueType.抽牌))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "抽"+valueDic[Value.ValueType.抽牌]+"张牌";
        }
        //如果有回费Key的情况下
        if (valueDic.ContainsKey(Value.ValueType.回费))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "回复" + valueDic[Value.ValueType.回费] + "点能量";
        }
        //如果有背水一战Key的情况下
        if (valueDic.ContainsKey(Value.ValueType.背水一战))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "获得「背水一战」（本场战斗无法再获得护甲）";
        }
        //如果有起势Key的情况下
        if (valueDic.ContainsKey(Value.ValueType.起势))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "获得"+valueDic[Value.ValueType.起势]+"层起势（触发连斩时，效果额外触发一次，消耗一层起势）";
        }
        //如果有保留手牌Key的情况下
        if (valueDic.ContainsKey(Value.ValueType.保留手牌))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "本回合结束时保留你的手牌";
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
                        cardData.des += "再造成"+canXin.CanXinValue.value+"点伤害";
                        break;
                    case Value.ValueType.护甲:
                        cardData.des += "获得" + canXin.CanXinValue.value + "点护甲";
                        break;
                    case Value.ValueType.回费:
                        cardData.des += "获得" + canXin.CanXinValue.value + "点能量";
                        break;
                    case Value.ValueType.回血:
                        cardData.des += "回复" + canXin.CanXinValue.value + "点生命";
                        break;
                    case Value.ValueType.惊吓:
                        cardData.des += "给予" + canXin.CanXinValue.value + "层惊吓";
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
                        cardData.des += "再造成" + combo.comboValue.value + "点伤害";
                        break;
                    case Value.ValueType.护甲:
                        cardData.des += "获得" + combo.comboValue.value + "点护甲";
                        break;
                    case Value.ValueType.回费:
                        cardData.des += "获得" + combo.comboValue.value + "点能量";
                        break;
                    case Value.ValueType.回血:
                        cardData.des += "回复" + combo.comboValue.value + "点生命";
                        break;
                }
            }
        }
        //如果有无何有词条的情况下
        if (valueDic.ContainsKey(Value.ValueType.无何有))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "无何有";
        }

        if (valueDic.ContainsKey(Value.ValueType.击杀回费))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "如果这个单位因此死去，则回复"+valueDic[Value.ValueType.击杀回费]+"点能量";
        }
    }
    private void Start()
    {
        showCardGo = CardManager.Instance.showCard;
        showSpellCardGo = CardManager.Instance.showSpellCard;
        explanationText = CardManager.Instance.explanationText;
    }


}
