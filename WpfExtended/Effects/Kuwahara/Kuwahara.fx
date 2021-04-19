// Sampler Registers //

sampler2D implicitInputSampler : register(S0);

// Constants Registers //

float radius : register(C0);
float width : register(C1);
float height : register(C2);

half getGray(half4 c)
{
	return(dot(c.rgb, ((0.33333).xxx)));
}

#define GrabPix(n,a) half3 n = tex2D(implicitInputSampler,(a)).xyz;

float4 main(float2 uv : TEXCOORD) : COLOR
{
	half2 ox = half2(radius / width, 0.0);
	half2 oy = half2(0.0, radius / height);
	half2 ox2 = 2 * ox;
	half2 oy2 = 2 * oy;
	half2 PP = uv - oy2;
	GrabPix(c00, PP - ox2)
	GrabPix(c01, PP - ox)
	GrabPix(c02, PP)
	GrabPix(c03, PP + ox)
	GrabPix(c04, PP + ox2)
	PP = uv - oy;
	GrabPix(c10, PP - ox2)
	GrabPix(c11, PP - ox)
	GrabPix(c12, PP)
	GrabPix(c13, PP + ox)
	GrabPix(c14, PP + ox2)
	PP = uv;
	GrabPix(c20, PP - ox2)
	GrabPix(c21, PP - ox)
	GrabPix(c22, PP)
	GrabPix(c23, PP + ox)
	GrabPix(c24, PP + ox2)
	half3 m00 = (c00 + c01 + c02 + c10 + c11 + c12 + c20 + c21 + c22) / 9;
	half3 d = (c00 - m00); half v00 = dot(d, d);
	d = (c01 - m00); v00 += dot(d, d);
	d = (c02 - m00); v00 += dot(d, d);
	d = (c10 - m00); v00 += dot(d, d);
	d = (c11 - m00); v00 += dot(d, d);
	d = (c12 - m00); v00 += dot(d, d);
	d = (c20 - m00); v00 += dot(d, d);
	d = (c21 - m00); v00 += dot(d, d);
	d = (c12 - m00); v00 += dot(d, d);
	half3 m01 = (c02 + c03 + c04 + c12 + c13 + c14 + c22 + c23 + c24) / 9;
	d = (c02 - m01); half v01 = dot(d, d);
	d = (c03 - m01); v01 += dot(d, d);
	d = (c04 - m01); v01 += dot(d, d);
	d = (c12 - m01); v01 += dot(d, d);
	d = (c13 - m01); v01 += dot(d, d);
	d = (c14 - m01); v01 += dot(d, d);
	d = (c22 - m01); v01 += dot(d, d);
	d = (c23 - m01); v01 += dot(d, d);
	d = (c14 - m01); v01 += dot(d, d);
	PP = uv + oy;
	GrabPix(c30, PP - ox2)
	GrabPix(c31, PP - ox)
	GrabPix(c32, PP)
	GrabPix(c33, PP + ox)
	GrabPix(c34, PP + ox2)
	PP = uv + oy;
	GrabPix(c40, PP - ox2)
	GrabPix(c41, PP - ox)
	GrabPix(c42, PP)
	GrabPix(c43, PP + ox)
	GrabPix(c44, PP + ox2)
	half3 m10 = (c20 + c21 + c22 + c30 + c31 + c32 + c40 + c41 + c42) / 9;
	d = (c20 - m10); half v10 = dot(d, d);
	d = (c21 - m10); v10 += dot(d, d);
	d = (c22 - m10); v10 += dot(d, d);
	d = (c30 - m10); v10 += dot(d, d);
	d = (c31 - m10); v10 += dot(d, d);
	d = (c32 - m10); v10 += dot(d, d);
	d = (c40 - m10); v10 += dot(d, d);
	d = (c41 - m10); v10 += dot(d, d);
	d = (c42 - m10); v10 += dot(d, d);
	half3 m11 = (c22 + c23 + c24 + c32 + c33 + c34 + c42 + c43 + c44) / 9;
	d = (c22 - m11); half v11 = dot(d, d);
	d = (c23 - m11); v11 += dot(d, d);
	d = (c24 - m11); v11 += dot(d, d);
	d = (c32 - m11); v11 += dot(d, d);
	d = (c33 - m11); v11 += dot(d, d);
	d = (c34 - m11); v11 += dot(d, d);
	d = (c42 - m11); v11 += dot(d, d);
	d = (c43 - m11); v11 += dot(d, d);
	d = (c44 - m11); v11 += dot(d, d);
	half3 result = m00;
	half rv = v00;
	if (v01 < rv) { result = m01; rv = v01; }
	if (v10 < rv) { result = m10; rv = v10; }
	if (v11 < rv) { result = m11; }
	return half4(result, 1);
}