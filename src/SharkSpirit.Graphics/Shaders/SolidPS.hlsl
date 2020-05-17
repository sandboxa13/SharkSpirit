cbuffer CBuf
{
    float4 color;
};

const float4 inner_color = { 1.0, 0.0, 0.0, 1.0 };

float4 PS() : SV_Target
{
    return color;
}