using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public bool isOptional;//�Ƿ��ѡ
    public bool isStarted;//�Ƿ��ǿ���
    public bool isFinished;//�Ƿ������
    public SceneData sceneData;
    public Text typeText;//������ʾ�¼����͵��ı�
    public void OnPointerDown()
    {
        if (!isOptional||isFinished)
            return;
        switch (sceneData.type)
        {
            case SceneManager.SceneType.NormalCombat:
            case SceneManager.SceneType.EliteCombat:

                BattleManager.Instance.BattleStart(sceneData.battleData);
                break;
        }

        isFinished = true;
        SceneManager.Instance.UpdateSceneState();
    }
    void Start()
    {

        InitScene();
    }

    public void InitScene()
    {
        List<GameScene> gameScenes = SceneManager.Instance.inGameSceneList;
        int index = gameScenes.IndexOf(this);
        if (index < 7)
            isOptional = true;
        sceneData = SceneManager.Instance.sceneDataList[Random.Range(0, SceneManager.Instance.sceneDataList.Count)];
        if (sceneData.type==SceneManager.SceneType.NormalCombat)
        {
            sceneData.battleData = BattleManager.Instance.normalBattleDataList[Random.Range(0,BattleManager.Instance.normalBattleDataList.Count)];
            GetComponent<Image>().sprite = SceneManager.Instance.sceneSpriteList[0];
            return ;
        }
        if (sceneData.type == SceneManager.SceneType.EliteCombat)
        {
            sceneData.battleData = BattleManager.Instance.eliteBattleDataList[Random.Range(0, BattleManager.Instance.eliteBattleDataList.Count)];
            GetComponent<Image>().sprite = SceneManager.Instance.sceneSpriteList[1];

            return;
        }
    }

    
    void Update()
    {


    }
}
