using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [Tooltip("The UI text to change when a cube is hovered")]
    public UiText UiText;

    [Tooltip("The messgage cube we will be making")]
    public MessageCube cubePrefab;

    public List<MessageCube> cubes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //This method is for the creation of a cube
    public void CreateCube(MessageCube cube)
    {
        MessageCube newCube = Instantiate(cubePrefab, this.transform);
        newCube.X = cube.X;
        newCube.Y = cube.Y;
        newCube.Z = cube.Z;
        newCube.Message = cube.Message;
        newCube.uiText = this.UiText;
        cubes.Add(newCube);
    }

    // This Method is for creating all cubes once they have been received from making initial connection
    public void InitializeCubes()
    {
        foreach (MessageCube cube in cubes)
        {
            CreateCube(cube);
        }
    }
}
