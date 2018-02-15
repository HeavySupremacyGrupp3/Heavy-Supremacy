using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopSystem : MonoBehaviour
{
    public static List<Item> MyInventory = new List<Item>();
    //[HideInInspector]
    public List<Item> ShopInventory = new List<Item>();
    //[HideInInspector]
    public List<Button> ShopButtons = new List<Button>();
    private List<Text> PriceTexts = new List<Text>();
    private List<Text> NameTexts = new List<Text>();

    public GameObject AreYouSurePanel;
    public Button YesButton;
    public Text ProductDescription;
    public AudioClip ExpensivePurchaseSound;
    public AudioClip RegularPurchaseSound;
    public AudioClip CheapPurchaseSound;
    public int ExpensiveTreshold = 50;
    public int RegularTreshold = 25;
    public int CheapTreshold = 1;

    private moneyStatScript moneyStatScript;
    private string itemToBePurchased;

    void OnEnable()
    {
        moneyStatScript = FindObjectOfType<moneyStatScript>();
        if (moneyStatScript.getAmount() == 0)
        {
            MyInventory.Clear();
        }

        FetchShopUIElements();
    }

    void FetchShopUIElements()
    {
        ShopButtons.RemoveAll(Button => Button == null);
        ShopInventory.RemoveAll(Item => Item == null);

        for (int i = 0; i < ShopButtons.Count; i++)
        {
            for (int j = 0; j < ShopButtons[i].transform.childCount; j++)
            {
                if (ShopButtons[i].transform.GetChild(j).name.Contains("Price"))
                    PriceTexts.Add(ShopButtons[i].transform.GetChild(j).GetComponent<Text>());

                if (ShopButtons[i].transform.GetChild(j).name.Contains("Name"))
                    NameTexts.Add(ShopButtons[i].transform.GetChild(j).GetComponent<Text>());
            }
        }

        UpdateShopUI();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
            FindObjectOfType<moneyStatScript>().addOrRemoveAmount(100);
    }

    public void AtemptPurchase(string name)
    {
        itemToBePurchased = name;
        AreYouSurePanel.SetActive(true);
        Item item = FindItemByName(itemToBePurchased);
        ProductDescription.text = item.Description;
        //Sufficient funds.
        if (moneyStatScript.getAmount() - item.Price >= 0)
        {
            //Either the item has not been purched but can be purched once. Or the item can be purched multiple times.
            if (item.OneTimePurchase && !MyInventory.Contains(item) || !item.OneTimePurchase)
            {
                YesButton.interactable = true;
            }
            else
            {
                YesButton.interactable = false;
                Debug.Log("THE ITEM IS UNIQUE");
            }
        }
        else
        {
            YesButton.interactable = false;
            Debug.Log("TOO POOR");
        }
    }

    public void MakePurchase()
    {
        Item item = FindItemByName(itemToBePurchased);
        PlayPurchaseSound(item);

        moneyStatScript.addOrRemoveAmount(-item.Price);
        item.ActivatePurchase();

        MyInventory.Add(item);

        UpdateShopUI();
        UpdateHUBEnvironment();
    }

    Item FindItemByName(string name)
    {
        for (int i = 0; i < ShopInventory.Count; i++)
        {
            if (ShopInventory[i].Name == name)
            {
                return ShopInventory[i];
            }
        }

        return null;
    }

    void UpdateShopUI()
    {
        UpdateShopPriceTexts();
        UpdateShopSprites();
        UpdateShopNameTexts();
    }

    void UpdateShopPriceTexts()
    {
        for (int i = 0; i < ShopInventory.Count; i++)
        {
            PriceTexts[i].text = "$" + ShopInventory[i].Price;
        }
    }

    void UpdateShopSprites()
    {
        for (int i = 0; i < ShopButtons.Count; i++)
        {
            if (ShopInventory[i].OneTimePurchase && MyInventory.Contains(ShopInventory[i]) || FindObjectOfType<moneyStatScript>().getAmount() - ShopInventory[i].Price < 0)
            {
                ShopButtons[i].GetComponent<Image>().color = Color.black;
            }
            else
            {
                ShopButtons[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    void UpdateShopNameTexts()
    {
        for (int i = 0; i < ShopInventory.Count; i++)
        {
            NameTexts[i].text = ShopInventory[i].Name;
        }
    }

    void PlayPurchaseSound(Item item)
    {
        //In order: cheap, regular, expensive.
        if (item.Price <= CheapTreshold)
            AudioManager.instance.Play("ShopCheapPurchase");
        else if (item.Price <= RegularTreshold)
            AudioManager.instance.Play("ShopRegularPurchase");
        else
            AudioManager.instance.Play("ShopExpensivePurchase");
    }

    public static void UpdateHUBEnvironment()
    {
        for (int i = 0; i < MyInventory.Count; i++)
        {
            if (MyInventory[i].Type == Item.ItemType.Furniture)
                MyInventory[i].UpdateFurniture();
        }
    }
}
