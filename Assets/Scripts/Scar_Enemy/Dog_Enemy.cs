using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Enemy : MonoBehaviour
{
    public string myName; // Имя НПС в консоли. Делал для отладки - в игре это не нужно 

    public float speed; // Скорость НПС
    public float stoppingDistance; // Поле зрения НПС
    float timeLeft = 1.0f; // Интервал между атаками.

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

    bool moveingRight; // переключатель движения нпс вправо/влево
    bool chill = false; // переключает НПС в состояние патрулирования
    bool angry = false; // переключает НПС в состояние преследования
    bool angrySpeed = true; // Костыль, чтобы увеличивать скорость когда НПС видит игрока, но только в первом кадре и также выключать
    bool goBack = false; // Отправляет НПС обратно в зону патрулирования 
    bool checkDie_bot = true; // Проверка на то, сдох ли НПС или НЕТ!!!!!!!!!!!
    private bool facingRight = true;


    // Старт вызывается перед обновлением первого кадра
    void Start()
    {
        // Устанавливаем значение player равным сущности с тэгом Player
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

    // Обновление вызывается один раз за кадр
    void Update()
    {
        //  Debug.Log("Player: HealthBar: " + HealthBar.currentValue);
        // Если игрок подойдёт достаточно близко к НПС, то: 
        if (Vector2.Distance(transform.position, player.position) < 1)
        {
            //Включается таймер
            timeLeft -= Time.deltaTime;
            //Debug.Log(timeLeft);
            //Таймер равен 0, то:
            if (timeLeft < 0)
            {
                //Таймер сбрасывается
                timeLeft = 1.0f;
                // Debug.Log(myName + ": if(Vector2.Distance(transform.position, player.position) < 2 && Time.deltaTime == 2)");
                //Игрок получает урон
                DamageToPlayer();
            }
        }

        // Если игрок и НПС близко, и игрок нажал клавишу "удар", то единожды вызвать метод урона НПС
        if (Vector2.Distance(transform.position, player.position) < 2 && Input.GetKeyDown(KeyCode.B) && PlayerController.checkDie)
        {
            //Debug.Log(myName + ": if (Vector2.Distance(transform.position, player.position) == 1)");
            //Debug.Log(myName + ": " + Time.deltaTime);
            DamageToBot();
        }

        //Debug.Log("Update(): I am alive!");
        //Если расстояни между вектором и точкой меньше, чем positionOfPatrol - по сути граница триггера, и нпс не заагрен, то начать патрулирование.
        if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && !angry)
        {
            chill = true;
        }
        //Если расстояни между вектором и точкой меньше, чем stoppingDistance - по сути поле зрения нпс, то начать преследование.
        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            angry = true;
            //Debug.Log("Update() : speed is " + speed);
            chill = false;
            // Костыль, чтобы скорость не улетала в ебеня. 
            if (angrySpeed)
            {
                //Чтобы он не поворачивался за нами, будучи трупом
                if (checkDie_bot)
                    Flip();
                //Debug.ClearDeveloperConsole();
                speed += 2f;
                //Debug.Log(myName + ": Angry() : speed is " + speed);
                angrySpeed = false;
            }
            goBack = false;
        }
        //Если расстояни между вектором и точкой больше, чем stoppingDistance - по сути поле зрения нпс, то идти назад.
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            goBack = true;
            angry = false;
            // Костыль, чтобы скорость не улетала в ебеня. 
            if (!angrySpeed)
            {
                //Чтобы он не поворачивался за нами, будучи трупом
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
            //Debug.Log(myName + "to player: DamageToPlayer();");
            HealthBar.AdjustCurrentValue(-2);
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

    void Chill()
    {
        //Если по оси х позиция нпс больше, чем позиция точки и коэффицента positionOfPatrol - по сути границы триггера, то нпс поворачивает налево.
        //Вернее устанавливается значение переменной moveingRight как false и потом уже поворачиваем
        //_____________/Т\<--(НПС)_______
        if (transform.position.x > point.position.x + positionOfPatrol)
        {
            if (checkDie_bot)
                Flip();
            //Debug.Log(myName + ": Move to left");
            moveingRight = false;
        }
        //Если по оси х позиция нпс меньше, чем позиция точки и коэффицента positionOfPatrol - по сути границы триггера, то нпс поворачивает направо. 
        //Вернее устанавливается значение переменной moveingRight как true и потом уже поворачиваем
        //_____________(НПС)-->/Т\_______
        else if (transform.position.x < point.position.x - positionOfPatrol)
        {
            if (checkDie_bot)
                Flip();
            moveingRight = true;
        }

        if (moveingRight)
            //Как я понял. Скрипт тут(в Unity) исполняется каждый кадр, и соответственно передвижение происходит по принципу: сдвинуть на n координат картинку по икс(transform.position.x)
            // и игрек(transform.position.у) Переменная speed просто некий коэффицент, на который мы двигаем картинку нпс.
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        else
            //Как я понял. Скрипт тут(в Unity) исполняется каждый кадр, и соответственно передвижение происходит по принципу: сдвинуть на n координат картинку по икс(transform.position.x)
            // и игрек(transform.position.у) Переменная speed просто некий коэффицент, на который мы двигаем картинку нпс.
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
    }

    void Angry()
    {
        // Сводка: MoveTowards()
        //     Moves a point current towards target.
        //     Перевод: Перемещает текущую точку к цели.
        //
        // Параметры:
        //   current:
        //
        //   target:
        //
        //   maxDistanceDelta:
        //
        // Мы перемещаем нашего нпс(current: transform.position)  на некоторую
        // координату(maxDistanceDelta: speed * Time.deltaTime) каждый кадр в сторону цели(target: player.position)
        //
        // В данном случае мы смещаем его в сторону игрока
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void GoBack()
    {
        // Сводка: MoveTowards()
        //     Moves a point current towards target. 
        //     Перевод: Перемещает текущую точку к цели.
        //
        // Параметры:
        //   current:
        //
        //   target:
        //
        //   maxDistanceDelta:
        //
        // Мы перемещаем нашего нпс(current: transform.position) на некоторую
        // координату(maxDistanceDelta: speed * Time.deltaTime) каждый кадр в сторону цели(target: point.position)
        //
        // В данном случае мы смещаем его в зону патрулирования
        transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
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