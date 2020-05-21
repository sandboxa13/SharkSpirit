//static const float3 lightPos = { 0.0, 0.0, 0.0 };
//static const float3 ambient = { 0.0, 0.0, 0.0 };
//static const float3 diffuseColor = { 1.0, 1.0, 1.0 };
//static const float diffuseIntensity = 1;
//static const float attConst = 1.0;
//static const float attLin = 0.345;
//static const float attQuad = 0.3075;


//static const float3 materialColor = { 1.0, 1.0, 1.0 };
//static const float specularIntensity = 0.6;
//static const float specularPower = 30.0;



cbuffer LightCBuf : register(b0)
{
    float3 lightPos;
    float diffuseIntensity;
    float3 ambient;
    float attConst;
    float3 diffuseColor;
    float attLin;
    float attQuad;
};

cbuffer ObjectCBuf : register(b1)
{
    float3 materialColor;
    float specularIntensity;
    float specularPower;
};


float4 PS(float3 worldPos : Position, float3 n : Normal) : SV_Target
{
	// fragment to light vector data
    const float3 vToL = lightPos - worldPos;
    const float distToL = length(vToL);
    const float3 dirToL = vToL / distToL;
	// attenuation
    const float att = 1.0f / (attConst + attLin * distToL + attQuad * (distToL * distToL));
	// diffuse intensity
    const float3 diffuse = diffuseColor * diffuseIntensity * att * max(0.0f, dot(dirToL, n));
	// reflected light vector
    const float3 w = n * dot(vToL, n);
    const float3 r = w * 2.0f - vToL;
	// calculate specular intensity based on angle between viewing vector and reflection vector, narrow with power function
    const float3 specular = att * (diffuseColor * diffuseIntensity) * specularIntensity * pow(max(0.0f, dot(normalize(-r), normalize(worldPos))), specularPower);
	// final color
    return float4(saturate((diffuse + ambient + specular) * materialColor), 1.0f);
}