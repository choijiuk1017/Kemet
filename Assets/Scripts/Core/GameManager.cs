using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Core.Unit.Player;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글턴 인스턴스
    public Vector3 playerSpawnPosition; // 플레이어의 스폰 위치 저장
    public GameObject gameOverPanel;
    public Seth player; 

    private bool isGameOver = false;

    void Awake()
    {
        // 싱글턴 패턴: 기존 인스턴스가 있으면 삭제, 없으면 유지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
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
            gameOverPanel.SetActive(true); // **이미 존재하는 패널을 활성화**
        }
        
    }

    // 씬이 변경될 때 호출되는 함수
    public void ChangeScene(string sceneName, Vector3 newSpawnPosition)
    {
        playerSpawnPosition = newSpawnPosition; // 새로운 스폰 위치 저장
        SceneManager.LoadScene(sceneName);
    }
}
