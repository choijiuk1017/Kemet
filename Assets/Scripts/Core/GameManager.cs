using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱��� �ν��Ͻ�
    public Vector3 playerSpawnPosition; // �÷��̾��� ���� ��ġ ����

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

    // ���� ����� �� ȣ��Ǵ� �Լ�
    public void ChangeScene(string sceneName, Vector3 newSpawnPosition)
    {
        playerSpawnPosition = newSpawnPosition; // ���ο� ���� ��ġ ����
        SceneManager.LoadScene(sceneName);
    }
}
