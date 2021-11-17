#include "Device.h"
#include <Core/GraphicsThrowMacros.h>

#pragma comment (lib, "D3D11.lib")
#pragma comment (lib, "d3dcompiler.lib")

namespace sharkspirit::render
{
	#pragma region Common

	void device::clear(render_target* renderTarget, DirectX::XMVECTORF32 color)
	{
		m_immediate_context->ClearRenderTargetView(renderTarget->m_render_target_view.Get(), color);
	}

	void device::set_view_port(UINT& width, UINT& height)
	{
		m_width = width;
		m_height = height;

		D3D11_VIEWPORT vp;
		vp.Width = static_cast<FLOAT>(m_width);
		vp.Height = static_cast<FLOAT>(m_height);
		vp.MinDepth = 0.0f;
		vp.MaxDepth = 1.0f;
		vp.TopLeftX = 0;
		vp.TopLeftY = 0;
		m_immediate_context->RSSetViewports(1, &vp);
	}

	void device::draw_indexed(UINT indexCount)
	{
		m_immediate_context->DrawIndexed(indexCount, 0, 0);
	}

	render_target* device::create_render_target(render_target_desc* desc)
	{
		HRESULT hr;
		auto renderTarget = new render_target();

		D3D11_TEXTURE2D_DESC textureDesc = {};

		textureDesc.Width = desc->m_width;
		textureDesc.Height = desc->m_height;
		textureDesc.MipLevels = desc->m_mip_levels;
		textureDesc.ArraySize = desc->m_array_size;
		textureDesc.Format = desc->m_format;
		textureDesc.SampleDesc.Count = desc->m_sample_desc.Count;
		textureDesc.SampleDesc.Quality = desc->m_sample_desc.Quality;
		textureDesc.Usage = D3D11_USAGE_DEFAULT;
		textureDesc.BindFlags = D3D11_BIND_RENDER_TARGET | D3D11_BIND_SHADER_RESOURCE;

		ID3D11Texture2D* texture = 0;
		GFX_THROW_INFO(m_device.Get()->CreateTexture2D(&textureDesc, nullptr, &texture));

		D3D11_RENDER_TARGET_VIEW_DESC rtvDesc;
		rtvDesc.Format = textureDesc.Format;
		rtvDesc.ViewDimension = D3D11_RTV_DIMENSION_TEXTURE2D;
		rtvDesc.Texture2DArray.MipSlice = 0;
		GFX_THROW_INFO(m_device.Get()->CreateRenderTargetView(texture, &rtvDesc, &renderTarget->m_render_target_view));

		D3D11_SHADER_RESOURCE_VIEW_DESC srvDesc;
		srvDesc.Format = textureDesc.Format;
		srvDesc.ViewDimension = D3D11_SRV_DIMENSION_TEXTURE2D;
		srvDesc.Texture2DArray.MipLevels = -1;
		srvDesc.Texture2DArray.MostDetailedMip = 0;
		GFX_THROW_INFO(m_device.Get()->CreateShaderResourceView(texture, &srvDesc, &renderTarget->m_shader_resource_view));

		texture->Release();

		m_render_targets.push_back(renderTarget);

		return renderTarget;
	}

	render_target* device::create_swap_chain_render_target()
	{
		HRESULT hr;
		auto renderTarget = new render_target();

		ID3D11Texture2D* pBackBuffer = nullptr;
		GFX_THROW_INFO(m_swap_chain.Get()->GetBuffer(0, __uuidof(ID3D11Texture2D), reinterpret_cast<void**>(&pBackBuffer)));

		auto device = m_device.Get();

		GFX_THROW_INFO(device->CreateRenderTargetView(pBackBuffer, nullptr, &renderTarget->m_render_target_view));

		pBackBuffer->Release();

		m_render_targets.push_back(renderTarget);

		return renderTarget;
	}

