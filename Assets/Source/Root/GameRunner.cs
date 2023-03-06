using UnityEngine;

public class GameRunner : MonoBehaviour
{
    [SerializeField] public GameBootsrapper gameBootsrapperPrefab;

    private void Awake()
    {
        GameBootsrapper bootsrapper = FindObjectOfType<GameBootsrapper>();

        if (bootsrapper == null)
            Instantiate(gameBootsrapperPrefab);
    }
}
