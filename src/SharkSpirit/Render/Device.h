#pragma once
#include <directxcolors.h>
#include <directxmath.h>
#include <d3d11_1.h>
#include <wrl.h>
#include <d3dcompiler.h>
#include <vector>
#include "ImGui/imgui.h"
#include "ImGui/imgui_impl_win32.h"
#include "ImGui/imgui_impl_dx11.h"

#include <Core/DxgiInfoManager.h>

namespace shark_spirit::render
{
	struct render_target_desc
	{
		UINT m_width;
		UINT m_height;
		UINT m_mip_levels;
		UINT m_array_size;
		DXGI_FORMAT m_format;
		DXGI_SAMPLE_DESC m_sample_desc;
		D3D11_USAGE m_usage;
	};
	struct render_target
	{
	public:
		Microsoft::WRL::ComPtr<ID3D11RenderTargetView> m_render_target_view;
		Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_shader_resource_view;
	};

	class device
	{
	public:
		#pragma region Exceptions

		class Exception : public SSException
		{
			using SSException::SSException;
		};
		class HrException : public Exception
		{
		public:
			HrException(int line, const char* file, HRESULT hr, std::vector<std::string> infoMsgs = {}) noexcept;
			const char* what() const noexcept override;
			const char* GetType() const noexcept override;
			HRESULT GetErrorCode() const noexcept;
			std::string GetErrorString() const noexcept;
			std::string GetErrorDescription() const noexcept;
			std::string GetErrorInfo() const noexcept;
		private:
			HRESULT hr;
			std::string info;
		};
		class InfoException : public Exception
		{
		public:
			InfoException(int line, const char* file, std::vector<std::string> infoMsgs) noexcept;
			const char* what() const noexcept override;
			const char* GetType() const noexcept override;
			std::string GetErrorInfo() const noexcept;
		private:
			std::string info;
		};
		class DeviceRemovedException : public HrException
		{
			using HrException::HrException;
		public:
			const char* GetType() const noexcept override;
		private:
			std::string reason;
		};

#pragma endregion

		// common
		void clear(render_target* renderTarget, DirectX::XMVECTORF32 color);
		void set_view_port(UINT& width, UINT& height);
		void draw_indexed(UINT indexCount);
		render_target* create_render_target(render_target_desc* desc);
		render_target* create_swap_chain_render_target();
		Microsoft::WRL::ComPtr<ID3D11BlendState> create_blend_sate();

		// pixel shader
		void ps_set(ID3D11PixelShader* pixelShader);
		void ps_set_samplers(UINT startSlot, UINT num, ID3D11SamplerState* const* sampler);
		void ps_set_shader_resources(UINT startSLot, UINT num, ID3D11ShaderResourceView* const* resources);

		// vertex shader
		void vs_set(ID3D11VertexShader* pixelShader);
		void vs_set_constant_buffers(UINT StartSlot, UINT NumBuffers, ID3D11Buffer* const* ppConstantBuffers);

		// output merger
		void om_set_render_targets(int number, render_target* renderTarget);
		void om_set_blend_state(ID3D11BlendState* state, const FLOAT blendFactor[4], UINT mask);

		// input assembler
		void ia_set_input_layout(ID3D11InputLayout* layout);
		void ia_set_vertex_buffer(UINT startSLot, UINT num, ID3D11Buffer* const* buffer, const UINT* stride, const UINT* offset);
		void ia_set_index_buffer(ID3D11Buffer* bufer, DXGI_FORMAT format, UINT offset);


		void present();
	private:
		Microsoft::WRL::ComPtr<ID3D11Device> m_device;
		Microsoft::WRL::ComPtr<ID3D11DeviceContext> m_immediate_context;
		Microsoft::WRL::ComPtr<IDXGISwapChain> m_swap_chain;
		UINT m_width;
		UINT m_height;
		std::vector<render_target> m_render_targets;
		D3D_FEATURE_LEVEL m_featureLevel = D3D_FEATURE_LEVEL_11_0;


		HRESULT initialize(HWND& hwnd);

#ifdef _DEBUG
		DxgiInfoManager m_info_manager;
#endif
	};
}