using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string myName; // Имя НПС в консоли. Делал для отладки - в игре это не нужно 

    public Transform ProjectilePoint;
    private Transform PlayerPoint;
    private Vector2 vector;
    private bool checkSpawnProjectile = true;
    public float speed = 2f;
    public float angel = 45f;
    private float timeLeft = 1.0f;

    private void Start()
    {
        
    }

    void Update()
    {
        if (checkSpawnProjectile)
        {
            PlayerPoint = GameObject.FindGameObjectWithTag("Player").transform;
            checkSpawnProjectile = false;
            vector.x = PlayerPoint.position.x;
            vector.y = PlayerPoint.position.y;
        }

        //timeLeft -= Time.deltaTime;
        ////Таймер равен 0, то:
        //if (timeLeft < 0)
        //{
        //    //Таймер сбрасывается
        //    timeLeft = 1.0f;
            if (ProjectilePoint.position.x != vector.x && ProjectilePoint.position.y != vector.y)
            {
                Fly();
            }
        //}
    }

    void Fly()
    {
        Debug.Log(myName + ": void Fly()");
        transform.position = new Vector2(transform.position.x - (speed * Mathf.Cos(45) * Time.deltaTime), transform.position.y + (speed * Mathf.Sin(45) * Time.deltaTime) - ((9.8f * Mathf.Pow(Time.deltaTime, 2)) / 2));
        //transform.position = new Vector2(transform.position.x + speed, transform.position.x * Mathf.Tan(angel) - ((9.8f / (2 * Mathf.Pow(speed, 2) * Mathf.Pow(Mathf.Cos(angel), 2))) * Mathf.Pow(transform.position.x, 2)));
    }
}
