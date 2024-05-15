using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsPaused = !IsPaused;
        }
    }
}
