using System.Collections;
using UnityEngine;

/// <summary>
/// Author: Sanketh Bhat
/// Last Modified: 11/30/2017
/// Purpose: Fires a bullet when the correct key is pressed.
/// </summary>
[RequireComponent(typeof(PlayerController))]
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
    public AudioClip m_aFire1;
    public AudioClip m_aFire2;
    bool m_active = true;
    public GunType m_currentGun=GunType.SHOTGUN;


    #endregion


    public int pistolDammage = 2;
    public int shotgunDammage = 1;
    public int machineGunDammage = 1;
    public int grenadeDammage = 10;
    public int sniperDammage = 5;


    #region FireGun Methods
    private void Start()
    {
        m_player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButton(m_fireKey) && m_active)
        {
            // Add shooting to player state
            switch (m_player.m_playerState)
            {
                case PlayerController.PlayerState.IDLE:
                    m_player.m_playerState = PlayerController.PlayerState.IDLE_SHOOT;
                    break;
                case PlayerController.PlayerState.RUN:
                    m_player.m_playerState = PlayerController.PlayerState.RUN_SHOOT;
                    break;
                case PlayerController.PlayerState.JUMP:
                    m_player.m_playerState = PlayerController.PlayerState.JUMP_SHOOT;
                    break;
            }

            switch(m_currentGun)
            {
                case GunType.PISTOL:
                    FirePistol();
                    break;
                case GunType.SHOTGUN:
                    FireShotgun();
                    break;
                case GunType.SMG:
                    FireSmg();
                    break;
                case GunType.RIFLE:
                    FireRifle();
                    break;
                case GunType.GRENADE:
                    Fire2();
                    break;
                
            }
        }
        if (Input.GetButtonDown(m_fire2Key) && m_active)
        {
            // Add shooting to player state
            switch (m_player.m_playerState)
            {
                case PlayerController.PlayerState.IDLE:
                    m_player.m_playerState = PlayerController.PlayerState.IDLE_SHOOT;
                    break;
                case PlayerController.PlayerState.RUN:
                    m_player.m_playerState = PlayerController.PlayerState.RUN_SHOOT;
                    break;
                case PlayerController.PlayerState.JUMP:
                    m_player.m_playerState = PlayerController.PlayerState.JUMP_SHOOT;
                    break;
            }

            //Fire2();
        }
    }

    private void FirePistol()
    {
        GameObject firedBullet = Instantiate(m_bullet, new Vector3( transform.position.x + m_player.m_dirX*.3f, transform.position.y , 0), Quaternion.identity);
        firedBullet.GetComponent<Bullet>().Dammage = pistolDammage;
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet.GetComponent<Rigidbody2D>().velocity=(transform.right * m_player.m_dirX * 30);
        firedBullet.transform.localScale = firedBullet.transform.localScale * 0.5f;

        GetComponent<AudioSource>().clip = m_aFire1;
        GetComponent<AudioSource>().Play();
        Destroy(firedBullet, 5);
        m_active = false;
        StartCoroutine("Reload");
    }

    private void FireShotgun()
    {
        GameObject firedBullet1 = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        GameObject firedBullet2 = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        GameObject firedBullet3 = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        firedBullet1.GetComponent<Bullet>().Dammage = shotgunDammage;
        firedBullet2.GetComponent<Bullet>().Dammage = shotgunDammage;
        firedBullet3.GetComponent<Bullet>().Dammage = shotgunDammage;

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
        GetComponent<AudioSource>().clip = m_aFire2;
        GetComponent<AudioSource>().pitch = 0.5f;
        GetComponent<AudioSource>().Play();
        m_fireRate = 1.0f;
        Destroy(firedBullet1, 0.2f);
        Destroy(firedBullet2, 0.2f);
        Destroy(firedBullet3, 0.2f);
        m_active = false;
        StartCoroutine("Reload");
    }

    private void FireSmg()
    {
        GameObject firedBullet = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        firedBullet.transform.localScale = firedBullet.transform.localScale * 0.3f;
        firedBullet.GetComponent<Bullet>().Dammage = machineGunDammage;
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet.GetComponent<Rigidbody2D>().velocity = (transform.right * m_player.m_dirX * m_bulletSpeed);
        m_fireRate = 0.3f;
        Destroy(firedBullet, 3);
        m_active = false;

        GetComponent<AudioSource>().clip = m_aFire1;
        GetComponent<AudioSource>().pitch = 1.5f;
        GetComponent<AudioSource>().Play();
      
        StartCoroutine("Reload");
    }

    private void FireRifle()
    {
        GameObject firedBullet = Instantiate(m_bullet, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        firedBullet.GetComponent<Bullet>().Dammage = sniperDammage;

        firedBullet.transform.localScale = firedBullet.transform.localScale * 0.8f;
        m_fireRate = 3;
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet.GetComponent<Rigidbody2D>().velocity = (transform.right * m_player.m_dirX * 60);
        GetComponent<AudioSource>().clip = m_aFire2;
        GetComponent<AudioSource>().pitch = 2.0f;
        GetComponent<AudioSource>().Play();
        
        Destroy(firedBullet, 2);
        m_active = false;
        StartCoroutine("Reload");
    }



    private void Fire2()
    {
        GameObject firedBullet = Instantiate(m_grenade, new Vector3(transform.position.x + m_player.m_dirX * .3f, transform.position.y, 0), Quaternion.identity);
        firedBullet.GetComponent<Bullet>().Dammage = grenadeDammage;
        m_fireRate = 4;
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        //firedBullet.GetComponent<Rigidbody2D>().AddForce(transform.right * m_player.m_dirX * m_bulletSpeed);
        firedBullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(m_player.m_dirX * 50f, 50f));
        Destroy(firedBullet, 5);
        m_active = false;
        StartCoroutine("Reload");
    }

 
   

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_fireRate);
        m_active = true;
        GetComponent<AudioSource>().pitch = 1.0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
    }

    #endregion
}
