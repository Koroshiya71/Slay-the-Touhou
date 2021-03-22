using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEventManager : MonoBehaviour//用来管理一系列UI事件
{
    public static MenuEventManager Instance;
    public List<Card> showCardList;//用于显示的卡牌列表
    public GameObject cardDisplayCanvas;//用来预览牌堆情况的画布
    public RectTransform displayContent;//滑动菜单的内容范围
    public bool isPreviewing;//是否正在显示卡牌

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InitShowCardCanvas();
    }

    void InitShowCardCanvas() //对卡牌预览界面进行初始化
    {
        cardDisplayCanvas.SetActive(false);//取消显示
        Card[] showCards = cardDisplayCanvas.GetComponentsInChildren<Card>();
        foreach (var card in showCards)
        {
            showCardList.Add(card);//将所有预制卡牌加入管理列表
        }

    }

    
    public void ChooseCardFromDesk()//从手牌外的范围进行卡牌选择
    {
        CardManager.Instance.isChoosing = true;
        cardDisplayCanvas.SetActive(true);
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
        isPreviewing = false;
        cardDisplayCanvas.SetActive(false);
    }
    public void ShowDeskButtonDown() //查看牌库按钮按下的回调事件
    {
        isPreviewing = true;

        List<string> deskList = CardManager.Instance.cardDeskList;
        cardDisplayCanvas.SetActive(true);
        displayContent.sizeDelta = new Vector2(1835, 939 + (deskList.Count / 5 - 1) * 420);
        for (int i = 0; i < deskList.Count; i++)
        {
            showCardList[i].gameObject.SetActive(true);//将等同于弃牌堆数量的展示卡牌初始化并显示出来
            foreach (var data in CardManager.Instance.cardDataList)
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

        List<CardData> drawCardList = CardManager.Instance.RandomSortList(CardManager.Instance.drawCardList);
        cardDisplayCanvas.SetActive(true);
        displayContent.sizeDelta = new Vector2(1835, 939 + (drawCardList.Count / 5 - 2) * 420);
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

        List<CardData> discardList = CardManager.Instance.discardList;
        cardDisplayCanvas.SetActive(true);
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

    public void NotShowButtonDown() //取消卡牌预览按钮按下的回调事件
    {

    }

    void Update()
    {
        
    }
}
