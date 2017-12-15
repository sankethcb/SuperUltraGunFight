using UnityEngine;

/// <summary>
/// Author: Dante Nardo
/// Last Modified: 11/30/2017
/// Purpose: Adds player specific controls and jump to the Controller2D.
/// </summary>
public class PlayerController : Controller2D
{
    #region PlayerController Members

    public PlayerAnimation m_playerAnimator;

    [Header("Input Strings")]
    public string m_iHorizontal;
    public string m_iVertical;
    public string m_iJump;
    public AudioClip m_aJump;

    public PlayerState m_playerState;
    public enum PlayerState
    {
        IDLE,
        IDLE_SHOOT,
        RUN,
        RUN_SHOOT,
        JUMP,
        JUMP_SHOOT
    }

    protected PlayerFacing m_facing;
    public PlayerFacing Facing
    {
        get
        {
            if (m_dirX == 1)
            {
                m_facing = PlayerFacing.RIGHT;
                return m_facing;
            }
            else
            {
                m_facing = PlayerFacing.LEFT;
                return m_facing;
            }
        }
    }
    public enum PlayerFacing
    {
        LEFT,
        RIGHT
    }
    #endregion

    #region PlayerController Methods
    protected new void Awake()
    {
        base.Awake();
        m_playerAnimator = GetComponent<PlayerAnimation>();
    }

    protected void Update()
    {
        GetInput();
        ComputeVelocity();
        Move(m_velocity * Time.deltaTime);

        if (m_collisions.m_above || m_collisions.m_below)
        {
            m_velocity.y = 0f;
        }

        #region Player State Updating
        // Set player state for idle, jump, and idle - no shooting
        if (m_playerAnimator.m_shooting == false)
        {
            if (m_velocity.y != 0)
            {
                m_playerState = PlayerState.JUMP;
            }
            else if (m_velocity.x != 0 && m_playerState != PlayerState.JUMP &&
                     Mathf.Abs(m_velocity.x) > 0.01f)
            {
                m_playerState = PlayerState.RUN;
            }
            else
            {
                m_playerState = PlayerState.IDLE;
            }
        }
        // Set player state for idle, jump, and idle - shooting
        else
        {
            if (m_velocity.y != 0)
            {
                m_playerState = PlayerState.JUMP_SHOOT;
            }
            else if (m_velocity.x != 0 && m_playerState != PlayerState.JUMP_SHOOT &&
                     Mathf.Abs(m_velocity.x) > 0.01f)
            {
                m_playerState = PlayerState.RUN_SHOOT;
            }
            else
            {
                m_playerState = PlayerState.IDLE_SHOOT;
            }
        }
        #endregion
    }

    private void GetInput()
    {
        // Get input and set speed
        m_input = new Vector2(
            Input.GetAxisRaw(m_iHorizontal),
            Input.GetAxisRaw(m_iVertical));

        // Get jump input
        if (Input.GetButtonDown(m_iJump) && m_canJump && m_grounded)
        {
            m_playerState = PlayerState.JUMP;
            JumpInputDown();
           
        }

        
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
            GetComponent<AudioSource>().clip = m_aJump;
            GetComponent<AudioSource>().Play();
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
