using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private Vector2 Direction;
    [SerializeField] private float Speed;
    [SerializeField] private float AirSpeedMultiplier = 5;
    public void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speedM = 1;
            if (rb.CompareTag("Boulder") && !collision.GetComponent<Boulder>().touchingGround)
            {
                speedM = AirSpeedMultiplier;
            }
            else if (rb.CompareTag("Player") && collision.GetComponent<Player>().InTheAir)
            {
                speedM = AirSpeedMultiplier;
            }
            rb.velocity = rb.velocity + Direction.normalized.RotatedBy(transform.rotation.z) * Speed * Time.fixedDeltaTime * speedM * 2;
        }
    }
}
