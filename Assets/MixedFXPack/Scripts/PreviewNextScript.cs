using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewNextScript : MonoBehaviour {

    GameObject particleSystems;
    int childIndex = 0;

    Text nameText;

	void Start ()
    {
        nameText = GameObject.Find("Name").GetComponent<Text>();

		particleSystems = GameObject.Find("ParticleSystems");
        if (particleSystems == null)
            particleSystems = this.gameObject;

        for (int i = 1; i < particleSystems.transform.childCount; i++)
        {
            particleSystems.transform.GetChild(i).gameObject.SetActive(false);
        }
	}
	
	void Update ()
    {
        nameText.text = particleSystems.transform.GetChild(childIndex).name;
	}

    public void NextParticleSystem(bool forward)
    {
        particleSystems.transform.GetChild(childIndex).gameObject.SetActive(false);

        if (forward)
            childIndex++;
        else if (!forward)
            childIndex--;

        if (childIndex > particleSystems.transform.childCount - 1)
            childIndex = 0;
        else if (childIndex < 0)
            childIndex = particleSystems.transform.childCount - 1;

        particleSystems.transform.GetChild(childIndex).gameObject.SetActive(true);
    }

    public void Replay()
    {
        particleSystems.transform.GetChild(childIndex).gameObject.SetActive(false);
        particleSystems.transform.GetChild(childIndex).gameObject.SetActive(true);
    }
}
