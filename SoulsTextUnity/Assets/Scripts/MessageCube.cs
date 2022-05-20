using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageCube : MonoBehaviour
{
    [SerializeField]
    Transform cube;

    [SerializeField]
    public UiText uiText;

    [SerializeField]
    Transform parent;

    [SerializeField]
    public float X = 0;

    [SerializeField]
    public float Y = 0;

    [SerializeField]
    public float Z = 0;

    [SerializeField]
    public string Message = "";

    [SerializeField]
    int VoteCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(X, Y, Z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Put message up when hovering
    private void OnMouseOver()
    {
        uiText.SetUiText(Message);
    }

    //No longer putting up message
    private void OnMouseExit()
    {
        uiText.ResetUiText();
    }
}
