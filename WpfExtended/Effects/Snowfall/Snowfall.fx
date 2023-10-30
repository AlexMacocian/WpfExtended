//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- BloomEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Speed : register(C0);
float WindStrength : register(C1);
float WindDirection : register(C2);
float FlakeSize : register(C3);
float Time : register(C4);
float Threshold : register(C5);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

// 
// Description : Array and textureless GLSL 2D simplex noise function.
//      Author : Ian McEwan, Ashima Arts.
//  Maintainer : ijm
//     Lastmod : 20110822 (ijm)
//     License : Copyright (C) 2011 Ashima Arts. All rights reserved.
//               Distributed under the MIT License. See LICENSE file.
//               https://github.com/ashima/webgl-noise
// simpled by guowei
// https://github.com/guoweish/glsl-noise-simplex

float3 mod289(float3 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}

float2 mod289(float2 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}

float3 permute(float3 x)
{
    return mod289(((x * 34.0) + 1.0) * x);
}

float snoise(float2 v)
{
    const float4 C = float4(0.211324865405187, // (3.0-sqrt(3.0))/6.0
                      0.366025403784439, // 0.5*(sqrt(3.0)-1.0)
                     -0.577350269189626, // -1.0 + 2.0 * C.x
                      0.024390243902439); // 1.0 / 41.0
// First corner
    float2 i = floor(v + dot(v, C.yy));
    float2 x0 = v - i + dot(i, C.xx);

// Other corners
    float2 i1;
  //i1.x = step( x0.y, x0.x ); // x0.x > x0.y ? 1.0 : 0.0
  //i1.y = 1.0 - i1.x;
    i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
  // x0 = x0 - 0.0 + 0.0 * C.xx ;
  // x1 = x0 - i1 + 1.0 * C.xx ;
  // x2 = x0 - 1.0 + 2.0 * C.xx ;
    float4 x12 = x0.xyxy + C.xxzz;
    x12.xy -= i1;

// Permutations
    i = mod289(i); // Avoid truncation effects in permutation
    float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0))
    + i.x + float3(0.0, i1.x, 1.0));

    float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
    m = m * m;
    m = m * m;

// Gradients: 41 points uniformly over a line, mapped onto a diamond.
// The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)

    float3 x = 2.0 * frac(p * C.www) - 1.0;
    float3 h = abs(x) - 0.5;
    float3 ox = floor(x + 0.5);
    float3 a0 = x - ox;

// Normalise gradients implicitly by scaling m
// Approximation of: m *= inversesqrt( a0*a0 + h*h );
    m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);

// Compute final noise value at P
    float3 g;
    g.x = a0.x * x0.x + h.x * x0.y;
    g.yz = a0.yz * x12.xz + h.yz * x12.yw;
    return 130.0 * dot(m, g);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInputSampler, uv);

    // Calculate a parallax offset based on depth. Here, we just use the Y coordinate
    float depth = uv.y;
    uv.y -= Time * Speed;
    uv.x += WindStrength * Time * WindDirection;

    // Tiling the UV coordinates to generate multiple snowflakes
    float2 tiledUV = uv * FlakeSize;

    // Get the random value to decide where to place a snowflake
    float randVal = (snoise(floor(tiledUV)) + 1) / 2;

    // Generate a dynamic circular snowflake if within the threshold
    if (randVal > Threshold)
    {
        color.rgb = lerp(color.rgb, float3(1, 1, 1), 0.5 + (randVal - Threshold) / (1 - Threshold));
    }

    return color;
}


