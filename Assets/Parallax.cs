using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private float ParallaxAmt = 0.05f;
    private Vector2 origPos;
    void Start()
    {
        origPos = transform.position;
    }
    void Update()
    {
        if(Player.MainCamera != null)
        {
            transform.position = Vector2.Lerp(origPos, (Vector2)Player.MainCamera.transform.position, ParallaxAmt);
        }
    }
}
