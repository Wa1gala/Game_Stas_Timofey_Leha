//--Script By: BERKYT
//--Links: VK: https://vk.com/b_e_r_k_y_t Discord: https://discord.gg/amMreCC YouTube: https://www.youtube.com/channel/UCaPBjmrAYO6p-ksHNaymwLg?view_as=subscriber
//--The script is distributed freely, provided that the author of the script is indicated - BERKYT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitting_enemy : MonoBehaviour
{
    public string myName; // ��� ��� � �������. ����� ��� ������� - � ���� ��� �� ����� 

    public float speed; // �������� ���
    public float stoppingDistance; // ���� ������ ���
    public float speedOfProjectile = 3.0f;
    float timeLeft = 0; // �������� ����� �������.
    float timerTurn = 0.1f; // �� �������!
    float xPosition; // �� �������!

    public int positionOfPatrol; // ������� ��������
    public int healh; //��

    // �������� �� 
    public int Health
    {
        get
        {
            if (healh < 0)
            {
                return 0;
            }
            else
            {
                return healh;
            }
        }
        set
        {
            healh = value;
        }
    }

    public Transform point; // ������� ���
    public Transform player; // ���������� ������
    public Transform spawnPoint; // ���������� �������
    public GameObject projectile; // ������ 

    bool checkDie_bot = true; // �������� �� ��, ���� �� ��� ��� ���!!!!!!!!!!!
    bool checkTurn = true; // �������� �� ��, �������� �� ��� ��� ��!
    private bool facingRight = true; // �� �������!

    // ����� ���������� ����� ����������� ������� �����
    void Start()
    {
        // ������������� �������� player ������ �������� � ����� Player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //rb.AddForce(Vector3.up * 10f);

        Debug.Log("I am alive and my name is " + myName);
    }

    // ���������� ���������� ���� ��� �� ����
    void Update()
    {
        // ���� ����� ������� ���������� ������ � ���, ��: 
        if (Vector2.Distance(transform.position, player.position) < 8)
        {
            //���������� ������
            timeLeft -= Time.deltaTime;
            //Debug.Log(timeLeft);
            //������ ����� 0, ��:
            if (timeLeft < 0)
            {
                //������ ������������
                timeLeft = speedOfProjectile;
                 Debug.Log(myName + ": (Vector2.Distance(transform.position, player.position) < 4)");
                //����� �������� ����
                DamageToPlayer();
            }
        }

        // ���� ����� � ��� ������, � ����� ����� ������� "����", �� �������� ������� ����� ����� ���
        if (transform.position.x < player.position.x)
        {
            if (Vector2.Distance(transform.position, player.position) < 2 && Input.GetKeyDown(KeyCode.B) && PlayerController.checkDie && PlayerController.checkTurn)
            {
                DamageToBot();
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) < 2 && Input.GetKeyDown(KeyCode.B) && PlayerController.checkDie && !PlayerController.checkTurn)
            {
                DamageToBot();
            }
        }
    }

    //����� �����
    void DamageToBot()
    {
        Debug.Log(myName + ": DamageToBot();");
        Debug.Log(myName + ": Health: " + Health);
        Health -= 2;

        //���� �� ������ 0 � checkDie ������, �� �������
        if (Health <= 0 && checkDie_bot)
        {
            // Debug.Log(myName + ": if (Health <= 0 && checkDie)");
            Die();
        }
    }

    void DamageToPlayer()
    {
        //����� �� �� ������ ��� ������ ������
        if (checkDie_bot)
        {
            //HealthBar.AdjustCurrentValue(-2);
            Debug.Log(myName + ": if (checkDie_bot) ");
            Instantiate(projectile, spawnPoint.position, Quaternion.identity);
        }

        //���� �� ������ 0 �� ������� �����
        if (HealthBar.currentValue <= 0 && PlayerController.checkDie)
        {
            PlayerController.checkDie = false;
            Debug.Log("Player: I am die...");
            //Die();
        }
    }

    //����
    void Die()
    {
        //Debug.Log(myName + ": Die(): " + checkDie_bot);
        //���� checkDie ������, ��.........
        if (checkDie_bot)
        {
            //Debug.Log(myName + ": Die(): " + checkDie_bot);
            //GUI.Label(new Rect(10, 10, 100, 20), "Hello World!");
            //������ � ������, ����� �� ������� 1000+ ��� � �������, � ������ ���� ���.
            checkDie_bot = false;
            Debug.Log(myName + ": I am die...");
        }
        //Debug.Log(myName + ": Die()_2: " + checkDie_bot);
    }

    // �������� ������ ����
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}