#include "Shaders.h"

namespace SharkSpirit
{

	HRESULT vertex_shader::Initialize(ID3D11Device* device, std::wstring shaderpath, D3D11_INPUT_ELEMENT_DESC* layoutDesc, UINT numElements)
	{
		HRESULT hr = D3DReadFileToBlob(shaderpath.c_str(), this->shader_buffer.GetAddressOf());

		shaderpath.clear();
		if (FAILED(hr))
		{
			return hr;
		}

		hr = device->CreateVertexShader(this->shader_buffer->GetBufferPointer(), this->shader_buffer->GetBufferSize(), NULL, this->shader.GetAddressOf());
		if (FAILED(hr))
		{
			return hr;
		}

		hr = device->CreateInputLayout(layoutDesc, numElements, this->shader_buffer->GetBufferPointer(), this->shader_buffer->GetBufferSize(), this->inputLayout.GetAddressOf());
		if (FAILED(hr))
		{
			return hr;
		}

		return hr;
	}

	ID3D11VertexShader* vertex_shader::GetShader()
	{
		return this->shader.Get();
	}

	ID3D10Blob* vertex_shader::GetBuffer()
	{
		return this->shader_buffer.Get();
	}

	ID3D11InputLayout* vertex_shader::GetInputLayout()
	{
		return this->inputLayout.Get();
	}

	HRESULT pixel_shader::Initialize(ID3D11Device* device, std::wstring shaderpath)
	{
		HRESULT hr = D3DReadFileToBlob(shaderpath.c_str(), this->shader_buffer.GetAddressOf());

		shaderpath.clear();

		if (FAILED(hr))
		{
			return hr;
		}

		hr = device->CreatePixelShader(this->shader_buffer.Get()->GetBufferPointer(), this->shader_buffer.Get()->GetBufferSize(), NULL, this->shader.GetAddressOf());
		if (FAILED(hr))
		{
			return hr;
		}

		return hr;
	}

	ID3D11PixelShader* pixel_shader::GetShader()
	{
		return this->shader.Get();
	}

	ID3D10Blob* pixel_shader::GetBuffer()
	{
		return this->shader_buffer.Get();
	}
}