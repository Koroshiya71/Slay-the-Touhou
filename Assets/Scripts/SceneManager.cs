using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
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
        
    }

    void Update()
    {
        
    }
}
