using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
