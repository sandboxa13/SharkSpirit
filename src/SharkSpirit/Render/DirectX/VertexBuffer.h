#pragma once
#include <DirectXMath.h>

namespace SharkSpirit
{
	struct vertex
	{
		vertex() {}
		vertex(float x, float y, float z, float u, float v)
			: pos(x, y, z), texCoord(u, v) {}

		DirectX::XMFLOAT3 pos;
		DirectX::XMFLOAT2 texCoord;
	};

	template<class T>
	class vertex_buffer
	{
	private:
		Microsoft::WRL::ComPtr<ID3D11Buffer> buffer;
		UINT stride = sizeof(T);
		UINT vertexCount = 0;

	public:
		vertex_buffer() {}

		vertex_buffer(const vertex_buffer<T>& rhs)
		{
			this->buffer = rhs.buffer;
			this->vertexCount = rhs.vertexCount;
			this->stride = rhs.stride;
		}

		vertex_buffer<T>& operator=(const vertex_buffer<T>& a)
		{
			this->buffer = a.buffer;
			this->vertexCount = a.vertexCount;
			this->stride = a.stride;
			return *this;
		}

		ID3D11Buffer* Get()const
		{
			return buffer.Get();
		}

		ID3D11Buffer* const* GetAddressOf()const
		{
			return buffer.GetAddressOf();
		}

		UINT VertexCount() const
		{
			return this->vertexCount;
		}

		const UINT Stride() const
		{
			return this->stride;
		}

		const UINT* StridePtr() const
		{
			return &this->stride;
		}

		HRESULT Initialize(ID3D11Device* device, T* data, UINT vertexCount)
		{
			if (buffer.Get() != nullptr)
				buffer.Reset();

			this->vertexCount = vertexCount;

			D3D11_BUFFER_DESC vertexBufferDesc;
			ZeroMemory(&vertexBufferDesc, sizeof(vertexBufferDesc));

			vertexBufferDesc.Usage = D3D11_USAGE_DEFAULT;
			vertexBufferDesc.ByteWidth = stride * vertexCount;
			vertexBufferDesc.BindFlags = D3D11_BIND_VERTEX_BUFFER;
			vertexBufferDesc.CPUAccessFlags = 0;
			vertexBufferDesc.MiscFlags = 0;

			D3D11_SUBRESOURCE_DATA vertexBufferData;
			ZeroMemory(&vertexBufferData, sizeof(vertexBufferData));
			vertexBufferData.pSysMem = data;

			HRESULT hr = device->CreateBuffer(&vertexBufferDesc, &vertexBufferData, this->buffer.GetAddressOf());
			return hr;
		}
	};
}