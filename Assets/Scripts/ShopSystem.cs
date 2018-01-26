using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public static List<Item> MyInventory = new List<Item>();
    public List<Item> ShopInventory = new List<Item>();
    public List<Button> ShopButtons = new List<Button>();
    public GameObject AreYouSurePanel;
    public Button YesButton;
    public Text ProductDescription;

    private moneyStatScript moneyStatScript;
    private string itemToBePurchased;

    void Start()
    {
        moneyStatScript = FindObjectOfType<moneyStatScript>();
        if (moneyStatScript.getAmount() == 0)
        {
            MyInventory.Clear();
        }

        UpdateShopUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
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

        moneyStatScript.addOrRemoveAmount(-item.Price);
        item.ActivatePurchase();
        MyInventory.Add(item);

        UpdateShopUI();

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
}
