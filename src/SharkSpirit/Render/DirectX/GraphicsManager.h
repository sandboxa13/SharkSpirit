#pragma once
#include <d3d11_1.h>
#include <wrl.h>
#include <directxcolors.h>
#include <string>
#include <d3dcompiler.h>
#include <directxmath.h>
#include <vector>
#include <sstream>

#include "Utils/Math.h"
#include <Core/SSException.h>
#include <Core/DxgiInfoManager.h>
#include <Core/DxErr/dxerr.h>
#include <Core/GraphicsThrowMacros.h>
#include <Render/DirectX/Camera2d.h>
#include <Render/DirectX/ConstantBuffer.h>
#include <Render/DirectX/IndexBuffer.h>
#include <Render/DirectX/VertexBuffer.h>
#include "ImGui/imgui.h"
#include "ImGui/imgui_impl_win32.h"
#include "ImGui/imgui_impl_dx11.h"
#include <Render/DirectX/Shaders.h>

using namespace Microsoft::WRL;

namespace SharkSpirit 
{
	class graphics_manager
	{
	public:
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

		void clear_rt() const;
		void present();
		graphics_manager(HWND hwnd);
		virtual ~graphics_manager() = default;
		ComPtr<ID3D11Device> get_device() const;
		ComPtr<ID3D11DeviceContext> get_device_context() const;
		DirectX::XMMATRIX get_projection_matrix() const;
		void set_projection_matrix();
		DirectX::XMMATRIX get_camera_matrix() const;
		void set_camera_matrix(DirectX::XMMATRIX cameraMatrix);
		void resize(UINT width, UINT height);
		float get_aspect_ratio() const;
		void draw_indexed(int count);
		void clear_light_rt() const;
		void clear_color_rt() const;
		void clear_context() const;
		void ps_set_shader_resources(UINT StartSlot, UINT NumViews, ID3D11ShaderResourceView* const* ppShaderResourceViews);
		void vs_set_constant_buffers(UINT StartSlot, UINT NumBuffers, ID3D11Buffer* const* ppConstantBuffers);

		camera_2d m_camera_2d;

		vertex_buffer<vertex> m_vertices_light_pass;
		vertex_buffer<vertex> m_vertices;
		constant_buffer<constant_buffer_2d>* m_cb_vs_vertexshader_2d;
		index_buffer m_indices;
		index_buffer m_indicies_light_pass;
		ComPtr<ID3D11DepthStencilView> m_pDepthStencilView;
		vertex_shader m_vertex_shader;
		pixel_shader m_pixel_shader;
		ID3D11InputLayout* m_layout;
		ID3D11SamplerState* m_sampleState;

		UINT m_width;
		UINT m_height;
		ComPtr<ID3D11Device> m_device;
		ComPtr<ID3D11Device1> m_device1;
		ComPtr<ID3D11DeviceContext> m_immediateContext;
		ComPtr<ID3D11DeviceContext1> m_immediateContext1;
		ComPtr<IDXGISwapChain> m_pSwapChain;
		ComPtr<IDXGISwapChain1> m_pSwapChain1;
		ComPtr<ID3D11RenderTargetView> m_pRenderTargetView;
		ComPtr<ID3D11ShaderResourceView> m_pSRV;
		ComPtr<ID3D11RenderTargetView> m_pLightMapRTV;
		ComPtr<ID3D11ShaderResourceView> m_pLightMapSRV;
		ComPtr<ID3D11RenderTargetView> m_pColorMapRTV;
		ComPtr<ID3D11ShaderResourceView> m_pColorMapSRV;
		ComPtr<ID3D11BlendState> m_blend_state;

		ComPtr<ID3D11Texture2D> m_pDepthStencilBuffer;
		D3D_FEATURE_LEVEL m_featureLevel = D3D_FEATURE_LEVEL_11_0;
		DirectX::XMMATRIX m_projectionMatrix;
		DirectX::XMMATRIX m_cameraMatrix;

	private:
		HRESULT initialize(HWND & hwnd);
		void create_and_bind_depth_buffer(UINT & width, UINT & height);
		void create_and_bind_view_port(UINT & width, UINT & height) const;
		void create_light_map();
		void create_color_map();

#ifdef _DEBUG
		DxgiInfoManager infoManager;
#endif
	};
}