using System;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController Instance {get; private set;}

    public Action OnSceneCleanup;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SceneCleanUp()
    {
        OnSceneCleanup?.Invoke();
    }
}