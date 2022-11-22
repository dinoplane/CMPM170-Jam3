using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPlaceholder : MonoBehaviour
{
    bool turnOver = false;
    Button buttonComponent;

    // Start is called before the first frame update
    void Start()
    {
        buttonComponent = this.GetComponent<Button>();
    }


    public void TakeTurn()
    {
        turnOver = true;
        buttonComponent.interactable = false;
        this.GetComponent<Image>().color = Color.blue;
    }

    public void MakeActive()
    {
        turnOver = true;
        buttonComponent.interactable = true;
        this.GetComponent<Image>().color = Color.white;
    }
}
