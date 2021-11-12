#include "GraphicsManager.h"


#pragma comment (lib, "D3D11.lib")
#pragma comment (lib, "d3dcompiler.lib")

namespace SharkSpirit
{
	graphics_manager::graphics_manager(HWND hwnd)
	{
		initialize(hwnd);
	}

	void graphics_manager::create_and_bind_view_port(UINT& width, UINT& height) const
	{
		D3D11_VIEWPORT vp;
		vp.Width = static_cast<FLOAT>(width);
		vp.Height = static_cast<FLOAT>(height);
		vp.MinDepth = 0.0f;
		vp.MaxDepth = 1.0f;
		vp.TopLeftX = 0;
		vp.TopLeftY = 0;
		m_immediateContext.Get()->RSSetViewports(1, &vp);
	}

	void graphics_manager::create_color_map()
	{
		HRESULT hr;

		D3D11_TEXTURE2D_DESC descLight = {};

		descLight.Width = m_width;
		descLight.Height = m_height;
		descLight.MipLevels = 1u;
		descLight.ArraySize = 1u;
		descLight.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
		descLight.SampleDesc.Count = 1u;
		descLight.SampleDesc.Quality = 0u;
		descLight.Usage = D3D11_USAGE_DEFAULT;
		descLight.BindFlags = D3D11_BIND_RENDER_TARGET | D3D11_BIND_SHADER_RESOURCE;

		ID3D11Texture2D* texture = 0;
		GFX_THROW_INFO(m_device.Get()->CreateTexture2D(&descLight, nullptr, &texture));

		ID3D11RenderTargetView* lightMapRTV;

		D3D11_RENDER_TARGET_VIEW_DESC rtvDesc;
		rtvDesc.Format = descLight.Format;
		rtvDesc.ViewDimension = D3D11_RTV_DIMENSION_TEXTURE2D;
		rtvDesc.Texture2DArray.MipSlice = 0;

		GFX_THROW_INFO(m_device.Get()->CreateRenderTargetView(texture, &rtvDesc, &m_pColorMapRTV));

		D3D11_SHADER_RESOURCE_VIEW_DESC srvDesc;
		srvDesc.Format = descLight.Format;
		srvDesc.ViewDimension = D3D11_SRV_DIMENSION_TEXTURE2D;
		srvDesc.Texture2DArray.MipLevels = -1;
		srvDesc.Texture2DArray.MostDetailedMip = 0;

		GFX_THROW_INFO(m_device.Get()->CreateShaderResourceView(texture, &srvDesc, &m_pColorMapSRV));
	}


	void graphics_manager::create_light_map()
	{
		HRESULT hr;

		D3D11_TEXTURE2D_DESC descLight = {};

		descLight.Width = m_width;
		descLight.Height = m_height;
		descLight.MipLevels = 1u;
		descLight.ArraySize = 1u;
		descLight.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
		descLight.SampleDesc.Count = 1u;
		descLight.SampleDesc.Quality = 0u;
		descLight.Usage = D3D11_USAGE_DEFAULT;
		descLight.BindFlags = D3D11_BIND_RENDER_TARGET | D3D11_BIND_SHADER_RESOURCE;

		ID3D11Texture2D* texture = 0;
		GFX_THROW_INFO(m_device.Get()->CreateTexture2D(&descLight, nullptr, &texture));

		ID3D11RenderTargetView* lightMapRTV;

		D3D11_RENDER_TARGET_VIEW_DESC rtvDesc;
		rtvDesc.Format = descLight.Format;
		rtvDesc.ViewDimension = D3D11_RTV_DIMENSION_TEXTURE2D;
		rtvDesc.Texture2DArray.MipSlice = 0;

		GFX_THROW_INFO(m_device.Get()->CreateRenderTargetView(texture, &rtvDesc, &m_pLightMapRTV));

		D3D11_SHADER_RESOURCE_VIEW_DESC srvDesc;
		srvDesc.Format = descLight.Format;
		srvDesc.ViewDimension = D3D11_SRV_DIMENSION_TEXTURE2D;
		srvDesc.Texture2DArray.MipLevels = -1;
		srvDesc.Texture2DArray.MostDetailedMip = 0;

		GFX_THROW_INFO(m_device.Get()->CreateShaderResourceView(texture, &srvDesc, &m_pLightMapSRV));

		D3D11_BLEND_DESC desc = {0};
		desc.RenderTarget[0].BlendEnable = TRUE;
		desc.RenderTarget[0].RenderTargetWriteMask = D3D11_COLOR_WRITE_ENABLE_ALL;
		desc.RenderTarget[0].BlendOp = D3D11_BLEND_OP_ADD;
		desc.RenderTarget[0].SrcBlend = D3D11_BLEND_SRC_COLOR ;
		desc.RenderTarget[0].DestBlend = D3D11_BLEND_INV_SRC1_COLOR;
		desc.RenderTarget[0].BlendOpAlpha = D3D11_BLEND_OP_MIN;
		desc.RenderTarget[0].DestBlendAlpha = D3D11_BLEND_ZERO;
		desc.RenderTarget[0].SrcBlendAlpha = D3D11_BLEND_ONE;

		GFX_THROW_INFO(m_device->CreateBlendState(&desc, &m_blend_state));
	}

