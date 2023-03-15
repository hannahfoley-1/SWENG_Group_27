#include <stereokit.hlsli>

//--color: color = 1,1,1,1
//--color2:color = 0,0,0,1
//--slope     = 1000
//--threshold = 0.2

float4 color;
float4 color2;
float  threshold;
float  slope;

struct vsIn {
	float4 pos  : SV_POSITION;
	float3 norm : NORMAL0;
	float2 uv   : TEXCOORD0;
	float4 col  : COLOR0;
};
struct psIn {
	float4 pos     : SV_POSITION;
	float4 color   : COLOR0;
	float3 world   : TEXCOORD0;
	float3 normal  : TEXCOORD1;
	float3 view_dir: TEXCOORD2;
	uint   view_id : SV_RenderTargetArrayIndex;
};

float sigmoid(float x, float slope) {
	// A graph of this can be found here:
	// https://www.desmos.com/calculator/mqb8farthj
	return (0.5 / ((1 / (1 + exp(-slope)) - 0.5))) *
		(1 / (1 + exp(-(2 * x - 1) * slope)) - 0.5) + 0.5;
}

psIn vs(vsIn input, uint id : SV_InstanceID) {
	psIn o;
	o.view_id = id % sk_view_count;
	id = id / sk_view_count;

	o.world = mul(float4(input.pos.xyz, 1), sk_inst[id].world).xyz;
	o.pos = mul(float4(o.world, 1), sk_viewproj[o.view_id]);
	o.normal = normalize(mul(input.norm, (float3x3)sk_inst[id].world));
	o.view_dir = sk_camera_pos[o.view_id].xyz - o.world;
	o.color = input.col * color * sk_inst[id].color;
	return o;
}
float4 ps(psIn input) : SV_TARGET{
	float  fresnel = dot(normalize(input.view_dir), normalize(input.normal));
	fresnel = sigmoid(fresnel + threshold, slope);

	float4 col = lerp(color, color2, fresnel);
	return col * input.color;
}