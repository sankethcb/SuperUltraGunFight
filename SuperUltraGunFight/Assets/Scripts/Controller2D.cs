using System.Collections;
using UnityEngine;

/// <summary>
/// Author: Dante Nardo
/// Last Modified: 11/29/2017
/// Purpose: Handles the basic movement and collisions of all controllers.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    #region Controller2D Members

    // Syntax sugar for raycast origin locations
    public struct RaycastOrigins
    {
        public Vector2 m_topLeft, m_topRight;
        public Vector2 m_bottomLeft, m_bottomRight;
        public float m_horizontalSpacing, m_verticalSpacing;
        public int m_horizontalCount, m_verticalCount;
    }

    // Syntax sugar and struct to keep family of data
    public struct CollisionInfo
    {
        public bool m_above, m_below;
        public bool m_left, m_right;
        public Vector2 m_moveAmountOld;

        public void Reset()
        {
            m_above = m_below = false;
            m_left = m_right = false;
        }
    }

    [Header("Platforming Variables")]
    public float m_velocityXSmooth = 1f;
    public float m_minVelocity = 0.1f;
    public float m_maxVelocity = 5f;
    public float m_minAcceleration = 0.1f;
    public float m_maxAcceleration = 10.0f;
    public float m_maxJumpVelocity = 10.5f;
    public float m_minJumpVelocity = 0.3f;

    public float m_groundFactor = 0.01f;
    public float m_airFactor = 0.05f;

    protected CollisionInfo m_collisions;           // A struct for collision data
    protected BoxCollider2D m_collider;             // The bounding box of the sprite
    protected RaycastOrigins m_rayInfo;             // The four corners to raycast from, the ray spacing, and raycount
    public LayerMask m_collisionMask;               // The layer which we detect collisions on
    public const float m_buffer = .015f;            // A buffer for raycasting
    public const float m_rayDistance = .25f;        // The distance between each ray

    public float m_mass;                            // The mass of the object
    protected Vector2 m_input;                      // Input values from gamepad/keyboard
    protected Vector2 m_velocity;                   // Determines how quickly we change position
    protected Vector2 m_acceleration;               // Determines how quickly we change velocity
    public float m_gravity;                         // Gravity value to increase negative velocity
    public float m_speed;                           // Speed value to add to velocity
    public int m_dirX;                              // Current direction the controller is moving : 1 is right, -1 is left
    public int m_dirY;                              // Current direction the controller is moving : 1 is up, -1 is down

    protected bool m_moving;                        // Whether or not we are moving
    protected bool m_grounded;                      // Whether or not we are grounded
    protected bool m_jumping = false;               // Whether or not we are jumping
    protected bool m_jumpedLast;                    // Whether or not we jumped last frame
    protected bool m_canJump = true;                // Whether or not we can jump right now
    #endregion

    #region Controller2D Methods
    private void Awake()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        CalculateRaycastOrigins();
        CalculateRaycastCount();
    }

    protected void Move(Vector2 moveAmount)
    {
        // Prepare for collision detection
        CalculateRaycastOrigins();
        m_collisions.Reset();
        m_collisions.m_moveAmountOld = moveAmount;

        // Set facing direction
        if (m_input.x != 0)
        {
            m_dirX = (int)Mathf.Sign(moveAmount.x);
        }
        
        // Check for horizontal collisions - always necessary
        HorizontalCollisions(ref moveAmount);

        // Check for vertical collisions if necessary
        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);
    }

    protected void HorizontalCollisions(ref Vector2 moveAmount)
    {
        // Prepare origin and ray length for raycasting
        if (moveAmount.x != 0)
        {
            m_dirX = (int)Mathf.Sign(moveAmount.x);
        }
        float width = m_rayInfo.m_bottomRight.x - m_rayInfo.m_bottomLeft.x;
        float rayLength = Mathf.Abs(moveAmount.x) + m_buffer + width;

        // Complete all horizontal raycasts
        for (int i = 0; i < m_rayInfo.m_horizontalCount; i++)
        {
            // Update origin for each ray
            Vector2 rayOrigin = (m_dirX == 1) ? m_rayInfo.m_bottomLeft : m_rayInfo.m_bottomRight;
            rayOrigin += Vector2.up * (m_rayInfo.m_horizontalSpacing * i);

            // Raycast and process hit
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * m_dirX, rayLength, m_collisionMask);
            if (hit && hit.distance != 0)
            {
                moveAmount.x = (hit.distance - m_buffer - width) * m_dirX;
                rayLength = hit.distance;
                m_collisions.m_left = m_dirX == -1;
                m_collisions.m_right = m_dirX == 1;
            }
        }
    }

    protected void VerticalCollisions(ref Vector2 moveAmount)
    {
        // Prepare origin and ray length for raycasting
        m_dirY = (int)Mathf.Sign(moveAmount.y);
        float height = m_rayInfo.m_topRight.y - m_rayInfo.m_bottomRight.y;
        float rayLength = Mathf.Abs(moveAmount.y) + m_buffer + height;

        // Complete all vertical raycasts
        for (int i = 0; i < m_rayInfo.m_verticalCount; i++)
        {
            // Update origin for each ray
            Vector2 rayOrigin = (m_dirY == 1) ? m_rayInfo.m_bottomLeft : m_rayInfo.m_topLeft;
            rayOrigin += Vector2.right * (m_rayInfo.m_verticalSpacing * i);

            // Raycast and process hit
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * m_dirY, rayLength, m_collisionMask);
            if (hit && hit.distance != 0)
            {
                moveAmount.y = (hit.distance - m_buffer - height) * m_dirY;
                rayLength = hit.distance;
                m_collisions.m_below = m_dirY == -1;
                m_collisions.m_above = m_dirY == 1;
            }

            // Detect ground/finish jumping
            if (m_collisions.m_below && (hit.distance - height <= m_buffer * 2))
            {
                m_grounded = true;
                m_canJump = true;
                m_jumping = false;
            }
        }
    }

    protected void OnGroundExit()
    {
        // If we are not jumping when we leave the ground, and
        // coyote time isn't active, perform Coyote Time Coroutine
        if (!m_jumping)
            StartCoroutine(CoyoteTime(0.5f));

        m_grounded = false;
    }

    protected void CalculateRaycastOrigins()
    {
        Bounds bounds = m_collider.bounds;
        bounds.Expand(m_buffer * -2);

        m_rayInfo.m_bottomLeft = new Vector2(m_collider.bounds.min.x, m_collider.bounds.min.y);
        m_rayInfo.m_bottomRight = new Vector2(m_collider.bounds.max.x, m_collider.bounds.min.y);
        m_rayInfo.m_topLeft = new Vector2(m_collider.bounds.min.x, m_collider.bounds.max.y);
        m_rayInfo.m_topRight = new Vector2(m_collider.bounds.max.x, m_collider.bounds.max.y);
    }

    protected void CalculateRaycastCount()
    {
        Bounds bounds = m_collider.bounds;
        bounds.Expand(m_buffer * -2);

        m_rayInfo.m_horizontalCount = Mathf.RoundToInt(bounds.size.y / m_rayDistance);
        m_rayInfo.m_verticalCount = Mathf.RoundToInt(bounds.size.x / m_rayDistance);

        m_rayInfo.m_horizontalSpacing = bounds.size.y / (m_rayInfo.m_horizontalCount - 1);
        m_rayInfo.m_verticalSpacing = bounds.size.x / (m_rayInfo.m_verticalCount - 1);
    }

    #region Velocity Methods

    protected virtual void ComputeVelocity()
    {
        float targetVelocityX = m_input.x * m_speed;
        m_velocity.x = Mathf.SmoothDamp(m_velocity.x, targetVelocityX, ref m_velocityXSmooth, (m_collisions.m_below ? m_groundFactor : m_airFactor));
        m_velocity.y += m_gravity * Time.deltaTime;
    }

    /// <summary>
    /// A special function always performed after ComputeVelocity
    /// This handles all of the "game feel" of the velocity
    /// modifying it such that it feels good to move controller around
    /// </summary>
    private void EnhanceVelocity()
    {
        //// Ignore low speed
        //if (m_speed * m_speed < m_minVelocity)
        //    m_speed = 0;

        //// Give speed boost when the controller changes direction
        //if (m_speed > 0 != m_velocity.x > 0 && m_velocity.x != 0)
        //    m_speed *= SPEED_BOOST;

        //// Increase friction significantly if the player stopped
        //// moving near a threat such as a spike
        //if (!m_moving && m_grounded && ThreatNearby())
        //    m_friction = HIGH_FRICTION;
    }

    // TODO: Implement threat detection
    private bool ThreatNearby()
    {
        return false;
    }
    #endregion

    #region Jump Methods

    /// <summary>
    /// No longer used, but necessary TODO the edge cases.
    /// </summary>
    //protected virtual void Jump()
    //{
    //    // If we just jumped set the starting values
    //    if (!m_jumping && m_collisions.m_below && m_canJump)
    //    {
    //        m_velocity.y = JUMP_START - m_gravity;
    //        m_jumpTime = 0;
    //        m_jumping = true;
    //        m_jumpTime++;
    //        return;
    //    }

    //    // EDGE CASE: Players pressing jump momentarily before
    //    // they touch the ground. We start a Coroutine that
    //    // makes them jump once they hit the ground
    //    if (ControllerNearGround())
    //        StartCoroutine(JumpOnGroundHit());

    //    // EDGE CASE: Players pressing jump momentarily after 
    //    // they leave touch the ground. We treat this as if the
    //    // player just jumped normally
    //    if (ControllerJustFell())
    //    {
    //        m_velocity.y = JUMP_START - m_gravity;
    //        m_jumpTime = 0;
    //        m_jumping = true;
    //        m_jumpTime++;
    //        return;
    //    }
    //}

    // TODO: Implement controller near ground detection
    private bool ControllerNearGround()
    {
        return false;
    }

    // TODO: Implement controller left ground detection
    private bool ControllerJustFell()
    {
        return false;
    }

    // TODO: Implement jump once controller hits ground
    private IEnumerator JumpOnGroundHit()
    {
        yield return null;
    }

    // TODO: Implement can jump for period of time after
    // the controller leaves the ground
    private IEnumerator CoyoteTime(float time)
    {
        m_canJump = true;
        yield return new WaitForSeconds(time);
        m_canJump = false;
    }
    #endregion
    #endregion
}
