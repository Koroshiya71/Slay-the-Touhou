using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public bool turnHasEnd;
    public List<Action> actionsEndTurn = new List<Action>(); //回合结束时触发的效果
    public List<Action> actionsTurnStart = new List<Action>(); //回合开始时触发的效果
    public bool hasCanXin; //上回合是否触发过残心
    public int effectTimes; //卡牌效果触发的次数
    public int cardCombo; //本回合使用的卡牌数量
    public bool extraTurn; //该回合是否是额外回合
    public int tiShuCardCombo; //本回合使用的体术牌数量
    public int danmakuCardCombo; //本回合使用的体术牌数量

    public GameObject enemyPrefab; //敌人预制体
    public bool isInBattle; //是否正在战斗中
    public List<BattleData> normalBattleDataList = new List<BattleData>(); //保存所有普通战斗场景数据的列表
    public List<BattleData> eliteBattleDataList = new List<BattleData>(); //保存所有精英战斗场景数据的列表
    public int battleExp; //本场战斗累积的经验值
    public int battleGold; //本场战斗累积的金币
    public List<Vector2> enemyPositionList = new List<Vector2>();
    public GameObject statisticImage; //结算面板
    public Text getExpText; //获取经验文本
    public Text getGoldText; //获取金币文本
    public Text turnStartAndEndText; //回合结束文本

    public bool gameIsOver = false;
    public void TurnEnd()
    {
        if (CardManager.Instance.isChoosingFromHand) //如果正在进行选牌，则跳过检测
            return;
        if (turnHasEnd) //如果回合已结束在进行运行其他方法时跳过检测
            return;
        if (MenuEventManager.Instance.isPreviewing) //如果正在进行卡牌预览则不进行检测
            return;

        hasCanXin = false;

        if (actionsEndTurn != null)
        {
            foreach (var action in actionsEndTurn) action();

            actionsEndTurn = new List<Action>();
        }

        foreach (var ally in AllyManager.Instance.inGameAlliesList) ally.OnTurnEnd();
        RelicManager.Instance.RelicEffectOnTurnEnd();
        if (Player.Instance.CheckState(Value.ValueType.额外回合)) //如果玩家拥有额外回合跳过敌人行动直接开始新的玩家回合
        {
            extraTurn = true;
            TurnStart();
            return;
        }

        turnStartAndEndText.enabled = true;
        turnStartAndEndText.text = "敌方回合";
        Invoke(nameof(NotShowTurnText), 0.5f);

        for (var i = 0; i < EnemyManager.Instance.InGameEnemyList.Count; i++)
            if (!EnemyManager.Instance.InGameEnemyList[i].CheckState(Value.ValueType.保留护甲))
                EnemyManager.Instance.InGameEnemyList[i].shield = 0;

        //在执行行动前清空全新状态列表
        Player.Instance.newStateList = new List<Value>();

        for (var i = 0; i < EnemyManager.Instance.InGameEnemyList.Count; i++)
            EnemyManager.Instance.InGameEnemyList[i].newStateList = new List<Value>();
        for (var i = 0; i < EnemyManager.Instance.InGameEnemyList.Count; i++)
            EnemyManager.Instance.InGameEnemyList[i].TakeAction();


        Invoke(nameof(TurnStart), 1);

        if (!Player.Instance.CheckState(Value.ValueType.保留手牌)) CardManager.Instance.DropAllCard();
        turnHasEnd = true;
        cardCombo = 0;
        tiShuCardCombo = 0;

    }
    //回合开始
    public void TurnStart()
    {
        //如果拥有紫的阳伞则统计计数
        if(RelicManager.Instance.CheckRelic(8))
            RelicManager.Instance.yokariPrasolCount++;
        //如果紫的阳伞计数可以整除7，且拥有遗物紫的阳伞，则获得一层神隐
        if (RelicManager.Instance.yokariPrasolCount  == 7 )
        {
            StateManager.AddStateToPlayer(new Value()
            {
                type=Value.ValueType.神隐,
                value=1
                
            });
            RelicManager.Instance.yokariPrasolCount = 0;

        }

        //如果拥有万宝槌则统计计数
        if (RelicManager.Instance.CheckRelic(14))
            RelicManager.Instance.wanBaoChuiCount++;
        if (RelicManager.Instance.wanBaoChuiCount == 2)
        {
            while (true)
            {
                var cardNo = Random.Range(0, CardManager.Instance.CardDataList.Count);
                var card = CardManager.Instance.CardDataList[cardNo];
                if (card.type==Card.CardType.体术)
                {
                    CardManager.Instance.GetCard(card.cardID,true);
                    break;
                }
            }

            RelicManager.Instance.wanBaoChuiCount = 0;
        }
        if (!extraTurn)
            foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                enemy.UpdateCurrentAction();
        turnStartAndEndText.enabled = true;
        turnStartAndEndText.text = "玩家回合";
        Invoke(nameof(NotShowTurnText), 0.5f);
        turnHasEnd = false;
        Player.Instance.InitEnergy();

        StateManager.UpdatePlayerState(); //对玩家身上的状态进行更新
        StateManager.UpdateEnemiesState(); //对敌人身上的状态进行更新

        if (!extraTurn) //如果是额外回合，则不清除护甲并保留手牌
            Player.Instance.shield = 0;
        extraTurn = false;
        var drawCardNum = Player.Instance.CheckState(Value.ValueType.抽牌减1)
            ? Player.Instance.drawCardNum - 1
            : Player.Instance.drawCardNum;

        foreach (var card in CardManager.Instance.handCardList) card.GetComponent<Card>().UpdateCardState();
        if (actionsTurnStart != null)
        {
            foreach (var action in actionsTurnStart) action();

            actionsTurnStart = new List<Action>();
        }

        for (var i = 0; i < drawCardNum; i++) CardManager.Instance.DrawCard();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SceneManager.Instance.battleSceneCanvas.enabled = false;
        InitEnemyPosList();
        statisticImage.SetActive(false);
    }

    private void InitEnemyPosList()
    {
        enemyPositionList.Add(new Vector2(4.5f, 0));
        enemyPositionList.Add(new Vector2(2.5f, 0));
        enemyPositionList.Add(new Vector2(6.5f, 0));
        enemyPositionList.Add(new Vector2(4.5f, 3));
        enemyPositionList.Add(new Vector2(4.5f, -3));
        enemyPositionList.Add(new Vector2(2.5f, 3));
        enemyPositionList.Add(new Vector2(2.5f, -3));
        enemyPositionList.Add(new Vector2(6.5f, 3));
        enemyPositionList.Add(new Vector2(6.5f, -3));
    }

    public void BattleStart(BattleData data) //战斗开始
    {
        battleExp = 0;
        battleGold = 0;
        RelicManager.Instance.isWuShu = false;
        SceneManager.Instance.battleSceneCanvas.enabled = true;
        SceneManager.Instance.mapSceneCanvas.enabled = false;
        StateManager.Instance.InitPlayerState();
        CardManager.Instance.InitAllCardList();
        CardManager.Instance.InitDrawCardList();
        CreateEnemies(data.EnemyList);
        if (data.EnemyList.Count >= 3 && RelicManager.Instance.CheckRelic(7))
        {
            RelicManager.Instance.isWuShu = true;

        }
        //每场战斗重置咲夜的怀表状态
        RelicManager.Instance.sakuyaClock = false;
        RelicManager.Instance.RelicEffectOnBattleStart();
        TurnStart();
        isInBattle = true;
    }

    public void BattleEnd()
    {
        hasCanXin = false;
        statisticImage.SetActive(true);
        if (RelicManager.Instance.CheckRelic(9))
        {
            battleGold = (int) (battleGold * 1.2);
        }

        if (RelicManager.Instance.CheckRelic(10))
        {
            battleExp = (int) (battleExp * 1.2);
        }
        getExpText.text = "获得经验   " + battleExp;
        getGoldText.text = "获得金币   " + battleGold;
        Player.Instance.GetExp(battleExp);
        Player.Instance.GetGold(battleGold);
        actionsEndTurn = new List<Action>();
        actionsTurnStart = new List<Action>();
    }


    public void CreateEnemies(List<BattleData.SceneEnemy> enemyList) //创建敌人
    {
        foreach (var enemyS in enemyList)
        foreach (var enemy in EnemyManager.Instance.enemyDataList)
            if (enemy.ID == enemyS.ID)
            {
                var newEnemy = Instantiate(enemyPrefab, enemyS.Pos, Quaternion.identity).GetComponent<Enemy>();
                newEnemy.InitEnemy(enemy);
                Transform transform1;
                (transform1 = newEnemy.transform).SetParent(SceneManager.Instance.battleSceneCanvas.transform);
                transform1.localScale = new Vector3(1, 1, 1);
                break;
            }
    }

    public void CreateEnemy(int enemyID, Vector2 pos)
    {
        foreach (var enemy in EnemyManager.Instance.enemyDataList)
            if (enemy.ID == enemyID)
            {
                var newEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity).GetComponent<Enemy>();
                newEnemy.InitEnemy(enemy);
                Transform transform1;
                (transform1 = newEnemy.transform).SetParent(SceneManager.Instance.battleSceneCanvas.transform);
                transform1.localScale = new Vector3(1, 1, 1);
                break;
            }
    }

    public void NotShowTurnText() //取消回合文本显示
    {
        turnStartAndEndText.enabled = false;
    }

    private void Update()
    {
    }
}