	ComPtr<ID3D11BlendState> device::create_blend_sate()
	{
		HRESULT hr;
		ComPtr<ID3D11BlendState> blendState = {0};

		D3D11_BLEND_DESC desc = { 0 };
		desc.RenderTarget[0].BlendEnable = TRUE;
		desc.RenderTarget[0].RenderTargetWriteMask = D3D11_COLOR_WRITE_ENABLE_ALL;
		desc.RenderTarget[0].BlendOp = D3D11_BLEND_OP_ADD;
		desc.RenderTarget[0].SrcBlend = D3D11_BLEND_SRC_COLOR;
		desc.RenderTarget[0].DestBlend = D3D11_BLEND_INV_SRC1_COLOR;
		desc.RenderTarget[0].BlendOpAlpha = D3D11_BLEND_OP_MIN;
		desc.RenderTarget[0].DestBlendAlpha = D3D11_BLEND_ZERO;
		desc.RenderTarget[0].SrcBlendAlpha = D3D11_BLEND_ONE;

		GFX_THROW_INFO(m_device->CreateBlendState(&desc, &blendState));

		return blendState;
	}

	Microsoft::WRL::ComPtr<ID3D11Device> device::get_device()
	{
		return m_device;
	}

	Microsoft::WRL::ComPtr<ID3D11DeviceContext> device::get_device_context()
	{
		return m_immediate_context;
	}

	#pragma endregion

	#pragma region Pixel Shader

	void device::ps_set(ID3D11PixelShader* pixelShader)
	{
		m_immediate_context->PSSetShader(pixelShader, NULL, 0);
	}

	void device::ps_set_samplers(UINT startSlot, UINT num, ID3D11SamplerState* const* sampler)
	{
		m_immediate_context->PSSetSamplers(startSlot, num, sampler);
	}

	void device::ps_set_shader_resources(UINT startSLot, UINT num, ID3D11ShaderResourceView* const* resources)
	{
		m_immediate_context->PSSetShaderResources(startSLot, num, resources);
	}

	#pragma endregion

	#pragma region Vertex Shader

	void device::vs_set(ID3D11VertexShader* vertexShader)
	{
		m_immediate_context->VSSetShader(vertexShader, NULL, 0);
	}

	void device::vs_set_constant_buffers(UINT StartSlot, UINT NumBuffers, ID3D11Buffer* const* ppConstantBuffers)
	{
		m_immediate_context->VSSetConstantBuffers(StartSlot, NumBuffers, ppConstantBuffers);
	}

	#pragma endregion

	#pragma region Output Merger

	void device::om_set_render_targets(int number, render_target* renderTarget)
	{
		m_immediate_context->OMSetRenderTargets(number, renderTarget->m_render_target_view.GetAddressOf(), nullptr);
	}

	void device::om_set_blend_state(ID3D11BlendState* state, const FLOAT blendFactor[4], UINT mask)
	{
		m_immediate_context->OMSetBlendState(state, blendFactor, mask);
	}

	#pragma endregion

	#pragma region Input Assembler

	void device::ia_set_input_layout(ID3D11InputLayout* layout)
	{
		m_immediate_context->IASetInputLayout(layout);
	}

	void device::ia_set_vertex_buffer(UINT startSLot, UINT num, ID3D11Buffer* const* buffer, const UINT* stride, const UINT* offset)
	{
		m_immediate_context->IASetVertexBuffers(startSLot, num, buffer, stride, offset);
	}

	void device::ia_set_index_buffer(ID3D11Buffer* bufer, DXGI_FORMAT format, UINT offset)
	{
		m_immediate_context->IASetIndexBuffer(bufer, format, offset);
	}

	
	#pragma endregion

