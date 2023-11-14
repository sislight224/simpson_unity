using UnityEngine;
using System;


public class MultiSpiral:MonoBehaviour{
    // 	Copyright 2012 Unluck Software, Egil Andre Larsen 
    //	http://www.chemicalbliss.com
    public Transform targetStart;
    public Transform targetEnd;
    public float rotationOffset;
    public bool autoCalculateCircle;
    public int startEffects = 1;
    public GameObject[] spiralWithRotationArray;
    public GameObject[] spiralArray;
    
    public void Start() {
    	if(autoCalculateCircle)
    	rotationOffset = (float)(360/spiralWithRotationArray.Length);
    	for(int i = 0; i < spiralWithRotationArray.Length; i++){
    		GameObject newSpiral = (GameObject)Instantiate(spiralWithRotationArray[i]);
    		SpiralGenerator spiralScript = newSpiral.transform.GetComponent<SpiralGenerator>();
    		spiralScript.rotationOffset = rotationOffset*i;
    		spiralScript.targetStart = targetStart;
    		spiralScript.targetEnd = targetEnd;
    //		if(i >= startEffects)
    	//	Destroy(newSpiral.transform.FindChild("Start").gameObject);
    	}
    }
}