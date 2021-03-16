using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance;//单例
    public Enemy targetEnemy;//卡牌的
    private void Awake()
    {
        Instance = this;
    }
    public void UseThisCard(Card card)//触发卡牌效果
    {
        if (Player.Instance.energy<card.cardCost||BattleManager.Instance.turnHasEnd)
        {
            return;
        }
        switch (card.cardID)//根据卡牌的ID触发效果
        {
            case "0001"://斩击
                Attack(card,1);
                break;
            case "0002"://格挡
                Skill(card,1);
                break;
            case "0003"://现世斩
                Attack(card,2);
                break;
        }
    }

    void Attack(Card card,int times)//攻击卡的通用方法：卡牌对象;进行几次伤害
    {
        if (card.needTarget)
        {
            if (targetEnemy==null)//如果需要目标但是目标敌人为空时使用失败
            {
                return;
            }
        }
        Player.Instance.PlayAttackAnim();//播放攻击动画
        for (int i = 0; i < times; i++)//进行多次攻击
        {
            targetEnemy.TakeDamage(card.valueDic[Value.ValueType.Damage]);
        }
        CardManager.Instance.UseCard();
        
        Player.Instance.energy -= card.cardCost;
        CardManager.Instance.Discard(card);

    }

    void Skill(Card card, int times) //技能卡的通用方法
    {
        if (card.needTarget)
        {
            if (targetEnemy == null)//如果需要目标但是目标敌人为空时使用失败
            {
                return;
            }
        }
        Player.Instance.PlayAttackAnim();//播放攻击动画
        for (int i = 0; i < times; i++)//攻击times次
        {
            Player.Instance.GetShield(card.valueDic[Value.ValueType.Shield]);
        }
        CardManager.Instance.UseCard();//使用卡牌
        Player.Instance.energy -= card.cardCost;//消耗费用
        CardManager.Instance.Discard(card);//弃牌

    }
}
