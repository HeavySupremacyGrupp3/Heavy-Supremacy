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
    public Sprite FameSprite, MusicSprite, SocialSprite;

    public void RefreshEvent()
    {
        if (GetComponent<Button>().interactable)
            return;

        GetComponent<Button>().interactable = true;

        NodeType = (GameEventManager.nodeType)Random.Range(0, 3);


        if (NodeType == GameEventManager.nodeType.fame)
        {
            Node = GameEventManager.fameNodes[Random.Range(0, GameEventManager.fameNodes.Count)];
            GetComponent<Image>().sprite = FameSprite;
        }
        else if (NodeType == GameEventManager.nodeType.musical)
        {
            Node = GameEventManager.musicNodes[Random.Range(0, GameEventManager.musicNodes.Count)];
            GetComponent<Image>().sprite = MusicSprite;
        }
        else if (NodeType == GameEventManager.nodeType.social)
        {
            Node = GameEventManager.socialNodes[Random.Range(0, GameEventManager.socialNodes.Count)];
            GetComponent<Image>().sprite = SocialSprite;
        }

        if (Node.EnergyBonus != null && Node.EnergyBonus != "")
            EnergyCost = float.Parse(Node.EnergyBonus);

        EnergyText.text = "Energy Cost: " + EnergyCost;
    }

    private void OnDestroy()
    {
        GameManager.sleep -= RefreshEvent;
    }
}
