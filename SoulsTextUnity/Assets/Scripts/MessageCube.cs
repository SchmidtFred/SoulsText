using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageCube : MonoBehaviour
{
    [SerializeField]
    Transform cube;

    [SerializeField]
    UiText uiText;

    [SerializeField]
    Transform parent;

    [SerializeField]
    float x = 0;

    [SerializeField]
    float y = 0;

    [SerializeField]
    float z = 0;

    [SerializeField]
    string message = "";

    [SerializeField]
    int voteCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Put message up when hovering
    private void OnMouseOver()
    {
        uiText.SetUiText(message);
    }

    //No longer putting up message
    private void OnMouseExit()
    {
        uiText.ResetUiText();
    }
}
