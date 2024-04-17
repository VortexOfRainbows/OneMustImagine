Shader "Hidden/Custom/CameraBlur"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	int _Range;
	float _Bake;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		//float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		
		float2 pixelRange = (_ScreenParams.zw - float2(1, 1));
		float4 colorSum;
		for (int x = -_Range; x <= _Range; x++)
		{
			for (int y = -_Range; y <= _Range; y++)
			{
				colorSum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(pixelRange.x * x, pixelRange.y * y));
			}
		}
		float4 colorAverage = colorSum / ((1+2*_Range)*(1+2*_Range)) * _Bake;
		//float4 colorDiff = color - colorAvg;
		return (colorAverage);
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}