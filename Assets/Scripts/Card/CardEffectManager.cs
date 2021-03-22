using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance; //单例
    public Enemy targetEnemy; //卡牌的
    public int attackTimes; //体术的攻击次数

    private void Awake()
    {
        
        Instance = this;
    }

    public IEnumerator UseThisCard(Card card) //触发卡牌效果
    {
        //如果有体术限制状态，而该卡不是体术，则跳过检测
        if (Player.Instance.CheckState(Value.ValueType.体术限制)&&card.cardData.type!=Card.CardType.体术)
        {
            yield break;
        }
        switch (card.cardData.cardID)
        {
            case "0001"://斩击
            case "0003"://三连斩
            case "0005"://残月斩
            case "0006"://剑气
            case "0011"://冥想斩
                SingleAttack(card,1);
                break;
            case "0002"://防御
                Defend(card, 1);
                break;
            case "0004"://二刀
            case "0008"://紫电一闪
            case "0009"://两百由旬之一闪
                Buff(card, 1);
                break;
            case "0007"://剑与弹幕
                AttackAll(card, 1);
                break;
            case "0010"://冥想
                for (int i = 0; i < card.valueDic[Value.ValueType.抽牌]; i++)
                {
                    CardManager.Instance.DrawCard();
                }
                Player.Instance.PlayAttackAnim(); //播放攻击动画
                CardManager.Instance.UseCard(card.gameObject); //使用卡牌
                Player.Instance.energy -= card.cardData.cost; //消耗费用
                CardManager.Instance.Discard(card);
                StartCoroutine(CardManager.Instance.ChooseCardFromHand(2, true,card));
                break;
            case "0012"://散华
                RandomAttack(card,1);
                break;

        }
    }

    private void AttackAll(Card card,int times)//群体攻击的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;
        Player.Instance.PlayAttackAnim(); //播放攻击动画
        var time = Player.Instance.CheckState(Value.ValueType.二刀流) ? 2 : 1;

        for (int i = 0; i < time; i++)//如果有双刀则对卡牌效果结算2次
        {
            if (card.valueDic.ContainsKey(Value.ValueType.伤害))//伤害结算
            {
                for (var t = 0; t < card.cardData.times; t++)
                    foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                    {
                        enemy.TakeDamage(card.valueDic[Value.ValueType.伤害]);
                    }
            }
            //残心检测
            if ((card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy)||card.canXin)
            {
                card.canXin = true;
                BattleManager.Instance.actionsTurnStart.Add(()=>
                {

                    BattleManager.Instance.hasCanXin = true;
                });

                foreach (var canXin in card.cardData.canXinList)
                {
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.伤害:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                {
                                    enemy.TakeDamage(canXin.CanXinValue.value);

                                }
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                    {
                                        enemy.TakeDamage(canXin.CanXinValue.value);

                                    }

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.护甲:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetShield(canXin.CanXinValue.value);

                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.回费:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetEnergy(canXin.CanXinValue.value);


                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);


                                }));
                            }

                            break;
                    }

                }
            }
            //连斩检测
            if (card.cardData.comboList.Count>0)
            {
                foreach (var combo in card.cardData.comboList)
                {
                    if (BattleManager.Instance.cardCombo>=combo.comboNum)
                    {
                        switch (combo.comboValue.type)
                        {
                            case Value.ValueType.伤害:
                                foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                {
                                    enemy.TakeDamage(combo.comboValue.value);
                                }
                                break;
                            case Value.ValueType.护甲:
                                Player.Instance.GetShield(combo.comboValue.value);
                                break;
                            case Value.ValueType.回费:
                                Player.Instance.GetEnergy(combo.comboValue.value);

                                break;
                        }
                    }
                }
            }
        }

        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);
    }
    private void SingleAttack(Card card, int times) //单体攻击的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;
        if (targetEnemy == null) //如果目标敌人为空则跳过
                return;
        Player.Instance.PlayAttackAnim(); //播放攻击动画
        var time = Player.Instance.CheckState(Value.ValueType.二刀流) ? 2 : 1;

        

        for (int i = 0; i < time; i++)//如果有双刀则对卡牌效果结算2次
        {   
            if (card.valueDic.ContainsKey(Value.ValueType.伤害))//伤害结算
            {
                for (var t = 0; t< card.cardData.times; t++)
                    targetEnemy.TakeDamage(card.valueDic[Value.ValueType.伤害]);
            }

            if (card.valueDic.ContainsKey(Value.ValueType.击杀回费))
            {
                if (targetEnemy.hp<=0)
                {
                    Player.Instance.GetEnergy(card.valueDic[Value.ValueType.击杀回费]);
                }
            }
            //残心检测
            if ((card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy) || card.canXin)
            {
                card.canXin = true;
                BattleManager.Instance.actionsTurnStart.Add((() =>
                {
                    BattleManager.Instance.hasCanXin = true;

                }));
                foreach (var canXin in card.cardData.canXinList)
                {
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.伤害:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);
                                    break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.护甲:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetShield(canXin.CanXinValue.value);

                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.回费:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);


                                }));
                            }

                            break;
                    }

                }
            }
            //连斩检测
            if (card.cardData.comboList.Count > 0)
            {
                foreach (var combo in card.cardData.comboList)
                {
                    if (BattleManager.Instance.cardCombo >= combo.comboNum)
                    {
                        switch (combo.comboValue.type)
                        {
                            case Value.ValueType.伤害:
                                targetEnemy.TakeDamage(combo.comboValue.value);
                                break;
                            case Value.ValueType.护甲:
                                Player.Instance.GetShield(combo.comboValue.value);
                                break;
                            case Value.ValueType.回费:
                                Player.Instance.GetEnergy(combo.comboValue.value);

                                break;
                        }
                    }
                }
            }
        }

        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);
    }
    private void RandomAttack(Card card, int times) //随机攻击的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;

        Player.Instance.PlayAttackAnim(); //播放攻击动画
        var time = Player.Instance.CheckState(Value.ValueType.二刀流) ? 2 : 1;

        for (int i = 0; i < time; i++)//如果有双刀则对卡牌效果结算2次
        {
            if (card.valueDic.ContainsKey(Value.ValueType.伤害))//伤害结算
            {
                for (var t = 0; t < card.cardData.times; t++)
                {
                    int n = Random.Range(0, EnemyManager.Instance.InGameEnemyList.Count);
                    EnemyManager.Instance.InGameEnemyList[n].TakeDamage(card.valueDic[Value.ValueType.伤害]);
                }
            }

            if (card.valueDic.ContainsKey(Value.ValueType.击杀回费))
            {
                if (targetEnemy.hp <= 0)
                {
                    Player.Instance.GetEnergy(card.valueDic[Value.ValueType.击杀回费]);
                }
            }
            //残心检测
            if ((card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy) || card.canXin)
            {
                card.canXin = true;
                BattleManager.Instance.actionsTurnStart.Add((() =>
                {
                    BattleManager.Instance.hasCanXin = true;

                }));

                foreach (var canXin in card.cardData.canXinList)
                {
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.伤害:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                targetEnemy.TakeDamage(canXin.CanXinValue.value);
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.护甲:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetShield(canXin.CanXinValue.value);

                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.回费:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);


                                }));
                            }

                            break;
                        case Value.ValueType.回血:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.Recover(canXin.CanXinValue.value);
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.Recover(canXin.CanXinValue.value);


                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.Recover(canXin.CanXinValue.value);


                                }));
                            }
                            break;
                    }

                }
            }
            //连斩检测
            if (card.cardData.comboList.Count > 0)
            {
                foreach (var combo in card.cardData.comboList)
                {
                    if (BattleManager.Instance.cardCombo >= combo.comboNum)
                    {
                        switch (combo.comboValue.type)
                        {
                            case Value.ValueType.伤害:
                                targetEnemy.TakeDamage(combo.comboValue.value);
                                break;
                            case Value.ValueType.护甲:
                                Player.Instance.GetShield(combo.comboValue.value);
                                break;
                            case Value.ValueType.回费:
                                Player.Instance.GetEnergy(combo.comboValue.value);

                                break;
                        }
                    }
                }
            }
        }

        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);
    }


    private void Buff(Card card, int times) //状态卡的通用方法
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
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
                case Value.ValueType.流转:
                    StateManager.AddStateToPlayer(newValue);
                    break;
                case Value.ValueType.额外回合:
                    StateManager.AddStateToPlayer(newValue);
                    if (card.valueDic.ContainsKey(Value.ValueType.体术限制))
                    {
                        BattleManager.Instance.actionsTurnStart.Add((() =>
                        {
                            StateManager.AddStateToPlayer(new Value(){type = Value.ValueType.体术限制,value=card.valueDic[Value.ValueType.体术限制]});
                        }));
                    }
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
        
        Player.Instance.PlayAttackAnim(); //播放攻击动画
        for (var i = 0; i < card.cardData.times; i++) //获得护盾times次
            Player.Instance.GetShield(card.valueDic[Value.ValueType.护甲]);
        CardManager.Instance.UseCard(); //使用卡牌
        Player.Instance.energy -= card.cardData.cost; //消耗费用
        CardManager.Instance.Discard(card); //弃牌
    }
}