	void graphics_manager::create_and_bind_depth_buffer(UINT& width, UINT& height)
	{
		HRESULT hr;

		D3D11_DEPTH_STENCIL_DESC dsDesc{};
		dsDesc.DepthEnable = FALSE;
		dsDesc.DepthWriteMask = D3D11_DEPTH_WRITE_MASK_ALL;
		dsDesc.DepthFunc = D3D11_COMPARISON_LESS;
		dsDesc.StencilEnable = false;
		dsDesc.StencilReadMask = D3D11_DEFAULT_STENCIL_READ_MASK;
		dsDesc.StencilWriteMask = D3D11_DEFAULT_STENCIL_WRITE_MASK;
		dsDesc.FrontFace.StencilDepthFailOp = D3D11_STENCIL_OP_KEEP;
		dsDesc.FrontFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
		dsDesc.FrontFace.StencilPassOp = D3D11_STENCIL_OP_REPLACE;
		dsDesc.FrontFace.StencilFunc = D3D11_COMPARISON_ALWAYS;
		dsDesc.BackFace = dsDesc.FrontFace;

		ComPtr<ID3D11DepthStencilState> pDSState;

		GFX_THROW_INFO(m_device.Get()->CreateDepthStencilState(&dsDesc, &pDSState));
		m_immediateContext.Get()->OMSetDepthStencilState(pDSState.Get(), 1u);

		D3D11_TEXTURE2D_DESC descDepth = {};

		descDepth.Width = width;
		descDepth.Height = height;
		descDepth.MipLevels = 1u;
		descDepth.ArraySize = 1u;
		descDepth.Format = DXGI_FORMAT_D32_FLOAT;
		descDepth.SampleDesc.Count = 1u;
		descDepth.SampleDesc.Quality = 0u;
		descDepth.Usage = D3D11_USAGE_DEFAULT;
		descDepth.BindFlags = D3D11_BIND_DEPTH_STENCIL;

		GFX_THROW_INFO(m_device.Get()->CreateTexture2D(&descDepth, nullptr, m_pDepthStencilBuffer.GetAddressOf()));

		D3D11_DEPTH_STENCIL_VIEW_DESC descDSV = {};
		descDSV.Format = DXGI_FORMAT_D32_FLOAT;
		descDSV.ViewDimension = D3D11_DSV_DIMENSION_TEXTURE2D;
		descDSV.Texture2D.MipSlice = 0u;

		GFX_THROW_INFO(m_device.Get()->CreateDepthStencilView(m_pDepthStencilBuffer.Get(), &descDSV, m_pDepthStencilView.GetAddressOf()));

		m_immediateContext.Get()->OMSetRenderTargets(1, m_pRenderTargetView.GetAddressOf(), m_pDepthStencilView.Get());
	}

