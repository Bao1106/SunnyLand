using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private AudioSource MenuTheme;

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}

