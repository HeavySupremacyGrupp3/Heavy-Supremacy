using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : Item
{
    public GameObject[] FurnitureToInstantiate;
    public string[] GameObjectToDestroy;
    public string GameObjectToReplace;
    private Vector3 myTransform;

    public override void UpdateFurniture()
    {
        FindPosition();
        foreach (GameObject go in FurnitureToInstantiate)
            AddGameObject(go);
        foreach (string s in GameObjectToDestroy)
            RemoveGameObject(s);
    }

    void FindPosition()
    {
        RectTransform myRect = GetComponent<RectTransform>();
        myTransform = new Vector3(myRect.position.x, myRect.position.y, 0);

        myTransform = Camera.main.ViewportToScreenPoint(transform.position);
    }

    void AddGameObject(GameObject go)
    {
        if (go != null && CheckForDuplicates())
        {
            GameObject goTemp = Instantiate(go, GameObject.Find("Furniture").transform, false) as GameObject;
            goTemp.name = go.name;
        }
    }

    void RemoveGameObject(string name)
    {
        if (name != null)
            Destroy(GameObject.Find(name));
    }

    void ReplaceSprite(string name)
    {
        if (name != null && name != "")
        {
            Button goReplaceable = GameObject.Find(name).GetComponent<Button>();
            goReplaceable.image.sprite = GetComponent<Image>().sprite;

            goReplaceable.transform.localPosition = transform.position;
            goReplaceable.transform.localRotation = transform.rotation;
            goReplaceable.transform.localScale = transform.localScale;
            goReplaceable.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
        }
    }

    bool CheckForDuplicates()
    {
        Furniture[] furnitures = FindObjectsOfType<Furniture>();

        foreach (Furniture f in furnitures)
        {
            if (f.Name == Name)
                return false;
        }
        return true;
    }
}
