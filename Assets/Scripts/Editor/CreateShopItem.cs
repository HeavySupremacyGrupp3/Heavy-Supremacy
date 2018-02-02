using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

public class CreateShopItem : EditorWindow
{
    //Create and save a prefab with Image, Recttransform and Item scripts
    //Create according button for the item in the shop.
    //Assign the item and the button to the ShopSystem.

    private static GameObject ItemButton;
    private static GameObject PriceText;
    private static GameObject NameText;
    private static GameObject ItemPrefab;
    private static ShopSystem Shop;
    private static Transform BrowserWindow;

    [MenuItem("Shop/Create Shop Item")]
    static void ShowWindow()
    {
        GetWindow(typeof(CreateShopItem));

        ItemButton = new GameObject("Item_Button");
        ItemPrefab = new GameObject("Item_Item");
        PriceText = new GameObject("Price_Text");
        NameText = new GameObject("Name_Text");

        ItemButton.AddComponent<Image>();
        ItemButton.AddComponent<Button>();

        ItemPrefab.AddComponent<Furniture>();
        ItemPrefab.AddComponent<Image>();

        PriceText.AddComponent<Text>().text = "Price";
        NameText.AddComponent<Text>().text = "Name";
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Shop");
        Shop = EditorGUILayout.ObjectField(Shop, typeof(ShopSystem), true) as ShopSystem;
        EditorGUILayout.LabelField("Shop Browser Window");
        BrowserWindow = EditorGUILayout.ObjectField(BrowserWindow, typeof(Transform), true) as Transform;

        Furniture item = ItemPrefab.GetComponent<Furniture>();
        item.Name = EditorGUILayout.TextField("Name", item.Name);
        item.Description = EditorGUILayout.TextField("Description", item.Description);
        item.Price = EditorGUILayout.IntField("Price", item.Price);
        item.Type = Item.ItemType.Furniture; //(Item.ItemType)EditorGUILayout.EnumPopup("Type", item.Type);
        item.OneTimePurchase = EditorGUILayout.Toggle(item.OneTimePurchase, "One Time Purchase");

        EditorGUILayout.LabelField("Button Sprite");
        ItemButton.GetComponent<Image>().sprite = EditorGUILayout.ObjectField(ItemButton.GetComponent<Image>().sprite, typeof(Sprite), true) as Sprite;
        EditorGUILayout.LabelField("Item Sprite");
        ItemPrefab.GetComponent<Image>().sprite = EditorGUILayout.ObjectField(ItemPrefab.GetComponent<Image>().sprite, typeof(Sprite), true) as Sprite;

        if (GUILayout.Button("Create Item"))
            SpawnItem();
    }

    public void SpawnItem()
    {
        ItemButton.name = ItemPrefab.GetComponent<Furniture>().Name + "_Button";
        ItemPrefab.name = ItemPrefab.GetComponent<Furniture>().Name + "_Item";

        Debug.Log(ItemButton.name);
        ItemButton.GetComponent<Button>().onClick.AddListener(() => Shop.AtemptPurchase(ItemPrefab.GetComponent<Furniture>().Name));
        Shop.ShopButtons.Add(ItemButton.GetComponent<Button>());

        ItemButton.transform.SetParent(BrowserWindow, false);

        PriceText.transform.SetParent(ItemButton.transform, false);
        PriceText.GetComponent<Text>().color = Color.green;
        PriceText.GetComponent<Text>().fontSize = 41;
        PriceText.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
        PriceText.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
        PriceText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        PriceText.transform.localPosition = new Vector2(-8, 167);
        PriceText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);


        NameText.transform.SetParent(ItemButton.transform, false);
        NameText.GetComponent<Text>().color = Color.black;
        NameText.GetComponent<Text>().fontSize = 41;
        NameText.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
        NameText.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
        NameText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        NameText.transform.localPosition = new Vector2(0, -159);
        NameText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);

        ItemButton.transform.position = Vector3.zero;

        Object prefab = PrefabUtility.CreatePrefab("Assets/Prefabs/Items/" + ItemPrefab.name + ".prefab", ItemPrefab);
        GameObject item = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Items/" + ItemPrefab.name + ".prefab", typeof(GameObject)) as GameObject;
        Shop.ShopInventory.Add(item.GetComponent<Item>());
        DestroyImmediate(ItemPrefab);

        Close();
    }
}