	HRESULT graphics_manager::initialize(HWND& hwnd)
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
				D3D11_SDK_VERSION, &m_device, &m_featureLevel, &m_immediateContext));

			if (hr == E_INVALIDARG)
			{
				// DirectX 11.0 platforms will not recognize D3D_FEATURE_LEVEL_11_1 so we need to retry without it
				GFX_THROW_INFO(D3D11CreateDevice(nullptr, driver_type, nullptr, createDeviceFlags, &featureLevels[1], numFeatureLevels - 1,
					D3D11_SDK_VERSION, &m_device, &m_featureLevel, &m_immediateContext));
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
				GFX_THROW_INFO(m_immediateContext.Get()->QueryInterface(__uuidof(ID3D11DeviceContext1), (&m_immediateContext1)));
			}

			DXGI_SWAP_CHAIN_DESC1 sd = {};
			sd.Width = width;
			sd.Height = height;
			sd.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
			sd.SampleDesc.Count = 1;
			sd.SampleDesc.Quality = 0;
			sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT ;
			sd.BufferCount = 1;

			GFX_THROW_INFO(dxgiFactory2->CreateSwapChainForHwnd(m_device.Get(), hwnd, &sd, nullptr, nullptr, &m_pSwapChain1));
			if (SUCCEEDED(hr))
			{
				GFX_THROW_INFO(m_pSwapChain1->QueryInterface(__uuidof(IDXGISwapChain), (&m_pSwapChain)));
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

			GFX_THROW_INFO(dxgiFactory->CreateSwapChain(m_device.Get(), &sd, &(m_pSwapChain)));
		}

		dxgiFactory->Release();

		// Create a render target view
		ID3D11Texture2D* pBackBuffer = nullptr;
		GFX_THROW_INFO(m_pSwapChain.Get()->GetBuffer(0, __uuidof(ID3D11Texture2D), reinterpret_cast<void**>(&pBackBuffer)));

		auto device = m_device.Get();

		GFX_THROW_INFO(device->CreateRenderTargetView(pBackBuffer, nullptr, m_pRenderTargetView.GetAddressOf()));

	/*	D3D11_SHADER_RESOURCE_VIEW_DESC srvDesc;
		srvDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
		srvDesc.ViewDimension = D3D11_SRV_DIMENSION_TEXTURE2D;
		srvDesc.Texture2DArray.MipLevels = -1;
		srvDesc.Texture2DArray.MostDetailedMip = 0;

		GFX_THROW_INFO(m_device.Get()->CreateShaderResourceView(pBackBuffer, &srvDesc, m_pSRV.GetAddressOf()));*/

		pBackBuffer->Release();

		m_immediateContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY::D3D10_PRIMITIVE_TOPOLOGY_TRIANGLELIST);


		create_and_bind_view_port(width, height);

		create_and_bind_depth_buffer(width, height);

		create_light_map();
		create_color_map();

		set_projection_matrix();
		m_camera_2d.SetProjectionValues(width, height, 0.0f, 1.0f);

		// initialize sprite 

		std::vector<vertex_2d> vertexData =
		{
			vertex_2d(-0.5f, -0.5f, 0.0f, 0.0f, 0.0f), //TopLeft
			vertex_2d(0.5f, -0.5f, 0.0f, 1.0f, 0.0f), //TopRight
			vertex_2d(-0.5, 0.5, 0.0f, 0.0f, 1.0f), //Bottom Left
			vertex_2d(0.5, 0.5, 0.0f, 1.0f, 1.0f), //Bottom Right
		};

		std::vector<DWORD> indexData =
		{
			0, 1, 2,
			2, 1, 3
		};

		hr = m_vertices.Initialize(m_device.Get(), vertexData.data(), vertexData.size());

		hr = m_indices.Initialize(m_device.Get(), indexData.data(), indexData.size());

		

		D3D11_SAMPLER_DESC samplerDesc = {};
		samplerDesc.Filter = D3D11_FILTER_MIN_MAG_MIP_LINEAR;
		samplerDesc.AddressU = D3D11_TEXTURE_ADDRESS_WRAP;
		samplerDesc.AddressV = D3D11_TEXTURE_ADDRESS_WRAP;
		samplerDesc.AddressW = D3D11_TEXTURE_ADDRESS_WRAP;

		m_device.Get()->CreateSamplerState(&samplerDesc, &m_sampleState);

		//2d shaders
		D3D11_INPUT_ELEMENT_DESC layout2D[] =
		{
			{"POSITION", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
			{"TEXCOORD", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
		};

		UINT numElements2D = ARRAYSIZE(layout2D);

		hr = m_vertex_shader.Initialize(m_device.Get(), L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\full_screen_vs.cso", layout2D, numElements2D);
		hr = m_pixel_shader.Initialize(m_device.Get(), L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\2d_light_pass.cso");
		m_cb_vs_vertexshader_2d = new constant_buffer<constant_buffer_2d>();
		hr = m_cb_vs_vertexshader_2d->Initialize(m_device.Get(), m_immediateContext.Get());

		std::vector<vertex_2d> vertices =
		{
			vertex_2d(-1.0f, -1.0f, 0.0f, 0.0f, 1.0f), //TopLeft
			vertex_2d(-1.0f, 1.0f, 0.0f, 0.0f, 0.0f), //TopRight
			vertex_2d(1.0f, 1.0f, 0.0f, 1.0f, 0.0f), //Bottom Left
			vertex_2d(1.0f, -1.0f, 0.0f, 1.0f, 1.0f), //Bottom Right
		};

		std::vector<DWORD> indicies =
		{
			0, 1, 2,
			0, 2, 3
		};

		hr = m_vertices_light_pass.Initialize(m_device.Get(), vertices.data(), vertices.size());
		hr = m_indicies_light_pass.Initialize(m_device.Get(), indicies.data(), indicies.size());
		
		vertices.clear();
		indicies.clear();

		//set_camera_matrix(DirectX::XMMatrixTranslation(0.0f, 0.0f, 20.0f));

		return S_OK;
	}

	ComPtr<ID3D11Device> graphics_manager::get_device() const
	{
		return m_device;
	}

	ComPtr<ID3D11DeviceContext> graphics_manager::get_device_context() const
	{
		return m_immediateContext;
	}

	void graphics_manager::resize(UINT width, UINT height)
	{
		//todo rework this crutch 
		if (width == 0 && height == 0)
			return;

		HRESULT hr = { 0 };

		m_pRenderTargetView->Release();
		m_pDepthStencilBuffer->Release();
		m_pDepthStencilView->Release();

		GFX_THROW_INFO(m_pSwapChain->ResizeBuffers(1, width, height, DXGI_FORMAT_R8G8B8A8_UNORM, 0));
		ID3D11Texture2D* pBackBuffer;

		GFX_THROW_INFO(m_pSwapChain.Get()->GetBuffer(0, __uuidof(ID3D11Texture2D), reinterpret_cast<void**>(&pBackBuffer)));
		if (FAILED(hr))
			return;

		GFX_THROW_INFO(m_device.Get()->CreateRenderTargetView(pBackBuffer, nullptr, m_pRenderTargetView.GetAddressOf()));
		pBackBuffer->Release();
		if (FAILED(hr))
			return;

		create_and_bind_view_port(width, height);

		create_and_bind_depth_buffer(width, height);

		this->m_width = width;
		this->m_height = height;

		set_projection_matrix();
		create_light_map();
		create_color_map();


		m_camera_2d.SetProjectionValues(width, height, 0.0f, 1000.0f);
		//set_camera_matrix(DirectX::XMMatrixTranslation(0.0f, 0.0f, 20.0f));
	}

	void graphics_manager::set_projection_matrix()
	{
		m_projectionMatrix = DirectX::XMMatrixPerspectiveFovLH(0.5f * M_PI, get_aspect_ratio(), 1.0f, 1000.0f);
	}

	float graphics_manager::get_aspect_ratio() const
	{
		return static_cast<float>(m_width) / m_height;
	}

	void graphics_manager::draw_indexed(const int count)
	{
		(m_immediateContext->DrawIndexed(count, 0, 0));
	}

	void graphics_manager::ps_set_shader_resources(UINT StartSlot, UINT NumViews, ID3D11ShaderResourceView* const* ppShaderResourceViews)
	{
		m_immediateContext->PSSetShaderResources(StartSlot, NumViews, ppShaderResourceViews);
	}

	void graphics_manager::vs_set_constant_buffers(UINT StartSlot, UINT NumBuffers, ID3D11Buffer* const* ppConstantBuffers)
	{
		m_immediateContext->VSSetConstantBuffers(StartSlot, NumBuffers, ppConstantBuffers);
	}

	DirectX::XMMATRIX graphics_manager::get_camera_matrix() const
	{
		return m_cameraMatrix;
	}

	void graphics_manager::set_camera_matrix(DirectX::XMMATRIX cameraMatrix)
	{
		m_cameraMatrix = cameraMatrix;
	}

	DirectX::XMMATRIX graphics_manager::get_projection_matrix() const
	{
		return m_projectionMatrix;
	}

	void graphics_manager::clear_rt() const
	{
		m_immediateContext.Get()->ClearRenderTargetView(m_pRenderTargetView.Get(), DirectX::Colors::Black);
		m_immediateContext.Get()->ClearDepthStencilView(m_pDepthStencilView.Get(), D3D11_CLEAR_DEPTH , 1.0f, 0u);
	}

	void graphics_manager::clear_light_rt() const
	{
		m_immediateContext.Get()->ClearRenderTargetView(m_pLightMapRTV.Get(), DirectX::Colors::Black);
	}

	void graphics_manager::clear_color_rt() const
	{
		m_immediateContext.Get()->ClearRenderTargetView(m_pColorMapRTV.Get(), DirectX::Colors::Black);
	}

	void graphics_manager::clear_context() const
	{
		m_immediateContext->ClearState();
		m_immediateContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY::D3D10_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
		D3D11_VIEWPORT vp;
		vp.Width = static_cast<FLOAT>(m_width);
		vp.Height = static_cast<FLOAT>(m_height);
		vp.MinDepth = 0.0f;
		vp.MaxDepth = 1.0f;
		vp.TopLeftX = 0;
		vp.TopLeftY = 0;
		m_immediateContext.Get()->RSSetViewports(1, &vp);
	}

	void graphics_manager::present()
	{
		HRESULT hr;
#ifdef _DEBUG
		infoManager.Set();
#endif
		ImGui::Render();
		ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());


		if (FAILED(hr = m_pSwapChain->Present(0u, 0u)))
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

	graphics_manager::HrException::HrException(int line, const char* file, HRESULT hr, std::vector<std::string> infoMsgs) noexcept
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

	const char* graphics_manager::HrException::what() const noexcept
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

	const char* graphics_manager::HrException::GetType() const noexcept
	{
		return "Shark Spirit Graphics Exception";
	}

	HRESULT graphics_manager::HrException::GetErrorCode() const noexcept
	{
		return hr;
	}

	std::string graphics_manager::HrException::GetErrorString() const noexcept
	{
		return DXGetErrorString(hr);
	}

	std::string graphics_manager::HrException::GetErrorDescription() const noexcept
	{
		char buf[512];
		DXGetErrorDescription(hr, buf, sizeof(buf));
		return buf;
	}

	std::string graphics_manager::HrException::GetErrorInfo() const noexcept
	{
		return info;
	}


	const char* graphics_manager::DeviceRemovedException::GetType() const noexcept
	{
		return "Shark Spirit Graphics Exception [Device Removed] (DXGI_ERROR_DEVICE_REMOVED)";
	}
	graphics_manager::InfoException::InfoException(int line, const char* file, std::vector<std::string> infoMsgs) noexcept
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


	const char* graphics_manager::InfoException::what() const noexcept
	{
		std::ostringstream oss;
		oss << GetType() << std::endl
			<< "\n[Error Info]\n" << GetErrorInfo() << std::endl << std::endl;
		oss << GetOriginString();
		whatBuffer = oss.str();
		return whatBuffer.c_str();
	}

	const char* graphics_manager::InfoException::GetType() const noexcept
	{
		return "Graphics Info Exception";
	}

	std::string graphics_manager::InfoException::GetErrorInfo() const noexcept
	{
		return info;
	}

#pragma endregion
}