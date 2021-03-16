using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum CardType//��������
    {
        Attack,
        Skill,
        Power
    }

    #region ��������
    //չʾ�ÿ��Ƶ���Ϸ����
    private GameObject showGo;
    //��������
    public CardType type;
    //��������
    public int cardCost;
    //�������
    public string cardID;
    //�Ƿ���Ҫ����ʹ��Ŀ��
    public bool needTarget;
    //�Ƿ�������չʾ�Ŀ���
    public bool isShowCard;
    //Ч���ֵ�
    public Dictionary<Value.ValueType, int> valueDic = new Dictionary<Value.ValueType, int>();
    #endregion

    #region UI����
    //�����ı�
    public Text nameText;
    //���������ı�
    public Text desText;
    //���������ı�
    public Text costText;
    //��ͼ
    public Image img;
    #endregion
    
    private void OnMouseDown()
    {
        if (isShowCard)//�����չʾ�õĿ����򲻽��м��
            return;
        showGo.SetActive(false);

        CardManager.Instance.selectedCard = this;
    }

    private void OnMouseEnter()
    {
        if (isShowCard)//�����չʾ�õĿ����򲻽��м��
        {
            return;
        }
        showGo.SetActive(true);//չʾ����
        foreach (var data in CardManager.Instance.cardDataList)//����ѡ�п��Ƶ�ID��ʼ��չʾ�ÿ���
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
        if (isShowCard)//�����չʾ�õĿ����򲻽��м��
            return;
        if (CardManager.Instance.selectedCard==null)
        {
            return;
        }
        if (Input.mousePosition.y>-0.5)//�������ק���Ƶ�һ��λ�����ϲ�������˿��Ƶ�ʹ��
        {
            CardEffectManager.Instance.UseThisCard(CardManager.Instance.selectedCard);
        }
        else
        {
            CardManager.Instance.selectedCard = null;

        }
    }

    public void InitCard(CardData data)//����CardData��ʼ������
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
