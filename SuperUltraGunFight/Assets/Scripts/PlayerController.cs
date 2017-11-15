using UnityEngine;

/// <summary>
/// Author: Dante Nardo
/// Last Modified: 11/14/2017
/// Purpose: Adds player specific controls and jump to the Controller2D.
/// </summary>
public class PlayerController : Controller2D
{
    #region PlayerController Members

    [Header("Input Strings")]
    public string m_iHorizontal;
    public string m_iVertical;
    public string m_iJump;
    #endregion

    #region PlayerController Methods
    protected void Update()
    {
        GetInput();
        ComputeVelocity();
        Move(m_velocity * Time.deltaTime);

        if (m_collisions.m_above || m_collisions.m_below)
            m_velocity.y = 0f;
    }

    private void GetInput()
    {
        // Get input and set speed
        m_input = new Vector2(
            Input.GetAxisRaw(m_iHorizontal), 
            Input.GetAxisRaw(m_iVertical));
        
        if (Input.GetButtonDown(m_iJump) && m_canJump)
            JumpInputDown();

        if (Input.GetButtonUp(m_iJump) && m_canJump)
            JumpInputUp();
    }

    protected override void ComputeVelocity()
    {
        float targetVelocityX = m_input.x * m_speed;
        m_velocity.x = Mathf.SmoothDamp(m_velocity.x, targetVelocityX, ref m_velocityXSmooth, (m_collisions.m_below ? m_groundFactor : m_airFactor));
        m_velocity.y -= m_gravity * Time.deltaTime;
    }

    protected void JumpInputDown()
    {
        if (m_collisions.m_below)
        {
            m_velocity.y = m_maxJumpVelocity;
            m_canJump = false;
            m_jumping = true;
            m_jumpedLast = true;
        }
    }

    protected void JumpInputUp()
    {
        if (m_velocity.y < m_minJumpVelocity)
        {
            m_velocity.y = m_minJumpVelocity;
            m_canJump = false;
            m_jumping = true;
            m_jumpedLast = true;
        }
    }
    #endregion
}
