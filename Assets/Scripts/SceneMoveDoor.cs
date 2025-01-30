using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneMoveDoor : MonoBehaviour
{
    public string nextSceneName; // �̵��� �� �̸�
    public Vector3 nextSpawnPosition; // ���� ������ �÷��̾ ������ ��ġ

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            GameManager.instance.ChangeScene(nextSceneName, nextSpawnPosition);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
