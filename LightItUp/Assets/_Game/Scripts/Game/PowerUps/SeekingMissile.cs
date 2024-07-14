using System;
using System.Collections;
using System.Collections.Generic;
using LightItUp.Data;
using LightItUp.Game;
using UnityEngine;

public class SeekingMissile : MonoBehaviour
{
    public float missileSpeed = 5;      // Adjust the speed amount of the missile
    public float forceAmount = 10f;     // Adjust the force amount as needed
    public LayerMask blockLayer;

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
        GameManager.Instance.currentLevel.player.camFocus.AddTempTarget(GetComponent<Collider2D>(), GameSettings.CameraFocus.blockExplodePartsFocusDuration);
                
    }

    private void OnTriggerEnter2D(Collider2D collider2D) 
    {
        if (collider2D.gameObject.TryGetComponent(out BlockController bc))
        {
            if (bc.IsLit) return;
            if(collider2D.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                // Calculate the direction of the force to apply
                Vector2 forceDirection = (collider2D.transform.position - transform.position).normalized;

                // Apply the force at the point of the collision
                rb.AddForceAtPosition(forceDirection * forceAmount, transform.position, ForceMode2D.Impulse);
            }
            
            gameObject.SetActive(false);
        }
    }
}
