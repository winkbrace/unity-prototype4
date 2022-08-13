using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public float speed = 5.0f;
    public bool hasPowerup = false;
    private const float powerupStrength = 15.0f;
    public GameObject powerupIndicator;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        var forwardInput = Input.GetAxis("Vertical");
        
        playerRb.AddForce(forwardInput * speed * focalPoint.transform.forward);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup")) {
            Destroy(other.GameObject());
            EnablePowerup();
        }
    }

    IEnumerator countdownPowerupRoutine()
    {
        yield return new WaitForSeconds(7);
        disablePowerup();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup) {
            var enemy = collision.gameObject;
            Debug.Log("Player collided with " + enemy.name + " while powerup was " + hasPowerup);

            var enemyRb = enemy.GetComponent<Rigidbody>();
            var awayFromPlayer = enemy.transform.position - transform.position;
            
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    private void EnablePowerup()
    {
        hasPowerup = true;
        powerupIndicator.gameObject.SetActive(true);
        StartCoroutine(countdownPowerupRoutine());
    }

    private void disablePowerup()
    {
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }
}
