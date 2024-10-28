using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    { 
        get 
        { 
            if (instance == null)
            {
                instance = new GameObject().AddComponent<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance == this)
            {
                Destroy(gameObject);
            }
        }
    }

    private PlayerObject player;
    public PlayerObject Player { get { return player; }  set { player = value; } }
}
