using UnityEngine;
using System;
using UnityEditor;
/****************************************					
	Copyright 2013 Unluck Software	
 	www.chemicalbliss.com																																				
*****************************************/
[CustomEditor(typeof(SpiralGenerator))]
[CanEditMultipleObjects]
[System.Serializable]
public class SpiralGeneratorEditor: Editor {
 	public SerializedProperty myProperty;
 	public float counter;
 	//var toggleAuto:boolean;
		
    public override void OnInspectorGUI() {
    //	DrawDefaultInspector();
     	
     var target_cs = (SpiralGenerator)target;
     EditorGUILayout.Space();
     if(GUILayout.Button("Preview Spiral")) {
     	target_cs.CreateSpiral();
     	target_cs.GenerateSpiral(UnityEngine.Random.value);
     	target_cs.PositionSpiral();
     }
//     toggleAuto = EditorGUILayout.Toggle("Auto Preview", toggleAuto);
     	EditorGUILayout.Space();
     	
     
     EditorGUILayout.LabelField("Start to End Colors", EditorStyles.boldLabel);
     target_cs.colorStart = EditorGUILayout.ColorField(target_cs.colorStart);
     target_cs.colorEnd = EditorGUILayout.ColorField(target_cs.colorEnd);
     EditorGUILayout.Space();
     EditorGUILayout.LabelField("Start to End Positions", EditorStyles.boldLabel); 
	target_cs.targetStart = (Transform)EditorGUILayout.ObjectField("Start Transform", target_cs.targetStart, typeof(Transform), true);
	target_cs.targetEnd = (Transform)EditorGUILayout.ObjectField("End Transform", target_cs.targetEnd, typeof(Transform), true);
	EditorGUILayout.Space();
	EditorGUILayout.LabelField("Spiral Curve", EditorStyles.boldLabel);
	target_cs.growCurve = EditorGUILayout.CurveField(target_cs.growCurve);
	EditorGUILayout.Space();
	target_cs._bend = EditorGUILayout.Toggle("Bend", target_cs._bend);
	target_cs._bendMultiplier = EditorGUILayout.FloatField("Bend Multiplier", target_cs._bendMultiplier);
	EditorGUILayout.LabelField("Bend Curve", EditorStyles.boldLabel);
	target_cs.bendCurve = EditorGUILayout.CurveField(target_cs.bendCurve);
	
	EditorGUILayout.Space();
		EditorGUILayout.LabelField("Noise", EditorStyles.boldLabel);
	target_cs.noise =  EditorGUILayout.FloatField("Noise Amount", target_cs.noise);
	if(target_cs.noise != 0){
		target_cs.noiseScale = EditorGUILayout.FloatField("Noise Scale", target_cs.noiseScale);
		target_cs.noiseSpeed = EditorGUILayout.FloatField("Noise Speed", target_cs.noiseSpeed);
	}
	
	EditorGUILayout.Space();
	EditorGUILayout.LabelField("Line Renderer", EditorStyles.boldLabel);
	target_cs.widthStart = EditorGUILayout.FloatField("Line Start Width", target_cs.widthStart);
    target_cs.widthEnd = EditorGUILayout.FloatField("Line End Width", target_cs.widthEnd); 
	target_cs.LenghtOfLineRenderer = EditorGUILayout.FloatField("Vertex Resolution", target_cs.LenghtOfLineRenderer);
	target_cs.smooth = EditorGUILayout.Toggle("Smooth Resolution", target_cs.smooth);
   
    EditorGUILayout.Space();
	EditorGUILayout.LabelField("Material", EditorStyles.boldLabel);
    target_cs.lineMaterial = (Material)EditorGUILayout.ObjectField("Material", target_cs.lineMaterial, typeof(Material), false); 
    target_cs.stretchMaterial = EditorGUILayout.FloatField("Material X Tiling", target_cs.stretchMaterial);
    target_cs.stretchYMaterial = EditorGUILayout.FloatField("Material Y Tiling", target_cs.stretchYMaterial);
    target_cs.offsetMaterialSpeed = EditorGUILayout.FloatField("Material X Speed", target_cs.offsetMaterialSpeed);
    target_cs.offsetYMaterialSpeed = EditorGUILayout.FloatField("Material Y Speed", target_cs.offsetYMaterialSpeed); 
    
    EditorGUILayout.Space();
	EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
    target_cs.rotateSpeed = EditorGUILayout.FloatField("Rotation Speed", target_cs.rotateSpeed);
    target_cs.rotationOffset = EditorGUILayout.FloatField("Rotation Offset", target_cs.rotationOffset);  
    
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("Spiral", EditorStyles.boldLabel);
    
    target_cs.sineDepth = EditorGUILayout.FloatField("Width", target_cs.sineDepth);
    target_cs.sineDepthFrequency = EditorGUILayout.FloatField("Width Frequency", target_cs.sineDepthFrequency); 
    target_cs.sineHeight = EditorGUILayout.FloatField("Height", target_cs.sineHeight);
    target_cs.sineHeightFrequency = EditorGUILayout.FloatField("Height Frequency", target_cs.sineHeightFrequency); 
    target_cs.sineLenght = EditorGUILayout.FloatField("Inner Wave", target_cs.sineLenght);
    target_cs.sineLenghtFrequency = EditorGUILayout.FloatField("Wave Frequency", target_cs.sineLenghtFrequency); 
    target_cs.sineGrowMultiplier = EditorGUILayout.FloatField("Grow Multiplier", target_cs.sineGrowMultiplier);
    target_cs.sineLenghtSpeed = EditorGUILayout.FloatField("Wave Speed", target_cs.sineLenghtSpeed); 
    target_cs.sineHeightSpeed = EditorGUILayout.FloatField("Height Speed", target_cs.sineHeightSpeed);
    target_cs.sineDepthSpeed = EditorGUILayout.FloatField("Depth Speed", target_cs.sineDepthSpeed); 
    EditorGUILayout.Space();
    target_cs.runOnceThenDisable = EditorGUILayout.Toggle("Stop On Start", target_cs.runOnceThenDisable);
    EditorGUILayout.LabelField("Disable calculation when the game starts (spiral will still rotate)", EditorStyles.miniLabel); 
	
     
	EditorGUILayout.Space();EditorGUILayout.Space();	
	if (GUI.changed) {
            EditorUtility.SetDirty (target_cs);
         if(!UnityEngine.Application.isPlaying){
			target_cs.GenerateSpiral(counter);
   			counter+=.1f;
   		 	target_cs.PositionSpiral();
   		 }
   	}
	//DrawDefaultInspector();
    }	
}