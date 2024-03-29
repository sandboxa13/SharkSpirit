#pragma once
#include <d3d11.h>
#include <wrl/client.h>
#include <d3dcompiler.h>
#include <string>

namespace sharkspirit::render
{
	class vertex_shader
	{
	public:
		HRESULT Initialize(ID3D11Device* device, std::wstring shaderpath, D3D11_INPUT_ELEMENT_DESC* layoutDesc, UINT numElements);
		HRESULT InitializeFromBlob(ID3D11Device* device, ID3D10Blob* blob, D3D11_INPUT_ELEMENT_DESC* layoutDesc, UINT numElements);
		ID3D11VertexShader* GetShader();
		ID3D10Blob* GetBuffer();
		ID3D11InputLayout* GetInputLayout();
	private:
		Microsoft::WRL::ComPtr<ID3D11VertexShader> shader;
		Microsoft::WRL::ComPtr<ID3D10Blob> shader_buffer;
		Microsoft::WRL::ComPtr<ID3D11InputLayout> inputLayout;
	};

	class pixel_shader
	{
	public:
		HRESULT Initialize(ID3D11Device* device, std::wstring shaderpath);
		HRESULT InitializeFromBlob(ID3D11Device* device, ID3D10Blob* blob);
		ID3D11PixelShader* GetShader();
		ID3D10Blob* GetBuffer();
	private:
		Microsoft::WRL::ComPtr<ID3D11PixelShader> shader;
		Microsoft::WRL::ComPtr<ID3D10Blob> shader_buffer;
	};
}