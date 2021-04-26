using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuEventManager : MonoBehaviour//用来管理一系列UI事件
{
    public static MenuEventManager Instance;
    public List<Card> showCardList;//用于显示的卡牌列表
    public List<Card> showSpellCardList;//用于显示符卡的卡牌列表
    public List<Card> chooseCardList;//用于选卡的卡牌列表
    public GameObject cardDisplayView;//用来预览牌堆情况的视窗
    public GameObject spellCardDisplayView;//用来预览符卡堆情况的视窗
    public GameObject cardChooseView;//用来选卡的视窗
    public RectTransform displayContent;//滑动菜单的内容范围
    public RectTransform spellCardContent;//滑动菜单的内容范围
    public Canvas cardPreviewCanvas;//用于展示卡牌的画布
    public bool isPreviewing;//是否正在显示卡牌
    public Text stateExplanationText;//状态说明文本
    public Text actionExplanationText;//敌人的行动意图说明文本
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InitShowCardCanvas();
        stateExplanationText.enabled = false;
    }

    void InitShowCardCanvas() //对卡牌预览界面进行初始化
    {
        //取消显示
        cardDisplayView.SetActive(false);
        spellCardDisplayView.SetActive(false);
        cardPreviewCanvas.enabled = false;
        cardChooseView.SetActive(false);
        Card[] showCards = cardDisplayView.GetComponentsInChildren<Card>();
        foreach (var card in showCards)
        {
            showCardList.Add(card);//将所有预制卡牌加入管理列表
        }

    }

    
    public void ChooseCardFromDesk()//从手牌外的范围进行卡牌选择
    {
        CardManager.Instance.isChoosingFromHand = true;
        cardDisplayView.SetActive(true);
        List<CardData> optionalList= CardManager.Instance.optionalCardList;
        for (int i = 0; i < optionalList.Count; i++)
        {
            showCardList[i].gameObject.SetActive(true);//将等同于抽牌堆数量的展示卡牌初始化并显示出来
            showCardList[i].InitCard(optionalList[i]);
        }
        for (int i = optionalList.Count; i < showCardList.Count; i++)//将其他卡牌隐藏显示
        {
            showCardList[i].gameObject.SetActive(false);
        }
    }
    public void ExitDisplayButtonDown() //取消预览卡牌按钮的回调事件
    {
        cardPreviewCanvas.enabled = false;
        isPreviewing = false;
        cardDisplayView.SetActive(false);
        spellCardDisplayView.SetActive(false);
    }
    public void ShowDeskButtonDown() //查看牌库按钮按下的回调事件
    {
        isPreviewing = true;
        cardPreviewCanvas.enabled = true;
        List<string> deskList = CardManager.Instance.cardDeskList;
        cardDisplayView.SetActive(true);
        displayContent.sizeDelta = new Vector2(1835, 939 + (deskList.Count / 5 - 1) * 420);
        for (int i = 0; i < deskList.Count; i++)
        {
            showCardList[i].gameObject.SetActive(true);//将等同于弃牌堆数量的展示卡牌初始化并显示出来
            foreach (var data in CardManager.Instance.CardDataList)
            {
                if (data.cardID == deskList[i])
                {
                    showCardList[i].InitCard(data);
                    break;
                }
            }
        }

        for (int i = deskList.Count; i < showCardList.Count; i++)//将其他卡牌隐藏显示
        {
            showCardList[i].gameObject.SetActive(false);
        }

    }
    public void ShowDrawCardButtonDown() //查看抽牌堆按钮按下的回调事件
    {
        isPreviewing = true;
        cardPreviewCanvas.enabled = true;

        List<CardData> drawCardList = CardManager.Instance.RandomSortList(CardManager.Instance.drawCardList);
        cardDisplayView.SetActive(true);
        displayContent.sizeDelta = new Vector2(1835, 939 + (drawCardList.Count / 5 - 1) * 420);
        for (int i = 0; i < drawCardList.Count; i++)
        {
            showCardList[i].gameObject.SetActive(true);//将等同于抽牌堆数量的展示卡牌初始化并显示出来
            showCardList[i].InitCard(drawCardList[i]);
        }

        for (int i = drawCardList.Count; i < showCardList.Count; i++)//将其他卡牌隐藏显示
        {
            showCardList[i].gameObject.SetActive(false);
        }
        
    }
    public void ShowDiscardButtonDown() //查看弃牌堆堆按钮按下的回调事件
    {
        isPreviewing = true;
        cardPreviewCanvas.enabled = true;

        List<CardData> discardList = CardManager.Instance.discardList;
        cardDisplayView.SetActive(true);
        displayContent.sizeDelta=new Vector2(1835, 939 + (discardList.Count / 5 - 2) * 420);
       
        for (int i = 0; i < discardList.Count; i++)
        {
            showCardList[i].gameObject.SetActive(true);//将等同于弃牌堆数量的展示卡牌初始化并显示出来
            showCardList[i].InitCard(discardList[i]);
        }

        for (int i = discardList.Count; i < showCardList.Count; i++)//将其他卡牌隐藏显示
        {
            showCardList[i].gameObject.SetActive(false);
        }

    }

    public void ShowSpellCardDeskButtonDown() //查看符卡列表按下的回调事件
    {
        isPreviewing = true;
        cardPreviewCanvas.enabled = true;
        spellCardDisplayView.SetActive(true);
        List<CardData> spellCardList = CardManager.Instance.spellCardList;
        spellCardContent.sizeDelta = new Vector2(1835+ (spellCardList.Count-3)*600, 939);
        for (int i = 0; i < spellCardList.Count; i++)
        {
            showSpellCardList[i].gameObject.SetActive(true);//将等同于弃牌堆数量的展示卡牌初始化并显示出来
            showSpellCardList[i].InitCard(spellCardList[i]);
        }
        for (int i = spellCardList.Count; i < showSpellCardList.Count; i++)//将其他卡牌隐藏显示
        {
            showSpellCardList[i].gameObject.SetActive(false);
        }

    }

    public void ReturnToMapButtonDown() //返回按钮回调事件
    {
        SceneManager.Instance.battleSceneCanvas.enabled = false;
        SceneManager.Instance.mapSceneCanvas.enabled = true;
        BattleManager.Instance.statisticImage.SetActive(false);
    }

    

    public void InitStateExplanationText() //初始化状态描述
    {
        stateExplanationText.text = "";
        switch (hideFlags)
        {
            
        }
    }
    void Update()
    {
        
    }
}
