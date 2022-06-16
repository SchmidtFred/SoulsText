using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public List<UserProfile> allUsers;
    public UserProfile user;
    public CubeManager cubeManager;


    // Start is called before the first frame update
    void Start()
    {
        //Initialize SignalR
        var connection = new SignalR();
        connection.Init("http://localhost:5000/SoulsHub");

        //Handler Callbacks
        connection.On("ReceiveAllUsers", (string payload) =>
        {
            // Deserialize the payload from JSON
            allUsers = JsonUtility.FromJson<List<UserProfile>>(payload);
            //logging goes here
            Debug.Log($"ReceiveAllUsers has occured");
        });

        connection.On("ReceiveAllMessages", (string payload) =>
        {
            //Deserialize from JSON
            cubeManager.cubes = JsonUtility.FromJson<List<MessageCube>>(payload);
            //Logging
            Debug.Log($"ReceiveAllMessages has occured");
            //Instantiate all Cubes
            cubeManager.InitializeCubes();
            //Logging
            Debug.Log($"Cubes have been initialized in scene");
        });

        connection.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
