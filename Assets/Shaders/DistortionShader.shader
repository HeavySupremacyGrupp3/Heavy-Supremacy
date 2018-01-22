Shader "Custom/DistortionShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "black" {}
		_Color("Color", Color) =  (1,1,1,1)
		_DistTex("Distortion", 2D) = "grey" {}
		_DistMultiplier("Distortion Multiplier", Range(0,0.01)) = 0.025
		_DistMask("Distorion Mask", 2D) = "black" {}
		_Speed("Speed", float) = 1
		_Rotation("Rotation/Direction", Range(0,360)) = 0
	}

	SubShader
	{
		Tags{
		"Queue" = "Transparent"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
	}
		LOD 100
		ZWrite Off
		AlphaToMask On

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _DistTex;
			sampler2D _DistMask;
			float _DistMultiplier;
			float _Speed;
			float _DirectionX;
			float _DirectionY;
			float4 _Color;
			float _Rotation;
	#define _PI 3.1415926535897932384626433832795


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//3.1415926 = pi.
				//Divided by 180 because slider is between 0-360 and
				// 2*pi = 360 degrees.
				_DirectionY = -sin(_Rotation * _PI / 180);
				_DirectionX = -cos(_Rotation * _PI / 180);

				float2 distScroll = float2(_Time.x * _DirectionX, _Time.x * _DirectionY);
				fixed2 dist = (tex2D(_DistTex, i.uv + distScroll * _Speed).rg - 0.5) * 2;
				fixed distMask = tex2D(_DistMask, i.uv)[0];

				fixed4 col = _Color * tex2D(_MainTex, i.uv + dist * distMask * _DistMultiplier);
				fixed bg = col.a;

				return col;
			}
			ENDCG
		}
	}
}