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
        ����,//����
        ��Ļ,//��Ļ
        ����,//����
        ����,//����
        ����,//����
        ����//����
    }

    #region ��������
    //չʾ�ÿ��Ƶ���Ϸ����
    private GameObject showGo;

    //�Ƿ�������չʾ�Ŀ���
    public bool isShowCard;
    //��������
    public CardData cardData;
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
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }
        showGo.SetActive(false);//ȡ������չʾ
        CardManager.Instance.hasShow = false;

        CardManager.Instance.selectedCard = this;
    }

    private void OnMouseEnter()
    {
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }
        if (CardManager.Instance.hasShow)
        {
            return;
        }
        if (isShowCard)//�����չʾ�õĿ����򲻽��м��
        {
            return;
        }
        showGo.SetActive(true);//չʾ����
        showGo.GetComponent<Card>().InitCard(cardData);
        CardManager.Instance.hasShow = true;
    }

    private void OnMouseExit()
    {
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }
        showGo.SetActive(false);
        CardManager.Instance.hasShow = false;

    }

    private void OnMouseUp()
    {
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }
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
        cardData = data;
        costText.text = ""+cardData.cost;
        nameText.text = cardData.name;
        img.sprite = CardManager.Instance.spriteList[cardData.spriteID];

        foreach (var v in cardData.valueList) //��ʼ������Ч���ֵ�
        {
            valueDic.Add(v.type, v.value);
        }

        cardData.des = "";
        InitDes();//��ʼ���ı�����
        desText.text = cardData.des;

    }

    public void InitDes()//���ݿ���Ч���ֵ��ʼ�������ı�
    {
        if (valueDic.ContainsKey(Value.ValueType.�˺�))//�����DamageKEY�����
        {
            cardData.des += "���"+valueDic[Value.ValueType.�˺�]+"���˺�";
            if (cardData.times>1)
            {
                cardData.des += cardData.times + "��";
            }
        }
        if (valueDic.ContainsKey(Value.ValueType.����))//�����ShieldKEY�����
        {
            if (cardData.des!=null)
            {
                cardData.des += "\n";
            }
            cardData.des += "���" + valueDic[Value.ValueType.����] + "�㻤��";
            if (cardData.times > 1)
            {
                cardData.des += cardData.times + "��";
            }
        }
    }
    private void Start()
    {
        showGo = CardManager.Instance.showCard;
    }

    
}
