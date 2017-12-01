using System.Collections;
using UnityEngine;

/// <summary>
/// Author: Dante Nardo
/// Last Modified: 11/30/2017
/// Purpose: Controls the player animations.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerController))]
public class PlayerAnimation : MonoBehaviour
{
    #region PlayerAnimation Members
    // Components and variables for animation
    public PlayerController m_player;
    public SpriteRenderer m_sprRender;
    public float m_fps = 4;
    public bool m_running = false;
    public bool m_shooting = false;
    public float m_shootTime;

    // Sprites
    public Sprite m_playerIdle;
    public Sprite m_playerIdleShoot;
    public Sprite[] m_playerRun;
    public Sprite[] m_playerRunShoot;
    public Sprite m_playerJump;
    public Sprite m_playerJumpShoot;
    #endregion

    #region PlayerAnimation Methods
    private void Start()
    {
        m_player = GetComponent<PlayerController>();
        m_sprRender = GetComponent<SpriteRenderer>();
        m_running = false;
        m_shooting = false;
    }

    private void Update()
    {
        switch (m_player.m_playerState)
        {
            // IDLE ANIMATION
            case PlayerController.PlayerState.IDLE:
                m_sprRender.sprite = m_playerIdle;
                break;

            // IDLE SHOOT ANIMATION
            case PlayerController.PlayerState.IDLE_SHOOT:
                if (!m_shooting)
                {
                    StartCoroutine(Shooting());
                }
                m_sprRender.sprite = m_playerIdleShoot;
                break;

            // RUN ANIMATION
            case PlayerController.PlayerState.RUN:
                if (!m_running)
                {
                    StartCoroutine(Running());
                }
                break;

            // RUN SHOOT ANIMATION
            case PlayerController.PlayerState.RUN_SHOOT:
                if (!m_shooting)
                {
                    StartCoroutine(Shooting());
                }
                if (!m_running)
                {
                    StartCoroutine(Running());
                }
                break;

            // JUMP ANIMATION
            case PlayerController.PlayerState.JUMP:
                m_sprRender.sprite = m_playerJump;
                break;

            // JUMP SHOOT ANIMATION
            case PlayerController.PlayerState.JUMP_SHOOT:
                if (!m_shooting)
                {
                    StartCoroutine(Shooting());
                }
                m_sprRender.sprite = m_playerJumpShoot;
                break;
        }

        if (m_player.Facing == PlayerController.PlayerFacing.LEFT)
        {
            m_sprRender.flipX = true;
        }
        else if (m_player.Facing == PlayerController.PlayerFacing.RIGHT)
        {
            m_sprRender.flipX = false;
        }
    }

    private IEnumerator Shooting()
    {
        m_shooting = true;

        yield return new WaitForSeconds(m_shootTime);

        m_shooting = false;
        yield break;
    }

    private IEnumerator Running()
    {
        m_running = true;
        int index = 0;
        float timeToNextFrame = 1 / m_fps;
        while (m_player.m_playerState == PlayerController.PlayerState.RUN ||
               m_player.m_playerState == PlayerController.PlayerState.RUN_SHOOT)
        {
            yield return new WaitForSeconds(timeToNextFrame);
            
            if (!m_shooting)
            {
                if (++index == m_playerRun.Length)
                {
                    index = 0;
                }
                m_sprRender.sprite = m_playerRun[index];
            }
            else
            {
                if (++index == m_playerRunShoot.Length)
                {
                    index = 0;
                }
                m_sprRender.sprite = m_playerRunShoot[index];
            }
        }

        m_running = false;
        yield break;
    }
    #endregion
}
