using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using System.IO;
using UnityEngine.UI;
using Newtonsoft.Json;

public class GameEventManager : MonoBehaviour
{
    public GameObject EventPanel;
    public Text TitleText, DescriptionText;
    public GameObject[] ChoiceButtons;
    public GameObject ClosePanelButton;

    [Range(0, 1)]
    public float SpecialNodeChance = 0;

    private List<StoryNode> nodes = new List<StoryNode>();
    private List<StoryNode> choices = new List<StoryNode>();

    private List<StoryNode> socialNodes = new List<StoryNode>();
    private List<StoryNode> musicNodes = new List<StoryNode>();
    private List<StoryNode> fameNodes = new List<StoryNode>();
    private List<StoryNode> specialNodes = new List<StoryNode>();

    [HideInInspector]
    public enum nodeType { social, musical, fame, special }

    private void Start()
    {
        LoadEvents("fil", nodes);
        LoadEvents("socialNodes", socialNodes);
        LoadEvents("musicNodes", musicNodes);
        LoadEvents("fameNodes", fameNodes);
        LoadEvents("specialNodes", specialNodes);

        if (Random.Range(0f, 1f) <= SpecialNodeChance)
            TriggerEvent(specialNodes[Random.Range(0, specialNodes.Count)]);
    }

    void LoadEvents(string _fileName, List<StoryNode> _nodes)
    {

        using (StreamReader s = new StreamReader(_fileName + ".txt"))
        {
            string line = s.ReadLine();
            while (line != null)
            {
                StoryNode node = JsonConvert.DeserializeObject<StoryNode>(line);
                _nodes.Add(node);

                line = s.ReadLine();
            }
        }
    }

    public void TriggerEventFromPool(EventEnum eventEnum)
    {
        nodeType type = eventEnum.NodeType;

        if (type == nodeType.fame)
            TriggerEvent(fameNodes[Random.Range(0, fameNodes.Count)]);
        else if (type == nodeType.musical)
            TriggerEvent(musicNodes[Random.Range(0, musicNodes.Count)]);
        else if (type == nodeType.special)
            TriggerEvent(specialNodes[Random.Range(0, specialNodes.Count)]);
        else if (type == nodeType.social)
            TriggerEvent(socialNodes[Random.Range(0, socialNodes.Count)]);
    }

    public void TriggerEvent(StoryNode node)
    {
        Debug.Log(node.Title);

        EventPanel.SetActive(true);

        TitleText.text = node.Title;
        DescriptionText.text = node.Text;

        choices.Clear();
        for (int i = 0; i < node.Choices.Count; i++)
        {
            Debug.Log(node.Choices.Count);

            choices.Add(node.Choices[i]);
            ChoiceButtons[i].SetActive(true);
            ChoiceButtons[i].transform.GetComponentInChildren<Text>().text = choices[i].Title;
        }

        GiveRewards(node);

        if (!ChoiceButtons[0].activeSelf)
        {
            ClosePanelButton.SetActive(true);
        }
        else
        {
            ClosePanelButton.SetActive(false);
        }
    }

    void GiveRewards(StoryNode node)
    {
        FindObjectOfType<metalStatScript>().addOrRemoveAmount(float.Parse(node.MetalBonus));
        FindObjectOfType<moneyStatScript>().addOrRemoveAmount(float.Parse(node.CashBonus));
        FindObjectOfType<angstStatScript>().addOrRemoveAmount(float.Parse(node.AngstBonus));
        FindObjectOfType<fameStatScript>().addOrRemoveAmount(float.Parse(node.FameBonus));
        FindObjectOfType<energyStatScript>().addOrRemoveAmount(float.Parse(node.EnergyBonus));
    }

    public void MakeChoice(int index)
    {
        for (int i = 0; i < ChoiceButtons.Length; i++)
        {
            ChoiceButtons[i].SetActive(false);
        }

        Debug.Log(choices.Count);
        TriggerEvent(choices[index]);
    }
}
