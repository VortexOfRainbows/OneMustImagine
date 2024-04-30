using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [SerializeField]
    private float VelocityToBreak = 10;
    [SerializeField]
    private bool VelocityPersistsAfterBreaking = true;
    [SerializeField]
    private SpriteRenderer Renderer;
    [SerializeField]
    private Collider2D Collider;
    private Color DefaultColor;
    private Color DeadColor;
    private void Start()
    {
        if(Renderer == null)
        {
            Renderer = GetComponent<SpriteRenderer>();
        }
        if(Collider == null)
        {
            Collider = GetComponent<Collider2D>();
        }
        DefaultColor = Renderer.color;
        DeadColor = Renderer.color * 0.1f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollide(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollide(collision);
    }
    private void OnCollide(Collision2D collision)
    {
        if(collision.collider.tag == "Boulder")
        {
            if (collision.relativeVelocity.magnitude > VelocityToBreak)
            {
                Dead = true;
                Collider.enabled = false;
                if (VelocityPersistsAfterBreaking)
                {
                    collision.collider.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity;
                }
            }
            else
            {
                BreakTimer += Time.deltaTime * 2; //Break slowly if the boulder is colliding but not high enough speed
            }
        }
    }
    [SerializeField]
    private bool Respawnable = true;
    [SerializeField]
    private float RespawnTime = 4f;
    [SerializeField]
    private float CrumbleTime = 5f;
    private bool Dead = false;
    private bool LastAliveState = false;
    private float ResTimer = 0;
    private float BreakTimer = 0;
    private void Update()
    {
        BreakTimer -= Time.deltaTime;
        BreakTimer = Mathf.Max(BreakTimer, 0);
        if (BreakTimer > CrumbleTime)
        {
            Dead = true;
            Collider.enabled = false;
            BreakTimer = 0;
        }
        if(Dead != LastAliveState)
        {
            if(LastAliveState) //If I am now dead, but was alive
            {

            }
            else //If I am now alive, but was dead
            {

            }
        }
        if(Dead)
        {
            if(Respawnable)
                ResTimer += Time.deltaTime;
            if(ResTimer >= RespawnTime)
            {
                Dead = false;
            }
            Renderer.color = DeadColor;
        }
        else
        {
            Renderer.color = DefaultColor;
            ResTimer = 0;
        }
        Collider.enabled = !Dead;
        LastAliveState = Dead;
    }
}
