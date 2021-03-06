using UnityEngine;
using System.Collections;

public class HASCamera : MonoBehaviour {
	public Transform target;
	public string playerTagName = "Player";
	public float walkDistance;
	public float runDistance;
	public float height;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	private float _x;
	private float _y;
	private bool _camButtonDown = false;
	private bool _rotateCameraKeyPressed = false;

	
	private Transform _myTransform;
	void Awake(){
		_myTransform = transform;
	}
	// Use this for initialization
	void Start () {
		if(target == null)
			Debug.LogWarning("We do not have a target");
		else
			CameraSetUp();
	}
	void Update(){
		
		if(Input.GetButtonDown("Rotate Camera Button")){
			_camButtonDown=true;				
		}
		if(Input.GetButtonUp("Rotate Camera Button")) {
			_x = 0;
			_y = 0;	
			_camButtonDown=false;
		}
		if(Input.GetButtonDown("Rotate Camera Horizontal Buttons") || Input.GetButtonDown("Rotate Camera Vertical Buttons"))
			_rotateCameraKeyPressed = true;
		
		if(Input.GetButtonUp("Rotate Camera Horizontal Buttons") || Input.GetButtonUp("Rotate Camera Vertical Buttons")){
			_x = 0;
			_y = 0;	
			_rotateCameraKeyPressed = false;
		}
	}
	void LateUpdate(){
		//_myTransform.position = new Vector3(target.position.x , target.position.y + height,target.position.z - walkDistance);
		//_myTransform.LookAt(target);
		if(target != null){
			if(_rotateCameraKeyPressed){
			_x += Input.GetAxis("Rotate Camera Horizontal Buttons") * xSpeed * 0.02f;
	        _y -= Input.GetAxis("Rotate Camera Vertical Buttons") * ySpeed * 0.02f;
			}
			else if(_camButtonDown){
			_x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
	        _y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
	 	
			RotateCamera();
			}
		else{
			//_myTransform.position = new Vector3(target.position.x , target.position.y + height,target.position.z - walkDistance);
			//_myTransform.LookAt(target);

			
			float wantedRotationAngle = target.eulerAngles.y;
			float wantedHeight = target.position.y + height;
			
			float currentRotationAngle = _myTransform.eulerAngles.y;
			float currentHeight = _myTransform.position.y;
			currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
			currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
			Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
			_myTransform.position = target.position;
			_myTransform.position -= currentRotation * Vector3.forward * walkDistance;
			_myTransform.position = new Vector3(_myTransform.position.x,currentHeight,_myTransform.position.z);
			_myTransform.LookAt (target);
			}
		}
		else {
			GameObject go = GameObject.FindGameObjectWithTag(playerTagName);
			
			if(go == null)
				return;
				
			target = go.transform;
		}
	}
	private void RotateCamera(){
			 		       
	    Quaternion rotation = Quaternion.Euler(_y, _x, 0);
	    Vector3 position = rotation * new Vector3(0.0f, 0.0f, -walkDistance) + target.position;
	    
		_myTransform.rotation = rotation;
	    _myTransform.position = position;
	}
	//90 2.05 turtorial
	public void CameraSetUp() {
		_myTransform.position = new Vector3(target.position.x , target.position.y + height,target.position.z - walkDistance);
		_myTransform.LookAt(target);
	}
}
