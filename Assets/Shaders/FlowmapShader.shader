Shader "Custom/FlowmapShader"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_FlowMap("Flow Map", 2D) = "white" {}
		_FlowSpeed("Flow Speed", float) = 0.05
		_MaskTex("Mask Texture", 2D) = "white" {}
	}

	SubShader
		{
			Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True" //Ignores prjecting other materials onto this material, due to potential transparency.
			"RenderType" = "Transparent"
		}

		Cull Off //Disable Culling.
		ZWrite Off //Disable Zbuffer.
		Blend One OneMinusSrcAlpha //The blend-mode which is used.

		Pass
		{
			CGPROGRAM
			#pragma vertex vert //Defines the vertex function below: v2f vert.
			#pragma fragment frag //Defines the fragment function below: fixed4 frag.
			#include "UnityCG.cginc" //Include the UnityCG library.

			//Pass information from UnityCG (perhaps...?).
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			//Pass information from the vertex-function to the fragment-function.
			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			fixed4 _Color;

			//Vertexshader. Executes on each vertex on the geometry in the scene.
			v2f vert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex); // Equals to "= mul(UNITY_MATRIX_MVP)", but more efficient. Tranforms position from object to homogenous space
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _FlowMap;
			sampler2D _MaskTex;
			float _FlowSpeed;

			//Fragmentshader. Executes on each pixel on the image in the renderwindow.
			fixed4 frag(v2f IN) : SV_Target
			{
				float3 flowDir = tex2D(_FlowMap, IN.texcoord) * 2.0f - 1.0f;
				flowDir *= _FlowSpeed;

				float phase0 = frac(_Time[1] * 0.5f + 0.5f); //frac = returns 0.23 if 1.23 was the input. Likewise if input is 5.6 it returns 0.6.
				float phase1 = frac(_Time[1] * 0.5f + 1.0f);

				half3 tex0 = tex2D(_MainTex, IN.texcoord + flowDir.xy * phase0); //Half = half the precision of a float.
				half3 tex1 = tex2D(_MainTex, IN.texcoord + flowDir.xy * phase1); //Render the maintexture twice and hide the one that will "jump back" at the end of the flow-loop. Phase0 and Phase1 are used for this method.

				float flowLerp = abs((0.5f - phase0) / 0.5f); //Used to create a seamless loop between the two textures.
				half3 finalColor = lerp(tex0, tex1, flowLerp); //Lerp between two textures depending on the third argument (flowLerp).

				fixed4 c = float4(finalColor, 1.0f) * IN.color;
				c.rgb *= c.a;
				return c;
			}
			ENDCG
		}
	}
}