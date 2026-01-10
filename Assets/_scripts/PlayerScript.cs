using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour {

    #region singleton

    private static PlayerScript instance = null;
    public static PlayerScript Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    public Rigidbody playerRB;
    public int health = 1;

    public int score = 0;

    [Space(10)]

    public GameObject mainCameraGO;
    public LayerMask damageable;
    public GameObject particlesDamage;

    public bool dying = false;

    #region movement

    [Space(10)]

    [Header("Movement")]

    public float walkingSpeedVertical = 1;
    public float walkingSpeedHorizontal = 1;

    public float angleMultiplier = 1;

    [Space(10)]

    public float jumpPower;
    public float fallingGravity = 9.81f;

    [Space(10)]

    bool grounded;
    public GameObject overlapFeetGO;
    public LayerMask ground;

    bool anotherJump;

    float playerDrag;
    float playerSpeedV;
    float playerSpeedH;
    #endregion

    #region combat

    [Space(10)]

    [Header("Combat")]

    public GameObject swordGO;

    public GameObject stonePF;

    public Transform aim;
    public int stones = 0;
    public float throwForce = 5;

    #endregion

    #region enemy pickup

    [Space(10)]

    [Header("Enemy Pickup")]

    Enemy[] enemies;

    public float enemyPickupRadius = 2;

    public GameObject shoulderEnemy;
    GameObject currentEnemy;
    public bool enemyOnShoulder = false;

    GameObject currentPranger;

    bool inButton = false;
    
    #endregion

    #region unlocks

    public bool unlockedSword = false;
    public bool unlockedDoubleJump = false;
    public bool unlockedKey = false;

    #endregion

    [Space(10)]

    public Animator animator;

    #region message events
    [Space(10)]

    public UnityEvent enterPrangerWithEnemy;
    bool prangerUsed = false;

    public UnityEvent enterButton;
    bool buttonUsed = false;

    public UnityEvent pickupFirstEnemy;
    bool firstEnemyUsed = false;

    #endregion


    private void Start()
    {
        grounded = true;
        playerDrag = playerRB.drag;
        playerSpeedV = walkingSpeedVertical;
        playerSpeedH = walkingSpeedHorizontal;

        enemies = FindObjectsOfType(typeof(Enemy)) as Enemy[];

        if (unlockedSword)
        {
            unlockedSword = true;
            animator.SetBool("hasWeapon", true);
            swordGO.SetActive(true);
        }

        UIManager.Instance.UpdateCounts(health, stones, score);
    }

    private void Update()
    {
        #region ground check

        if (Physics.CheckSphere(overlapFeetGO.transform.position, 0.4f, ground))
        {
            if (!grounded)
            {
                animator.SetTrigger("land");
            }

            grounded = true;
            anotherJump = true;
        }
        else
        {
            grounded = false;
        }

        UpdateDrag();
        #endregion

        #region falling

        if (!grounded)
        {
            playerRB.AddForce(Vector3.down * fallingGravity);

            if (playerRB.velocity.y < 0.001)
            {
                animator.SetBool("falling", true);
            }
            else
                animator.SetBool("falling", false);
        }
        else
        {
            animator.SetBool("falling", false);
        }
        #endregion

        #region combat
        if (unlockedSword)
        {
            if (Input.GetButtonDown("Controller Button X") && !dying)
            {
                animator.SetBool("melee_lunge", true);
            }

            if (Input.GetButtonUp("Controller Button X") && !dying)
            {
                animator.SetBool("melee_lunge", false);
                animator.SetTrigger("melee_strike");
            }
        }

        #endregion

        #region ranged attack 

        if (Input.GetButtonUp("Shoulder Button Right") && stones > 0 && !enemyOnShoulder && !dying)
        {
            animator.SetTrigger("ranged_throw");
            animator.SetBool("ranged_aim", false);
            Shoot();
        }

        if (Input.GetButton("Shoulder Button Right") && stones > 0 && !enemyOnShoulder && !dying)
        {
            animator.SetBool("ranged_aim", true);
        }
            
        #endregion

        if (Input.GetButtonDown("Controller Button B"))
        {
            if (!enemyOnShoulder)
            {
                GameObject enemy = GetNearestEnemy();

                if (Vector3.Distance(transform.position, enemy.transform.position) <= enemyPickupRadius && enemy.GetComponent<Enemy>().state == Enemy.NPCState.Laying)
                {
                    currentEnemy = enemy;
                    PickupEnemy();
                }
            }
            else if (!inButton)
            {
                LayDownEnemy();
            }
        }
    }

    void FixedUpdate()
    {  //Physics wie AddForce() immer in FixedUpdate() !!

        #region walk
        Vector3 movementVector = new Vector3(Input.GetAxis("Horizontal") * walkingSpeedHorizontal, 0, Input.GetAxis("Vertical") * walkingSpeedVertical);

        Vector3 correctedMovementVector = mainCameraGO.transform.TransformDirection(movementVector);

        Vector3 projectedMovementVector = Vector3.ProjectOnPlane(correctedMovementVector, Vector3.up).normalized * movementVector.magnitude;

       

        float groundAngle = 1;

        if (grounded)
        {
            Ray angleRay = new Ray(transform.position, Vector3.down);
            RaycastHit groundHit;

            if (Physics.Raycast(angleRay, out groundHit, 2, ground))
            {
                groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);

                //print(groundAngle);

                //if (groundAngle > 10)
                //    groundAngle *= angleMultiplier;

                //if (groundAngle == 0)
                //    groundAngle = 1;

                Vector3 groundDirection = Vector3.ProjectOnPlane(groundHit.normal, Vector3.up).normalized;

                if (!dying)
                {
                    if (grounded && groundAngle > 0.5 && Vector3.Angle(groundDirection - transform.up *-1, projectedMovementVector.normalized - transform.up * -1) > 45)
                    {
                        playerRB.AddForce(projectedMovementVector * groundAngle * angleMultiplier);
                    }
                    else
                    {
                        playerRB.AddForce(projectedMovementVector);
                    }
                }
            }            
        }
        else if (!dying)
        {
            playerRB.AddForce(projectedMovementVector);

            //In einer Update() wäre das " transform.Translate(projectedMovementVector * Time.deltaTime); ".
            //playerRB.AddRelativeForce(projectedMovementVector);
            //Use this, when you want to move relative to the playerobject itself
        }

        Debug.DrawRay(transform.position, correctedMovementVector, Color.yellow);
        Debug.DrawRay(transform.position, projectedMovementVector, Color.magenta);


        if (projectedMovementVector != new Vector3(0, 0, 0) && !dying)
        {
            playerRB.MoveRotation(Quaternion.LookRotation(projectedMovementVector));  //rotate the player to face the way, he's walking
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }

        #endregion

        #region jump / double jump

        if (Input.GetButtonDown("Controller Button A") && !dying)
        {
            if (grounded)
            {
                playerRB.velocity = new Vector3(0, jumpPower, 0);
                animator.SetTrigger("jump");
            }
            else if (anotherJump && unlockedDoubleJump)  //double jump
            {
                playerRB.velocity = new Vector3(0, jumpPower, 0);
                animator.SetTrigger("jump");

                anotherJump = false;
            }
        }

        #endregion
    }

    void UpdateDrag()
    {
        if (grounded)
        {
            playerRB.drag = playerDrag;
            walkingSpeedVertical = playerSpeedV;
            walkingSpeedHorizontal = playerSpeedH;
        }
        else
        {
            playerRB.drag = 0;
            walkingSpeedVertical = playerSpeedV / playerDrag;
            walkingSpeedHorizontal = playerSpeedH / playerDrag;
        }
    }

    void Shoot()
    {
        stones--;

        UIManager.Instance.UpdateCounts(health, stones, score);

        GameObject newStone = Instantiate(stonePF, aim.position, transform.rotation);
        newStone.GetComponent<Rigidbody>().AddForce((mainCameraGO.transform.forward + mainCameraGO.transform.up) * throwForce, ForceMode.Impulse);
    }

    public GameObject GetNearestEnemy()
    {
        enemies = FindObjectsOfType(typeof(Enemy)) as Enemy[];
        GameObject nearestEnemy = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Enemy element in enemies)
        {
            float dist = Vector3.Distance(element.transform.position, currentPos);
            if (dist < minDist)
            {
                nearestEnemy = element.transform.gameObject;
                minDist = dist;
            }
        }

        return nearestEnemy;
    }

    void PickupEnemy()
    {
        shoulderEnemy.SetActive(true);

        currentEnemy.SetActive(false);

        enemyOnShoulder = true;

        animator.SetBool("carrying", true);

        if (!firstEnemyUsed)
        {
            pickupFirstEnemy.Invoke();
            firstEnemyUsed = true;
        }
    }

    void LayDownEnemy()
    {
        currentEnemy.GetComponent<Enemy>().putdownTime = Time.time;

        if (currentPranger != null)
        {
            if (currentPranger.GetComponent<Pranger>().empty)
            {
                currentPranger.GetComponent<Pranger>().ActivateEnemy();
                Destroy(currentEnemy);
                score += 100;
                UIManager.Instance.UpdateCounts(health, stones, score);
            }
            else
            {
                //display "pillory already in use"
                return;
            }
            
        }
        else
        {
            currentEnemy.transform.position = overlapFeetGO.transform.position + transform.forward;
            currentEnemy.SetActive(true);
        }

        shoulderEnemy.SetActive(false);

        enemyOnShoulder = false;

        animator.SetBool("carrying", false);
    }

    #region platform parent
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            transform.parent = collision.transform;
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            transform.parent = null;
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        #region unlocks

        if (other.gameObject.tag == "club")
        {
            Destroy(other.gameObject);
            unlockedSword = true;
            animator.SetBool("hasWeapon", true);

            swordGO.SetActive(true);
        }

        if (other.gameObject.tag == "double jump")
        {
            Destroy(other.gameObject);
            unlockedDoubleJump = true;
        }

        if (other.gameObject.tag == "key")
        {
            Destroy(other.gameObject);
            unlockedKey = true;
        }

        #endregion

        if (other.gameObject.CompareTag("health pickup"))
        {
            if (health < 5)
            {
                Destroy(other.gameObject);

                health++;
            }

            UIManager.Instance.UpdateCounts(health, stones, score);
        }

        if (other.gameObject.CompareTag("stone pickup"))
        {
            Destroy(other.gameObject);

            stones++;
            UIManager.Instance.UpdateCounts(health, stones, score);
        }

        if (other.gameObject.CompareTag("pranger"))
        {
            currentPranger = other.gameObject;

            if (enemyOnShoulder && !prangerUsed)
            {
                enterPrangerWithEnemy.Invoke();
                prangerUsed = true;
            }               
        }

        if (other.gameObject.CompareTag("death trigger"))
        {
            health--;
            print("lava hits");
            ResetPlayer();
            UIManager.Instance.UpdateCounts(health, stones, score);
        }

        if (other.CompareTag("button"))
        {
            if (!buttonUsed)
            {
                enterButton.Invoke();
                buttonUsed = true;
            }

            inButton = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "pranger")
        {
            currentPranger = null;
        }

        if (other.CompareTag("button"))
        {
            inButton = false;
        }
    }

    RaycastHit GetCameraRay()
    {
        return mainCameraGO.GetComponent<CameraScript>().GetRaycastHit();
    }

    void Teleport(Vector3 pos)
    {
        transform.position = pos;
    }

    public void GetDamaged()
    {
        if (enemyOnShoulder)
            LayDownEnemy();

        if (!dying)
        {
            dying = true;
            health--;
            Instantiate(particlesDamage, transform.position, transform.rotation);

            UIManager.Instance.UpdateCounts(health, stones, score);

            animator.SetTrigger("die");

            if (health == 0)
            {
                print("GAME OVER");
            }
            else
            {
            }
        }
    }

    public void ResetPlayer()
    {
        if (enemyOnShoulder)
            LayDownEnemy();

        if (health > 0)
        {
            transform.position = CheckpointSystem.checkpoint + Vector3.up;
            dying = false;
        }
        else
        {
            SceneManagement.Instance.LoadScene("LosingScreen");
        }      
    }
}
