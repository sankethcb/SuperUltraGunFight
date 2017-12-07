using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGun : MonoBehaviour {

    public GunType m_GunPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2" || collision.gameObject.tag == "Player3" || collision.gameObject.tag == "Player4")
        {
            collision.gameObject.GetComponent<FireGun>().m_currentGun = m_GunPickup;


        }
        Debug.Log("Hit");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2" || collision.gameObject.tag == "Player3" || collision.gameObject.tag == "Player4")
        {
            collision.gameObject.GetComponent<FireGun>().m_currentGun = m_GunPickup;


        }
        Debug.Log("Hit");
    }
}
