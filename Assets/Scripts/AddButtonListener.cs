using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButtonListener : MonoBehaviour {

    public string Name;
    [HideInInspector]
    public ShopSystem Shop;

	void Start ()
    {
        GetComponent<Button>().onClick.AddListener(() => Shop.AtemptPurchase(Name));
    }
}
