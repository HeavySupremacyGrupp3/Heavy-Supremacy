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

    private List<StoryNode> nodes = new List<StoryNode>();
    private List<StoryNode> choices = new List<StoryNode>();

    private void Start()
    {
        LoadEvents();
        TriggerEvent(nodes[0]);
    }

    void LoadEvents()
    {
        using (StreamReader s = new StreamReader("fil.txt"))
        {
            string line = s.ReadLine();
            while(line != null)
            {
                StoryNode node = JsonConvert.DeserializeObject<StoryNode>(line);
                nodes.Add(node);

                line = s.ReadLine();
            }
        }
    }

    public void TriggerEvent(StoryNode node)
    {
        Debug.Log(node.Title);

        EventPanel.SetActive(true);

        TitleText.text = node.Title;
        DescriptionText.text = node.Text;

        //choices.Clear();
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
