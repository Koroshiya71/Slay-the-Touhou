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
        if (Player.Instance.CheckState(Value.ValueType.体术以外禁止) && card.cardData.type != Card.CardType.体术) yield break;
        if (Player.Instance.CheckState(Value.ValueType.体术限制) && card.cardData.type == Card.CardType.体术) yield break;
        if (Player.Instance.CheckState(Value.ValueType.法术限制) && card.cardData.type == Card.CardType.法术) yield break;
        if (Player.Instance.CheckState(Value.ValueType.防御限制) && card.cardData.type == Card.CardType.防御) yield break;
        if (Player.Instance.CheckState(Value.ValueType.技能限制) && card.cardData.type == Card.CardType.技能) yield break;
        if (Player.Instance.CheckState(Value.ValueType.弹幕限制) && card.cardData.type == Card.CardType.弹幕) yield break;


        //如果卡牌拥有无法使用属性，则跳过检测
        if (card.valueDic.ContainsKey(Value.ValueType.无法使用))
        {
            yield break;
            
        }
        if (card.cardData.type == Card.CardType.弹幕)
        {
            BattleManager.Instance.danmakuCardCombo++;
            if (RelicManager.Instance.CheckRelic(6))
            {
                if (BattleManager.Instance.danmakuCardCombo == 2)
                {
                    var n = Random.Range(0, EnemyManager.Instance.InGameEnemyList.Count);
                    var enemy = EnemyManager.Instance.InGameEnemyList[n];
                    var damage = 2;
                    if (enemy.CheckState(Value.ValueType.魂体))
                    {
                        damage = (int)((damage * 0.7) * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f));
                    }
                    enemy.TakeDamage(damage, DamageType.Danmaku);
                    BattleManager.Instance.danmakuCardCombo = 0;
                    Debug.Log("小阴阳玉触发");
                }
            }
        }
        switch (card.cardData.cardID)
        {
            case "0001": //斩击
            case "0003": //三连斩
            case "0005": //残月斩
            case "0006": //剑气
            case "0011": //冥想斩
            case "0013": //居合
                SingleAttack(card, 1);
                break;
            case "0002": //防御
                Defend(card, 1);
                break;
            case "0004": //二刀
            case "0008": //紫电一闪
            case "0009": //两百由旬之一闪
            case "0015": //背水一战
            case "0016": //起势
            case "0017": //庭师的智慧
                Buff(card, 1);
                break;
            case "0007": //剑与弹幕
            case "0014": //快速剑弹
                AttackAll(card, 1);
                break;
            case "0012": //散华
                RandomAttack(card, 1);
                break;
            case "0010": //冥想
                if (Player.Instance.energy < card.cardData.cost) break;
                for (var i = 0; i < card.valueDic[Value.ValueType.抽牌]; i++) CardManager.Instance.DrawCard();
                Player.Instance.PlayAttackAnim(); //播放攻击动画
                CardManager.Instance.UseCard(card.gameObject); //使用卡牌
                Player.Instance.energy -= card.cardData.cost; //消耗费用
                StartCoroutine(CardManager.Instance.ChooseCardFromHand(2, true, card));
                CardManager.Instance.Discard(card);

                break;
            case "0018": //六根清净
                if (Player.Instance.energy < card.cardData.cost) break;
                foreach (var c in CardManager.Instance.drawCardList)
                {
                    bool isHad = false;
                    foreach (var value in c.valueList)
                    {
                        if (value.type == Value.ValueType.无何有)
                        {
                            isHad = true;
                            break;
                        }
                    }
                    if (!isHad)
                    {
                        c.valueList.Add(new Value() { type = Value.ValueType.无何有, value = 1 });
                        c.keepChangeInBattle = true;
                    }
                }
                foreach (var c in CardManager.Instance.discardList)
                {
                    bool isHad = false;
                    foreach (var value in c.valueList)
                    {
                        if (value.type == Value.ValueType.无何有)
                        {
                            isHad = true;
                            break;
                        }
                    }
                    if (!isHad)
                    {
                        c.valueList.Add(new Value() { type = Value.ValueType.无何有, value = 1 });
                        c.keepChangeInBattle = true;
                    }
                }
                foreach (var c in CardManager.Instance.handCardList)
                {
                    bool isHad = false;
                    Card cd = c.GetComponent<Card>();
                    foreach (var value in cd.cardData.valueList)
                    {
                        if (value.type == Value.ValueType.无何有)
                        {
                            isHad = true;
                            break;
                        }
                    }
                    if (!isHad)
                    {
                        cd.cardData.valueList.Add(new Value() { type = Value.ValueType.无何有, value = 1 });
                        cd.cardData.keepChangeInBattle = true;
                        cd.InitCard(cd.cardData);
                    }
                }
                StateManager.AddStateToPlayer(new Value() { type = Value.ValueType.六根清净, value = 1 });
                Player.Instance.PlayAttackAnim(); //播放攻击动画
                CardManager.Instance.UseCard(card.gameObject); //使用卡牌
                if (card.cardData.type != Card.CardType.符卡)
                {
                    Player.Instance.energy -= card.cardData.cost; //消耗费用
                }
                CardManager.Instance.Discard(card);

                break;
        }
    }

    private void AttackAll(Card card, int times) //群体攻击的通用方法
    {
        DamageType damageType = DamageType.TiShu;
        switch (card.cardData.type)
        {
            case Card.CardType.体术:
                damageType = DamageType.TiShu;
                break;
            case Card.CardType.弹幕:
                damageType = DamageType.Danmaku;
                break;
            case Card.CardType.符卡:
                damageType = DamageType.SpellCard;
                break;
        }
        

        if ((Player.Instance.energy < card.cardData.cost && card.cardData.type != Card.CardType.符卡) || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;
        Player.Instance.PlayAttackAnim(); //播放攻击动画
        var time = Player.Instance.CheckState(Value.ValueType.二刀流) ? 2 : 1;

        for (var i = 0; i < time; i++) //如果有双刀则对卡牌效果结算2次
        {
            if (card.valueDic.ContainsKey(Value.ValueType.伤害)) //伤害结算
                for (var t = 0; t < card.cardData.times; t++)
                {
                    for (var j = 0; j < EnemyManager.Instance.InGameEnemyList.Count; j++)
                    {
                        var enemy = EnemyManager.Instance.InGameEnemyList[j];
                        if ((enemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                            (enemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                        {
                            enemy.TakeDamage((int)(card.valueDic[Value.ValueType.伤害] * 0.7f
                                * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)),damageType);
                        }
                        else
                            enemy.TakeDamage((int)(card.valueDic[Value.ValueType.伤害]
                                                   * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                        
                    }
                      
                }

            //残心检测
            if (card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy || card.canXin)
            {
                Player.Instance.effectAnimator.Play("CanXin");

                card.canXin = true;
                BattleManager.Instance.actionsTurnStart.Add(() => { BattleManager.Instance.hasCanXin = true; });

                foreach (var canXin in card.cardData.canXinList)
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.伤害:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                for (var j = 0; j < EnemyManager.Instance.InGameEnemyList.Count; j++)
                                {
                                    var enemy = EnemyManager.Instance.InGameEnemyList[j];
                                    if ((enemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                                        (enemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                                    {
                                        enemy.TakeDamage((int)(canXin.CanXinValue.value * 0.7f
                                            * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                    }
                                    else
                                        enemy.TakeDamage((int)(canXin.CanXinValue.value
                                                               * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);

                                }

                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    for (int i = 0; i < EnemyManager.Instance.InGameEnemyList.Count; i++)
                                    {
                                        var enemy = EnemyManager.Instance.InGameEnemyList[i];
                                        if ((enemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                                            (enemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                                        {
                                            enemy.TakeDamage((int)(canXin.CanXinValue.value * 0.7f
                                                * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                        }
                                        else
                                            enemy.TakeDamage((int)(canXin.CanXinValue.value
                                                                   * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);

                                    }

                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    for (int i = 0; i < EnemyManager.Instance.InGameEnemyList.Count; i++)
                                    {
                                        var enemy = EnemyManager.Instance.InGameEnemyList[i];
                                        if ((enemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                                            (enemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                                        {
                                            enemy.TakeDamage((int)(canXin.CanXinValue.value * 0.7f
                                                * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                        }
                                        else
                                            enemy.TakeDamage((int)(canXin.CanXinValue.value
                                                                   * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);

                                    }
                                });
                            break;
                        case Value.ValueType.护甲:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetShield(canXin.CanXinValue.value);

                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                });
                            break;
                        case Value.ValueType.回费:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetEnergy(canXin.CanXinValue.value);


                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                });

                            break;
                        case Value.ValueType.惊吓:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                    StateManager.AddStateToEnemy(canXin.CanXinValue, enemy);


                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                        StateManager.AddStateToEnemy(canXin.CanXinValue, enemy);
                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                        StateManager.AddStateToEnemy(canXin.CanXinValue, enemy);
                                });

                            break;
                    }
            }

            //连斩检测
            if (card.cardData.comboList.Count > 0)
                foreach (var combo in card.cardData.comboList)
                    if (BattleManager.Instance.cardCombo >= combo.comboNum)
                    {
                        var comboTimes = Player.Instance.CheckState(Value.ValueType.起势) ? 2 : 1;

                        for (var j = 0; j < comboTimes; j++)
                            switch (combo.comboValue.type)
                            {
                                case Value.ValueType.伤害:
                                    for (int k = 0; k < EnemyManager.Instance.InGameEnemyList.Count; k++)
                                    {
                                        var enemy = EnemyManager.Instance.InGameEnemyList[k];
                                        if ((enemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                                            (enemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                                        {
                                            enemy.TakeDamage((int)(combo.comboValue.value * 0.7f
                                                * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                        }
                                        else
                                            enemy.TakeDamage((int)(combo.comboValue.value
                                                                   * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);

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

        CardManager.Instance.UseCard();
        BattleManager.Instance.cardCombo++;
        if (card.GetComponent<Card>().cardData.type == Card.CardType.体术) BattleManager.Instance.tiShuCardCombo++;
        if (card.cardData.type != Card.CardType.符卡)
        {
            Player.Instance.energy -= card.cardData.cost; //消耗费用
        }
        CardManager.Instance.Discard(card);
    }

    private void SingleAttack(Card card, int times) //单体攻击的通用方法
    {
        DamageType damageType = DamageType.TiShu;
        switch (card.cardData.type)
        {
            case Card.CardType.体术:
                damageType = DamageType.TiShu;
                break;
            case Card.CardType.弹幕:
                damageType = DamageType.Danmaku;
                break;
            case Card.CardType.符卡:
                damageType = DamageType.SpellCard;
                break;
        }
        if ((Player.Instance.energy < card.cardData.cost && card.cardData.type != Card.CardType.符卡) || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;
        if (targetEnemy == null) //如果目标敌人为空则跳过
            return;
        var attackedEnemy = targetEnemy;
        Player.Instance.PlayAttackAnim(); //播放攻击动画
        var time = Player.Instance.CheckState(Value.ValueType.二刀流) ? 2 : 1;


        for (var i = 0; i < time; i++) //如果有双刀则对卡牌效果结算2次
        {
            if (card.valueDic.ContainsKey(Value.ValueType.伤害)) //伤害结算
                for (var t = 0; t < card.cardData.times; t++)
                {
                    if (card.cardData.cardID == "0013") //居合
                        if (BattleManager.Instance.tiShuCardCombo > 0)
                            break;
                    if ((targetEnemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                        (targetEnemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                    {
                        targetEnemy.TakeDamage((int)(card.valueDic[Value.ValueType.伤害] * 0.7f
                            *(RelicManager.Instance.CheckRelic(11)?1.5f:1.0f)), damageType);
                    }
                    else
                        targetEnemy.TakeDamage((int)(card.valueDic[Value.ValueType.伤害]
                            * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                }

            if (card.valueDic.ContainsKey(Value.ValueType.击杀回费))
                if (targetEnemy.hp <= 0)
                    Player.Instance.GetEnergy(card.valueDic[Value.ValueType.击杀回费]);
            //残心检测
            if (card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy || card.canXin)
            {
                Player.Instance.effectAnimator.Play("CanXin");

                card.canXin = true;
                BattleManager.Instance.actionsTurnStart.Add(() => { BattleManager.Instance.hasCanXin = true; });
                foreach (var canXin in card.cardData.canXinList)
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.伤害:
                            if (attackedEnemy == null)
                                break;
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                if ((attackedEnemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                                    (attackedEnemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                                {
                                    attackedEnemy.TakeDamage((int)(canXin.CanXinValue.value * 0.7f
                                        * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                }
                                else
                                    attackedEnemy.TakeDamage((int)(canXin.CanXinValue.value
                                                                   * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);

                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    if ((attackedEnemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                                        (attackedEnemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                                    {
                                        attackedEnemy.TakeDamage((int)(canXin.CanXinValue.value * 0.7f
                                            * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                    }
                                    else
                                        attackedEnemy.TakeDamage((int)(canXin.CanXinValue.value
                                                                       * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    if ((attackedEnemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                                        (attackedEnemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                                    {
                                        attackedEnemy.TakeDamage((int)(canXin.CanXinValue.value * 0.7f
                                            * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                    }
                                    else
                                        attackedEnemy.TakeDamage((int)(canXin.CanXinValue.value
                                                                       * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                });
                            break;
                        case Value.ValueType.护甲:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetShield(canXin.CanXinValue.value);

                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                });
                            break;
                        case Value.ValueType.回费:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                });

                            break;
                    }
            }

            //连斩检测
            if (card.cardData.comboList.Count > 0)
                foreach (var combo in card.cardData.comboList)
                    if (BattleManager.Instance.cardCombo >= combo.comboNum)
                    {
                        var comboTimes = Player.Instance.CheckState(Value.ValueType.起势) ? 2 : 1;

                        for (var j = 0; j < comboTimes; j++)
                            switch (combo.comboValue.type)
                            {
                                case Value.ValueType.伤害:
                                    if ((attackedEnemy.CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                                        (attackedEnemy.CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                                    {
                                        attackedEnemy.TakeDamage((int)(combo.comboValue.value * 0.7f
                                            * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                                    }
                                    else
                                        attackedEnemy.TakeDamage((int)(combo.comboValue.value
                                                                       * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
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

        CardManager.Instance.UseCard();
        BattleManager.Instance.cardCombo++;
        if (card.GetComponent<Card>().cardData.type == Card.CardType.体术) BattleManager.Instance.tiShuCardCombo++;
        if (card.cardData.type != Card.CardType.符卡)
        {
            Player.Instance.energy -= card.cardData.cost; //消耗费用
        }
        CardManager.Instance.Discard(card);
    }

    private void RandomAttack(Card card, int times) //随机攻击的通用方法
    {
        DamageType damageType = DamageType.TiShu;
        switch (card.cardData.type)
        {
            case Card.CardType.体术:
                damageType = DamageType.TiShu;
                break;
            case Card.CardType.弹幕:
                damageType = DamageType.Danmaku;
                break;
            case Card.CardType.符卡:
                damageType = DamageType.SpellCard;
                break;
        }
        if ((Player.Instance.energy < card.cardData.cost && card.cardData.type != Card.CardType.符卡) || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;

        Player.Instance.PlayAttackAnim(); //播放攻击动画
        var time = Player.Instance.CheckState(Value.ValueType.二刀流) ? 2 : 1;
        var n = Random.Range(0, EnemyManager.Instance.InGameEnemyList.Count);
        for (var i = 0; i < time; i++) //如果有双刀则对卡牌效果结算2次
        {
            n = Random.Range(0, EnemyManager.Instance.InGameEnemyList.Count);
            if (card.valueDic.ContainsKey(Value.ValueType.伤害)) //伤害结算
                for (var t = 0; t < card.cardData.times; t++)
                {
                    n = Random.Range(0, EnemyManager.Instance.InGameEnemyList.Count);
                    if ((EnemyManager.Instance.InGameEnemyList[n].CheckState(Value.ValueType.灵体) && card.cardData.type == Card.CardType.体术) ||
                        (EnemyManager.Instance.InGameEnemyList[n].CheckState(Value.ValueType.魂体) && card.cardData.type == Card.CardType.弹幕))
                    {
                        EnemyManager.Instance.InGameEnemyList[n].TakeDamage((int)(card.valueDic[Value.ValueType.伤害] * 0.7f 
                            * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                    }
                    else
                        EnemyManager.Instance.InGameEnemyList[n].TakeDamage((int)(card.valueDic[Value.ValueType.伤害] 
                            * (RelicManager.Instance.CheckRelic(11) ? 1.5f : 1.0f)), damageType);
                }

            if (card.valueDic.ContainsKey(Value.ValueType.击杀回费))
                if (EnemyManager.Instance.InGameEnemyList[n].hp <= 0)
                    Player.Instance.GetEnergy(card.valueDic[Value.ValueType.击杀回费]);
            //残心检测
            if (card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy || card.canXin)
            {
                card.canXin = true;

                BattleManager.Instance.actionsTurnStart.Add(() => { BattleManager.Instance.hasCanXin = true; });
                Player.Instance.effectAnimator.Play("CanXin");
                foreach (var canXin in card.cardData.canXinList)
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.伤害:
                            //if (Player.Instance.CheckState(Value.ValueType.流转))
                            //{
                            //    targetEnemy.TakeDamage(canXin.CanXinValue.value);
                            //    break;
                            //}

                            //if (canXin.IsTurnEnd)
                            //    BattleManager.Instance.actionsEndTurn.Add(() =>
                            //    {
                            //        targetEnemy.TakeDamage(canXin.CanXinValue.value);
                            //    });
                            //else
                            //    BattleManager.Instance.actionsTurnStart.Add(() =>
                            //    {
                            //        targetEnemy.TakeDamage(canXin.CanXinValue.value);
                            //    });
                            break;

                        case Value.ValueType.护甲:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetShield(canXin.CanXinValue.value);

                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                });
                            break;
                        case Value.ValueType.回费:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                });

                            break;
                        case Value.ValueType.回血:
                            if (Player.Instance.CheckState(Value.ValueType.流转))
                            {
                                Player.Instance.Recover(canXin.CanXinValue.value);
                                break;
                            }

                            if (canXin.IsTurnEnd)
                                BattleManager.Instance.actionsEndTurn.Add(() =>
                                {
                                    Player.Instance.Recover(canXin.CanXinValue.value);
                                });
                            else
                                BattleManager.Instance.actionsTurnStart.Add(() =>
                                {
                                    Player.Instance.Recover(canXin.CanXinValue.value);
                                });
                            break;
                    }
            }

            //连斩检测
            if (card.cardData.comboList.Count > 0)
                foreach (var combo in card.cardData.comboList)
                    if (BattleManager.Instance.cardCombo >= combo.comboNum)
                    {
                        var comboTimes = Player.Instance.CheckState(Value.ValueType.起势) ? 2 : 1;

                        for (var j = 0; j < comboTimes; j++)
                            switch (combo.comboValue.type)
                            {
                                case Value.ValueType.伤害:
                                    //targetEnemy.TakeDamage(combo.comboValue.value);
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

        CardManager.Instance.UseCard();
        BattleManager.Instance.cardCombo++;
        if (card.GetComponent<Card>().cardData.type == Card.CardType.体术) BattleManager.Instance.tiShuCardCombo++;
        if (card.cardData.type != Card.CardType.符卡)
        {
            Player.Instance.energy -= card.cardData.cost; //消耗费用
        }
        CardManager.Instance.Discard(card);
    }


    private void Buff(Card card, int times) //状态卡的通用方法
    {
        if ((Player.Instance.energy < card.cardData.cost && card.cardData.type != Card.CardType.符卡) || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
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
                    if (card.valueDic.ContainsKey(Value.ValueType.体术以外禁止))
                        BattleManager.Instance.actionsTurnStart.Add(() =>
                        {
                            StateManager.AddStateToPlayer(new Value()
                            { type = Value.ValueType.体术以外禁止, value = card.valueDic[Value.ValueType.体术以外禁止] });
                        });
                    break;
                case Value.ValueType.抽牌:
                    for (var i = 0; i < value.Value; i++) CardManager.Instance.DrawCard();
                    break;
                case Value.ValueType.回费:
                    Player.Instance.GetEnergy(value.Value);
                    break;
                case Value.ValueType.背水一战:
                    StateManager.AddStateToPlayer(newValue);
                    break;
                case Value.ValueType.起势:
                    StateManager.AddStateToPlayer(newValue);
                    break;
                case Value.ValueType.保留手牌:
                    StateManager.AddStateToPlayer(newValue);
                    break;
            }
        }

        Player.Instance.PlayAttackAnim(); //播放攻击动画
        CardManager.Instance.UseCard(); //使用卡牌
        BattleManager.Instance.cardCombo++;
        if (card.GetComponent<Card>().cardData.type == Card.CardType.体术) BattleManager.Instance.tiShuCardCombo++;
        if (card.cardData.type != Card.CardType.符卡)
        {
            Player.Instance.energy -= card.cardData.cost; //消耗费用
        }
        CardManager.Instance.Discard(card); //弃牌
    }

    private void Defend(Card card, int times) //防御卡的通用方法
    {
        if ((Player.Instance.energy < card.cardData.cost && card.cardData.type != Card.CardType.符卡) || BattleManager.Instance.turnHasEnd) //如果费用不够则使用失败
            return;

        Player.Instance.PlayAttackAnim(); //播放攻击动画
        for (var i = 0; i < card.cardData.times; i++) //获得护盾times次
            Player.Instance.GetShield(card.valueDic[Value.ValueType.护甲]);
        BattleManager.Instance.cardCombo++;
        if (card.GetComponent<Card>().cardData.type == Card.CardType.体术) BattleManager.Instance.tiShuCardCombo++;
        CardManager.Instance.UseCard(); //使用卡牌
        if (card.cardData.type != Card.CardType.符卡)
        {
            Player.Instance.energy -= card.cardData.cost; //消耗费用
        }
        CardManager.Instance.Discard(card); //弃牌
    }
}