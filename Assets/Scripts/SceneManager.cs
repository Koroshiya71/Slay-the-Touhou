using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneManager : MonoBehaviour
{
    public List<GameScene> inGameSceneList=new List<GameScene>();//��Ϸ�еĳ����б�
    public static SceneManager Instance;
    public Canvas battleSceneCanvas;//ս����������
    public Canvas mapSceneCanvas;//��ͼ��������
    private void Awake()
    {
        Instance = this;
    }

    public enum SceneType//��������
    {
        NormalCombat,//��ͨս��
        EliteCombat,//��Ӣս��
        Event,//�¼�
        Store//�̵�
    }
    public List<Sprite> sceneSpriteList = new List<Sprite>();//����ͼƬ��sprite����
    public List<SceneData> sceneDataList = new List<SceneData>();//�������г����������ݵ��б�
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
                
            }//��ͨս��
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

            }//��Ӣս��

            if (scene.sceneData.type == SceneManager.SceneType.Event)//�¼�
            {
                
                scene.sceneData.eventID = Random.Range(0,EventManager.Instance.eventList.Count);

            }//��Ӣս��

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
