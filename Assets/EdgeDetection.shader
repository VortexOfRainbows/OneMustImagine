Shader "Hidden/Custom/EdgeDetection"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _Range;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		
		float2 pixelRange = _Range * (_ScreenParams.zw - float2(1, 1));
		float4 colorTotals;
		for (int a = -1; a <= 1; a++)
		{
			for (int b = -1; b <= 1; b++)
			{
				if(a != 0 || b != 0)
				{
					colorTotals += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(pixelRange.x * a, pixelRange.y * b));
				}
			}
		}
		float4 colorAvg = colorTotals / 8;
		float4 colorDiff = color - colorAvg;
		return lerp(color, float4(0, 0, 0, 0), sqrt(sqrt(abs(colorDiff.r) + abs(colorDiff.g) + abs(colorDiff.b) + abs(colorDiff.a))));
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