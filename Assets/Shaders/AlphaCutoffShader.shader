﻿Shader "Custom/AlphaCutoffShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_CutoffTex("Cutoff Texture", 2D) = "white" {}
		_Cutoff("Cutoff", Range(0,1)) = 0
		_EdgeColor("Edge Color", Color) = (1,1,1,1)
		_EdgeWidth("Edge Width", Range(0,1)) = 0
		_EdgeCutoffMultiplier("Edge Width multiplied by Cutoff", float) = 0
		_CutoffFade("Cutoff Fade", Range(0,1)) = 0
	}
		SubShader{
			Tags{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True" //Ignores prjecting other materials onto this material, due to potential transparency.
			"RenderType" = "Transparent"
		}

			Cull Off //Disable Culling.
			ZWrite Off //Disable Zbuffer.
			Blend One OneMinusSrcAlpha //The blend-mode which is used.

			LOD 200
pass
		{
			CGPROGRAM
				#pragma vertex vert //Defines the vertex function below: v2f vert.
				#pragma fragment frag //Defines the fragment function below: fixed4 frag.
				#include "UnityCG.cginc" //Include the UnityCG library.


					struct appdata_t
					{
						float4 vertex   : POSITION;
						float2 texcoord : TEXCOORD0;
					};

					//Pass information from the vertex-function to the fragment-function.
					struct v2f
					{
						float4 vertex   : SV_POSITION;
						half2 texcoord  : TEXCOORD0;
					};


				sampler2D _MainTex;
				sampler2D _CutoffTex;
				float _Cutoff, _EdgeWidth, _EdgeCutoffMultiplier, _CutoffFade, _EdgeWidthMultipliedMax;
				fixed4 _Color, _EdgeColor;

				v2f vert(appdata_t i)
				{
					v2f o;

					o.vertex = UnityObjectToClipPos(i.vertex); // Equals to "= mul(UNITY_MATRIX_MVP)", but more efficient. Tranforms position from object to homogenous space
					o.texcoord = i.texcoord;

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					if (_Cutoff == 0)
						_EdgeWidthMultipliedMax = _EdgeWidth;

					fixed4 cutOffTex = tex2D(_CutoffTex, i.texcoord);

					fixed4 wholeTex = tex2D(_MainTex, i.texcoord);
					
					float a = 0;
					if (cutOffTex.a > _Cutoff)
					{
						a = wholeTex.a;

						if (_EdgeWidth <= _EdgeWidthMultipliedMax && _EdgeCutoffMultiplier > 0)
							_EdgeWidth = (_Cutoff * _EdgeCutoffMultiplier);

						if (cutOffTex.a - _Cutoff < _EdgeWidth)
						{

							if (_CutoffFade > 0)
							{
								a -= lerp(1, 0, _EdgeWidth - _CutoffFade);
								if (a < 0)
									a = 0;
								else if (a > 1)
									a = 1;
							}
							wholeTex = _EdgeColor;

						}
					}
					else
					{
						a = 0;
					}

					fixed4 c = float4(wholeTex[0], wholeTex[1], wholeTex[2], a) * _Color;
					c.rgb *= c.a;
					return c;
				}

				ENDCG
		}
	}
}