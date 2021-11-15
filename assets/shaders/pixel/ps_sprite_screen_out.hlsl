Texture2D colorTextures : TEXTURE: register(t0);
Texture2D lightTextures : TEXTURE: register(t1);

SamplerState SampleType;

struct PixelInputType
{
    float4 position : SV_POSITION;
    float2 tex : TEXCOORD0;
};

float4 main(PixelInputType input) : SV_TARGET
{
    float4 color;
    float4 lightColor;
    float4 finalColor;

    // Get the pixel color from the color texture.
    color = colorTextures.Sample(SampleType, input.tex);

    // Get the pixel color from the light map.
    lightColor = lightTextures.Sample(SampleType, input.tex);
    
    // Blend the two pixels together.
    finalColor = color * (lightColor * 8);

    return finalColor;
}