	HRESULT device::initialize(HWND& hwnd)
	{
		HRESULT hr = { 0 };

		RECT rect;

		GetClientRect(hwnd, &rect);

		UINT width = rect.right - rect.left;
		UINT height = rect.bottom - rect.top;

		this->m_width = width;
		this->m_height = height;

		UINT createDeviceFlags = 0;
#if defined(_DEBUG)
		// If the project is in a debug build, enable the debug layer.
		createDeviceFlags |= D3D11_CREATE_DEVICE_DEBUG;
#endif

		D3D_DRIVER_TYPE driverTypes[] =
		{
			D3D_DRIVER_TYPE_HARDWARE,
			D3D_DRIVER_TYPE_WARP,
			D3D_DRIVER_TYPE_REFERENCE,
		};
		const UINT numDriverTypes = ARRAYSIZE(driverTypes);

		D3D_FEATURE_LEVEL featureLevels[] =
		{
			D3D_FEATURE_LEVEL_11_1,
			D3D_FEATURE_LEVEL_11_0,
			D3D_FEATURE_LEVEL_10_1,
			D3D_FEATURE_LEVEL_10_0,
		};
		const UINT numFeatureLevels = ARRAYSIZE(featureLevels);

		for (UINT driverTypeIndex = 0; driverTypeIndex < numDriverTypes; driverTypeIndex++)
		{
			const auto driver_type = driverTypes[driverTypeIndex];

			GFX_THROW_INFO(D3D11CreateDevice(nullptr, driver_type, nullptr, createDeviceFlags, featureLevels, numFeatureLevels,
				D3D11_SDK_VERSION, &m_device, &m_featureLevel, &m_immediate_context));

			if (hr == E_INVALIDARG)
			{
				// DirectX 11.0 platforms will not recognize D3D_FEATURE_LEVEL_11_1 so we need to retry without it
				GFX_THROW_INFO(D3D11CreateDevice(nullptr, driver_type, nullptr, createDeviceFlags, &featureLevels[1], numFeatureLevels - 1,
					D3D11_SDK_VERSION, &m_device, &m_featureLevel, &m_immediate_context));
			}

			if (SUCCEEDED(hr))
				break;
		}

		// Obtain DXGI factory from device (since we used nullptr for pAdapter above)
		IDXGIFactory1* dxgiFactory = nullptr;
		{
			IDXGIDevice* dxgiDevice = nullptr;
			GFX_THROW_INFO(m_device.Get()->QueryInterface(__uuidof(IDXGIDevice), reinterpret_cast<void**>(&dxgiDevice)));
			if (SUCCEEDED(hr))
			{
				IDXGIAdapter* adapter = nullptr;
				GFX_THROW_INFO(dxgiDevice->GetAdapter(&adapter));
				if (SUCCEEDED(hr))
				{
					GFX_THROW_INFO(adapter->GetParent(__uuidof(IDXGIFactory1), reinterpret_cast<void**>(&dxgiFactory)));
					adapter->Release();
				}
				dxgiDevice->Release();
			}
		}


		IDXGIFactory2* dxgiFactory2 = nullptr;
		GFX_THROW_INFO(dxgiFactory->QueryInterface(__uuidof(IDXGIFactory2), reinterpret_cast<void**>(&dxgiFactory2)));
		if (dxgiFactory2)
		{
			// DirectX 11.1 or later
			GFX_THROW_INFO(m_device.Get()->QueryInterface(__uuidof(ID3D11Device1), (&m_device1)));
			if (SUCCEEDED(hr))
			{
				GFX_THROW_INFO(m_immediate_context.Get()->QueryInterface(__uuidof(ID3D11DeviceContext1), (&m_immediate_context1)));
			}

			DXGI_SWAP_CHAIN_DESC1 sd = {};
			sd.Width = width;
			sd.Height = height;
			sd.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
			sd.SampleDesc.Count = 1;
			sd.SampleDesc.Quality = 0;
			sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT ;
			sd.BufferCount = 1;

			GFX_THROW_INFO(dxgiFactory2->CreateSwapChainForHwnd(m_device.Get(), hwnd, &sd, nullptr, nullptr, &m_swap_chain1));
			if (SUCCEEDED(hr))
			{
				GFX_THROW_INFO(m_swap_chain1->QueryInterface(__uuidof(IDXGISwapChain), (&m_swap_chain)));
			}

			dxgiFactory2->Release();
		}
		else
		{
			// DirectX 11.0 systems
			DXGI_SWAP_CHAIN_DESC sd = {};
			sd.BufferCount = 1;
			sd.BufferDesc.Width = width;
			sd.BufferDesc.Height = height;
			sd.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
			sd.BufferDesc.RefreshRate.Numerator = 60;
			sd.BufferDesc.RefreshRate.Denominator = 1;
			sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
			sd.OutputWindow = hwnd;
			sd.SampleDesc.Count = 1;
			sd.SampleDesc.Quality = 0;
			sd.Windowed = TRUE;

			GFX_THROW_INFO(dxgiFactory->CreateSwapChain(m_device.Get(), &sd, &(m_swap_chain)));
		}

		dxgiFactory->Release();

		set_view_port(width, height);

		m_immediate_context->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY::D3D10_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

		return hr;
	}

