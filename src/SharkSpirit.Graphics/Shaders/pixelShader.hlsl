Texture2D ShaderTexture : register(t0);
SamplerState Sampler : register(s0);

//--------------------------------------------------------------------------------------
struct VS_OUTPUT
{
	float4 Pos : SV_POSITION;
	float2 TextureUV : TEXCOORD;
};


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 PS(VS_OUTPUT input) : SV_Target
{
	return ShaderTexture.Sample(Sampler, input.TextureUV);
}