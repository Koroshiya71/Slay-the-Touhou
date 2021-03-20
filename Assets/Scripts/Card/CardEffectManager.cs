using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance;//单例
    public Enemy targetEnemy;//卡牌的
    public int attackTimes;//体术的攻击次数
    private void Awake()
    {
        Instance = this;
    }
    public void UseThisCard(Card card)//触发卡牌效果
    {
        
        switch (card.cardData.cardID)//根据卡牌的ID触发效果
        {
            case "0001"://斩击
                TiShu(card,1);
                break;
            case "0002"://格挡
                Defend(card,1);
                break;
            case "0003"://现世斩
                TiShu(card,1);
                break;
            case "0004"://二刀的心得
                Skill(card,1);
                break;
            case "0005"://残月斩
                TiShu(card,1);
                if (Player.Instance.energy==0)
                {
                    BattleManager.Instance.actionsTurnStart.Add((() =>
                    {
                        Player.Instance.GetEnergy(1);
                        BattleManager.Instance.hasCanXin = true;
                    }));
                }
                
                break;
        }
    }

    void TiShu(Card card,int times)//体术卡的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd)//如果费用不够则使用失败
        {
            return;
        }
        if (card.cardData.needTarget)
        {
            if (targetEnemy==null)//如果需要目标但是目标敌人为空时使用失败
            {
                return;
            }
        }
        Player.Instance.PlayAttackAnim();//播放攻击动画

        if (Player.Instance.DoubleBlade())//如果有二刀流的情况
        {
            for (int i = 0; i < card.cardData.times*2; i++)//进行多次攻击
            {
                targetEnemy.TakeDamage(card.valueDic[Value.ValueType.伤害]);
            }
        }

        else
        {
            for (int i = 0; i < card.cardData.times; i++) //进行多次攻击
            {
                targetEnemy.TakeDamage(card.valueDic[Value.ValueType.伤害]);
            }
        }

        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);

    }

    void Skill(Card card, int times) //技能卡的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd)//如果费用不够则使用失败
        {
            return;
        }
        if (card.cardData.needTarget)
        {
            if (targetEnemy == null)//如果需要目标但是目标敌人为空时使用失败
            {
                return;
            }
        }

        foreach (var value in card.valueDic)
        {
            Value newValue = new Value();
            newValue.type = value.Key;
            newValue.value = value.Value;
            switch (value.Key)
            {
                case Value.ValueType.二刀流:
                    StateManager.AddStateToPlayer(newValue);
                    break;
            }
        }
        
        Player.Instance.PlayAttackAnim();//播放攻击动画
        CardManager.Instance.UseCard();//使用卡牌
        Player.Instance.energy -= card.cardData.cost;//消耗费用
        CardManager.Instance.Discard(card);//弃牌
    }
    void Defend(Card card,int times) //防御卡的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd)//如果费用不够则使用失败
        {
            return;
        }
        if (card.cardData.needTarget)
        {
            if (targetEnemy == null)//如果需要目标但是目标敌人为空时使用失败
            {
                return;
            }
        }
        Player.Instance.PlayAttackAnim();//播放攻击动画
        for (int i = 0; i < card.cardData.times; i++)//获得护盾times次
        {
            Player.Instance.GetShield(card.valueDic[Value.ValueType.护甲]);
        }
        CardManager.Instance.UseCard();//使用卡牌
        Player.Instance.energy -= card.cardData.cost;//消耗费用
        CardManager.Instance.Discard(card);//弃牌
    }
}
