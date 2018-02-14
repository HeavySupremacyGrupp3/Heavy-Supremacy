using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

public class EventEnum : MonoBehaviour
{
    public GameEventManager.nodeType NodeType = GameEventManager.nodeType.fame;

    public StoryNode Node;
    public float EnergyCost = 0;
    public Text EnergyText;

    private void Start()
    { 
        if (NodeType == GameEventManager.nodeType.fame)
            Node = GameEventManager.fameNodes[Random.Range(0, GameEventManager.fameNodes.Count)];
        else if (NodeType == GameEventManager.nodeType.musical)
            Node = GameEventManager.musicNodes[Random.Range(0, GameEventManager.musicNodes.Count)];
        else if (NodeType == GameEventManager.nodeType.social)
            Node = GameEventManager.socialNodes[Random.Range(0, GameEventManager.socialNodes.Count)];

        EnergyCost = float.Parse(Node.EnergyBonus);
        EnergyText.text = "Energy Cost: " + EnergyCost;

    }
}
