using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour//用来管理一系列UI事件
{
    public static MenuManager Instance;
    public List<Card> showCardList;//用于显示的卡牌列表
    public GameObject cardDisplayCanvas;//用来预览牌堆情况的画布
    public RectTransform displayContent;//滑动菜单的内容范围
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
        Card[] showCards = GetComponentsInChildren<Card>();
        foreach (var card in showCards)
        {
            showCardList.Add(card);//将所有预制卡牌加入管理列表
        }
    }
    public void ShowDeskButtonDown() //查看牌库按钮按下的回调事件
    {
        cardDisplayCanvas.SetActive(true);

    }
    public void ShowDrawCardButtonDown() //查看抽牌堆按钮按下的回调事件
    {
        cardDisplayCanvas.SetActive(true);
        displayContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,//根据抽牌堆的卡牌数量动态调整content大小
            340+(CardManager.Instance.drawCardList.Count%5-2)*160);
        
    }
    public void ShowDiscardButtonDown() //查看弃牌堆堆按钮按下的回调事件
    {
        cardDisplayCanvas.SetActive(true);

    }

    public void NotShowButtonDown() //取消卡牌预览按钮按下的回调事件
    {

    }

    void Update()
    {
        
    }
}
