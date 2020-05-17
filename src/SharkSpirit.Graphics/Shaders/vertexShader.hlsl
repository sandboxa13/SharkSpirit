cbuffer ConstantBuffer : register(b0)
{
    matrix modelView;
    matrix modelViewProj;
}

//--------------------------------------------------------------------------------------
struct VS_OUTPUT
{
	float4 Pos : SV_POSITION;
	float2 TextureUV : TEXCOORD;
};

//--------------------------------------------------------------------------------------
// Vertex Shader
//--------------------------------------------------------------------------------------
VS_OUTPUT VS(float3 Pos : POSITION, float2 TextureUV : TEXCOORD)
{
	VS_OUTPUT output = (VS_OUTPUT)0;
    output.Pos = mul(float4(Pos, 1.0f), modelViewProj);
	output.TextureUV = TextureUV;
	return output;
}
