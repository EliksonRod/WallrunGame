using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject PrimaryProjectile;
    [SerializeField] GameObject TimebombProjectile;
    [SerializeField] GameObject TimezoneProjectile;
    GameObject SecondaryProjectile;

    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public Animator shootCounterAnim;
    public LineRenderer lineRenderer;
    [SerializeField] GameManager gm;
    [SerializeField] TextMeshProUGUI BulletCounter;
    [SerializeField] GameObject PauseMenu;

    [Header("Settings")]
    public int MaxThrows;
    int currentThrows;
    public float FireRate;
    public float BulletVelocity;
    public float UpwardThrowForce;

    [Header("Secondary Fire Settings")]
    float secondaryFireCooldown = 5f;
        bool readyToSecondaryFire = true;
    float secondaryFireSpeed;

    [Header("Slow Time Settings")]
    [SerializeField] float slowTimeRate = 0.35f;
    [SerializeField] float slowTimeDuration = 1.5f;
    float slowTimeDurationTimer;
    [SerializeField] float slowTimeDelayCooldown = 5f;
    bool SlowingTime;
    bool CanSlowTime = true;

    [Header("Beam Settings")]
    int maxBeamReflections = 3;
    float laserWidth = 0.05f;

    private List<Vector3> laserPoints = new List<Vector3>();



    [Header("Keybinds")]
    public KeyCode throwKey = KeyCode.Mouse0;

    public enum ThrowAbility
    {
        Timezone,
        Timecontrol,
    }
    ThrowAbility currentAbility;

    bool readyToThrow;
    private void Start()
    {
        lineRenderer.SetPosition(0, attackPoint.position);
        readyToThrow = true;
        shootCounterAnim.enabled = false;
        slowTimeDurationTimer = slowTimeDuration;

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
    }

    private void Update()
    {


        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (CanSlowTime) SlowTimeAbility();
            DrawProjectilePath();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            CanSlowTime = true;
            SlowingTime = false;
            gm.NormalTime();

            FireProjectile();
        }

        if (Input.GetKey(throwKey) && readyToThrow && MaxThrows > 0 && !PauseMenu.activeInHierarchy)
        {
            shootCounterAnim.enabled = true;
            shootCounterAnim.Play("BulletCounterShake", -1, 0f);
            PrimaryFire();
        }

        BulletCounter.text = MaxThrows.ToString();
        slowTimeDurationTimer = Mathf.Clamp(slowTimeDurationTimer, 0, slowTimeDuration);
        if (slowTimeDurationTimer < slowTimeDuration && !SlowingTime)
        {
            Debug.Log("Recharging Slow Time Ability" + slowTimeDurationTimer);
            Invoke(nameof(RechargeSlowTimeAbility), slowTimeDelayCooldown);
        }
    }

    public void SelectAbility1(InputAction.CallbackContext context)
    {
        if (context.performed) currentAbility = ThrowAbility.Timezone;
    }

    public void SelectAbility2(InputAction.CallbackContext context)
    {
        if (context.performed) currentAbility = ThrowAbility.Timecontrol;
    }

    void FixedUpdate()
    {
        switch(currentAbility)
        {
            case ThrowAbility.Timezone:
                SecondaryProjectile = TimebombProjectile;
                break;
            case ThrowAbility.Timecontrol:
                SecondaryProjectile = TimezoneProjectile;
                break;
        }

        
    }

    public void PrimaryFire()
    {   
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(PrimaryProjectile, attackPoint.position, cam.rotation);

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

    public void SecondaryFire()
    {
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(SecondaryProjectile, attackPoint.position, cam.rotation);

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

    public void DrawProjectilePath()
    {
        laserPoints.Clear();
        Vector3 direction = attackPoint.forward;
        Vector3 StartPos = attackPoint.position;

        lineRenderer.positionCount = maxBeamReflections + 2;
        lineRenderer.SetPosition(0, StartPos);

        int currentIndex = 1;
        laserPoints.Add(StartPos);

        for (int i = 0; i <= maxBeamReflections; i++)
        {
            Ray ray = new Ray(StartPos, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 56))
            {
                // Set line point to hit position
                lineRenderer.SetPosition(currentIndex, hit.point);
                currentIndex++;

                laserPoints.Add(hit.point);

                // Reflection math
                Vector3 incoming = direction.normalized;
                Vector3 normal = hit.normal.normalized;

                direction = incoming - 2f * Vector3.Dot(incoming, normal) * normal;

                // Offset to prevent hitting same surface again
                StartPos = hit.point + direction * 0.01f;
            }
            else
            {
                // No hit, extend laser forward
                lineRenderer.SetPosition(currentIndex, StartPos + direction * 56);
                currentIndex++;
                laserPoints.Add(StartPos + direction);

                break;
            }

            
        }

        lineRenderer.positionCount = laserPoints.Count;

        for (int j = 0; j < laserPoints.Count; j++)
        {
            lineRenderer.SetPosition(j, laserPoints[j]);
        }
        // Trim unused positions
        lineRenderer.positionCount = currentIndex;
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    void SlowTimeAbility()
    {
        slowTimeDurationTimer -= Time.deltaTime * (1 + slowTimeRate);
        //Duration timer runs out
        if (slowTimeDurationTimer <= 0)
        {
            gm.NormalTime();
            SlowingTime = false;
            CanSlowTime = false;

        }
        else
        {
            SlowingTime = true;
            gm.SlowTime(slowTimeRate);
            Debug.Log("Time Slowed" + slowTimeDurationTimer);
        }
    }
    void RechargeSlowTimeAbility()
    {
        slowTimeDurationTimer += Time.deltaTime;
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(SecondaryProjectile, laserPoints[0], Quaternion.identity);

        StartCoroutine(MoveProjectile(projectile));
    }

    IEnumerator MoveProjectile(GameObject projectile)
    {
        for (int i = 0; i < laserPoints.Count - 1; i++)
        {
            Vector3 start = laserPoints[i];
            Vector3 end = laserPoints[i + 1];

            while (Vector3.Distance(projectile.transform.position, end) > 0.05f)
            {
                Vector3 direction = (end - projectile.transform.position).normalized;

                projectile.transform.position += direction * secondaryFireSpeed * Time.deltaTime;

                // Rotate projectile to face movement direction
                if (direction != Vector3.zero)
                {
                    projectile.transform.rotation =
                        Quaternion.LookRotation(direction);
                }

                yield return null;
            }

            projectile.transform.position = end;
        }

        Destroy(projectile);
    }


}