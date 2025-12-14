using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public Animator shootCounterAnim;
    [SerializeField] TextMeshProUGUI BulletCounter;
    [SerializeField] GameObject PauseMenu;

    [Header("Settings")]
    public int MaxThrows;
    int currentThrows;
    public float FireRate;
    public float BulletVelocity;
    public float UpwardThrowForce;

    [Header("Keybinds")]
    public KeyCode throwKey = KeyCode.Mouse0;

    bool readyToThrow;
    private void Start()
    {
        readyToThrow = true;
        shootCounterAnim.enabled = false;
    }

    private void Update()
    {
        Debug.Log(readyToThrow);
        if (Input.GetKey(throwKey) && readyToThrow && MaxThrows > 0 && !PauseMenu.activeInHierarchy)
        {
            shootCounterAnim.enabled = true;
            shootCounterAnim.Play("BulletCounterShake", -1, 0f);

            Shoot();
        }

        BulletCounter.text = MaxThrows.ToString();
    }

    public void Shoot()
    {   
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * BulletVelocity + transform.up * UpwardThrowForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        MaxThrows--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), FireRate);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}