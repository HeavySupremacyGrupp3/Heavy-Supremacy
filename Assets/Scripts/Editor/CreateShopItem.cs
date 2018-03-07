using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.Events;
using UnityEngine.Events;

public class CreateShopItem : EditorWindow
{
    //Create and save a prefab with Image, Recttransform and Item scripts
    //Create according button for the item in the shop.
    //Assign the item and the button to the ShopSystem.

    private UnityAction unityAction;

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

        ItemPrefab.GetComponent<Furniture>().Price = 10;
        ItemPrefab.GetComponent<Furniture>().Description = "";
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Shop");
        Shop = EditorGUILayout.ObjectField(Shop, typeof(ShopSystem), true) as ShopSystem;

        EditorGUILayout.LabelField("Shop Browser Window");
        BrowserWindow = EditorGUILayout.ObjectField(BrowserWindow, typeof(Transform), true) as Transform;

        if (Shop != null && BrowserWindow != null)
        {
            Furniture item = ItemPrefab.GetComponent<Furniture>();
            EditorGUILayout.LabelField("");
            item.Name = EditorGUILayout.TextField("Name", item.Name);
            if (item.Name != null && item.Name != "")
            {
                item.Description = EditorGUILayout.TextField("Description", item.Description);
                item.Price = EditorGUILayout.IntField("Price", item.Price);
                item.Type = Item.ItemType.Furniture; //(Item.ItemType)EditorGUILayout.EnumPopup("Type", item.Type);
                item.OneTimePurchase = EditorGUILayout.Toggle("One Time Purchase", item.OneTimePurchase);

                EditorGUILayout.LabelField("Button Sprite");
                ItemButton.GetComponent<Image>().sprite = EditorGUILayout.ObjectField(ItemButton.GetComponent<Image>().sprite, typeof(Sprite), true) as Sprite;
                EditorGUILayout.LabelField("Item Sprite");
                ItemPrefab.GetComponent<Image>().sprite = EditorGUILayout.ObjectField(ItemPrefab.GetComponent<Image>().sprite, typeof(Sprite), true) as Sprite;

                item.ProductImage = ItemPrefab.GetComponent<Image>().sprite;

                if (GUILayout.Button("Create Item"))
                    CreateItem();
            }
        }
    }

    public void CreateItem()
    {
        SetUpButtonDefaultSettings();
        SavePrefab();
        Close();
    }

    void SetUpButtonDefaultSettings()
    {
        ItemButton.name = ItemPrefab.GetComponent<Furniture>().Name + "_Button";
        ItemPrefab.name = ItemPrefab.GetComponent<Furniture>().Name + "_Item";

        AddOnClickEvent();
        Shop.ShopButtons.Add(ItemButton.GetComponent<Button>());

        ItemButton.transform.SetParent(BrowserWindow, false);


        PriceText.transform.SetParent(ItemButton.transform, false);
        Text t = PriceText.GetComponent<Text>();
        t.color = Color.green;
        t.fontSize = 41;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        t.alignment = TextAnchor.MiddleCenter;

        PriceText.transform.localPosition = new Vector2(-8, 167);
        PriceText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);


        NameText.transform.SetParent(ItemButton.transform, false);
        t = NameText.GetComponent<Text>();
        t.color = Color.black;
        t.fontSize = 41;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        t.alignment = TextAnchor.MiddleCenter;

        NameText.transform.localPosition = new Vector2(0, -159);
        NameText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);

        ItemButton.transform.position = Vector3.zero;
    }

    void AddOnClickEvent()
    {
        //Shitty solution below...
        ItemButton.AddComponent<AddButtonListener>().Name = ItemPrefab.GetComponent<Furniture>().Name;
        ItemButton.GetComponent<AddButtonListener>().Shop = Shop;

        //ItemButton.GetComponent<Button>().onClick.AddListener(() => Shop.AtemptPurchase(ItemPrefab.GetComponent<Furniture>().Name));

        //System.Type[] types = new System.Type[1];
        //types[0] = typeof(string);
        //System.Reflection.MethodInfo targetInfo = UnityEvent.GetValidMethodInfo(Shop.GetComponent<ShopSystem>(), "AtemptPurchase", types);
        //UnityAction methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), Shop.GetComponent<ShopSystem>(), targetInfo) as UnityAction;
        //ItemButton.GetComponent<Button>().onClick += Shop.AtemptPurchase("TEST");
        //unityAction += CallAtemptPurchase();
        //UnityEventTools.AddPersistentListener(ItemButton.GetComponent<Button>().onClick, new UnityAction(() => Shop.AtemptPurchase("Guitar")));
    }

    void SavePrefab()
    {
        Object prefab = PrefabUtility.CreatePrefab("Assets/Prefabs/Items/" + ItemPrefab.name + ".prefab", ItemPrefab);
        GameObject item = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Items/" + ItemPrefab.name + ".prefab", typeof(GameObject)) as GameObject;
        Shop.ShopInventory.Add(item.GetComponent<Item>());
        DestroyImmediate(ItemPrefab);
    }

    private void OnDestroy()
    {
        if (ItemButton.transform.parent == null)
        {
            DestroyImmediate(ItemButton);
            DestroyImmediate(ItemPrefab);
            DestroyImmediate(PriceText);
            DestroyImmediate(NameText);
        }
    }   
}
