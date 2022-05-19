using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiText : MonoBehaviour
{
    public Text myText;
    // Start is called before the first frame update
    void Start()
    {
        //Set base text as empty string
        myText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUiText(string text)
    {
        myText.text = text;
    }

    public void ResetUiText()
    {
        myText.text = "";
    }    
}
