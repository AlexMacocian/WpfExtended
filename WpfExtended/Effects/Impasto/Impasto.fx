//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float textureWidth : register(C0);
float textureHeight : register(C1);
float strength : register(C2);
float depth : register(C3);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D  implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 lightDirection = float4(0.5, 0.21, 0.85, 1);

float emboss(float2 uv)
{
    float4 outC = { 0.5, 0.5, 0.5, 1.0 };

    outC -= tex2D(implicitInputSampler, uv - (1 / depth)) * strength;
    outC += tex2D(implicitInputSampler, uv + (1 / depth)) * strength;
    outC.rgb = (outC.r + outC.g + outC.b) / 3.0f;

    return outC;
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    // Sample the color
    float4 color = tex2D(implicitInputSampler, uv);
    float2 resolution = float2(textureWidth, textureHeight);
    
    // Use the emboss value of the color as the bump value
    float bumpValue = emboss(uv);

    // Calculate a basic normal based on the bump value
    float2 offset = 1.0 / resolution;
    float left = emboss(uv - float2(offset.x, 0));
    float right = emboss(uv + float2(offset.x, 0));
    float top = emboss(uv - float2(0, offset.y));
    float bottom = emboss(uv + float2(0, offset.y));
    
    float3 normal;
    normal.x = (left - right) * 0.5; // Multiplier to increase the effect, tweak as necessary
    normal.y = (top - bottom) * 0.5; // Same as above
    normal.z = 0.5;
    normal = normalize(normal);

    // Compute Lambertian lighting based on the normal
    float lightIntensity = dot(normal, lightDirection.xyz);
    float4 invertedColor = 1 - color;
    invertedColor.a = 1;

    color.rgb = (invertedColor.rgb * lightIntensity * 2) + color.rgb;
    return color;
}


