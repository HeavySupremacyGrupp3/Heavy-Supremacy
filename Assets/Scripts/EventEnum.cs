using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

public class EventEnum : MonoBehaviour
{

    public StoryNode Node;
    public Text EnergyText;

    public GameEventManager.nodeType NodeType;
    public Sprite FameSprite, MusicSprite, SocialSprite;
    public StatPreviewData[] PreviewData;

    private float EnergyCost = 0, FameBonus = 0, MetalBonus = 0, AngstBonus = 0;

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
        if (Node.FameBonus != null && Node.FameBonus != "")
            FameBonus = float.Parse(Node.FameBonus);
        if (Node.MetalBonus != null && Node.MetalBonus != "")
            MetalBonus = float.Parse(Node.MetalBonus);
        if (Node.AngstBonus != null && Node.AngstBonus != "")
            AngstBonus = float.Parse(Node.AngstBonus);

        EnergyText.text = "Energy Cost: " + EnergyCost;

        PreviewData[0].Value = EnergyCost;
        PreviewData[1].Value = FameBonus;
        PreviewData[2].Value = MetalBonus;
        PreviewData[3].Value = AngstBonus;
    }

    public void UpdateStatBars()
    {
        for (int i = 0; i < PreviewData.Length; i++)
        {
            FindObjectOfType<GameManager>().UpdateStatPreviewFill(PreviewData[i]);
        }
    }

    public void ResetBars()
    {
        FindObjectOfType<GameManager>().ResetAllStatPreviews();
    }

    private void OnDestroy()
    {
        GameManager.sleep -= RefreshEvent;
    }
}
