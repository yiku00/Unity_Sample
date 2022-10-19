using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;

    private AudioManager_PrototypeHero m_audioManager;
    [SerializeField] GameObject m_RunStopDust;
    [SerializeField] GameObject m_JumpDust;
    [SerializeField] GameObject m_LandingDust;

    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        m_audioManager = AudioManager_PrototypeHero.instance;

        
        m_RunStopDust = Resources.Load<GameObject>("Character/Prototype_Hero_Demo/Demo/RunStopDust");
        m_JumpDust = Resources.Load<GameObject>("Character/Prototype_Hero_Demo/Demo/JumpDust");
        m_LandingDust = Resources.Load<GameObject>("Character/Prototype_Hero_Demo/Demo/LandingDust");
        Instantiate(m_RunStopDust);
        Instantiate(m_JumpDust);
        Instantiate(m_LandingDust);

        SetupSoundFile();

    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);

        }
            

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
    void SpawnDustEffect(GameObject dust, float dustXOffset = 0)
    {
        if (dust != null)
        {
            // Set dust spawn position
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * m_facingDirection, 0.0f, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            // Turn dust in correct X direction
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(m_facingDirection, 1, 1);
        }
        else
        {
            Debug.Log("The Object" + dust.name + " Is NUll");
        }
    }

    void AE_runStop()
    {
        m_audioManager.PlaySound("RunStop");
        // Spawn Dust
        float dustXOffset = 0.6f;
        SpawnDustEffect(m_RunStopDust, dustXOffset);
    }
    void AE_Attack()
    {
        m_audioManager.PlaySound("Attack");
    }

    void AE_footstep()
    {
        m_audioManager.PlaySound("Footstep");
    }

    void AE_jump()
    {
        m_audioManager.PlaySound("Jump");
        // Spawn Dust
        SpawnDustEffect(m_JumpDust);
    }

    void AE_Landing()
    {
        m_audioManager.PlaySound("Landing");
        // Spawn Dust
        SpawnDustEffect(m_LandingDust);
    }
    void AE_death()
    {
        m_audioManager.PlaySound("Death");
    }

    void AE_hurt()
    {
        m_audioManager.PlaySound("Hurt");
    }

    void AE_Block()
    {
        m_audioManager.PlaySound("Block");
    }

    void AE_Evade()
    {
        m_audioManager.PlaySound("Evade");
    }

    private void SetupSoundFile()
    {
        m_audioManager.AddSound("Character/Prototype_Hero_Demo/Audio/Footstep1", "Footstep");
        m_audioManager.AddSound("Character/Prototype_Hero_Demo/Audio/Footstep2", "Footstep");
        m_audioManager.AddSound("Character/Prototype_Hero_Demo/Audio/Footstep3", "Footstep");
        m_audioManager.AddSound("Character/Prototype_Hero_Demo/Audio/Footstep1", "RunStop");
        m_audioManager.AddSound("Character/Prototype_Hero_Demo/Audio/Jump", "Jump");
        m_audioManager.AddSound("Character/Prototype_Hero_Demo/Audio/Landing", "Landing");
        m_audioManager.AddSound("Sound/Blades&Bludgeonings-Free/Swing1-Free-1", "Attack");
        m_audioManager.AddSound("Sound/Blades&Bludgeonings-Free/Swing4-Free-1", "Attack");
        m_audioManager.AddSound("Sound/Voice_Male/Voice_Male_Pain/Voice_Male_V2_Pain_Mono_04", "Death");
        m_audioManager.AddSound("Sound/Voice_Male/Voice_Male_Pain/Voice_Male_V1_Pain_Mono_11", "Death");
        m_audioManager.AddSound("Sound/Voice_Male/Voice_Male_Hit/Voice_Male_V1_Hit_Short_Mono_11", "Hurt");
        m_audioManager.AddSound("Sound/Voice_Male/Voice_Male_Hit/Voice_Male_V1_Hit_Short_Mono_13", "Hurt");
        m_audioManager.AddSound("Sound/Voice_Male/Voice_Male_Hit/Voice_Male_V1_Hit_Short_Mono_09", "Hurt");
        m_audioManager.AddSound("Sound/RPG_Essentials_Free/10_Battle_SFX/39_Block_03", "Block");
        m_audioManager.AddSound("Sound/RPG_Essentials_Free/10_Battle_SFX/35_Miss_Evade_02", "Evade");
    }
}
