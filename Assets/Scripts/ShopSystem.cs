using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public static List<Item> MyInventory = new List<Item>();
    public List<Item> ShopInventory = new List<Item>();
    public List<Button> ShopButtons = new List<Button>();

    private moneyStatScript moneyStatScript;

    void Start()
    {
        moneyStatScript = FindObjectOfType<moneyStatScript>();
        UpdateShopUI();
    }

    public void MakePurchase(int index)
    {
        Item item = ShopInventory[index];
        if (moneyStatScript.getAmount() - item.Price >= 0)
        {
            moneyStatScript.addOrRemoveAmount(-item.Price);
            item.ActivatePurchase();
            MyInventory.Add(item);

            UpdateShopUI();
        }
        else
        {
            Debug.Log("TOO POOR");
        }
    }

    void UpdateShopUI()
    {
        for (int i = 0; i < ShopButtons.Count; i++)
        {
            if (ShopInventory[i].OneTimePurchase && MyInventory.Contains(ShopInventory[i]) || FindObjectOfType<metalStatScript>().getAmount() - ShopInventory[i].Price < 0)
                ShopButtons[i].interactable = false;
        }
    }
}
