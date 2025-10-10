using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float TIME_STOPPED = 0.0f;

    private void Start()
    {
        Time.timeScale = TIME_STOPPED;
    }
}
