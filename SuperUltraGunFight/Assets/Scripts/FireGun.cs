using System.Collections;
using UnityEngine;

/// <summary>
/// Author: Sanketh Bhat
/// Last Modified: 11/15/2017
/// Purpose: Fires a bullet when the correct key is pressed.
/// </summary>
public class FireGun : MonoBehaviour
{
    #region FireGun Members
    public PlayerController m_player;
    public string m_fireKey;
    public string m_fire2Key;
    public GameObject m_bullet;
    public GameObject m_grenade;
    public float m_fireRate;
    public float m_bulletSpeed;
    bool m_active = true;
    GunType m_currentGun=GunType.shotgun;
    enum GunType
    {
        pistol,
        shotgun,
        smg,
        rifle

    };

    #endregion

    #region FireGun Methods
    void Start()
    {
        m_player = GetComponent<PlayerController>();
    }
    
    void Update()
    {
        if (Input.GetButton(m_fireKey) && m_active)
        {

        switch(m_currentGun)
            {
                case GunType.pistol: FirePistol();
                    break;
                case GunType.shotgun:
                    FireShotgun();
                    break;
                case GunType.smg:
                    FireSmg();
                    break;
                case GunType.rifle:
                    FireRifle();
                    break;
            }
        }
        if (Input.GetButtonDown(m_fire2Key) && m_active)
        {
            Fire2();
        }
    }
    
    void FirePistol()
    {
        GameObject firedBullet = Instantiate(m_bullet, new Vector3( transform.position.x + m_player.m_dirX*.3f, transform.position.y , 0), Quaternion.identity);
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet.GetComponent<Rigidbody2D>().velocity=(transform.right * m_player.m_dirX * 30);
        firedBullet.transform.localScale = firedBullet.transform.localScale * 0.5f;
        Destroy(firedBullet, 5);
        m_active = false;
        StartCoroutine("Reload");
    }

    void FireShotgun()
    {
        GameObject firedBullet1 = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        GameObject firedBullet2 = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        GameObject firedBullet3 = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);


        Physics2D.IgnoreCollision(firedBullet1.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet1.GetComponent<Rigidbody2D>().velocity = (Quaternion.Euler(new Vector3(0, 0, -10))*(transform.right * m_player.m_dirX) * 30);
        firedBullet1.transform.localScale = firedBullet3.transform.localScale * 0.3f;

        Physics2D.IgnoreCollision(firedBullet2.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet2.GetComponent<Rigidbody2D>().velocity = (transform.right * m_player.m_dirX * 30);
        firedBullet2.transform.localScale = firedBullet3.transform.localScale * 0.3f;
         
        Physics2D.IgnoreCollision(firedBullet3.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet3.GetComponent<Rigidbody2D>().velocity = (Quaternion.Euler(new Vector3(0,0,10))*(transform.right * m_player.m_dirX) * 30);
        firedBullet3.transform.localScale = firedBullet3.transform.localScale * 0.3f;


        Physics2D.IgnoreCollision(firedBullet1.GetComponent<BoxCollider2D>(), firedBullet2.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(firedBullet1.GetComponent<BoxCollider2D>(), firedBullet3.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(firedBullet2.GetComponent<BoxCollider2D>(), firedBullet3.GetComponent<BoxCollider2D>());

        Destroy(firedBullet1, 0.2f);
        Destroy(firedBullet2, 0.2f);
        Destroy(firedBullet3, 0.2f);
        m_active = false;
        StartCoroutine("Reload");
    }

    void FireSmg()
    {
        GameObject firedBullet = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        firedBullet.transform.localScale = firedBullet.transform.localScale * 0.3f;
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet.GetComponent<Rigidbody2D>().velocity = (transform.right * m_player.m_dirX * m_bulletSpeed);
        m_fireRate = 0.3f;
        Destroy(firedBullet, 3);
        m_active = false;
        StartCoroutine("Reload");
    }

    void FireRifle()
    {
        GameObject firedBullet = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        firedBullet.transform.localScale = firedBullet.transform.localScale * 0.8f;
        m_fireRate = 3;
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet.GetComponent<Rigidbody2D>().velocity = (transform.right * m_player.m_dirX * 60);

        Destroy(firedBullet, 2);
        m_active = false;
        StartCoroutine("Reload");
    }

    void Fire()
    {
        GameObject firedBullet = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet.GetComponent<Rigidbody2D>().velocity = (transform.right * m_player.m_dirX * m_bulletSpeed);

        Destroy(firedBullet, 5);
        m_active = false;
        StartCoroutine("Reload");
    }
    void Fire2()
    {
        GameObject firedBullet = Instantiate(m_grenade, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        //firedBullet.GetComponent<Rigidbody2D>().AddForce(transform.right * m_player.m_dirX * m_bulletSpeed);
        firedBullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(m_player.m_dirX * 50f, 50f));
        Destroy(firedBullet, 5);
        m_active = false;
        StartCoroutine("Reload");
    }



    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_fireRate);
        m_active = true;
    }
    #endregion
}
