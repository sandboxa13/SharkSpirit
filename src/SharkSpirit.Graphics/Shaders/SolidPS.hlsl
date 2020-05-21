cbuffer CBuf
{
    float4 color;
};

float4 PS() : SV_Target
{
    return color;
}