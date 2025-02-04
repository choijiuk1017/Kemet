using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Core.Unit.Player;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱��� �ν��Ͻ�
    public Vector3 playerSpawnPosition; // �÷��̾��� ���� ��ġ ����
    public GameObject gameOverPanel;
    public Seth player; 

    private bool isGameOver = false;

    void Awake()
    {
        // �̱��� ����: ���� �ν��Ͻ��� ������ ����, ������ ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Update()
    {
        if(isGameOver)
        {
            if (player.isDead)
            {
                return;
            }
            else
            {
                isGameOver = false;
            }
           
        }



        if (player.isDead && !isGameOver)
        {
            isGameOver = true;
            gameOverPanel.SetActive(true); // **�̹� �����ϴ� �г��� Ȱ��ȭ**
        }
        
    }

    // ���� ����� �� ȣ��Ǵ� �Լ�
    public void ChangeScene(string sceneName, Vector3 newSpawnPosition)
    {
        playerSpawnPosition = newSpawnPosition; // ���ο� ���� ��ġ ����
        SceneManager.LoadScene(sceneName);
    }
}
