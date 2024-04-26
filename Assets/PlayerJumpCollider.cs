using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpCollider : MonoBehaviour
{
    [SerializeField] private Player player;
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTrigger2D(collision);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger2D(collision);
    }
    private void OnTrigger2D(Collider2D collision)
    {
        //if(collision.CompareTag("Platform"))
        player.RefreshJump();
    }
    private void Update()
    {
        transform.position = player.transform.position + new Vector3(0, -0.3f, 0f);
    }
}
