using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public List<GameEvent> eventList = new List<GameEvent>();//�¼��б�
    public Canvas eventCanvas;//�¼�����
    void Start()
    {
        eventCanvas.enabled = false;
    }

    void Update()
    {
        
    }
}
