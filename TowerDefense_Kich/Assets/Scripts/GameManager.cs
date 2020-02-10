using UnityEngine;

// Class handles the beginning and the ending of game

public class GameManager : MonoBehaviour
{
    // True when player has no move lives left and game is over
    public static bool isGameOver;

    private void Start()
    {
        isGameOver = false;
    }

    private void Update()
    {
        if (isGameOver)
        {
            Debug.Log("Game is over");
            enabled = false;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}
