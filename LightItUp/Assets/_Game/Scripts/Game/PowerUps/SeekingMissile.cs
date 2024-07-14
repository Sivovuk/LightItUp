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

    private Vector3 positionToTrack;

    private bool startTracking = false;

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

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.TryGetComponent(out BlockController bc))
        {
            if(collision2D.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                // Calculate the direction of the force to apply
                Vector2 forceDirection = (collision2D.transform.position - transform.position).normalized;

                // Apply the force at the point of the collision
                rb.AddForceAtPosition(forceDirection * forceAmount, transform.position, ForceMode2D.Impulse);
            }
            SceneCleanUp();
        }
    }

    public void SceneCleanUp()
    {
        positionToTrack = Vector2.zero;
        startTracking = false;
        gameObject.SetActive(false);
    }
}
