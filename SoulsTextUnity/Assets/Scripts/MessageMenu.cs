using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MessageMenu : MonoBehaviour
{
    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public GameObject MenuRoot;

    [Tooltip("Component for managing X position")]
    public TMP_InputField X;

    [Tooltip("Component for managing Y position")]
    public TMP_InputField Y;

    [Tooltip("Component for managing Z position")]
    public TMP_InputField Z;

    [Tooltip("Component for writing Message")]
    public TMP_InputField Message;

    [Tooltip("Component for Create Button")]
    public Button SaveButton;

    [Tooltip("The parent for created cubes")]
    public CubeManager cubeParent;

    [Tooltip("The messgage cube we will be making")]
    public MessageCube cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        MenuRoot.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetMessageMenuActivation(!MenuRoot.activeSelf);
        }
    }

    public void ClosePauseMenu()
    {
        SetMessageMenuActivation(false);
    }

    void SetMessageMenuActivation(bool active)
    {
        MenuRoot.SetActive(active);

        if (!MenuRoot.activeSelf)
        {
            //empty fields on closing of menu
            X.text = "";
            Y.text = "";
            Z.text = "";
            Message.text = "";
        }
    }

    public void CreateCube()
    {
        if (!string.IsNullOrWhiteSpace(Message.text))
        {
            MessageCube cube = Instantiate(cubePrefab, cubeParent.transform);
            cube.X = float.Parse(string.IsNullOrWhiteSpace(X.text) ? "0" : X.text);
            cube.Y = float.Parse(string.IsNullOrWhiteSpace(Y.text) ? "0" : Y.text);
            cube.Z = float.Parse(string.IsNullOrWhiteSpace(Z.text) ? "0" : Z.text);
            cube.Message = Message.text;
            cube.uiText = cubeParent.UiText;
            cubeParent.cubes.Add(cube);
        }
    }
}
