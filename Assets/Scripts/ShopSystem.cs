using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public static List<Item> MyInventory = new List<Item>();
    //public static List<Furniture> MyFurnitures = new List<Furniture>();
    public List<Item> ShopInventory = new List<Item>();
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
            //MyFurnitures.Clear();
        }

        FetchShopUIElements();
        UpdateShopUI();
        UpdateHUBEnvironment();
    }

    void FetchShopUIElements()
    {

        for (int i = 0; i < ShopButtons.Count; i++)
        {
            for (int j = 0; j < ShopButtons[i].transform.childCount; j++)
            {
                Debug.Log(ShopButtons[i].transform.childCount);
                if (ShopButtons[i].transform.GetChild(j).name.Contains("Price"))
                    PriceTexts.Add(ShopButtons[i].transform.GetChild(j).GetComponent<Text>());

                if (ShopButtons[i].transform.GetChild(j).name.Contains("Name"))
                    NameTexts.Add(ShopButtons[i].transform.GetChild(j).GetComponent<Text>());
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
            FindObjectOfType<moneyStatScript>().addMoney();
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

        //if (item.Type == Item.ItemType.Item)
            MyInventory.Add(item);
        //else if (item.Type == Item.ItemType.Furniture)
        //    MyFurnitures.Add(item.gameObject.GetComponent<Furniture>());


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
            FindObjectOfType<GameManager>().GetComponent<AudioSource>().PlayOneShot(CheapPurchaseSound);
        else if (item.Price <= RegularTreshold)
            FindObjectOfType<GameManager>().GetComponent<AudioSource>().PlayOneShot(RegularPurchaseSound);
        else
            FindObjectOfType<GameManager>().GetComponent<AudioSource>().PlayOneShot(ExpensivePurchaseSound);
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
