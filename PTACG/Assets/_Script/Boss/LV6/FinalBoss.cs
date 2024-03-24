using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    const string FinalBossIdle = "FinalBossIdle";
    const string FinalBossHurt = "FinalBossSealHurt";
    const string FinalBossLaserCast = "FinalBossLaserCast";
    const string FinalBossRange = "FinalBossRange";
    const string FinalBossMelee = "FinalBossMelee";
    const string FinalBossDeath = "FinalBossDeath";
    const string FinalBossSealDefault = "FinalBossSealDefault";

    [SerializeField] private GameObject bgm;

    private string currentState;
    Rigidbody2D rb2d;
    Animator animator;
    private SpriteRenderer spriteRenderer;
    //private Health bosshealth;
    //private CircleCollider2D circle2d;
    private bool canAttack = true;
    private bool isDead;
    private bool secondPhase;
    private bool respawn;
    private bool canUseSkill = true;
    private float randomAnimationTimer = 0f;
    private float randomAnimationInterval = 0f;

    [SerializeField] private float attackDelay = 1.1f;
    [SerializeField] private float bossMoveSpeed = 130f;
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float stopRange = 5f;
    [SerializeField] private float normalRandomTime = 7f;
    //[SerializeField] private float secondPhaseRandomTime = 5f;
    [SerializeField] private float minRandomTime = 2f;

    [Header("FinalBoss Projectile")]
    [SerializeField] private GameObject FB_ChargedProjectile;
    [SerializeField] private GameObject FB_Projectile;
    [SerializeField] private GameObject FB_HugeChargedProjectile;
    [SerializeField] private GameObject FB_HugeProjectile;
    [SerializeField] private GameObject FB_LaserCast;
    [SerializeField] private Transform LaserCastTransform;
    [SerializeField] private Health health;

    [Header("reference")]
    //[SerializeField] private GameObject Aura;
    [SerializeField] private Transform ShootPosition;
    //[SerializeField] private Transform CenterPosition;
    [SerializeField] private GameObject Player;
    //[SerializeField] private GameObject PlayerEnterCheck;
    //[SerializeField] private GameObject explosionPrefab;

    //private Coroutine randomAnimationCoroutine;

    // Start is called before the first frame update
    void Start()
    {

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        //bosshealth = GetComponentInParent<Health>();
        //circle2d = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //randomAnimationCoroutine = StartCoroutine(PlayRandomAnimation());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health.CurrentShield <= 0)
        {
            UIManager uiManager = GameObject.FindObjectOfType<UIManager>();
            bgm.SetActive(true);
            SoundManager.Instance.StopMusic();
            if (uiManager != null)
            {
                uiManager.StartCoroutine(uiManager.BossFight());
            }
            else
            {
                Debug.LogWarning("Lvl4UIManager component not found in the scene.");
            }

            // Update the random animation timer
            randomAnimationTimer += Time.deltaTime;

            // Check if it's time to play a random animation
            if (randomAnimationTimer >= randomAnimationInterval && !IsAttacking() && canUseSkill && canAttack && !isDead)
            {
                PlayRandomAnimation();
                randomAnimationTimer = 0f; // Reset the timer
            }

            if (health.CurrentHealth <= 0)
            {
                isDead = true;
                //Aura.SetActive(false);
                //secondPhase = false;
            }

            if (IsNotMoving() && canAttack && !IsAttacking() && !isDead)
            {
                ChangeITAAnimationState(FinalBossIdle);
            }


            if (!isDead)
            {

                if (withinAtkRange())
                {
                    Debug.Log("NormalAtk");
                    canAttack = false;
                    if (!IsAttacking())
                    {
                        ChangeITAAnimationState(FinalBossRange);
                        Invoke("FinishAtk", attackDelay);
                    }
                }
                else if (outsideAtkRange() && !IsAttacking())
                {
                    // Get the position of the player
                    Vector3 playerPosition = Player.transform.position;

                    // Calculate the direction to the player
                    Vector3 moveDirection = (playerPosition - transform.position).normalized;

                    // Add a force to the boss in the direction of the player
                    rb2d.velocity = moveDirection * bossMoveSpeed;
                    rb2d.AddForce(moveDirection * bossMoveSpeed);


                    // Flip the boss sprite if necessary
                    if (moveDirection.x < 0)
                    {
                        // Facing right
                        // Invert the sprite's x scale to face left
                        if (spriteRenderer.transform.localScale.x > 0)
                        {
                            spriteRenderer.transform.localScale = new Vector3(-1 * Mathf.Abs(spriteRenderer.transform.localScale.x), spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
                        }
                    }
                    else if (moveDirection.x > 0)
                    {
                        // Facing left
                        // Invert the sprite's x scale to face right
                        if (spriteRenderer.transform.localScale.x < 0)
                        {
                            spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(spriteRenderer.transform.localScale.x), spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
                        }
                    }
                    ChangeITAAnimationState(FinalBossIdle);
                }

            }
        }


    }

    private void PlayRandomAnimation()
    {
        // Choose a random animation
        int randomAnimation = Random.Range(0, 3); // Assuming you have 3 animations (0, 1, 2)
        string animationToPlay = FinalBossMelee; // Default to idle animation

        switch (randomAnimation)
        {
            case 0:
                Debug.Log("CastLser");
                animationToPlay = FinalBossRange;
                break;
            case 1:
                animationToPlay = FinalBossRange;
                break;
            case 2:
                animationToPlay = FinalBossRange;
                break;
        }

        // Play the chosen animation
        ChangeITAAnimationState(animationToPlay);
        Debug.Log("RandomAnimation");
        // Set the interval for the next random animation
        if (!secondPhase)
        {
            randomAnimationInterval = Random.Range(minRandomTime, normalRandomTime);
        }
        //else
        //{
        //    randomAnimationInterval = Random.Range(minRandomTime, secondPhaseRandomTime);
        //}
        Invoke("FinishSkill", 1.1f);
    }

    private bool IsAttacking()
    {
        // Check if the magnitude of the velocity is close to zero
        if (currentState == FinalBossMelee || currentState == FinalBossMelee)
            return true;
        else
            return false;
    }

    private bool IsNotMoving()
    {
        // Check if the magnitude of the velocity is close to zero
        return Mathf.Approximately(rb2d.velocity.sqrMagnitude, 0f);
    }

    private bool withinAtkRange()
    {
        // Calculate the distance between the boss and the player
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        return distance <= attackRange;
    }

    private bool outsideAtkRange()
    {
        if (canAttack)
        {
            // Calculate the distance between the boss and the player
            float distance = Vector3.Distance(transform.position, Player.transform.position);
            return distance > stopRange;
        }
        else
            return false;
    }

    private void FinishAtk()
    {
        Debug.Log("FinishAtk");
        canAttack = true;
        ChangeITAAnimationState(FinalBossIdle);
    }

    private void FinishSkill()
    {
        canAttack = true;
        canUseSkill = true;
        ChangeITAAnimationState(FinalBossIdle);
    }

    //change animation ========================================================================
    private void ChangeITAAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }

    public void RandomProjectile()
    {
        int randomNumber = Random.Range(0, 8);
        switch (randomNumber)
        {
            case 0:
                SpawnProjectile();
                //SpawnDarkBolt();
                break;
            case 1:
                SpawnChargedProjectile();
                //SpawnDarkSpark();
                break;
            case 2:
                Spawn360DegreeProjectile();
                //SpawnThunderBullet();
                break;
            case 3:
                Spawn360DegreeChargedProjectile();
                break;
            case 4:
                SpawnThreeProjectile();
                break;
            case 5:
                SpawnThreeChargedProjectile();
                break;
            case 6:
                SpawnHugeProjectile();
                break;
            case 7:
                SpawnHugeChargedProjectile();
                break;
                // Add cases for more animations if needed
        }
    }

    public void SpawnProjectile()
    {
        Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(FB_Projectile, ShootPosition.position, rotation);
    }

    public void SpawnThreeProjectile()
    {
        Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion rotationPlus20 = Quaternion.AngleAxis(angle + 20f, Vector3.forward);
        Quaternion rotationMinus20 = Quaternion.AngleAxis(angle - 20f, Vector3.forward);
        Instantiate(FB_Projectile, ShootPosition.position, rotationPlus20);
        Instantiate(FB_Projectile, ShootPosition.position, rotationMinus20);
        Instantiate(FB_Projectile, ShootPosition.position, rotation);
    }

    public void SpawnChargedProjectile() 
    {
        Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(FB_ChargedProjectile, ShootPosition.position, rotation);
    }

    public void SpawnThreeChargedProjectile() 
    {
        Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion rotationPlus20 = Quaternion.AngleAxis(angle + 20f, Vector3.forward);
        Quaternion rotationMinus20 = Quaternion.AngleAxis(angle - 20f, Vector3.forward);
        Instantiate(FB_ChargedProjectile, ShootPosition.position, rotationPlus20);
        Instantiate(FB_ChargedProjectile, ShootPosition.position, rotationMinus20);
        Instantiate(FB_ChargedProjectile, ShootPosition.position, rotation);
    }

    public void SpawnHugeProjectile() 
    {
        Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(FB_HugeProjectile, ShootPosition.position, rotation);
    }
    public void SpawnHugeChargedProjectile()
    {
        Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(FB_HugeChargedProjectile, ShootPosition.position, rotation);
    }

    public void Spawn360DegreeProjectile()
    {
        float angleIncreament = 45f;
        Quaternion[] directions = new Quaternion[8];

        for (int i = 0; i < 8; i++) 
        {
            float angle = i * angleIncreament;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(FB_Projectile, ShootPosition.position, rotation);
        }
    }

    public void Spawn360DegreeChargedProjectile()
    {
        float angleIncreament = 45f;
        Quaternion[] directions = new Quaternion[8];

        for (int i = 0; i < 8; i++)
        {
            float angle = i * angleIncreament;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(FB_ChargedProjectile, ShootPosition.position, rotation);
            Debug.Log(FB_ChargedProjectile);
        }
    }

    public void LaserCast() 
    {
        Debug.Log("Casting Laser");
        Instantiate(FB_LaserCast, LaserCastTransform.position, Quaternion.identity);
        Debug.Log(Instantiate(FB_LaserCast, LaserCastTransform.position, Quaternion.identity));
        //FB_LaserCast.SetActive(true);
    }

}
