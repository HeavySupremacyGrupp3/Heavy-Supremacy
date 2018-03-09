using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

public class EventEnum : MonoBehaviour
{
    public static StoryNode[] NodesSelected = new StoryNode[3];
    public int NodeIndex;
    public StoryNode Node;
    public Text EnergyText;

    public GameEventManager.nodeType NodeType;
    public Sprite FameSprite, MusicSprite, SocialSprite, SpecialSprite;
    public StatPreviewData[] PreviewData;

    private float EnergyCost = 0, FameBonus = 0, MetalBonus = 0, AngstBonus = 0;

    private float FameChance = 0, SocialChance = 0, MusicChance = 0, SpecialChance = 0;

    private void Start()
    {
        GameManager.sleep += RefreshEvent;
    }

    public void RefreshEvent()
    {
        if (GameManager.day % 7 == 0)
            goto skipCommonDayCheck;
        Debug.Log(GameManager.day % 3);
        if (GameManager.day % 3 != 0 || GetComponent<Button>().interactable) //If the event is not clicked, do not update it, unless it's the correct day.
            return;

        skipCommonDayCheck:

        GetComponent<Button>().interactable = true;

        ChooseStatPool();

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
        else if (NodeType == GameEventManager.nodeType.special)
        {
            Node = GameEventManager.specialNodes[Random.Range(0, GameEventManager.specialNodes.Count)];
            GetComponent<Image>().sprite = SpecialSprite;
        }

        if (Node != null)
            NodesSelected[NodeIndex] = Node;

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
        //PreviewData[1].Value = FameBonus;
        //PreviewData[2].Value = MetalBonus;
        //PreviewData[3].Value = AngstBonus;
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

    private void ChooseStatPool()
    {
        FameChance = FindObjectOfType<fameStatScript>().LastWeeksStatGain / FindObjectOfType<fameStatScript>().WeightWeeklyIncreaseCap;
        SocialChance = FindObjectOfType<angstStatScript>().LastWeeksStatGain / FindObjectOfType<angstStatScript>().WeightWeeklyIncreaseCap;
        MusicChance = FindObjectOfType<metalStatScript>().LastWeeksStatGain / FindObjectOfType<metalStatScript>().WeightWeeklyIncreaseCap;
        SpecialChance = FindObjectOfType<GameEventManager>().SpecialNodeChance;

        float sum = FameChance + SocialChance + MusicChance;
        if (sum <= 0)
            sum = 1;

        float fameRng = Mathf.Clamp((FameChance / sum) * (1 - SpecialChance), 0, 1);
        float socialRng = Mathf.Clamp((SocialChance / sum) * (1 - SpecialChance), 0, 1);
        float musicalRng = Mathf.Clamp((MusicChance / sum) * (1 - SpecialChance), 0, 1);

        float rng = Random.Range(0f, 1f);

        if (rng < fameRng)
            NodeType = GameEventManager.nodeType.fame;
        else if (rng < fameRng + socialRng)
            NodeType = GameEventManager.nodeType.social;
        else if (rng < fameRng + socialRng + musicalRng)
            NodeType = GameEventManager.nodeType.musical;
        else
            NodeType = GameEventManager.nodeType.special;


        Debug.Log("FameRNG: " + fameRng + " SocialRNG: " + socialRng + " MusicalRNG: " + musicalRng + " SpecialRNG: " + SpecialChance + " NodeType: " + NodeType + " RNG: " + rng);
    }

    private void OnDestroy()
    {
        GameManager.sleep -= RefreshEvent;
    }
}
