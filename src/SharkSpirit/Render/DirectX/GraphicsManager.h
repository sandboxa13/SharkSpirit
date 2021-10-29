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

using namespace Microsoft::WRL;

namespace SharkSpirit 
{
	class graphics_manager
	{
	public:
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
	private:
		UINT m_width;
		UINT m_height;
		ComPtr<ID3D11Device> m_device;
		ComPtr<ID3D11Device1> m_device1;
		ComPtr<ID3D11DeviceContext> m_immediateContext;
		ComPtr<ID3D11DeviceContext1> m_immediateContext1;
		ComPtr<IDXGISwapChain> m_pSwapChain;
		ComPtr<IDXGISwapChain1> m_pSwapChain1;
		ComPtr<ID3D11RenderTargetView> m_pRenderTargetView;
		ComPtr<ID3D11DepthStencilView> m_pDepthStencilView;
		ComPtr<ID3D11Texture2D> m_pDepthStencilBuffer;
		D3D_FEATURE_LEVEL m_featureLevel = D3D_FEATURE_LEVEL_11_0;
		DirectX::XMMATRIX m_projectionMatrix;
		DirectX::XMMATRIX m_cameraMatrix;
		HRESULT initialize(HWND & hwnd);
		void create_and_bind_depth_buffer(UINT & width, UINT & height);
		void create_and_bind_view_port(UINT & width, UINT & height) const;
	};
}