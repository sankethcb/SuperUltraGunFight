using UnityEngine;

/// <summary>
/// Author: Dante Nardo
/// Last Modified: 11/15/2017
/// Purpose: Controls the bullet physics.
/// </summary>
public class Bullet : MonoBehaviour
{
    #region Bullet Members
    public string m_killTag;
    #endregion

    #region Bullet Methods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == m_killTag)
        {
            collision.gameObject.transform.position = new Vector3(0, 6, 0);
            //Destroy(collision.collider.gameObject);
        }
    }
    #endregion
}
