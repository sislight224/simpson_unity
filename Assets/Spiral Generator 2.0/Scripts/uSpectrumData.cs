using UnityEngine;
using System;


public class uSpectrumData:MonoBehaviour{
    // 	Copyright 2013 Unluck Software
    //	www.chemicalbliss.com
    //
    //	This script is just a demonstration on how the spiral generator can be used with spectrum data
    // Ignore the GetSpectrumData warning
    public float growSpeed = 10.0f;
    public bool useFrequency = true;
    public float growFrequency = 5.0f;  
    public float growFrequencySpeed = 5.0f;
    public int channel = 1;
    public int numSamples = 256;
    public int freq = 2;
    //var number;
    
    AudioListener _audio;
    SpiralGenerator _spiral;
    float multiBuffer;
    float heightBuffer;
    float depthBuffer;
    
    public void Start() {
    	//number = new Array ();
    	_spiral = transform.GetComponent<SpiralGenerator>();
    	multiBuffer = _spiral.sineGrowMultiplier;
    	heightBuffer = _spiral.sineHeightFrequency;
    	depthBuffer = _spiral.sineDepthFrequency;
    	_audio = Camera.main.GetComponent<AudioListener>();
    }
       
        
    public void Update() {    
    	float[] number = AudioListener.GetSpectrumData(numSamples, channel, FFTWindow.Rectangular);
    //	var number = _audio.GetOutputData(numSamples, channel);
    	float dif = multiBuffer*number[freq]*10 - _spiral.sineGrowMultiplier;
    	_spiral.sineGrowMultiplier += dif*Time.deltaTime*growSpeed;
    	if(useFrequency){
    		dif = heightBuffer*number[freq]*growFrequency - _spiral.sineHeightFrequency;
    		_spiral.sineHeightFrequency += dif*Time.deltaTime*growFrequencySpeed;
    		dif = depthBuffer*number[freq]*growFrequency - _spiral.sineDepthFrequency;
    		_spiral.sineDepthFrequency += dif*Time.deltaTime*growFrequencySpeed;
    	}
    }
}