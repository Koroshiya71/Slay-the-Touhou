using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneManager : MonoBehaviour
{
    public List<GameScene> inGameSceneList=new List<GameScene>();//游戏中的场景列表
    public static SceneManager Instance;
    public Canvas battleSceneCanvas;//战斗场景画布
    public Canvas mapSceneCanvas;//地图场景画布
    private void Awake()
    {
        Instance = this;
    }

    public enum SceneType//场景类型
    {
        NormalCombat,//普通战斗
        EliteCombat,//精英战斗
        Event,//事件
        Store//商店
    }
    public List<Sprite> sceneSpriteList = new List<Sprite>();//场景图片的sprite数组
    public List<SceneData> sceneDataList = new List<SceneData>();//保存所有场景类型数据的列表
    void Start()
    {
        battleSceneCanvas.enabled = false;
        mapSceneCanvas.enabled = true;
        InitScenes();
    }

    public void InitScenes()
    {
        foreach (var scene in inGameSceneList)
        {
            int a = Random.Range(0, BattleManager.Instance.normalBattleDataList.Count);
            int b = Random.Range(0, BattleManager.Instance.eliteBattleDataList.Count);
            int index = inGameSceneList.IndexOf(scene);
            if (index < 7)
                scene.isOptional = true;
            scene.sceneData = new SceneData();
            scene.sceneData.battleData = new BattleData();
            scene.sceneData.type = sceneDataList[Random.Range(0, SceneManager.Instance.sceneDataList.Count)].type;
            if (scene.sceneData.type == SceneManager.SceneType.NormalCombat)
            {
                scene.sceneData.battleData = new BattleData();
                scene.sceneData.battleData.EnemyList = new List<BattleData.SceneEnemy>();
                foreach (var enemy in BattleManager.Instance.normalBattleDataList[a].EnemyList)
                {
                    scene.sceneData.battleData.EnemyList.Add(enemy);
                }
                scene.GetComponent<Image>().sprite = SceneManager.Instance.sceneSpriteList[0];
                
            }//普通战斗
            if (scene.sceneData.type == SceneManager.SceneType.EliteCombat)
            {

                scene.sceneData.battleData = new BattleData();
                scene.sceneData.battleData.EnemyList = new List<BattleData.SceneEnemy>();
                foreach (var enemy in BattleManager.Instance.eliteBattleDataList[b].EnemyList)
                {
                    scene.sceneData.battleData.EnemyList.Add(enemy);
                }
                scene.GetComponent<Image>().sprite = SceneManager.Instance.sceneSpriteList[0];
                scene.GetComponent<Image>().sprite = SceneManager.Instance.sceneSpriteList[1];

            }//精英战斗

            if (scene.sceneData.type == SceneManager.SceneType.Event)//事件
            {
                
                scene.sceneData.eventID = Random.Range(0,EventManager.Instance.eventList.Count);

            }//精英战斗

        }

    }
    public void UpdateSceneState()
    {
        foreach (var gs in inGameSceneList)
        {
            int index = inGameSceneList.IndexOf(gs);
            if (index < 7)
            {
                gs.isOptional = false;
                continue;
            }

            if (inGameSceneList[index - 7].isFinished || inGameSceneList[index - 6].isFinished ||
                (index >= 8 && inGameSceneList[index - 8].isFinished))
            {
                gs.isOptional = true;
            }
        }

    }
    void Update()
    {
        
    }
}
