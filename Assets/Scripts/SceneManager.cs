using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
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
        
    }

    void Update()
    {
        
    }
}
