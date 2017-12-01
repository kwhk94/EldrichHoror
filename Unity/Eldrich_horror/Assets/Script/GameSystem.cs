using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {
    private static GameSystem instance;
    public static GameSystem Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<GameSystem>();
            if (!instance)
                Debug.Log("GameSystem instance not find");
            return instance;
        }

        set { instance = value; }
    }
    
    
}
