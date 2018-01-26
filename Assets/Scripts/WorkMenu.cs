using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkMenu : MonoBehaviour {


    public MiniGameManager mgm;
    public GameObject menu;

	void Start ()
    {
        mgm = GetComponent<MiniGameManager>();
        menu.SetActive(false);
    }
	
	public void Clicked()
    {
        Debug.Log("Menu is clicked");
        mgm = FindObjectOfType<MiniGameManager>();
        mgm.MenuIsClicked();
        menu.SetActive(true);
        menu = Instantiate(menu, new Vector3(0, 0), Quaternion.identity);

    }
}
