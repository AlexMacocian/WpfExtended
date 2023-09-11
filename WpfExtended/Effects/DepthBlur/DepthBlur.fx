//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float FocusDepth : register(C0);
float FocusRange : register(C1);
float TextureWidth : register(C2);
float TextureHeight : register(C3);
float Strength : register(C5);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 originalColor = tex2D(implicitInputSampler, uv);
    
    // Compute the luminance
    float luminance = 0.299f * originalColor.r + 0.587f * originalColor.g + 0.114f * originalColor.b;
    
    // Calculate the blur amount based on the luminance
    float blurAmount = abs(luminance - FocusDepth) - FocusRange;
    blurAmount = clamp(blurAmount, 0.0f, 1.0f);
    
    float2 texelSize = 1.0f / float2(TextureWidth, TextureHeight); // Assuming a texture resolution of 512x512. Change as necessary.
    float4 color = float4(0, 0, 0, 0);
    float blurSize = 5;

    // Simple box blur
    for (int i = -blurSize; i <= blurSize; i++)
    {
        for (int j = -blurSize; j <= blurSize; j++)
        {
            color += tex2D(implicitInputSampler, uv + blurAmount * float2(i, j) * texelSize);
        }
    }

    color /= (2 * blurSize + 1) * (2 * blurSize + 1);
    
    // Set the pixel colors as a multiple of the luminance
    color.rgb = luminance * color.rgb;
    
    // Preserve the original alpha
    color.a = originalColor.a;

    return color;
}

