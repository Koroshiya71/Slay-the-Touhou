using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance; //单例
    public Enemy targetEnemy; //卡牌的
    public int attackTimes; //体术的攻击次数

    private void Awake()
    {
        Instance = this;
    }

    public void UseThisCard(Card card) //触发卡牌效果
    {
        switch (card.cardData.type) //根据卡牌的类型触发效果
        {
            case Card.CardType.体术:
                TiShu(card,1);
                break;
            case Card.CardType.防御:
                Defend(card, 1);
                break;
            case Card.CardType.技能:
                Skill(card, 1);
                break;
            case Card.CardType.法术:
                TiShu(card, 1);
                break;
            case Card.CardType.弹幕:
                TiShu(card, 1);
                break;

        }
    }

    private void TiShu(Card card, int times) //体术卡的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;
        if (card.cardData.needTarget)
            if (targetEnemy == null) //如果需要目标但是目标敌人为空时使用失败
                return;
        Player.Instance.PlayAttackAnim(); //播放攻击动画
        var time = Player.Instance.DoubleBlade() ? 2 : 1;

        if (card.valueDic.ContainsKey(Value.ValueType.伤害))//伤害结算
        {
            for (var i = 0; i < card.cardData.times; i++) 
                targetEnemy.TakeDamage(card.valueDic[Value.ValueType.伤害]);
        }

        for (int i = 0; i < time; i++)//如果有双刀则对卡牌效果结算2次
        {
            if (card.cardData.canXinList.Count>0 && card.cardData.cost == Player.Instance.energy)//残心检测
            {
                foreach (var canXin in card.cardData.canXinList)
                {
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.伤害:
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;

                                }));
                            }
                            break;
                        case Value.ValueType.护甲:
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;

                                }));
                            }
                            break;
                        case Value.ValueType.回费:
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;
                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;


                                }));
                            }

                            break;
                    }

                }
            }

        }

        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);
    }

    private void Skill(Card card, int times) //技能卡的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;
        if (card.cardData.needTarget)
            if (targetEnemy == null) //如果需要目标但是目标敌人为空时使用失败
                return;
        
        foreach (var value in card.valueDic)
        {
            var newValue = new Value();
            newValue.type = value.Key;
            newValue.value = value.Value;
            switch (value.Key)
            {
                case Value.ValueType.二刀流:
                    StateManager.AddStateToPlayer(newValue);
                    break;
            }
        }

        Player.Instance.PlayAttackAnim(); //播放攻击动画
        CardManager.Instance.UseCard(); //使用卡牌
        Player.Instance.energy -= card.cardData.cost; //消耗费用
        CardManager.Instance.Discard(card); //弃牌
    }

    private void Defend(Card card, int times) //防御卡的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;
        if (card.cardData.needTarget)
            if (targetEnemy == null) //如果需要目标但是目标敌人为空时使用失败
                return;
        Player.Instance.PlayAttackAnim(); //播放攻击动画
        for (var i = 0; i < card.cardData.times; i++) //获得护盾times次
            Player.Instance.GetShield(card.valueDic[Value.ValueType.护甲]);
        CardManager.Instance.UseCard(); //使用卡牌
        Player.Instance.energy -= card.cardData.cost; //消耗费用
        CardManager.Instance.Discard(card); //弃牌
    }
}