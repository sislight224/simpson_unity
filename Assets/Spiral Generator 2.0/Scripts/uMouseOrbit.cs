using UnityEngine;
using System;



public class uMouseOrbit:MonoBehaviour{
    public Transform target;
    public float distance = 10.0f;
    public float xSpeed = 300.0f;
    public float ySpeed = 300.0f;
    public int zoomRate = 50;
    
    float x = 0.0f;
    float y = 0.0f;
    bool rot;
    
    public void Start() {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }
    
    
    public void Update() {
    	if (target != null) {
    	
          
    		if(Input.GetMouseButtonDown(0)){
    		rot = true;
    		}else if(Input.GetMouseButtonUp(0)){
    		rot = false;
    		}
    		
    		if(rot){
    		x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
         	}
         	
            distance += -Input.GetAxis("Mouse ScrollWheel")* Time.deltaTime * zoomRate * Mathf.Abs(distance);
          
           		
     		
     		       
            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
            
            transform.rotation = rotation;
            transform.position = position;
            
             
        }
    	
    
    }


}