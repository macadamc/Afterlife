using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class RadialProjectileEmitter : State
{
    #region Variables

    [TabGroup("Variables"), Range(1, 360)]
    public float arc = 360f;
    [TabGroup("Variables")]
    public float angleOffset = 0f;
    [TabGroup("Variables"), MinValue(1)]
    public int numberOfBulletsPerWave = 6;
    [TabGroup("Variables"), MinValue(1)]
    public int numberOfBulletWaves = 1;
    [TabGroup("Variables"), MinValue(0)]
    public float instantiateOverXSeconds = 2f;
    [TabGroup("Variables")]
    public bool shootOnEnable = true;
    [TabGroup("Variables"), MinValue(0)]
    public float initialDelay = 0.1f;
    [TabGroup("Variables")]
    public bool changeStateOnFinish = true;
    [TabGroup("Variables"), ShowIf("changeStateOnFinish"), PropertyTooltip("If null, will just call Next().")]
    public GameObject nextState;
    [TabGroup("Variables"), Required, AssetList(Tags = "Projectile")]
    public GameObject bulletPrefab;

    [TabGroup("Events")]
    public UnityEvent onShootEvent;



    #endregion

    [Button, DisableInEditorMode]
    private void Execute()
    {
        StopAllCoroutines();
        StartCoroutine(ShootBulletWaves());
    }

    private IEnumerator ShootBulletWaves()
    {
        float delayBetweenWaves = instantiateOverXSeconds / numberOfBulletWaves;
        if (initialDelay > 0)
            yield return new WaitForSeconds(initialDelay);

        for (int r = 0; r < numberOfBulletWaves; r++)
        {
            float forwardAngle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

            if (numberOfBulletsPerWave == 1)
            {
                //Shoot(bulletPrefab, startAngle + currentRotation);
                Shoot(bulletPrefab, forwardAngle + angleOffset);
            }
            else
            {
                for (int i = 0; i < numberOfBulletsPerWave; i++)
                {
                    float deltaAngle = arc / numberOfBulletsPerWave;
                    //float currentAngle = startAngle - (arc / 2) + (deltaAngle / 2) + (deltaAngle * i);
                    float currentAngle = -(arc / 2) + (deltaAngle / 2) + (deltaAngle * i);

                    Shoot(bulletPrefab, currentAngle + forwardAngle + angleOffset);
                }
            }
            onShootEvent?.Invoke();

            if (delayBetweenWaves > 0)
                yield return new WaitForSeconds(delayBetweenWaves);
        }
        yield return null;

        if (changeStateOnFinish)
        {
            if (nextState == null)
                Next();
            else
                ChangeState(nextState);
        }
    }

    private void Shoot(GameObject bulletPrefab, Vector2 dir)
    {
        // creates bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //  gets the angle from the look direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //  rotates object to face the new angle
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Physics2D.IgnoreCollision(transform.GetComponentInParent<Collider2D>(), bullet.GetComponent<Collider2D>());
    }

    private void Shoot(GameObject bulletPrefab, float angle)
    {
        // creates bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //  rotates object to face the new angle
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Physics2D.IgnoreCollision(transform.GetComponentInParent<Collider2D>(), bullet.GetComponent<Collider2D>());
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (shootOnEnable)
            Execute();
    }
}
