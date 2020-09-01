using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class gravity : MonoBehaviour
{
    // 상태 변수
    [SerializeField] bool isGround;
    [SerializeField] bool movingCheck;
    [SerializeField] bool turnCheck;
    public static bool clearCheck; 

    //움직임 및 동작 제어
    int turnValue = 0;
    int turnningCount;
    
    //이동 시 속도나 물리제어
    [SerializeField] int gravityScale = 1;
    int speed = 2;
    [SerializeField] float moveDIrection = 5f;

    //플레이어의 마지막 이동
    [SerializeField] Vector3 moveDir;

    //사운드
    public AudioClip gravityRightSource, gravityLeftSource,groundedSource, clearPointSource;
    // Start is called before the first frame update
    void Start()
    {
        isGround = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, -transform.up * 1.5f, Color.red);
        GroundCheck();
        CameraMoving();
        PlayerMoving();
        ClearCheck();
        WallCheck();
        Turnning();
        Cheat();
        transform.Translate(moveDir * Time.deltaTime * speed);
    }
    public void PlayerMoving()
    {
        #region NoneVR
        //moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");
        #endregion

        #region VR
        //float dirX = 0; // 좌우
        float dirZ = 0; // 전진후진
        if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            if (coord.y > 0)
                dirZ = +1;
            //아래
            else if (coord.y < 0)
                dirZ = -1;
            else
                dirZ = 0;
            //}
        }
        //moveDir.z = dirZ;
        #endregion
        if (!Physics.Raycast(transform.position, transform.up * -1f, 1f, 1 << LayerMask.NameToLayer("Ground")) && turnCheck == false)
        {
            isGround = false;
        }
    }
    public void CameraMoving()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight) && turnCheck == false || Input.GetKeyDown(KeyCode.X))
        {
            transform.Rotate(0, 90, 0);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft)|| Input.GetKeyDown(KeyCode.Z))
        {
            transform.Rotate(0, -90, 0);
        }
    }
    public void GroundCheck()
    {
        if (!isGround)
        {
            moveDir.y = Physics.gravity.y * gravityScale;
        }
        if (Physics.Raycast(transform.position, transform.up * -1f, 1f, 1 << LayerMask.NameToLayer("Ground")))
        {
            //SoundManager.instance.PlaySingle(groundedSource);
            isGround = true;
            moveDir.y = 0;
        }
    }
    public void WallCheck()
    {
        if (Physics.Raycast(transform.position, this.transform.forward, 0.5f, 1 << LayerMask.NameToLayer("Ground")))
        {
            moveDir.z = Mathf.Clamp(moveDir.z, 0, -1);         
        }
        else if (Physics.Raycast(transform.position, -this.transform.forward, 0.5f, 1 << LayerMask.NameToLayer("Ground")))
        {
            moveDir.z = Mathf.Clamp(moveDir.z, 1, 0);
        }
    }
    public void Turnning()
    {
        #region VR
        if (turnCheck == false && isGround)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                turnValue = 1;                
                if (turnCheck == false)
                    SoundManager.instance.PlaySingle(gravityLeftSource);
                     StartCoroutine(MapTurnning());
            }
            else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                turnValue = -1;                
                if (turnCheck == false)
                    SoundManager.instance.PlaySingle(gravityRightSource);
                    StartCoroutine(MapTurnning());
            }
        }
        #endregion NoneVR

        #region NoneVR
        if (turnCheck == false && isGround)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                turnValue = 1;
                if (turnCheck == false)
                    SoundManager.instance.PlaySingle(gravityLeftSource);
                     StartCoroutine(MapTurnning());
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                turnValue = -1;
                if (turnCheck == false)
                    SoundManager.instance.PlaySingle(gravityRightSource);
                    StartCoroutine(MapTurnning());
            }
        }
        #endregion
    }
    IEnumerator MapTurnning()
    {        
        turnCheck = true;
        turnningCount = 0;
        //Vector3 turnDevelop = turnValue > 0 ? transform.right : transform.right * -1f;
        Vector3 turnDevelop = turnValue > 0 ? Vector3.forward : Vector3.back;
        while (turnningCount < 90)
        {
            transform.Rotate(turnDevelop);
            turnningCount += 1;
            yield return null;
        }
        turnCheck = false;
        isGround = false;
    }
    public void ClearCheck()
    {
        if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << LayerMask.NameToLayer("ClearBox")))
        {
            if (Physics.Raycast(transform.position, -transform.up, 1.5f, 1 << LayerMask.NameToLayer("ClearGround")))
            {
                SoundManager.instance.PlaySingle(clearPointSource);
                clearCheck = true;
            }
        }
    }
    public void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.M) || OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            SoundManager.instance.PlaySingle(clearPointSource);
            clearCheck = true;
        }
    }

}