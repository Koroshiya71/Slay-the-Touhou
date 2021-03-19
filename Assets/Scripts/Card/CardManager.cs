using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
    public Card selectedCard;//选中的卡牌
    public GameObject cardPrefab; //手牌预制体
    public GameObject showCard;//用来展示的卡牌
    #region 各种牌库
    public List<CardData> discardList=new List<CardData>();//弃牌堆
    public List<CardData> drawCardList = new List<CardData>();//抽牌堆
    public List<string> cardDeskList=new List<string>();//牌库
    public List<GameObject> handCardList = new List<GameObject>(); //手牌列表
#endregion

    public GameObject BeginPos; //生成手牌的最开始的位
    private float rotateAngel; //手牌动画旋转的角度
    public List<Sprite> spriteList=new List<Sprite>();
    public List<CardData> cardDataList=new List<CardData>();
    public bool hasShow;//是否已经展示了卡牌
    public Text discardNumText;//弃牌堆卡牌数量的文本
    public Text drawCardNumText;//抽牌堆卡牌数量的文本
    private void Start()
    {
        showCard.SetActive(false);
        cardDataList = DataManager.Instance.LoadCardData();
    }
    
    public void InitDrawCardList()
    {
        if (discardList.Count>0)//如果是在弃牌堆有卡牌的情况下初始化抽牌堆，则将所有弃牌堆的卡牌加入抽牌堆
        {
            foreach (var dCard in discardList)
            {
                drawCardList.Add(dCard);
            }

            discardList = new List<CardData>();//然后初始化弃牌堆
        }
        
        if (drawCardList.Count==0)//如果此时抽牌堆依然为空，那么说明是第一次从牌库初始化抽牌堆
        {
            foreach (var cardId in cardDeskList)//然后根据牌库中每张牌的ID号查找数据
            {
                foreach (var cardData in cardDataList)
                {
                    if (cardId==cardData.cardID)//查找对应ID的卡牌数据
                    {
                        drawCardList.Add(cardData);
                        break;
                    }

                }
                

            }
        }

        drawCardList=RandomSortList(drawCardList);//随机打乱抽牌堆顺序

    }
    public List<T> RandomSortList<T>(List<T> ListT)//对列表进行随机排序
    {
        System.Random random = new System.Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }
    
    private void Update()
    {
        UpdateUIState();
    }

    public void DrawCard() //从抽牌堆抽卡
    {
        //TODO 后续优化对象池
        //克隆预设
        var handCardGo = Instantiate(cardPrefab) as GameObject;
        if (drawCardList.Count==0)//如果无牌可抽，则初始化抽牌堆
        {
            InitDrawCardList();
        }
        var newCard = handCardGo.GetComponent<Card>();
        newCard.InitCard(drawCardList[0]);//根据抽牌堆的第一张卡牌数据来初始化卡牌
        drawCardList.Remove(drawCardList[0]);//将卡牌移除抽牌堆

        handCardGo.transform.position = BeginPos.transform.position;
        handCardGo.transform.SetParent(transform);

        //将新手牌添加到手牌列表
        handCardList.Add(handCardGo);

        //计算动画需要旋转的角度
        RotateAngel();
        //播放抽卡动画
        AddCardAnimations();
    }
    public void GetCards(int cardNo)//根据卡牌的序号获取指定卡牌
    {
        //克隆预设
        var handCardGo = Instantiate(cardPrefab) as GameObject;
        Card newCard = handCardGo.GetComponent<Card>();
        newCard.InitCard(cardDataList[cardNo]);

        handCardGo.transform.position = BeginPos.transform.position;
        handCardGo.transform.SetParent(transform);

        //将新手牌添加到手牌列表
        handCardList.Add(handCardGo);
        //计算动画需要旋转的角度
        RotateAngel();
    }

    public void UpdateUIState() //更新UI组件状态
    {
        drawCardNumText.text = drawCardList.Count+"";
        discardNumText.text = discardList.Count + "";
    }
    //为手牌添加动画
    private void HandCardAnimation(GameObject GO, float Vec3_Z)
    {
        GO.transform.DORotate(new Vector3(0, 0, Vec3_Z), 0.3F, RotateMode.Fast);
    }

    //增加手牌时播放的动画
    private void AddCardAnimations()
    {
        if (handCardList.Count == 1)
        {
            HandCardAnimation(handCardList[0], 0);
        }
        else
        {
            for (var i = 1; i < handCardList.Count; i++)
                HandCardAnimation(handCardList[i - 1], 30 - rotateAngel * (float) i * handCardList.Count + 2.5F);
            HandCardAnimation(handCardList[handCardList.Count - 1], -27.5F + rotateAngel);
        }
    }

    //使用手牌时播放的动画
    public void UseCard()
    {
        int index =handCardList.IndexOf(selectedCard.gameObject);

        if (handCardList[index]!=null)
        {
            Destroy(handCardList[index]);
            handCardList.Remove(handCardList[index]);
            RotateAngel();
            if (handCardList.Count == 1)
                HandCardAnimation(handCardList[0], 0);
            else if (handCardList.Count > 1)
                for (var i = 1; i < handCardList.Count + 1; i++)
                {
                    HandCardAnimation(handCardList[i - 1], 30 - rotateAngel * (float)i * handCardList.Count + 2.5F);
                    HandCardAnimation(handCardList[handCardList.Count - 1], -27.5F + rotateAngel);
                }
        }
        
    }

    public void DropAllCard() //丢弃所有手牌
    {
        for (int i = 0; i < handCardList.Count; i++)
        {
            Discard(handCardList[i].GetComponent<Card>());
            Destroy(handCardList[i]);
            
            //TODO:制作和播放弃牌动画
        }

        handCardList = new List<GameObject>();


    }
    //计算需要旋转的角度
    private void RotateAngel()
    {
        rotateAngel = 55F / (float) handCardList.Count / (float) handCardList.Count;
    }

    public void Discard(Card card) //弃牌
    {
        discardList.Add(card.cardData);

    }
    private void Awake()
    {
        Instance = this;
    }
}