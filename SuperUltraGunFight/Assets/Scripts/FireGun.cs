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
    #endregion

    #region FireGun Methods
    void Start()
    {
        m_player = GetComponent<PlayerController>();
    }
    
    void Update()
    {
        if (Input.GetButtonDown(m_fireKey) && m_active)
        {
            Fire();
        }
        if (Input.GetButtonDown(m_fire2Key) && m_active)
        {
            Fire2();
        }
    }
    
    void Fire()
    {
        GameObject firedBullet = Instantiate(m_bullet, new Vector3( transform.position.x + m_player.m_dirX*.3f, transform.position.y , 0), Quaternion.identity);
        Physics2D.IgnoreCollision(firedBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        firedBullet.GetComponent<Rigidbody2D>().AddForce(transform.right * m_player.m_dirX * m_bulletSpeed);
        //firedBullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(m_player.m_dirX*50f, 50f));
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
