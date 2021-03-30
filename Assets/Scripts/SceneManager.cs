using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
