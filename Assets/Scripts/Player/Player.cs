using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int currentHp;
    [SerializeField]
    public int max;

    private int maxComboCount = 3;
    private int comboCount = 0;

    public float moveSpeed = 5.0f;
    public float maxSpeed;

    public float atkCoolTime = 0.5f;
    public float comboTime = 1.0f;

    public float jumpForce = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
