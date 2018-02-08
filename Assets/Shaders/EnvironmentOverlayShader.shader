Shader "Custom/EnvironmentOverlayShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo Texture", 2D) = "white" {}
		_OverlayTex("Overlay Texture", 2D) = "white" {}
		_BumpMap("Main Normalmap Texture", 2D) = "bump" {}
		_OverlayBump("Overlay Normalmap Texture", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_NormalMultiplier("Normal Multiplier", float) = 1
		_OverlaySpread("Overlay Spread", float) = 0.5
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_OverlayBump;
			float3 worldPos; //Built-in value in shaders, yay?
			float3 worldNormal; //Built-in value, this is getting old...
			INTERNAL_DATA //Needed for normalmap.
		};

		sampler2D _MainTex;
		sampler2D _OverlayTex;
		sampler2D _BumpMap;
		sampler2D _OverlayBump;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _OverlaySpread;
		float _NormalMultiplier;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf(Input IN, inout SurfaceOutputStandard o) {

			//Main tex blending.
			float3 X = tex2D(_MainTex, IN.worldPos.zy);
			float3 Y = tex2D(_MainTex, IN.worldPos.zx);
			float3 Z = tex2D(_MainTex, IN.worldPos.xy);

			float3 blendNormal = saturate(pow(IN.worldNormal * 1.4, 4)); //Clamp and raise worldnormal to power of 4.
			float3 blendedT = Z;
			blendedT = lerp(blendedT, X, blendNormal.x);
			blendedT = lerp(blendedT, Y, blendNormal.y);

			//Overlay blending.
			float3 Xo = tex2D(_OverlayTex, IN.worldPos.zy);
			float3 Yo = tex2D(_OverlayTex, IN.worldPos.zx);
			float3 Zo = tex2D(_OverlayTex, IN.worldPos.xy);

			float3 blendedOverlay = Zo;
			blendedOverlay = lerp(blendedOverlay, Xo, blendNormal.x);
			blendedOverlay = lerp(blendedOverlay, Yo, blendNormal.y);

			//Same stuff but for normalmaps.
	/*		float3 mainBump = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			float3 overlayBump = UnpackNormal(tex2D(_OverlayBump, IN.uv_OverlayBump));
*/
			//float3 Xmn = tex2D(_BumpMap, IN.worldPos.zy);
			//float3 Ymn = tex2D(_BumpMap, IN.worldPos.zx);
			//float3 Zmn = tex2D(_BumpMap, IN.worldPos.xy);

			//float3 blendedMainBump = Zmn;
			//blendedMainBump = lerp(_BumpMap, Xmn, blendNormal.x);
			//blendedMainBump = lerp(_BumpMap, Ymn, blendNormal.y);

			////Overlay normalmap.
			//float3 Xon = tex2D(_OverlayBump, IN.worldPos.zy);
			//float3 Yon = tex2D(_OverlayBump, IN.worldPos.zx);
			//float3 Zon = tex2D(_OverlayBump, IN.worldPos.xy);

			//float3 blendedOverlayBump = Zon;
			//blendedOverlayBump = lerp(_OverlayBump, Xon, blendNormal.x);
			//blendedOverlayBump = lerp(_OverlayBump, Yon, blendNormal.y);



			if (dot(o.Normal, IN.worldNormal.y) >= _OverlaySpread && IN.worldNormal.y > 0)
			{
				o.Albedo = blendedOverlay.rgb * _Color;
				//o.Normal = blendedOverlayBump;
			}
			else
			{
				o.Albedo = blendedT.rgb * _Color;
				//o.Normal = blendedMainBump;
			}
			
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
		}
			FallBack "Diffuse"
}
