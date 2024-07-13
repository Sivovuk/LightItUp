using System;
using System.Collections;
using System.Collections.Generic;
using LightItUp.Game;
using UnityEngine;

public class SeekingMissile : MonoBehaviour
{
    public float missileSpeed = 5;

    private Vector3 positionToTrack;

    private bool startTracking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startTracking)
        {
            transform.position = Vector2.MoveTowards(transform.position, positionToTrack, missileSpeed);
        }
    }
    
    internal void SetupMissile(Vector3 position)
    {
        positionToTrack = position;
        startTracking = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject != null && collision.contacts.Length > 0)
        {
            if (collision.gameObject.TryGetComponent(out BlockController rb))
            {
                if (rb.IsLit) return;
                Destroy(gameObject, 0.5f);
            }
        }
    }


}
