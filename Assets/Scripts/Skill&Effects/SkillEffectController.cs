using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectController : MonoBehaviour
{
    private float moveSpeed = 5f;

    public void InitiateMovement(Vector3 direction)
    {
        // 플레이어가 보는 방향으로 이동
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * moveSpeed;
        }
    }
}
