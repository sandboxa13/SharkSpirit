#pragma once
#include <directxcolors.h>
#include <directxmath.h>
#include <d3d11_1.h>

namespace shark_spirit::render
{
	class render_target
	{

	};

	class device
	{
	public:
		// common
		void clear(render_target* renderTarget, DirectX::XMVECTORF32 color);
		void set_view_port(UINT& width, UINT& height);
		void draw_indexed(UINT indexCount);

		// pixel shader
		void ps_set(ID3D11PixelShader* pixelShader);
		void ps_set_samplers(UINT startSlot, UINT num, ID3D11SamplerState* sampler);
		void ps_set_shader_resources(UINT startSLot, UINT num, ID3D11ShaderResourceView* resource);
		void ps_set_shader_resources(UINT startSLot, UINT num, ID3D11ShaderResourceView** resources);

		// vertex shader
		void vs_set(ID3D11VertexShader* pixelShader);


		// output merger
		void om_set_render_targets(int number, render_target* renderTarget);
		

		// input assembler
		void ia_set_input_layout(ID3D11InputLayout* layout);
		void ia_set_vertex_buffer(UINT startSLot, UINT num, ID3D11Buffer* buffer, const UINT* stride, const UINT* offset);
		void ia_set_index_buffer(ID3D11Buffer* bufer, DXGI_FORMAT format, UINT offset);
	};
}