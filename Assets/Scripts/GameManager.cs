using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int index;

    public static GameManager instance;

    //public GameObject[] boards;

    public Skateboard[] skateboards;

    public int selectedSkateboardIndex = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
    }

    private void Start()
    {
        index = PlayerPrefs.GetInt("boardIndex");

        //GameObject board = Instantiate(boards[index], Vector3.zero, Quaternion.identity);

    }

    public void SetSelectedSkateboard(int index)
    {
        selectedSkateboardIndex = Mathf.Clamp(index, 0, skateboards.Length - 1);
    }

    public Skateboard GetSelectedSkateboard()
    {
        int boardIndex = PlayerPrefs.GetInt("boardIndex", 0);
        return skateboards[boardIndex];
    }
}
