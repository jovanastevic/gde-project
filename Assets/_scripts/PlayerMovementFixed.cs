using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementFixed : MonoBehaviour {

    public Rigidbody playerRB;

    [Space(10)]

    public GameObject mainCameraGO;
    public GameObject laserMinePF;
    public GameObject projectilePF;
    public LayerMask laserMask;
    public LayerMask damageable;

    #region movement

    enum MovementStates
    {
        Standing,
        Walking,
        Jumping,
        Falling
    };
    MovementStates movementState;

    [Space(10)]

    [Header("Movement")]

    public float walkingSpeedVertical = 1;
    public float walkingSpeedHorizontal = 1;

    [Space(10)]

    public float jumpPower;
    public float fallingGravity = 9.81f;

    [Space(10)]

    public bool grounded;
    public GameObject overlapFeetGO;
    public LayerMask ground;

    float playerDrag;
    float playerSpeedV;
    float playerSpeedH;
    bool speedDivideActivated;
    #endregion

    GameObject currentButton;


    private void Start()
    {
        grounded = true;
        playerDrag = playerRB.drag;
        playerSpeedV = walkingSpeedVertical;
        playerSpeedH = walkingSpeedHorizontal;
    }

    private void Update()
    {
        #region grounded
        /* if (Physics.OverlapSphere(overlapFeetGO.transform.position, 0.1f, ground).Length > 0)
           {
               grounded = true;
           }
           else
               grounded = false; */

        if (Physics.CheckSphere(overlapFeetGO.transform.position, 0.1f, ground))
        {
            grounded = true;
            playerRB.drag = playerDrag;
            walkingSpeedVertical = playerSpeedV;
            walkingSpeedHorizontal = playerSpeedH;
            speedDivideActivated = false;
        }
        else
        {
            grounded = false;
            playerRB.drag = 0;
            walkingSpeedVertical = playerSpeedV / playerDrag;
            walkingSpeedHorizontal = playerSpeedH / playerDrag;
        }
        #endregion

        if (grounded)
        {

        }
        else
        {
            if (playerRB.velocity.y > 0.001)
            {
                //movementState = MovementStates.Jumping;
            }
            else if (playerRB.velocity.y < 0.001)
            {
                movementState = MovementStates.Falling;
            }
        }

        #region falling

        if (!grounded)
        {
            playerRB.AddForce(Vector3.down * fallingGravity);
        }

        #endregion

        //if (Input.GetButton("Controller Button B") && currentButton != null)
        //{
        //    currentButton.GetComponent<Button>().Pressed();
        //}

        #region weaponry

        //if (Input.GetButtonDown("Controller Button X"))
        //{
        //    RangedAttack();
        //    //PlaceLaserMine();
        //}

        if (Input.GetButtonDown("Controller Button Y"))
        {
            ShootProjectile();
            //PlaceLaserMine();
        }

        #endregion
    }

    void FixedUpdate () {  //Physics wie AddForce() immer in FixedUpdate() !!

        #region walk
        Vector3 movementVector = new Vector3(Input.GetAxis("Horizontal") * walkingSpeedHorizontal, 0, Input.GetAxis("Vertical") * walkingSpeedVertical);

        Vector3 correctedMovementVector = mainCameraGO.transform.TransformDirection(movementVector);

        Vector3 projectedMovementVector = Vector3.ProjectOnPlane(correctedMovementVector, Vector3.up).normalized * movementVector.magnitude;


        playerRB.AddForce(projectedMovementVector);  //In einer Update() wäre das " transform.Translate(projectedMovementVector * Time.deltaTime); ".
        //playerRB.AddRelativeForce(projectedMovementVector);
        //Use this, when you want to move relative to the playerobject itself


        Debug.DrawRay(transform.position, correctedMovementVector, Color.yellow);
        Debug.DrawRay(transform.position, projectedMovementVector, Color.magenta);


        if(projectedMovementVector != new Vector3(0, 0, 0))
            playerRB.MoveRotation(Quaternion.LookRotation(projectedMovementVector));  //rotate the player to face the way, he's walking

        #endregion

        #region jump

        if (Input.GetButtonDown("Controller Button A") && grounded)
        {
            playerRB.velocity = new Vector3(0, jumpPower, 0);
            movementState = MovementStates.Jumping;
        }

        #endregion
        
    }

    RaycastHit GetCameraRay()
    {
        return mainCameraGO.GetComponent<CameraScript>().GetRaycastHit();
    }

    void PlaceLaserMine()
    {
        if ((laserMask | (1 << GetCameraRay().collider.gameObject.layer)) == laserMask)
        {
            GameObject newMine;
            newMine = Instantiate(laserMinePF, GetCameraRay().point, new Quaternion(0, 0, 0, 0));
            newMine.transform.rotation = Quaternion.LookRotation(GetCameraRay().normal);
        }           
    }

    void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePF, transform.position, transform.rotation);
        Vector3 forceDirection = GetCameraRay().point - transform.position;
        projectile.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * 20);
        Debug.Log("Vector3: " + "x: " + forceDirection.x + ", y: " + forceDirection.y + ", z: " + forceDirection.z);
    }

    void RangedAttack()
    {
        GameObject target = GetCameraRay().collider.gameObject;

        if ((damageable | (1 << target.layer)) == damageable)
        {
            target.GetComponent<Damageable>().GetDamaged();
            Destroy(target);
        }
    }

    //GetCameraRay().point

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "key")
        {
            Destroy(other.gameObject);
        }
    }

    public void OnGetCaught()
    {
        Debug.Log("Du wurdest gefangen! Na geh!");
    }
}
