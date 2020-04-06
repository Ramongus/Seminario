// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Clase05/ToonWater"
{
	Properties
	{
		[NoScaleOffset]_Pattern("Pattern", 2D) = "white" {}
		_SpeedX("SpeedX", Float) = 0
		_SpeedY("SpeedY", Float) = 0
		[NoScaleOffset]_Flowmap("Flowmap", 2D) = "white" {}
		_FlowmapIntensity("Flowmap Intensity", Range( 0 , 1)) = 0.2470588
		_Tiling("Tiling", Float) = 0
		_MainOpacity("Main Opacity", Range( 0 , 1)) = 0
		_DepthDistance("Depth Distance", Float) = 0
		_FallOff("FallOff", Float) = 0
		_FoamOpacity("Foam Opacity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _Pattern;
		uniform float _SpeedX;
		uniform float _SpeedY;
		uniform float _Tiling;
		uniform sampler2D _Flowmap;
		uniform float _FlowmapIntensity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;
		uniform float _FallOff;
		uniform float _FoamOpacity;
		uniform float _MainOpacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 color45 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			float2 appendResult9 = (float2(_SpeedX , _SpeedY));
			float2 temp_cast_0 = (_Tiling).xx;
			float2 uv_TexCoord16 = i.uv_texcoord * temp_cast_0;
			float4 lerpResult17 = lerp( float4( uv_TexCoord16, 0.0 , 0.0 ) , tex2D( _Flowmap, uv_TexCoord16 ) , _FlowmapIntensity);
			float2 panner3 = ( _Time.y * appendResult9 + lerpResult17.rg);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth30 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos )));
			float distanceDepth30 = abs( ( screenDepth30 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			float temp_output_35_0 = saturate( pow( distanceDepth30 , _FallOff ) );
			float4 lerpResult36 = lerp( color45 , tex2D( _Pattern, panner3 ) , temp_output_35_0);
			o.Emission = lerpResult36.rgb;
			float lerpResult37 = lerp( _FoamOpacity , _MainOpacity , temp_output_35_0);
			o.Alpha = lerpResult37;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
0;416;877;273;382.694;585.4398;2.446391;True;True
Node;AmplifyShaderEditor.CommentaryNode;25;-1565.357,-495.9985;Float;False;1129.108;504.6927;Flowmap;6;23;16;18;10;17;8;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1471.859,-435.2386;Float;False;Property;_Tiling;Tiling;5;0;Create;True;0;0;False;0;0;-3.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;39;-717.5739,74.74596;Float;False;977.8495;270.3554;Depth Fade;5;31;33;30;32;35;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;24;-434.3728,-335.7586;Float;False;654.7547;376.9999;Panner;4;7;9;3;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1249.28,-432.5421;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-1103.705,-315.2596;Float;True;Property;_Flowmap;Flowmap;3;1;[NoScaleOffset];Create;True;0;0;False;0;None;f36bc6e7b6b2eaf4e98216385c5baf17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-396.3759,-238.0771;Float;False;Property;_SpeedX;SpeedX;1;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1024.029,-125.6944;Float;False;Property;_FlowmapIntensity;Flowmap Intensity;4;0;Create;True;0;0;False;0;0.2470588;0.122;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-667.5739,152.1104;Float;False;Property;_DepthDistance;Depth Distance;7;0;Create;True;0;0;False;0;0;7.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-622.206,-68.65872;Float;False;Property;_SpeedY;SpeedY;2;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;17;-756.4884,-327.3;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-286.4027,-62.41462;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-312.6186,230.1014;Float;False;Property;_FallOff;FallOff;8;0;Create;True;0;0;False;0;0;0.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-216.3097,-249.3204;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DepthFade;30;-428.5737,130.1104;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;40;240.8188,-448.1031;Float;False;651.5063;494.7853;Textures;2;1;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;3;-50.61784,-285.7585;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;41;338.4241,63.65176;Float;False;546.1644;294.8972;Opacity;3;29;38;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PowerNode;32;-115.699,124.746;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;516.5277,122.2978;Float;False;Property;_FoamOpacity;Foam Opacity;9;0;Create;True;0;0;False;0;0;0.4301831;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;35;50.17598,195.0842;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;388.4236,186.2219;Float;False;Property;_MainOpacity;Main Opacity;6;0;Create;True;0;0;False;0;0;0.8301091;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;255.5342,-347.1782;Float;True;Property;_Pattern;Pattern;0;1;[NoScaleOffset];Create;True;0;0;False;0;None;efb68ca9a3e6f0d408b19a189d622ea4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;45;338.9912,-516.9408;Float;False;Constant;_Color0;Color 0;10;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;37;700.5877,202.549;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;43;-1178.246,250.6101;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;44;-1455.172,349.6655;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;36;627.6445,-343.2386;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;927.8145,-435.4257;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Clase05/ToonWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;23;0
WireConnection;10;1;16;0
WireConnection;17;0;16;0
WireConnection;17;1;10;0
WireConnection;17;2;18;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;30;0;31;0
WireConnection;3;0;17;0
WireConnection;3;2;9;0
WireConnection;3;1;5;0
WireConnection;32;0;30;0
WireConnection;32;1;33;0
WireConnection;35;0;32;0
WireConnection;1;1;3;0
WireConnection;37;0;38;0
WireConnection;37;1;29;0
WireConnection;37;2;35;0
WireConnection;36;0;45;0
WireConnection;36;1;1;0
WireConnection;36;2;35;0
WireConnection;0;2;36;0
WireConnection;0;9;37;0
ASEEND*/
//CHKSM=2E657EE74394F812FB87B0EC23E9254BD601E9E5