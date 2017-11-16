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
    public GameObject m_bullet;
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
    }
    
    void Fire()
    {
        GameObject firedBullet = Instantiate(m_bullet, transform.position, Quaternion.identity);
        firedBullet.GetComponent<Rigidbody2D>().AddForce(transform.right * m_player.m_dirX * m_bulletSpeed);
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