	void device::present()
	{
		HRESULT hr;
#ifdef _DEBUG
		m_info_manager.Set();
#endif
		ImGui::Render();
		ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());


		if (FAILED(hr = m_swap_chain->Present(0u, 0u)))
		{
			if (hr == DXGI_ERROR_DEVICE_REMOVED)
			{
				throw GFX_DEVICE_REMOVED_EXCEPT(m_device->GetDeviceRemovedReason());
			}
			else
			{
				throw GFX_EXCEPT(hr);
			}
		}
	}

	#pragma region Execptions

	device::HrException::HrException(int line, const char* file, HRESULT hr, std::vector<std::string> infoMsgs) noexcept
		:
		Exception(line, file),
		hr(hr)
	{
		// join all info messages with newlines into single string
		for (const auto& m : infoMsgs)
		{
			info += m;
			info.push_back('\n');
		}
		// remove final newline if exists
		if (!info.empty())
		{
			info.pop_back();
		}
	}

	const char* device::HrException::what() const noexcept
	{
		std::ostringstream oss;
		oss << GetType() << std::endl
			<< "[Error Code] 0x" << std::hex << std::uppercase << GetErrorCode()
			<< std::dec << " (" << static_cast<unsigned long>(GetErrorCode()) << ")" << std::endl
			<< "[Error String] " << GetErrorString() << std::endl
			<< "[Description] " << GetErrorDescription() << std::endl;
		if (!info.empty())
		{
			oss << "\n[Error Info]\n" << GetErrorInfo() << std::endl << std::endl;
		}
		oss << GetOriginString();
		whatBuffer = oss.str();
		return whatBuffer.c_str();
	}

	const char* device::HrException::GetType() const noexcept
	{
		return "Shark Spirit Graphics Exception";
	}

	HRESULT device::HrException::GetErrorCode() const noexcept
	{
		return hr;
	}

	std::string device::HrException::GetErrorString() const noexcept
	{
		return DXGetErrorString(hr);
	}

	std::string device::HrException::GetErrorDescription() const noexcept
	{
		char buf[512];
		DXGetErrorDescription(hr, buf, sizeof(buf));
		return buf;
	}

	std::string device::HrException::GetErrorInfo() const noexcept
	{
		return info;
	}


	const char* device::DeviceRemovedException::GetType() const noexcept
	{
		return "Shark Spirit Graphics Exception [Device Removed] (DXGI_ERROR_DEVICE_REMOVED)";
	}
	device::InfoException::InfoException(int line, const char* file, std::vector<std::string> infoMsgs) noexcept
		:
		Exception(line, file)
	{
		// join all info messages with newlines into single string
		for (const auto& m : infoMsgs)
		{
			info += m;
			info.push_back('\n');
		}
		// remove final newline if exists
		if (!info.empty())
		{
			info.pop_back();
		}
	}


	const char* device::InfoException::what() const noexcept
	{
		std::ostringstream oss;
		oss << GetType() << std::endl
			<< "\n[Error Info]\n" << GetErrorInfo() << std::endl << std::endl;
		oss << GetOriginString();
		whatBuffer = oss.str();
		return whatBuffer.c_str();
	}

	const char* device::InfoException::GetType() const noexcept
	{
		return "Graphics Info Exception";
	}

	std::string device::InfoException::GetErrorInfo() const noexcept
	{
		return info;
	}

#pragma endregion
}