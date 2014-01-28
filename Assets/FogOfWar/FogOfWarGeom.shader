Shader "Custom/FogOfWarGeom" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
    	_MainTex ("Color (RGB) Alpha (A)", 2D) = "white"
    	_FogRadius ("FogRadius", Float) = 1.0
    	_FogMaxRadius ("FogMaxRadius", Float) = 0.5
		_Player1_Pos ("UnitPos1", Vector) = (0,0,0,1)
		_Player2_Pos ("UnitPos2", Vector) = (0,0,0,1)
		_Player3_Pos ("UnitPos3", Vector) = (0,0,0,1)
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
    	Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On
		Cull Off 

		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert vertex:vert
		
		sampler2D _MainTex;
		fixed4 _Color;
		float4 _Player1_Pos;
		float4 _Player2_Pos;
		float4 _Player3_Pos;
		float _FogRadius;
		float _FogMaxRadius;
	
		struct Input {
	    	float4 pos;
	    	float alpha;
	    	float2 location;
		};
		
		float powerForPos(float4 pos, float2 nearVertex);
		
		void vert (inout appdata_full vertexData, out Input outData)
		{
		    float2 posVertex = mul (_Object2World, vertexData.vertex).xz;
		    outData.alpha = (1.0 - clamp(_Color.a 
        				+ ( powerForPos(_Player1_Pos, posVertex) 
        				+ powerForPos(_Player2_Pos, posVertex)
        				+ powerForPos(_Player3_Pos, posVertex) ), 0, 1.0));
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color.rgb;
        	o.Alpha = IN.alpha;
    	}
    	
    	float powerForPos(float4 pos, float2 nearVertex) {
 			float attenumat = clamp(_FogRadius - abs(length(pos.xz - nearVertex.xy)), 0.0, _FogRadius);
 			if(attenumat > _FogRadius*_FogMaxRadius) {
 				attenumat = _FogRadius*_FogMaxRadius;  
 			}
    		return (1.0/_FogMaxRadius)*(attenumat/_FogRadius);
    	}
	
		ENDCG
	}

Fallback "Transparent/VertexLit"
}
