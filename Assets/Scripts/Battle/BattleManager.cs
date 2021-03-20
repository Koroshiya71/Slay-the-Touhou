using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public bool turnHasEnd;
    public List<Action> actionsEndTurn=new List<Action>();//�غϽ���ʱ������Ч��
    public List<Action> actionsTurnStart=new List<Action>();//�غϿ�ʼʱ������Ч��
    public bool hasCanXin;//�ϻغ��Ƿ񴥷�������
    public int effectTimes;//����Ч�������Ĵ���
    public int cardCombo;//���غ�ʹ�õĿ�������
    public void TurnEnd()
    {
        if (turnHasEnd)//����غ��ѽ����ڽ���������������ʱ�������
        {
            return;
        }
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }

        foreach (var state in Player.Instance.stateList)
        {
            if (state.type==Value.ValueType.������)
            {
                state.value -= 1;
            }
        }
        StateManager.CleanPlayerState();//���Player���ϵ���Ч״̬
        foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
        {
            enemy.shield = 0;
            enemy.TakeAction();
        }
        hasCanXin = false;
        if (actionsEndTurn != null)
        {
            foreach (var action in actionsEndTurn)
            {
                action();
            }

            actionsEndTurn = new List<Action>();
        }

        Invoke(nameof(TurnStart), 1);
        CardManager.Instance.DropAllCard();
        turnHasEnd = true;
        cardCombo = 0;
    }

    public void TurnStart()
    {
        
        turnHasEnd = false;
        Player.Instance.InitEnergy();
        Player.Instance.shield = 0;
        
        for (int i = 0; i < Player.Instance.drawCardNum; i++)
        {
            CardManager.Instance.DrawCard();
        }

        if (actionsTurnStart!=null)
        {
            foreach (var action in actionsTurnStart)
            {
                action();
            }

            actionsTurnStart = new List<Action>();
        }
        foreach (var cardObject in CardManager.Instance.handCardList)
        {
            Card card=cardObject.GetComponent<Card>();
            card.UpdateCardState();
        }
    }
    void Start()
    {
        Instance = this;
        CardManager.Instance.InitDrawCardList();
        TurnStart();
    }

    void Update() 
    {
        
    }
}
