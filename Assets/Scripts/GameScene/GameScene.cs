using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public bool isOptional;//是否可选
    public bool isStarted;//是否是开局
    public bool isFinished;//是否已完成
    public SceneData sceneData;

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
            case SceneManager.SceneType.Event:
                EventManager.Instance.StartEvent(sceneData.eventID);
                break;
        }

        isFinished = true;
        int index=SceneManager.Instance.inGameSceneList.IndexOf(this);
        SceneManager.Instance.UpdateSceneState();
        
    }
    void Start()
    {

    }


    void Update()
    {


    }
}
