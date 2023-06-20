using UnityEngine;

public class GameObjectToggle : MonoBehaviour
{
    public GameObject[] gameObjects;
    public float toggleInterval = 10f;

    private bool[] gameObjectStates;
    private float timer;

    private void Start()
    {
        gameObjectStates = new bool[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjectStates[i] = true;
        }

        timer = toggleInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ToggleGameObjects();
            timer = toggleInterval;
        }
    }

    private void ToggleGameObjects()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjectStates[i] = !gameObjectStates[i];
            gameObjects[i].SetActive(gameObjectStates[i]);
        }
    }
}

