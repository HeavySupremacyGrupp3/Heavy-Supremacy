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
    public Text TitleText, DescriptionText, SenderNameTitle;
    public GameObject[] PanelChoiceButtons;
    public GameObject ClosePanelButton;
    public GameObject[] SMSChoiceButtons;
    public GameObject RecievedMessagePrefab;
    public GameObject SentMessagePrefab;
    public GameObject SMSScrollContent;

    [Range(0, 1)]
    public float SpecialNodeChance = 0, MessageNodeChance = 0;

    public float RecieveMessageDelay = 0.75f;

    private List<StoryNode> choices = new List<StoryNode>();

    private List<StoryNode> messageNodes = new List<StoryNode>();
    private List<StoryNode> socialNodes = new List<StoryNode>();
    private List<StoryNode> musicNodes = new List<StoryNode>();
    private List<StoryNode> fameNodes = new List<StoryNode>();
    private List<StoryNode> specialNodes = new List<StoryNode>();

    [HideInInspector]
    public enum nodeType { social, musical, fame, special }

    private void Start()
    {
        LoadEvents("messageNodes", messageNodes);
        LoadEvents("socialNodes", socialNodes);
        LoadEvents("musicNodes", musicNodes);
        LoadEvents("fameNodes", fameNodes);
        LoadEvents("specialNodes", specialNodes);

        if (Random.Range(0f, 1f) <= SpecialNodeChance)
            TriggerSMSEvent(specialNodes[Random.Range(0, specialNodes.Count)]);
        if (Random.Range(0f, 1f) <= MessageNodeChance)
        {
            ClearSMSPanel();
            TriggerSMSEvent(messageNodes[Random.Range(0, messageNodes.Count)]);
        }
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

    public void TriggerSMSEvent(StoryNode node, bool firstNode = true)
    {
        if (firstNode)
        {
            AudioManager.instance.Play("MobilNotification");
            SenderNameTitle.text = node.Title;
        }
        if (node.Text == "" || node.Text == null)
            return;

        Debug.Log(node.Title);

        GameObject message = Instantiate(RecievedMessagePrefab, SMSScrollContent.transform, false);
        message.GetComponentInChildren<Text>().text = node.Text;

        choices.Clear();
        for (int i = 0; i < node.Choices.Count; i++)
        {

            choices.Add(node.Choices[i]);
            SMSChoiceButtons[i].SetActive(true);
            SMSChoiceButtons[i].transform.GetComponentInChildren<Text>().text = choices[i].Title;
        }

        GiveRewards(node);
    }

    public void ClearSMSPanel()
    {
        for (int i = SMSScrollContent.transform.childCount; i > 0; i--)
        {
            Destroy(SMSScrollContent.transform.GetChild(i));
        }
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
            PanelChoiceButtons[i].SetActive(true);
            PanelChoiceButtons[i].transform.GetComponentInChildren<Text>().text = choices[i].Title;
        }

        GiveRewards(node);

        if (!PanelChoiceButtons[0].activeSelf)
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
        if (node.MetalBonus != null && node.MetalBonus != "")
            FindObjectOfType<metalStatScript>().addOrRemoveAmount(float.Parse(node.MetalBonus));
        if (node.CashBonus != null && node.CashBonus != "")
            FindObjectOfType<moneyStatScript>().addOrRemoveAmount(float.Parse(node.CashBonus));
        if (node.AngstBonus != null && node.AngstBonus != "")
            FindObjectOfType<angstStatScript>().addOrRemoveAmount(float.Parse(node.AngstBonus));
        if (node.FameBonus != null && node.FameBonus != "")
            FindObjectOfType<fameStatScript>().addOrRemoveAmount(float.Parse(node.FameBonus));
        if (node.EnergyBonus != null && node.EnergyBonus != "")
            FindObjectOfType<energyStatScript>().addOrRemoveAmount(float.Parse(node.EnergyBonus));
    }

    public void MakeChoice(int index)
    {
        for (int i = 0; i < PanelChoiceButtons.Length; i++)
        {
            PanelChoiceButtons[i].SetActive(false);
        }

        TriggerEvent(choices[index]);
    }

    public void MakeSMSChoice(int index)
    {
        for (int i = 0; i < SMSChoiceButtons.Length; i++)
        {
            SMSChoiceButtons[i].SetActive(false);
        }

        GameObject message = Instantiate(SentMessagePrefab, SMSScrollContent.transform, false);
        message.GetComponentInChildren<Text>().text = choices[index].Title;

        StartCoroutine(WaitForSMS(index));
    }

    IEnumerator WaitForSMS(int index)
    {
        SMSScrollContent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0;
        yield return new WaitForSeconds(RecieveMessageDelay);

        TriggerSMSEvent(choices[index], false);

        yield return new WaitForEndOfFrame();
        SMSScrollContent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0;
    }
}
