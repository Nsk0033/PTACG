using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillMove : MonoBehaviour
{
    [SerializeField] private LayerMask objectMask;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float duration = 10f;
    [SerializeField] private int damage = 10;
    public bool canDestroy = true;
    public bool setFalse;

    private void Start()
    {
        Invoke("DisableProjectile", duration);
    }

    private void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // You can add collision logic here, for example:
        if (MyLibrary.CheckLayer(collision.gameObject.layer, objectMask))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                if (canDestroy)
                    Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }

    private void DisableProjectile()
    {
        if (!setFalse)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
