using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Enemy : MonoBehaviour
{
    public string myName; // ��� ��� � �������. ����� ��� ������� - � ���� ��� �� ����� 

    public float speed; // �������� ���
    public float stoppingDistance; // ���� ������ ���
    float timeLeft = 1.0f; // �������� ����� �������.

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

    bool moveingRight; // ������������� �������� ��� ������/�����
    bool chill = false; // ����������� ��� � ��������� ��������������
    bool angry = false; // ����������� ��� � ��������� �������������
    bool angrySpeed = true; // �������, ����� ����������� �������� ����� ��� ����� ������, �� ������ � ������ ����� � ����� ���������
    bool goBack = false; // ���������� ��� ������� � ���� �������������� 
    bool checkDie_bot = true; // �������� �� ��, ���� �� ��� ��� ���!!!!!!!!!!!
    private bool facingRight = true;


    // ����� ���������� ����� ����������� ������� �����
    void Start()
    {
        // ������������� �������� player ������ �������� � ����� Player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //rb.AddForce(Vector3.up * 10f);

        Debug.Log("I am alive and my name is " + myName);
    }

    //void FixedUpdate()
    //{


    //    x++;

    //    Debug.Log(myName + ":" + x);
    //    if (Vector2.Distance(transform.position, player.position) < 1)
    //    {
    //        Debug.Log(myName + ": if(Vector2.Distance(transform.position, player.position) < 2 && Time.deltaTime == 2)");
    //        DamageToPlayer();
    //    }
    //}

    // ���������� ���������� ���� ��� �� ����
    void Update()
    {
        //  Debug.Log("Player: HealthBar: " + HealthBar.currentValue);
        // ���� ����� ������� ���������� ������ � ���, ��: 
        if (Vector2.Distance(transform.position, player.position) < 1)
        {
            //���������� ������
            timeLeft -= Time.deltaTime;
            //Debug.Log(timeLeft);
            //������ ����� 0, ��:
            if (timeLeft < 0)
            {
                //������ ������������
                timeLeft = 1.0f;
                // Debug.Log(myName + ": if(Vector2.Distance(transform.position, player.position) < 2 && Time.deltaTime == 2)");
                //����� �������� ����
                DamageToPlayer();
            }
        }

        // ���� ����� � ��� ������, � ����� ����� ������� "����", �� �������� ������� ����� ����� ���
        if (Vector2.Distance(transform.position, player.position) < 2 && Input.GetKeyDown(KeyCode.B) && PlayerController.checkDie)
        {
            //Debug.Log(myName + ": if (Vector2.Distance(transform.position, player.position) == 1)");
            //Debug.Log(myName + ": " + Time.deltaTime);
            DamageToBot();
        }

        //Debug.Log("Update(): I am alive!");
        //���� ��������� ����� �������� � ������ ������, ��� positionOfPatrol - �� ���� ������� ��������, � ��� �� �������, �� ������ ��������������.
        if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && !angry)
        {
            chill = true;
        }
        //���� ��������� ����� �������� � ������ ������, ��� stoppingDistance - �� ���� ���� ������ ���, �� ������ �������������.
        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            angry = true;
            //Debug.Log("Update() : speed is " + speed);
            chill = false;
            // �������, ����� �������� �� ������� � �����. 
            if (angrySpeed)
            {
                //����� �� �� ������������� �� ����, ������ ������
                if (checkDie_bot)
                    Flip();
                //Debug.ClearDeveloperConsole();
                speed += 2f;
                //Debug.Log(myName + ": Angry() : speed is " + speed);
                angrySpeed = false;
            }
            goBack = false;
        }
        //���� ��������� ����� �������� � ������ ������, ��� stoppingDistance - �� ���� ���� ������ ���, �� ���� �����.
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            goBack = true;
            angry = false;
            // �������, ����� �������� �� ������� � �����. 
            if (!angrySpeed)
            {
                //����� �� �� ������������� �� ����, ������ ������
                if (checkDie_bot)
                    Flip();
                Debug.ClearDeveloperConsole();
                angrySpeed = true;
                speed -= 2f;
                // Debug.Log(myName + ": chill : speed is " + speed);
            }
            //angrySpeed = false;
        }

        if (chill && checkDie_bot)
            Chill();
        else if (angry && checkDie_bot)
            Angry();
        else if (goBack && checkDie_bot)
            GoBack();

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
            //Debug.Log(myName + "to player: DamageToPlayer();");
            HealthBar.AdjustCurrentValue(-2);
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

    void Chill()
    {
        //���� �� ��� � ������� ��� ������, ��� ������� ����� � ����������� positionOfPatrol - �� ���� ������� ��������, �� ��� ������������ ������.
        //������ ��������������� �������� ���������� moveingRight ��� false � ����� ��� ������������
        //_____________/�\<--(���)_______
        if (transform.position.x > point.position.x + positionOfPatrol)
        {
            if (checkDie_bot)
                Flip();
            //Debug.Log(myName + ": Move to left");
            moveingRight = false;
        }
        //���� �� ��� � ������� ��� ������, ��� ������� ����� � ����������� positionOfPatrol - �� ���� ������� ��������, �� ��� ������������ �������. 
        //������ ��������������� �������� ���������� moveingRight ��� true � ����� ��� ������������
        //_____________(���)-->/�\_______
        else if (transform.position.x < point.position.x - positionOfPatrol)
        {
            if (checkDie_bot)
                Flip();
            moveingRight = true;
        }

        if (moveingRight)
            //��� � �����. ������ ���(� Unity) ����������� ������ ����, � �������������� ������������ ���������� �� ��������: �������� �� n ��������� �������� �� ���(transform.position.x)
            // � �����(transform.position.�) ���������� speed ������ ����� ����������, �� ������� �� ������� �������� ���.
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        else
            //��� � �����. ������ ���(� Unity) ����������� ������ ����, � �������������� ������������ ���������� �� ��������: �������� �� n ��������� �������� �� ���(transform.position.x)
            // � �����(transform.position.�) ���������� speed ������ ����� ����������, �� ������� �� ������� �������� ���.
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
    }

    void Angry()
    {
        // ������: MoveTowards()
        //     Moves a point current towards target.
        //     �������: ���������� ������� ����� � ����.
        //
        // ���������:
        //   current:
        //
        //   target:
        //
        //   maxDistanceDelta:
        //
        // �� ���������� ������ ���(current: transform.position)  �� ���������
        // ����������(maxDistanceDelta: speed * Time.deltaTime) ������ ���� � ������� ����(target: player.position)
        //
        // � ������ ������ �� ������� ��� � ������� ������
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void GoBack()
    {
        // ������: MoveTowards()
        //     Moves a point current towards target. 
        //     �������: ���������� ������� ����� � ����.
        //
        // ���������:
        //   current:
        //
        //   target:
        //
        //   maxDistanceDelta:
        //
        // �� ���������� ������ ���(current: transform.position) �� ���������
        // ����������(maxDistanceDelta: speed * Time.deltaTime) ������ ���� � ������� ����(target: point.position)
        //
        // � ������ ������ �� ������� ��� � ���� ��������������
        transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
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