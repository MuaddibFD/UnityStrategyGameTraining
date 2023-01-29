using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed;

    [SerializeField]
    private float rotatingSpeed;
    
    [SerializeField]
    private float zoomingSpeed;

    [SerializeField]
    private float zoomAmount;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    private Vector3 targetFollowOffset;
    private CinemachineTransposer cinemachineTransposer;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {

        //Movement
        HandleMovement();

        //Roration
        HandleRotation();

        //Zoom
        HandleZooming();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
            inputMoveDirection.z = +1f;
        if (Input.GetKey(KeyCode.S))
            inputMoveDirection.z = -1f;
        if (Input.GetKey(KeyCode.A))
            inputMoveDirection.x = -1f;
        if (Input.GetKey(KeyCode.D))
            inputMoveDirection.x = +1f;

        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveVector * movingSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 inputRotateDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
            inputRotateDirection.y = +1;
        if (Input.GetKey(KeyCode.E))
            inputRotateDirection.y = -1;

        transform.eulerAngles += inputRotateDirection * rotatingSpeed * Time.deltaTime;
    }

    private void HandleZooming()
    {
        if (Input.mouseScrollDelta.y > 0)
            targetFollowOffset.y -= zoomAmount;

        if (Input.mouseScrollDelta.y < 0)
            targetFollowOffset.y += zoomAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomingSpeed);
    }
}
