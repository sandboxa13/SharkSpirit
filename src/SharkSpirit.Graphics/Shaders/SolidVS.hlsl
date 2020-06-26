cbuffer CBuf
{
    matrix modelView;
    matrix modelViewProj;
};

float4 VS(float3 pos : Position) : SV_Position
{
    return mul(float4(pos, 1.0f), modelViewProj);
}