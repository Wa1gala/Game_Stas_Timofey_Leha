//--Script By: BERKYT
//--Links: VK: https://vk.com/b_e_r_k_y_t Discord: https://discord.gg/amMreCC YouTube: https://www.youtube.com/channel/UCaPBjmrAYO6p-ksHNaymwLg?view_as=subscriber
//--The script is distributed freely, provided that the author of the script is indicated - BERKYT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitting_enemy : MonoBehaviour
{
    public string myName; // Имя НПС в консоли. Делал для отладки - в игре это не нужно 

    public float speed; // Скорость НПС
    public float stoppingDistance; // Поле зрения НПС
    public float speedOfProjectile = 3.0f;
    float timeLeft = 0; // Интервал между атаками.
    float timerTurn = 0.1f; // Не трогать!
    float xPosition; // Не трогать!

    public int positionOfPatrol; // Граница триггера
    public int healh; //хп

    // Свойство хп 
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

    public Transform point; // Триггер НПС
    public Transform player; // Координаты игрока
    public Transform spawnPoint; // Координаты снаряда
    public GameObject projectile; // Снаряд 

    bool checkDie_bot = true; // Проверка на то, сдох ли НПС или НЕТ!!!!!!!!!!!
    bool checkTurn = true; // Проверка на то, повернул ли НПС ИЛИ НЕ!
    private bool facingRight = true; // Не трогать!

    // Старт вызывается перед обновлением первого кадра
    void Start()
    {
        // Устанавливаем значение player равным сущности с тэгом Player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //rb.AddForce(Vector3.up * 10f);

        Debug.Log("I am alive and my name is " + myName);
    }

    // Обновление вызывается один раз за кадр
    void Update()
    {
        // Если игрок подойдёт достаточно близко к НПС, то: 
        if (Vector2.Distance(transform.position, player.position) < 8)
        {
            //Включается таймер
            timeLeft -= Time.deltaTime;
            //Debug.Log(timeLeft);
            //Таймер равен 0, то:
            if (timeLeft < 0)
            {
                //Таймер сбрасывается
                timeLeft = speedOfProjectile;
                 Debug.Log(myName + ": (Vector2.Distance(transform.position, player.position) < 4)");
                //Игрок получает урон
                DamageToPlayer();
            }
        }

        // Если игрок и НПС близко, и игрок нажал клавишу "удар", то единожды вызвать метод урона НПС
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

    //Метод урона
    void DamageToBot()
    {
        Debug.Log(myName + ": DamageToBot();");
        Debug.Log(myName + ": Health: " + Health);
        Health -= 2;

        //Если хп меньше 0 и checkDie истина, то сдохнем
        if (Health <= 0 && checkDie_bot)
        {
            // Debug.Log(myName + ": if (Health <= 0 && checkDie)");
            Die();
        }
    }

    void DamageToPlayer()
    {
        //Чтобы он не убивал нас будучи трупом
        if (checkDie_bot)
        {
            //HealthBar.AdjustCurrentValue(-2);
            Debug.Log(myName + ": if (checkDie_bot) ");
            Instantiate(projectile, spawnPoint.position, Quaternion.identity);
        }

        //Если хп меньше 0 то сдохнет игрок
        if (HealthBar.currentValue <= 0 && PlayerController.checkDie)
        {
            PlayerController.checkDie = false;
            Debug.Log("Player: I am die...");
            //Die();
        }
    }

    //Сдох
    void Die()
    {
        //Debug.Log(myName + ": Die(): " + checkDie_bot);
        //Если checkDie истина, то.........
        if (checkDie_bot)
        {
            //Debug.Log(myName + ": Die(): " + checkDie_bot);
            //GUI.Label(new Rect(10, 10, 100, 20), "Hello World!");
            //Делаем её ложной, чтобы не дохнуть 1000+ раз в секунду, а только один раз.
            checkDie_bot = false;
            Debug.Log(myName + ": I am die...");
        }
        //Debug.Log(myName + ": Die()_2: " + checkDie_bot);
    }

    // Зеркалит спрайт бота
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}