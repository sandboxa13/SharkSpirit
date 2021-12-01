#pragma once
#include <DirectXColors.h>
#include <d3d11.h>

namespace sharkspirit::render
{
	struct constant_buffer_2d
	{
		DirectX::XMMATRIX wvpMatrix;
	};

	struct world_view_proj
	{
		DirectX::XMMATRIX wvpMatrix;
	};

	template<class T>
	class constant_buffer
	{
	private:
		constant_buffer(const constant_buffer<T>& rhs);

	private:

		Microsoft::WRL::ComPtr<ID3D11Buffer> buffer;
		ID3D11DeviceContext* m_deviceContext = nullptr;

	public:
		constant_buffer() {}

		T data;

		ID3D11Buffer* Get()const
		{
			return buffer.Get();
		}

		ID3D11Buffer* const* GetAddressOf()const
		{
			return buffer.GetAddressOf();
		}

		HRESULT Initialize(ID3D11Device* device, ID3D11DeviceContext* deviceContext)
		{
			/*if (buffer.Get() != nullptr)
				buffer.Reset();*/

			m_deviceContext = deviceContext;

			D3D11_BUFFER_DESC desc;
			desc.Usage = D3D11_USAGE_DYNAMIC;
			desc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
			desc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
			desc.MiscFlags = 0;
			desc.ByteWidth = static_cast<UINT>(sizeof(T) + (16 - (sizeof(T) % 16)));
			desc.StructureByteStride = 0;

			HRESULT hr = device->CreateBuffer(&desc, 0, buffer.GetAddressOf());
			return hr;
		}

		bool CopyToGPU()
		{
			D3D11_MAPPED_SUBRESOURCE mappedResource;
			HRESULT hr = this->m_deviceContext->Map(buffer.Get(), 0, D3D11_MAP_WRITE_DISCARD, 0, &mappedResource);
			if (FAILED(hr))
			{
				return false;
			}
			CopyMemory(mappedResource.pData, &data, sizeof(T));
			this->m_deviceContext->Unmap(buffer.Get(), 0);
			return true;
		}
	};
}