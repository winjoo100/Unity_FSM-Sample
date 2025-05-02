// kdw 2011.10.31.
// 특정대상을 타겟을 향하여 일정각도와 거리에 위치시키는 카메라 콘트롤. 

using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

    //---------------------------------------------
    public Transform _targetTransform;      //Target

	public Transform _cameraTransform;

	public Vector3 targetOffset = new Vector3(0, 1, 0);    //위치

	public float fDistance = 15.0f;          //Editor 우선 참조
	public float fPitch = 30.0f;
	public float fYaw = 0.0f;

	float minimumX = -360F;
	float maximumX = 360F;
	float minimumY = -0F;   // fPitch max
	float maximumY = 85F;
	float minimumZ = 1F;    // fDistance max
	float maximumZ = 30F;

	private float fVelocity = 0.0f;
	float fDistance_cur;
	float fSmoothTime = 0.1F;   // smooth, smaller is faster.

	private float angleVelocity = 0.0f;
	private float angularSmoothTime = 0.2f;
	private float angularMaxSpeed = 15.0f;

    //---------------------------------------------

    public float rotateSpeed = 360f;
    public float moveSpeed = 3f;

    private Vector3 mouseTarget;
	private Vector3 moveDirecton;

	// ---------------------------------------------
	void Awake()
	{
		if (!_cameraTransform && Camera.main)
			_cameraTransform = Camera.main.transform;
		if (!_cameraTransform) {
			Debug.Log("Please assign a camera to the CameraController script.");
			enabled = false;
		}

		//_targetTransform = transform;		

		if (!_targetTransform)
		{
			Debug.Log("Please assign a target Transform.");
		}

	}

	void Start() {
		// 초기값 설정
		fPitch = 0;
		fYaw = -180;
	}

	void Update() {

        fYaw += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        fPitch -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        fPitch = Mathf.Clamp(fPitch, minimumY, maximumY);

        fDistance -= Input.GetAxis("Mouse ScrollWheel");
        fDistance = Mathf.Clamp(fDistance, minimumZ, maximumZ);

        // LEGACY : 자동으로 카메라 조정으로 변경
        //if (Input.GetMouseButton(1)) //우클릭으로 카메라 조정
        //{
        //	fYaw += Input.GetAxis("Mouse X");
        //	//fYaw = Mathf.Clamp (fYaw, minimumX, maximumX);			
        //	fPitch -= Input.GetAxis("Mouse Y");
        //	fPitch = Mathf.Clamp(fPitch, minimumY, maximumY);
        //}

        // Move1();  //마우스 이동     move1.cs
        Move2();

    }

    void LateUpdate()
    {

        if (!_targetTransform) return;

        // target rotation 
        //float currentAngle	= _cameraTransform.eulerAngles.y; //y angle
        //float targetAngle 	= _targetTransform.eulerAngles.y; 
        //currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, angularSmoothTime, angularMaxSpeed);				
        //Quaternion currentRotation = Quaternion.Euler (fPitch, currentAngle + fYaw, 0);

        Quaternion currentRotation = Quaternion.Euler(fPitch, fYaw, 0);
        // target position
        Vector3 targetPos = _targetTransform.position + targetOffset;

        // camera position
        _cameraTransform.position = targetPos + currentRotation * Vector3.back * fDistance;

        // camera rotation
        Vector3 relativePos = targetPos - _cameraTransform.position;
        _cameraTransform.rotation = Quaternion.LookRotation(relativePos); // * Quaternion.Euler (fPitch, fYaw, 0 );
    }

    // 마우스 이동
    void Move1()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (true == (Physics.Raycast(ray.origin, ray.direction * 1000, out hit)))
			{
				mouseTarget = hit.point;
			}
		}

		if (Vector3.Distance(_targetTransform.position, mouseTarget) > 0.5f)
		{
			moveDirecton = (mouseTarget - _targetTransform.position).normalized;

			// 회전
			Quaternion tempRotation = Quaternion.LookRotation(moveDirecton);
			Vector3 eulerRotation = tempRotation.eulerAngles;
			eulerRotation.x = 0;
			eulerRotation.z = 0;
			_targetTransform.rotation = Quaternion.Slerp(
				_targetTransform.rotation, Quaternion.Euler(eulerRotation), rotateSpeed * Time.deltaTime);


			// 이동
			_targetTransform.position += moveDirecton * moveSpeed * Time.deltaTime;
		}
	}

	// 키보드 이동
	void Move2()
	{
        float speed = moveSpeed;

        moveDirecton = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			moveDirecton += _cameraTransform.forward;
        }

		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
            moveDirecton += -_cameraTransform.forward;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirecton += -_cameraTransform.right;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirecton += _cameraTransform.right;
        }

        // 회전
        moveDirecton.y = 0;
		moveDirecton = moveDirecton.normalized;

		if (moveDirecton != Vector3.zero) // 이동 입력이 있을 경우에만 회전 적용
		{
            Quaternion tempRotation = Quaternion.LookRotation(moveDirecton);
            Vector3 eulerRotation = tempRotation.eulerAngles;
            eulerRotation.x = 0;
            eulerRotation.z = 0;
            _targetTransform.rotation = Quaternion.Slerp(
                _targetTransform.rotation, Quaternion.Euler(eulerRotation), rotateSpeed * Time.deltaTime);
        }

		// 이동
		// _targetTransform.position += moveDirecton * speed * Time.deltaTime;
    }
}