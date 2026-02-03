using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject[] heartPlayer;
    private int currentHeartIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentHeartIndex = heartPlayer.Length;
    }
    public void TakeDamage()
    {
        if (currentHeartIndex > 0)
        {
            currentHeartIndex--;
            heartPlayer[currentHeartIndex].SetActive(false);
        }
    }
}
