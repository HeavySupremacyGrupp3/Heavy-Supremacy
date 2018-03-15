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
    public Sprite FameSprite, MusicSprite, SocialSprite, MoneySprite;
    public StatPreviewData[] PreviewData;

    private float EnergyCost = 0, FameBonus = 0, MetalBonus = 0, AngstBonus = 0, MoneyBonus = 0;

    private float  SocialChance = 0, MusicChance = 0, SpecialChance = 0, MoneyChance = 0;

    private float MoneySpriteWeightDivider = 6;

    public void StartEventEnum()
    {
        if (NodesSelected[NodeIndex] != null)
        {
            Debug.Log("BUTTON ON");
            GetComponent<Button>().interactable = true;
            Node = NodesSelected[NodeIndex];

            if (!string.IsNullOrEmpty(Node.EnergyBonus))
                EnergyCost = float.Parse(Node.EnergyBonus);

            EnergyText.text = "Energy Cost: " + EnergyCost;

            PreviewData[0].Value = EnergyCost;
        }
    }

    public void RefreshEvent()
    {
        if (this == null)
            return;

        if (GameManager.day == 7 || GameManager.day == 4 || !GetComponent<Button>().interactable && NodesSelected[NodeIndex] == null)
        {
            GetComponent<Button>().interactable = true;

            ChooseStatPool();

            if (NodeType == GameEventManager.nodeType.musical)
            {
                Node = GameEventManager.musicNodes[Random.Range(0, GameEventManager.musicNodes.Count)];
            }
            else if (NodeType == GameEventManager.nodeType.social)
            {
                Node = GameEventManager.socialNodes[Random.Range(0, GameEventManager.socialNodes.Count)];
            }
            else if (NodeType == GameEventManager.nodeType.special)
            {
                Node = GameEventManager.specialNodes[Random.Range(0, GameEventManager.specialNodes.Count)];
            }
            else if (NodeType == GameEventManager.nodeType.money)
            {
                Node = GameEventManager.moneyNodes[Random.Range(0, GameEventManager.moneyNodes.Count)];
            }

            if (Node != null)
                NodesSelected[NodeIndex] = Node;


            if (!string.IsNullOrEmpty(Node.EnergyBonus))
                EnergyCost = float.Parse(Node.EnergyBonus);
            if (!string.IsNullOrEmpty(Node.FameBonus))
                FameBonus = float.Parse(Node.FameBonus);
            if (!string.IsNullOrEmpty(Node.MetalBonus))
                MetalBonus = float.Parse(Node.MetalBonus);
            if (!string.IsNullOrEmpty(Node.AngstBonus))
                AngstBonus = float.Parse(Node.AngstBonus);
            if (!string.IsNullOrEmpty(Node.CashBonus))
                MoneyBonus = float.Parse(Node.CashBonus);


            AngstBonus *= -1; //Positive angst is bad and negative angst is good.

            float highestBonus = Mathf.Max(MetalBonus, FameBonus, AngstBonus, MoneyBonus / MoneySpriteWeightDivider);
            if (highestBonus == MetalBonus)
                GetComponent<Image>().sprite = MusicSprite;
            if (highestBonus == AngstBonus)
                GetComponent<Image>().sprite = SocialSprite;
            if (highestBonus == FameBonus)
                GetComponent<Image>().sprite = FameSprite;
            if (highestBonus == MoneyBonus)
                GetComponent<Image>().sprite = MoneySprite;

            Debug.Log(GetComponent<Image>().sprite);
            EnergyText.text = "Energy Cost: " + EnergyCost;

            PreviewData[0].Value = EnergyCost;
            //PreviewData[1].Value = FameBonus;
            //PreviewData[2].Value = MetalBonus;
            //PreviewData[3].Value = AngstBonus;
        }
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
        SocialChance = FindObjectOfType<angstStatScript>().LastWeeksStatGain / FindObjectOfType<GameEventManager>().SocialWeekCap;
        MusicChance = FindObjectOfType<metalStatScript>().LastWeeksStatGain / FindObjectOfType<GameEventManager>().MusicWeekCap;
        MoneyChance = FindObjectOfType<moneyStatScript>().LastWeeksStatGain / FindObjectOfType<GameEventManager>().MoneyCap;
        SpecialChance = FindObjectOfType<GameEventManager>().SpecialNodeChance;

        float sum =  SocialChance + MusicChance + MoneyChance;
        if (sum <= 0)
        {
            SocialChance = 1;
            MusicChance = 1;
            MoneyChance = 1;
            sum =  SocialChance + MusicChance + MoneyChance;
        }

        float socialRng = Mathf.Clamp((SocialChance / sum) * (1 - SpecialChance), 0, 1);
        float musicalRng = Mathf.Clamp((MusicChance / sum) * (1 - SpecialChance), 0, 1);
        float moneyRng = Mathf.Clamp((MoneyChance / sum) * (1 - SpecialChance), 0, 1);

        float rng = Random.Range(0f, 1f);

        if (rng < socialRng)
            NodeType = GameEventManager.nodeType.social;
        else if (rng < socialRng + musicalRng)
            NodeType = GameEventManager.nodeType.musical;
        else if (rng < socialRng + musicalRng + moneyRng)
            NodeType = GameEventManager.nodeType.money;
        else
            NodeType = GameEventManager.nodeType.special;


        Debug.Log("SocialRNG: " + socialRng + " MusicalRNG: " + musicalRng + " SpecialRNG: " + SpecialChance + " MoneyRNG: " + moneyRng + " NodeType: " + NodeType + " RNG: " + rng);
    }

    private void OnDestroy()
    {
        GameManager.sleep -= RefreshEvent;
    }
}
