struct PS_INPUT
{
	float4 inPosition : SV_POSITION;
	float2 inTexCoord : TEXCOORD;
};

Texture2D lightMap: TEXTURE: register(t0);
SamplerState objSamplerState : SAMPLER: register(s0);

float4 main(PS_INPUT input) : SV_TARGET
{
	float4 lightColor = lightMap.Sample(objSamplerState, input.inTexCoord);

	clip(lightColor.a - 0.1f);

	return lightColor;
}