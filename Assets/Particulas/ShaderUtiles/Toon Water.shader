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
		[NoScaleOffset]_Foam("Foam", 2D) = "white" {}
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

		uniform sampler2D _Foam;
		uniform float _SpeedX;
		uniform float _SpeedY;
		uniform float _Tiling;
		uniform sampler2D _Flowmap;
		uniform float _FlowmapIntensity;
		uniform sampler2D _Pattern;
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
			float4 lerpResult36 = lerp( tex2D( _Foam, panner3 ) , tex2D( _Pattern, panner3 ) , temp_output_35_0);
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
7.333333;80;1061;567;2669.37;1202.281;4.723289;False;False
Node;AmplifyShaderEditor.CommentaryNode;25;-1800.593,-497.2836;Float;False;1129.108;504.6927;Flowmap;5;23;16;18;10;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1750.593,-409.299;Float;False;Property;_Tiling;Tiling;5;0;Create;True;0;0;False;0;0;5.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;39;-260.5877,270.3892;Float;False;977.8495;270.3554;Depth Fade;5;31;33;30;32;35;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;24;-576.4849,-184.378;Float;False;654.7547;376.9999;Panner;5;7;8;9;3;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1543.223,-447.2836;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-210.5877,347.7536;Float;False;Property;_DepthDistance;Depth Distance;7;0;Create;True;0;0;False;0;0;0.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-526.2838,-88.73058;Float;False;Property;_SpeedX;SpeedX;1;0;Create;True;0;0;False;0;0;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1306.633,-351.1776;Float;True;Property;_Flowmap;Flowmap;3;1;[NoScaleOffset];Create;True;0;0;False;0;None;f36bc6e7b6b2eaf4e98216385c5baf17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-526.4849,-17.6802;Float;False;Property;_SpeedY;SpeedY;2;0;Create;True;0;0;False;0;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1272.184,-124.4909;Float;False;Property;_FlowmapIntensity;Flowmap Intensity;4;0;Create;True;0;0;False;0;0.2470588;0.14;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;17;-936.4848,-343.5906;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;144.3675,425.7446;Float;False;Property;_FallOff;FallOff;8;0;Create;True;0;0;False;0;0;5.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;30;28.41227,325.7536;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-478.6201,97.67984;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-358.4221,-78.33338;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;3;-192.7302,-134.3779;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;32;341.2872,320.3892;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;41;813.3939,73.82205;Float;False;546.1644;294.8972;Opacity;3;29;38;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;40;240.8188,-448.1031;Float;False;651.5063;494.7853;Textures;3;34;1;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;35;542.2618,324.4274;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;290.8188,-398.1031;Float;True;Property;_Foam;Foam;9;1;[NoScaleOffset];Create;True;0;0;False;0;None;42c203fc0e865e04f9864547557a7b82;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;294.3861,-183.3179;Float;True;Property;_Pattern;Pattern;0;1;[NoScaleOffset];Create;True;0;0;False;0;None;efb68ca9a3e6f0d408b19a189d622ea4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;863.3939,196.3922;Float;False;Property;_MainOpacity;Main Opacity;6;0;Create;True;0;0;False;0;0;0.73;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;863.5368,123.8221;Float;False;Property;_FoamOpacity;Foam Opacity;10;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;37;1175.558,212.7193;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;36;708.3251,-292.2165;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1581.055,-262.1571;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Clase05/ToonWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;23;0
WireConnection;10;1;16;0
WireConnection;17;0;16;0
WireConnection;17;1;10;0
WireConnection;17;2;18;0
WireConnection;30;0;31;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;3;0;17;0
WireConnection;3;2;9;0
WireConnection;3;1;5;0
WireConnection;32;0;30;0
WireConnection;32;1;33;0
WireConnection;35;0;32;0
WireConnection;34;1;3;0
WireConnection;1;1;3;0
WireConnection;37;0;38;0
WireConnection;37;1;29;0
WireConnection;37;2;35;0
WireConnection;36;0;34;0
WireConnection;36;1;1;0
WireConnection;36;2;35;0
WireConnection;0;2;36;0
WireConnection;0;9;37;0
ASEEND*/
//CHKSM=BA85E66D26D21011DEF05E4118C40E2938B8C94F