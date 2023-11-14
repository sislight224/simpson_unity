using UnityEngine;
using System;


public class SpiralGenerator:MonoBehaviour{
    // 	Copyright 2012 Unluck Software, Egil Andre Larsen 
    //	http://www.chemicalbliss.com
    //
    //  For a smooth spiral, keep depht/frequency/speed the same as height/frequency/speed
    //  Performance notes:
    //		Try to keep all unused variables to 0
    //		Use rotate instead of Sine Speeds if possible
    //		runOnceThenDisable turn off the spiral generation, huge performance gain (rotation and material animation still works)
    //	Creating a new spiral:
    //		Use an existing prefab
    //		Press play
    //		Change the variables while in play mode
    //		When the spiral looks like it is suppose to, drag it from Hierarchy to Project folder
    //		Drag the prefab from Project to Hierarcy
    //		Remove lineRenderer component from "Beam" child (optional but recommended)
    //		Assign start and end targets
    //		Delete the old spiral from Hierarcy if needed
    //	Creating a multi spiral:
    //		Create or use a existing spiral prefab
    //		Use a premade multiSpiral prefab or attach the multiSpiral script to a empty gameObject
    //		Set how many spirals the multiSpiral should spawn in the "spiralWithRotationArray" 
    //		Assign the spirals to each index of the array (if using the same spiral for several, set lenght to 1 first and assign the spiral then set to desired lenght)
    //		
    //		
    public Color colorStart = Color.yellow;			// Start color
    public Color colorEnd = Color.red;				// End color
    public Transform targetStart;						// Line start target transform
    public Transform targetEnd;						// Line end target transform
    public float noise = 0.0f;							// Line renderer perlin noise
    public float noiseScale = 1.0f;						// Noise frequency
    public float noiseSpeed = 1.0f;						// Noise flow speed (Time.time*noiseSpeed)
    public float LenghtOfLineRenderer = 100.0f;			// Sets base vertex count (more = more cpu)
    public bool smooth = true;						// Increases vertex count based on "lenghtResolutionMultiplier" 
    float lenghtResolutionMultiplier =.05f;// Increases vertex count based on the lines lenght (.05 = default/max needed, this can be set lower for better performance while "smooth" is active)
    public Material lineMaterial;						// Material used for the line renderer
    public float offsetMaterialSpeed = 0.0f;				// Material x offset over time
    public float offsetYMaterialSpeed = 0.0f;			// Material y offset over time
    public float stretchMaterial = 10.0f;					// Sets material x tiling
    public float stretchYMaterial = 1.0f;					// Sets material y tiling
    public float widthStart = 0.1f;					// line renderer start width
    public float widthEnd = 0.2f;						// line renderer end width
    public float rotateSpeed = 0.0f;						// Rotates the line, using this and runOnceThenDisable will imporeve performance +++
    public float rotationOffset;						// Rotates the beam at start
    public float sineDepth = -.02f;						// Depht of the slopes (Mathf.Cos)
    public float sineDepthFrequency = 5.8f;				// How many slopes the line has sideways
    public float sineHeight = -.05f;					// Height of the slopes (Mathf.Sin)
    public float sineHeightFrequency = 5.0f;				// How many slopes the line has up and down
    public float sineLenght = 0.0f;						// Ripple height 
    public float sineLenghtFrequency = 0.0f;				// Ripple height frequency 
    public float sineGrowMultiplier = 250.0f;				// Gradually grows the spiral/wave bigger, if used with sine this can be used for a bulge effect in the middle of the spiral/wave
    public AnimationCurve growCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(.5f, .5f), new Keyframe(1.0f, 0.0f));					// How the spiral grows over time
    public float sineLenghtSpeed = 0.0f;					// Speeds the ripples moves, can also be used for warp effect on big ripples
    public float sineHeightSpeed = -5.0f;					// Speed the spiral moves towards or away from target		
    public float sineDepthSpeed = -5.0f;					// Sideways speed
    public bool runOnceThenDisable;					// Stops generating spiral shape after completed drawing once, rotation is still enabled (use this for simple rotating spirals that do not change in size)
    public Transform start;							// Stores the Start transform that is positioned at the Start Target (good place to put particle systems and similar | no warning if missing)
    public Transform end;								// Stores the End transform that is positioned at the End Target
    public float vertexCount;							// For monitoring vertex count (this is not a editable value)
    public LineRenderer lineRenderer;					// LineRenderer attached to Beam gameObject
    
    public bool _bend;
    public float _bendMultiplier = 1.0f;
    public AnimationCurve bendCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(.5f, .5f), new Keyframe(1.0f, 0.0f));	
    
    bool runSpiral = true;
    public Material lineRendererMat;			
    
    public void Start() {
    	CreateSpiral();
    }
    
    public void CreateSpiral() {
    	if(transform.Find("Beam").gameObject.GetComponent<LineRenderer>() == null)
    	lineRenderer = transform.Find("Beam").gameObject.AddComponent<LineRenderer>();
    	else 
    	lineRenderer = transform.Find("Beam").gameObject.GetComponent<LineRenderer>();
    	transform.Find("Beam").transform.Rotate(0.0f,0.0f,rotationOffset);
    	lineRenderer.SetVertexCount ((int)LenghtOfLineRenderer);
    	lineRenderer.useWorldSpace = false;				
    	lineRenderer.material = lineMaterial;
    	
    	
    	if(end == null)
    	end = transform.Find("End");			// Sets start and end for positioning
    	if(start == null)
    	start = transform.Find("Start");
    	
    	if(targetEnd != null)
    	end.position = targetEnd.position;
    	if(targetStart != null)
    	start.position = targetStart.position;
    	
    	lineRendererMat = lineRenderer.material;
    }
    
    public void Update() {
    	if(runSpiral){
    			GenerateSpiral(Time.time);
    	}
    	PositionSpiral();
    }
    
    public void PositionSpiral() {
    	if(stretchMaterial < 0.01f)
    		stretchMaterial =0.01f;
    	if(stretchYMaterial < 0.01f)
    		stretchYMaterial =0.01f;
    	var tmp_cs1 = lineRendererMat.mainTextureScale;
        tmp_cs1.x = stretchMaterial;
        tmp_cs1.y = stretchYMaterial;
        lineRendererMat.mainTextureScale = tmp_cs1;
    	if(offsetMaterialSpeed != 0 )
    		{
                    var tmp_cs2 = lineRendererMat.mainTextureOffset;
                    tmp_cs2.x = Time.time*offsetMaterialSpeed%1;
                    lineRendererMat.mainTextureOffset = tmp_cs2;
                }
    	if(offsetYMaterialSpeed != 0 )
    		{
                    var tmp_cs3 = lineRendererMat.mainTextureOffset;
                    tmp_cs3.y = Time.time*offsetYMaterialSpeed%1;
                    lineRendererMat.mainTextureOffset = tmp_cs3;
                }
    	if(targetStart != null) transform.position = start.position;
    	transform.LookAt(end, transform.up);
    	transform.Rotate(Vector3.forward*Time.deltaTime*rotateSpeed);
    }
    
    
    public void GenerateSpiral(float time) {
    		if((targetEnd != null) && end.position!=targetEnd.position) end.position = targetEnd.position;
    		if((targetStart != null) && start.position!=targetStart.position) start.position = targetStart.position;
    		lineRenderer.SetWidth ( widthStart, widthEnd);
    		lineRenderer.SetColors (colorStart, colorEnd);
    		float distance = Vector3.Distance(start.position,end.position);	
    		if(smooth)
    			vertexCount = (float)Mathf.FloorToInt(LenghtOfLineRenderer*distance*lenghtResolutionMultiplier);
    		else
    			vertexCount = (float)Mathf.FloorToInt(LenghtOfLineRenderer);
    		if(vertexCount<15)
    			vertexCount =15.0f;
    		lineRenderer.SetVertexCount ((int)vertexCount);
    		float c = 1f/vertexCount;
    		float n = vertexCount/1000;	//fix for noiseScale
    		
    		for(int i = 0; i < vertexCount; i++){
    			float pos = distance*c*i;			
    			float grow = growCurve.Evaluate (i/vertexCount)*sineGrowMultiplier;
    			float bend = 0.0f;
    			if(_bend)
    			bend = bendCurve.Evaluate (i/vertexCount)*_bendMultiplier;
    			float rr = 1.0f;
    			if(noise != 0)
    				rr =Mathf.PerlinNoise((i*1.0f)/distance*noiseScale*n,time*noiseSpeed)*noise+1;
			float ry = 0, rx = 0, rz = 0;
					if (sineHeightFrequency!=0)
    				ry = Mathf.Sin(sineHeightFrequency*pos+(time*sineHeightSpeed))*sineHeight*grow*rr;
    			if(sineDepthFrequency!=0)
    				rx = Mathf.Cos(sineDepthFrequency*pos+(time*sineDepthSpeed))*sineDepth*grow*rr;
    			if(sineLenghtFrequency!=0)
    				rz = Mathf.Sin(sineLenghtFrequency*pos+(time*sineLenghtSpeed))*sineLenght*grow*rr;
    			lineRenderer.SetPosition(i, new Vector3(rx+bend, ry, rz + pos));		
    		}	
    		if(runOnceThenDisable)
    			runSpiral = false;
    }

}