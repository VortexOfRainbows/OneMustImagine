using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem BreakParticles;
    [SerializeField]
    private ParticleSystem RegenParticles;
    [SerializeField]
    private float VelocityToBreak = 12;
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
            float breakPercent = collision.relativeVelocity.magnitude / VelocityToBreak;
            breakPercent = Mathf.Clamp(breakPercent, 0, 1);
            if (breakPercent >= 1)
            {
                BreakEffect();
                if (VelocityPersistsAfterBreaking)
                {
                    collision.collider.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity;
                }
            }
            else
            {
                breakPercent = breakPercent * breakPercent * PartialBreakMultiplier * 0.5f;
                BreakTimer += breakPercent * CrumbleTime;
                BreakTimer += Time.deltaTime * 2; //Break slowly if the boulder is colliding but not high enough speed
                if (BreakTimer > CrumbleTime)
                {
                    BreakEffect();
                    if (VelocityPersistsAfterBreaking)
                    {
                        collision.collider.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity;
                    }
                }
            }
        }
    }
    [SerializeField]
    private float PartialBreakMultiplier = 0.2f;
    [SerializeField]
    private bool Respawnable = true;
    [SerializeField]
    private float RespawnTime = 4f;
    [SerializeField]
    private float CrumbleTime = 7f;
    private bool Dead = false;
    private bool LastDeadState = false;
    private float ResTimer = 0;
    private float BreakTimer = 0;
    private float PreviousBreakPercent = 0;
    private void BreakEffect()
    {
        Dead = true;
        Collider.enabled = false;
        BreakTimer = 0;
    }
    private void Update()
    {
        BreakTimer -= Time.deltaTime;
        BreakTimer = Mathf.Max(BreakTimer, 0);
        if (BreakTimer > CrumbleTime)
        {
            BreakEffect();
        }
        else if(BreakTimer > 0) //If the boulder is currently on top of the platform, some particles should be spawned
        {
            //This behavior is already created below, though
        }
        if (Dead != LastDeadState)
        {
            if (!LastDeadState) //If I am now dead, but was alive
            {
                //Spawn particles for the platform coming back together
                ParticleSystem module = Instantiate(BreakParticles.gameObject, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                ParticleSystem.MainModule main = module.main;
                main.startColor = DefaultColor;
                ParticleSystem.ShapeModule shape = module.shape;
                shape.scale = transform.localScale;
                shape.rotation = transform.eulerAngles;
            }
            else //If I am now alive, but was dead
            {
                //Spawn particles for the platform breaking

            }
        }
        float currentBreakPercentage = (BreakTimer / CrumbleTime);
        if (PreviousBreakPercent != currentBreakPercentage)
        {
            float PercentBreak = currentBreakPercentage - PreviousBreakPercent;
            if(PercentBreak > 0)
            {
                short count = (short)(100 * PercentBreak + 0.5f);
                if(count > 0)
                {
                    //Spawn particles here relative to the change in break. This will effectively make particles spawn if the platform is damaged slightly by the boulder 
                    ParticleSystem module = Instantiate(BreakParticles.gameObject, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule main = module.main;
                    main.startColor = DefaultColor;
                    ParticleSystem.ShapeModule shape = module.shape;
                    shape.scale = transform.localScale;
                    shape.rotation = transform.eulerAngles;
                    module.emission.SetBurst(0, new ParticleSystem.Burst(0, count));
                }
            }
        }
        PreviousBreakPercent = BreakTimer / CrumbleTime;
        if(Dead)
        {
            if(Respawnable)
                ResTimer += Time.deltaTime;
            if(ResTimer >= RespawnTime)
            {
                Dead = false;
            }
            Renderer.color = Color.Lerp(DefaultColor, DeadColor, 1f - 0.6f * ResTimer / RespawnTime);
        }
        else
        {
            Renderer.color = Color.Lerp(DeadColor, DefaultColor, 1f - 0.6f * BreakTimer / CrumbleTime);
            ResTimer = 0;
        }
        Collider.enabled = !Dead;
        LastDeadState = Dead;
    }
}
