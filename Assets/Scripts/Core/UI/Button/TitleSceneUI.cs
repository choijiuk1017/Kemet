using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneUI : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(()=> OnClickStartButton());
        exitButton.onClick.AddListener(()=> OnClickExitButton());   
    }

    void OnClickStartButton()
    {
        SceneManager.LoadScene("Main");
    }

    void OnClickExitButton()
    {
        Application.Quit();
    }
}
