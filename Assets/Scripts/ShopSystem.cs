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
    public Image ProductImage;
    public Text ProductPrice;
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

        //Shitty fix for reseting price when restarting game.
        Lesson x = gameObject.AddComponent<Lesson>();
        bool foundLesson = false;
        foreach (Item l in MyInventory)
        {
            if (l.GetType() == x.GetType())
            {
                foundLesson = true;
                break;
            }
        }
        if (!foundLesson)
        {
            foreach (Item l in ShopInventory)
            {
                if (l.GetType() == x.GetType())
                {
                    ((Lesson)l).ResetPrice();
                }
            }
        }
        Destroy(x);

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
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse was clicked over a UI element, if so, play click-sound.
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                AudioManager.instance.Play("ShopMouseClick");
            }
        }
    }

    public void AtemptPurchase(string name)
    {
        if (GameObject.Find("Browser_1"))
            AreYouSurePanel.GetComponent<Image>().color = new Color(0.85f, 0.87f, 0.96f); 
        else if (GameObject.Find("Browser_2"))
            AreYouSurePanel.GetComponent<Image>().color = new Color(0.725f, 0.725f, 0.725f);

        itemToBePurchased = name;
        AreYouSurePanel.SetActive(true);
        Item item = FindItemByName(itemToBePurchased);
        ProductDescription.text = item.Description;
        ProductImage.sprite = item.ProductImage;
        RectTransform rectTransform = ProductImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(item.ProductImage.rect.width, item.ProductImage.rect.height);
        ProductPrice.text = item.Price.ToString();

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
            }
        }
        else
        {
            YesButton.interactable = false;
        }
    }

    public void MakePurchase()
    {
        Item item = FindItemByName(itemToBePurchased);
        //PlayPurchaseSound(item);

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
            PriceTexts[i].text = "" + ShopInventory[i].Price;
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
            MyInventory[i].UpdateFurniture();
        }
    }
}
