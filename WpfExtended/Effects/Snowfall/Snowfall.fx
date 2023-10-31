//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Speed : register(C0);
float WindStrength : register(C1);
float FlakeSize : register(C2);
float Time : register(C3);
float Threshold : register(C4);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// https://github.com/keijiro/NoiseShader/tree/master/Packages/jp.keijiro.noiseshader/Shader
//--------------------------------------------------------------------------------------

float wglnoise_mod(float x, float y)
{
    return x - y * floor(x / y);
}

float2 wglnoise_mod(float2 x, float2 y)
{
    return x - y * floor(x / y);
}

float3 wglnoise_mod(float3 x, float3 y)
{
    return x - y * floor(x / y);
}

float4 wglnoise_mod(float4 x, float4 y)
{
    return x - y * floor(x / y);
}

float2 wglnoise_fade(float2 t)
{
    return t * t * t * (t * (t * 6 - 15) + 10);
}

float3 wglnoise_fade(float3 t)
{
    return t * t * t * (t * (t * 6 - 15) + 10);
}

float wglnoise_mod289(float x)
{
    return x - floor(x / 289) * 289;
}

float2 wglnoise_mod289(float2 x)
{
    return x - floor(x / 289) * 289;
}

float3 wglnoise_mod289(float3 x)
{
    return x - floor(x / 289) * 289;
}

float4 wglnoise_mod289(float4 x)
{
    return x - floor(x / 289) * 289;
}

float3 wglnoise_permute(float3 x)
{
    return wglnoise_mod289((x * 34 + 1) * x);
}

float4 wglnoise_permute(float4 x)
{
    return wglnoise_mod289((x * 34 + 1) * x);
}

float3 SimplexNoiseGrad(float2 v)
{
    const float C1 = (3 - sqrt(3)) / 6;
    const float C2 = (sqrt(3) - 1) / 2;

    // First corner
    float2 i = floor(v + dot(v, C2));
    float2 x0 = v - i + dot(i, C1);

    // Other corners
    float2 i1 = x0.x > x0.y ? float2(1, 0) : float2(0, 1);
    float2 x1 = x0 + C1 - i1;
    float2 x2 = x0 + C1 * 2 - 1;

    // Permutations
    i = wglnoise_mod289(i); // Avoid truncation effects in permutation
    float3 p = wglnoise_permute(i.y + float3(0, i1.y, 1));
    p = wglnoise_permute(p + i.x + float3(0, i1.x, 1));

    // Gradients: 41 points uniformly over a unit circle.
    // The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)
    float3 phi = p / 41 * 3.14159265359 * 2;
    float2 g0 = float2(cos(phi.x), sin(phi.x));
    float2 g1 = float2(cos(phi.y), sin(phi.y));
    float2 g2 = float2(cos(phi.z), sin(phi.z));

    // Compute noise and gradient at P
    float3 m = float3(dot(x0, x0), dot(x1, x1), dot(x2, x2));
    float3 px = float3(dot(g0, x0), dot(g1, x1), dot(g2, x2));

    m = max(0.5 - m, 0);
    float3 m3 = m * m * m;
    float3 m4 = m * m3;

    float3 temp = -8 * m3 * px;
    float2 grad = m4.x * g0 + temp.x * x0 +
                  m4.y * g1 + temp.y * x1 +
                  m4.z * g2 + temp.z * x2;

    return 99.2 * float3(grad, dot(m4, px));
}

float SimplexNoise(float2 v)
{
    return SimplexNoiseGrad(v).z;
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInputSampler, uv);

    // Calculate a parallax offset based on depth. Here, we just use the Y coordinate
    uv.y -= Time * Speed;
    uv.x += WindStrength * Time;

    // Get the random value to decide where to place a snowflake
    float randVal = (SimplexNoise(uv * FlakeSize) + 1) / 2;
    //float randVal = (snoise(floor(tiledUV)) + 1) / 2;

    // Generate a dynamic circular snowflake if within the threshold
    if (randVal > Threshold)
    {
        color.rgb = lerp(color.rgb, float3(1, 1, 1), 0.5 + (randVal - Threshold) / (1 - Threshold));
    }

    return color;
}


