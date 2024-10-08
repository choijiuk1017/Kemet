using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectController : MonoBehaviour
{
    private float moveSpeed = 5f;

    public void InitiateMovement(Vector3 direction)
    {
        // �÷��̾ ���� �������� �̵�
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * moveSpeed;
        }
    }
}
