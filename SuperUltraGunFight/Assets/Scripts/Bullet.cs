using UnityEngine;

/// <summary>
/// Author: Dante Nardo
/// Last Modified: 11/15/2017
/// Purpose: Controls the bullet physics.
/// </summary>
public class Bullet : MonoBehaviour
{
    #region Bullet Members
    public string[] m_killTag;
    #endregion

    #region Bullet Methods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < m_killTag.Length; i++)
        {
            if (collision.collider.gameObject.tag == m_killTag[i])
            {
                collision.gameObject.GetComponent<PlayerScore>().score -= 1;
                if (collision.gameObject.GetComponent<PlayerScore>().score > 0)
                {
                    collision.gameObject.transform.position = new Vector3(0, 6, 0);
                }
                else
                {
                    Destroy(collision.gameObject);
                    
                }
                Destroy(gameObject);
            }
        }

        if(collision.gameObject.tag == "Map")
        {
            Destroy(gameObject);
        }
    }


    #endregion
}
