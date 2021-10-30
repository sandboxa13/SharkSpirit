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

    void graphics_manager::create_and_bind_depth_buffer(UINT& width, UINT& height)
    {
        HRESULT hr;

        D3D11_DEPTH_STENCIL_DESC dsDesc{};
        dsDesc.DepthEnable = true;
        dsDesc.DepthWriteMask = D3D11_DEPTH_WRITE_MASK_ALL;
        dsDesc.DepthFunc = D3D11_COMPARISON_LESS;

        ComPtr<ID3D11DepthStencilState> pDSState;

        (m_device.Get()->CreateDepthStencilState(&dsDesc, &pDSState));
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

        (m_device.Get()->CreateTexture2D(&descDepth, nullptr, m_pDepthStencilBuffer.GetAddressOf()));

        D3D11_DEPTH_STENCIL_VIEW_DESC descDSV = {};
        descDSV.Format = DXGI_FORMAT_D32_FLOAT;
        descDSV.ViewDimension = D3D11_DSV_DIMENSION_TEXTURE2D;
        descDSV.Texture2D.MipSlice = 0u;

        (m_device.Get()->CreateDepthStencilView(m_pDepthStencilBuffer.Get(), &descDSV, m_pDepthStencilView.GetAddressOf()));

        m_immediateContext.Get()->OMSetRenderTargets(1, m_pRenderTargetView.GetAddressOf(), m_pDepthStencilView.Get());
    }

    HRESULT graphics_manager::initialize(HWND& hwnd)
    {
        HRESULT hr = {0};

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

            (D3D11CreateDevice(nullptr, driver_type, nullptr, createDeviceFlags, featureLevels, numFeatureLevels,
                D3D11_SDK_VERSION, &m_device, &m_featureLevel, &m_immediateContext));

            if (hr == E_INVALIDARG)
            {
                // DirectX 11.0 platforms will not recognize D3D_FEATURE_LEVEL_11_1 so we need to retry without it
                (D3D11CreateDevice(nullptr, driver_type, nullptr, createDeviceFlags, &featureLevels[1], numFeatureLevels - 1,
                    D3D11_SDK_VERSION, &m_device, &m_featureLevel, &m_immediateContext));
            }

            if (SUCCEEDED(hr))
                break;
        }

        // Obtain DXGI factory from device (since we used nullptr for pAdapter above)
        IDXGIFactory1* dxgiFactory = nullptr;
        {
            IDXGIDevice* dxgiDevice = nullptr;
            (m_device.Get()->QueryInterface(__uuidof(IDXGIDevice), reinterpret_cast<void**>(&dxgiDevice)));
            if (SUCCEEDED(hr))
            {
                IDXGIAdapter* adapter = nullptr;
                (dxgiDevice->GetAdapter(&adapter));
                if (SUCCEEDED(hr))
                {
                    (adapter->GetParent(__uuidof(IDXGIFactory1), reinterpret_cast<void**>(&dxgiFactory)));
                    adapter->Release();
                }
                dxgiDevice->Release();
            }
        }


        IDXGIFactory2* dxgiFactory2 = nullptr;
        (dxgiFactory->QueryInterface(__uuidof(IDXGIFactory2), reinterpret_cast<void**>(&dxgiFactory2)));
        if (dxgiFactory2)
        {
            // DirectX 11.1 or later
            (m_device.Get()->QueryInterface(__uuidof(ID3D11Device1), (&m_device1)));
            if (SUCCEEDED(hr))
            {
                (m_immediateContext.Get()->QueryInterface(__uuidof(ID3D11DeviceContext1), (&m_immediateContext1)));
            }

            DXGI_SWAP_CHAIN_DESC1 sd = {};
            sd.Width = width;
            sd.Height = height;
            sd.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
            sd.SampleDesc.Count = 1;
            sd.SampleDesc.Quality = 0;
            sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
            sd.BufferCount = 1;

            (dxgiFactory2->CreateSwapChainForHwnd(m_device.Get(), hwnd, &sd, nullptr, nullptr, &m_pSwapChain1));
            if (SUCCEEDED(hr))
            {
                (m_pSwapChain1->QueryInterface(__uuidof(IDXGISwapChain), (&m_pSwapChain)));
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

            (dxgiFactory->CreateSwapChain(m_device.Get(), &sd, &(m_pSwapChain)));
        }

        dxgiFactory->Release();

        // Create a render target view
        ID3D11Texture2D* pBackBuffer = nullptr;
        (m_pSwapChain.Get()->GetBuffer(0, __uuidof(ID3D11Texture2D), reinterpret_cast<void**>(&pBackBuffer)));

        auto device = m_device.Get();

        (device->CreateRenderTargetView(pBackBuffer, nullptr, m_pRenderTargetView.GetAddressOf()));

        pBackBuffer->Release();

        create_and_bind_view_port(width, height);

        create_and_bind_depth_buffer(width, height);

        set_projection_matrix();
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

        HRESULT hr = {0};

        m_pRenderTargetView->Release();
        m_pDepthStencilBuffer->Release();
        m_pDepthStencilView->Release();

        (m_pSwapChain->ResizeBuffers(1, width, height, DXGI_FORMAT_R8G8B8A8_UNORM, 0));
        ID3D11Texture2D* pBackBuffer;

        (m_pSwapChain.Get()->GetBuffer(0, __uuidof(ID3D11Texture2D), reinterpret_cast<void**>(&pBackBuffer)));
        if (FAILED(hr))
            return;

        (m_device.Get()->CreateRenderTargetView(pBackBuffer, nullptr, m_pRenderTargetView.GetAddressOf()));
        pBackBuffer->Release();
        if (FAILED(hr))
            return;

        create_and_bind_view_port(width, height);

        create_and_bind_depth_buffer(width, height);

        this->m_width = width;
        this->m_height = height;

        set_projection_matrix();
        //set_camera_matrix(DirectX::XMMatrixTranslation(0.0f, 0.0f, 20.0f));

        //IMGUI_CHECKVERSION();
        //ImGui::CreateContext();
        //ImGui::StyleColorsDark();

        //ImGui_ImplDX11_Init(m_device.Get(), m_immediateContext.Get());
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
        m_immediateContext.Get()->ClearRenderTargetView(m_pRenderTargetView.Get(), DirectX::Colors::LightBlue);
        m_immediateContext.Get()->ClearDepthStencilView(m_pDepthStencilView.Get(), D3D11_CLEAR_DEPTH, 1.0f, 0u);
    }

    void graphics_manager::present()
    {
        HRESULT hr;
#ifdef _DEBUG
        //infoManager.Set();
#endif



        /*  ImGui::Render();
          ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());*/


        if (FAILED(hr = m_pSwapChain->Present(0u, 0u)))
        {
            if (hr == DXGI_ERROR_DEVICE_REMOVED)
            {
                throw (m_device->GetDeviceRemovedReason());
            }
            else
            {
                throw (hr);
            }
        }
    }
}