#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
float4x4 World; 
float4x4 View;
float4x4 Projection;

float3 CameraPosition;

Texture SkyBoxTexture;
samplerCUBE SkyBoxSampler = sampler_state{
	texture = <SkyBoxTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VertexShaderInput{
	float4 Position : POSITION0;
};
      
struct VertexShaderOutput{
	float4 Position : POSITION0;
	float3 TextureCoordinate : TEXCOORD0;
};
         
VertexShaderOutput MainVS(VertexShaderInput input){
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position,World);
	float4 viewPosition = mul(worldPosition,View);
	output.Position = mul(viewPosition,Projection);
	 
	float4 VertexPosition = mul(input.Position,World);
	output.TextureCoordinate = (float3)VertexPosition - (float3)CameraPosition;
	
	 
	return output;          
}

float4 MainPS(VertexShaderOutput input): COLOR0
{
	float4 color = texCUBE(SkyBoxSampler, normalize(input.TextureCoordinate));
	return color + float4(0.0f,0.0f,0.0f,0.0f);
}

technique Skybox{
	pass Pass1{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 MainPS();
	}
}