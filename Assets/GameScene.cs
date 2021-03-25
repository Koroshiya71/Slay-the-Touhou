using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public SceneData sceneData;

    public void OnPointerDown()
    {
        switch (sceneData.type)
        {
            case SceneManager.SceneType.NormalCombat:
                BattleManager.Instance.BattleStart(sceneData.battleData);
                break;
        }
    }
    void Start()
    {
        sceneData = SceneManager.Instance.sceneDataList[Random.Range(0, SceneManager.Instance.sceneDataList.Count)];
    }

    void Update()
    {
        
    }
}
