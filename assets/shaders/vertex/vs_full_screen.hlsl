struct VS_INPUT
{
    float3 inPos : POSITION;
    float2 inTexCoord : TEXCOORD;
};

struct VS_OUTPUT
{
    float4 outPosition : SV_POSITION;
    float2 outTexCoord : TEXCOORD;
};

VS_OUTPUT main(VS_INPUT input)
{
    VS_OUTPUT vso;
    vso.outPosition = float4(input.inPos, 1.0f);
    vso.outTexCoord = float2((input.inPos.x + 1) / 2.0f, -(input.inPos.y - 1) / 2.0f);
    return vso;
}