using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

public class EventEnum : MonoBehaviour
{

    public StoryNode Node;
    public float EnergyCost = 0;
    public Text EnergyText;

    public GameEventManager.nodeType NodeType;

    private void Start()
    {
        NodeType = (GameEventManager.nodeType)Random.Range(0, 3);


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
