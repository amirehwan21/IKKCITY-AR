using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

    [SerializeField]
    private float units;

    [SerializeField]
    private Image fill;

    private float fillAmount;



	// Use this for initialization
	void Start () {

        StartCoroutine(BuildUnits());
		
	}
	
	// Update is called once per frame
	void Update () {

        UpdateBar();
		
	}

    public IEnumerator BuildUnits()
    {

        for (int i=0; i<=units; i++)
        {
            fillAmount = i / units;

            yield return null;
        }
    }

    private void UpdateBar()
    {
        fill.fillAmount = fillAmount;

    }

}
