// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Teleport"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Float0("Float 0", Float) = 1
		_Teleport("Teleport", Range( -20 , 20)) = -5.327737
		_Range("Range", Range( -20 , 20)) = 2.834983
		[HDR]_ColorTel("ColorTel", Color) = (0,766.9961,698.7294,1)
		_Alb("Alb", 2D) = "white" {}
		_Met("Met", 2D) = "white" {}
		_Nor("Nor", 2D) = "bump" {}
		_offsetForce("offsetForce", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float _Teleport;
		uniform float _Range;
		uniform float _offsetForce;
		uniform float _Float0;
		uniform sampler2D _Nor;
		uniform float4 _Nor_ST;
		uniform sampler2D _Alb;
		uniform float4 _Alb_ST;
		uniform float4 _ColorTel;
		uniform sampler2D _Met;
		uniform float4 _Met_ST;
		uniform float _Cutoff = 0.5;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 transform18 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float gradient16 = saturate( ( ( transform18.y + _Teleport ) / _Range ) );
			float mulTime7 = _Time.y * _Float0;
			float2 panner6 = ( mulTime7 * float2( 0,-5 ) + float2( 0,0 ));
			float2 uv_TexCoord1 = v.texcoord.xy * float2( 55,55 ) + panner6;
			float simplePerlin2D2 = snoise( uv_TexCoord1 );
			simplePerlin2D2 = simplePerlin2D2*0.5 + 0.5;
			float Noise11 = ( simplePerlin2D2 + 0.4 );
			float3 offset50 = ( ( ( ase_vertex3Pos * gradient16 ) * _offsetForce ) * Noise11 );
			v.vertex.xyz += offset50;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Nor = i.uv_texcoord * _Nor_ST.xy + _Nor_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Nor, uv_Nor ) );
			float2 uv_Alb = i.uv_texcoord * _Alb_ST.xy + _Alb_ST.zw;
			o.Albedo = tex2D( _Alb, uv_Alb ).rgb;
			float mulTime7 = _Time.y * _Float0;
			float2 panner6 = ( mulTime7 * float2( 0,-5 ) + float2( 0,0 ));
			float2 uv_TexCoord1 = i.uv_texcoord * float2( 55,55 ) + panner6;
			float simplePerlin2D2 = snoise( uv_TexCoord1 );
			simplePerlin2D2 = simplePerlin2D2*0.5 + 0.5;
			float Noise11 = ( simplePerlin2D2 + 0.4 );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 transform18 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float gradient16 = saturate( ( ( transform18.y + _Teleport ) / _Range ) );
			float4 emission39 = ( _ColorTel * ( Noise11 * gradient16 ) );
			o.Emission = emission39.rgb;
			float2 uv_Met = i.uv_texcoord * _Met_ST.xy + _Met_ST.zw;
			o.Metallic = tex2D( _Met, uv_Met ).r;
			o.Alpha = 1;
			float temp_output_31_0 = ( gradient16 * 1.0 );
			float opacity27 = ( ( ( ( 1.0 - gradient16 ) * Noise11 ) - temp_output_31_0 ) + ( 1.0 - temp_output_31_0 ) );
			clip( opacity27 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
532;435;959;341;2502.866;-746.4308;2.037198;True;False
Node;AmplifyShaderEditor.CommentaryNode;22;-2359.038,-146.5459;Inherit;False;1579.735;613.6957;Comment;9;11;8;7;6;5;10;2;9;1;Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;23;-2085.87,797.9962;Inherit;False;1480.678;536.5005;Comment;8;13;18;14;20;15;19;21;16;Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2309.038,331.0742;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;13;-2035.87,847.9962;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;7;-2136.996,332.3204;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1840.128,1090.468;Inherit;True;Property;_Teleport;Teleport;2;0;Create;True;0;0;False;0;-5.327737;0;-20;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;18;-1838.091,914.9652;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;6;-1921.346,257.6067;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-1604.019,895.4247;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1480.744,1146.217;Inherit;False;Property;_Range;Range;3;0;Create;True;0;0;False;0;2.834983;-20;-20;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;5;-2143.666,-2.980106;Inherit;False;Constant;_Tilling;Tilling;1;0;Create;True;0;0;False;0;55,55;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleDivideOpNode;19;-1269.342,981.7932;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1868.786,-96.54594;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;2;-1611.585,-61.87878;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1696.533,209.1498;Inherit;True;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;21;-1010.961,1172.055;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-1376.718,138.6096;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;35;-2270.896,1776.428;Inherit;False;2075.896;875.8376;Comment;10;33;17;24;26;30;31;32;25;34;27;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-848.1917,938.5972;Inherit;True;gradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-1022.302,30.31711;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-2220.896,2411.441;Inherit;True;16;gradient;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-1531.111,1938.874;Inherit;False;11;Noise;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-1405.62,2083.833;Inherit;False;16;gradient;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;24;-1980.106,2385.658;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;47;99.48875,1238.572;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;48;151.2178,1497.216;Inherit;False;16;gradient;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;698.0255,822.1337;Inherit;False;16;gradient;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;707.925,1615.839;Inherit;False;Property;_offsetForce;offsetForce;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1154.678,2095.479;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;669.903,671.2618;Inherit;False;11;Noise;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1457.403,1826.428;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;388.1579,1298.594;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;841.6072,750.8687;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;32;-892.7153,2078.31;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;33;-1410.958,2468.674;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;810.2824,1441.122;Inherit;False;11;Noise;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;860.2876,570.1823;Inherit;False;Property;_ColorTel;ColorTel;4;1;[HDR];Create;True;0;0;False;0;0,766.9961,698.7294,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;774.9623,1255.115;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;1068.737,1318.292;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1173.662,770.8541;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-621.3641,2399.266;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;1270.758,1329.101;Inherit;False;offset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-437.9997,2031.291;Inherit;True;opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;1313.516,706.9312;Inherit;False;emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;-83.01831,626.7683;Inherit;False;50;offset;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;46;20.75032,31.05774;Inherit;True;Property;_Nor;Nor;7;0;Create;True;0;0;False;0;-1;f4baef908ed4fab4aa5fa225973ae502;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;42;54.10072,-302.2978;Inherit;True;Property;_Alb;Alb;5;0;Create;True;0;0;False;0;-1;17bf72ec73a38cb479b90a200e03b5b3;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;12;47.77763,167.657;Inherit;False;39;emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;76.59393,258.3448;Inherit;True;27;opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;45;-85.01294,-142.4666;Inherit;True;Property;_Met;Met;6;0;Create;True;0;0;False;0;-1;8ccc09e27049be54e9cbb8dd8b5c8bad;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;284.0395,31.93501;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Teleport;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;8;0
WireConnection;18;0;13;0
WireConnection;6;1;7;0
WireConnection;15;0;18;2
WireConnection;15;1;14;0
WireConnection;19;0;15;0
WireConnection;19;1;20;0
WireConnection;1;0;5;0
WireConnection;1;1;6;0
WireConnection;2;0;1;0
WireConnection;21;0;19;0
WireConnection;9;0;2;0
WireConnection;9;1;10;0
WireConnection;16;0;21;0
WireConnection;11;0;9;0
WireConnection;24;0;17;0
WireConnection;31;0;30;0
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;49;0;47;0
WireConnection;49;1;48;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;32;0;25;0
WireConnection;32;1;31;0
WireConnection;33;0;31;0
WireConnection;52;0;49;0
WireConnection;52;1;53;0
WireConnection;55;0;52;0
WireConnection;55;1;54;0
WireConnection;41;0;40;0
WireConnection;41;1;38;0
WireConnection;34;0;32;0
WireConnection;34;1;33;0
WireConnection;50;0;55;0
WireConnection;27;0;34;0
WireConnection;39;0;41;0
WireConnection;0;0;42;0
WireConnection;0;1;46;0
WireConnection;0;2;12;0
WireConnection;0;3;45;0
WireConnection;0;10;28;0
WireConnection;0;11;51;0
ASEEND*/
//CHKSM=529003405600EB9323E50422CA27F265AB2E7CB9