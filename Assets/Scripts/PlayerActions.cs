using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour, IDamage
{
    public Transform posGun;
    public Transform cam;

    public LayerMask ingoreLayer;
    RaycastHit hit;
    public int life = 20;
    public int ammo = 10;
    public GameObject damageEffect;
    public float saveInterval = 0.5f;
    float saveTime;
    WaitForSeconds wait;

    void Start()
    {
        damageEffect.SetActive(false);
        saveTime = 0.0f;
        CanvasController.instance.AddTextHp(life);
        CanvasController.instance.AddTextAmmo(ammo);
        wait = new WaitForSeconds(0.2f);
    }

    private void Update()
    {
        Debug.DrawRay(cam.position, cam.forward * 100f, Color.red);
        Debug.DrawRay(posGun.position, cam.forward * 100f, Color.blue);

        if (ammo > 0 && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        saveTime -= Time.deltaTime;
    }

    void Shoot()
    {
        Vector3 direction = cam.TransformDirection(Vector3.forward);
        GameObject bulletObj = ObjectPollingManager.instance.GetBullet(true);

        bulletObj.transform.position = posGun.position;
        if (Physics.Raycast(cam.position, direction, out hit, Mathf.Infinity, ~ingoreLayer))
        {
            bulletObj.transform.LookAt(hit.point);
        }
        else
        {
            Vector3 dir = cam.position + direction * 10f;
            bulletObj.transform.LookAt(dir);
        }

        ammo--;
        CanvasController.instance.AddTextAmmo(ammo);
    }

    public bool DoDamage(int vld, bool isPlayer)
    {
        if (isPlayer) return false;
        else
        {
            if (saveTime <= 0)
            {
                life -= vld;
                CanvasController.instance.AddTextHp(life);
                StartCoroutine(Effect());

                if (life <= 0)
                {
                    GameManager.instance.FinGame(false);
                }
            }
        }
        return true;
    }

    IEnumerator Effect()
    {
        saveTime = saveInterval;
        damageEffect.SetActive(true);
        yield return wait;
        damageEffect.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        IBox box = other.GetComponent<IBox>();
        if (box != null)
        {
            int res = box.OpenBox();
            if (box.getID() == (int)BoxID.HEALTH)
            {
                life += res;
                CanvasController.instance.AddTextHp(life);
            }
            else if (box.getID() == (int)BoxID.AMMO)
            {
                ammo += res;
                CanvasController.instance.AddTextAmmo(ammo);
            }
            Destroy(other.gameObject);
        }
    }
}
