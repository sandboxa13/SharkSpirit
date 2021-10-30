#pragma once

#include <DirectXMath.h>
#include "Render/DirectX/Texture.h"
#include <Render/DirectX/ConstantBuffer.h>
#include <Render/DirectX/IndexBuffer.h>
#include <Render/DirectX/VertexBuffer.h>
#include <Render/DirectX/GraphicsManager.h>
#include <string>
#include <Render/DirectX/Shaders.h>

namespace SharkSpirit
{
	class sprite_component
	{
	public:
		sprite_component(
			graphics_manager* graphicsManager, 
			std::string spritePath) 
		{
			m_texture = new Texture(graphicsManager->get_device().Get(), graphicsManager->get_device_context().Get(), spritePath);

			D3D11_SAMPLER_DESC samplerDesc = {};
			samplerDesc.Filter = D3D11_FILTER_MIN_MAG_MIP_LINEAR;
			samplerDesc.AddressU = D3D11_TEXTURE_ADDRESS_WRAP;
			samplerDesc.AddressV = D3D11_TEXTURE_ADDRESS_WRAP;
			samplerDesc.AddressW = D3D11_TEXTURE_ADDRESS_WRAP;

			graphicsManager->get_device()->CreateSamplerState(&samplerDesc, &m_pSampler);

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

			HRESULT hr = vertices.Initialize(graphicsManager->get_device().Get(), vertexData.data(), vertexData.size());

			hr = indices.Initialize(graphicsManager->get_device().Get(), indexData.data(), indexData.size());

			vertexData.clear();
			indexData.clear();

			//2d shaders
			D3D11_INPUT_ELEMENT_DESC layout2D[] =
			{
				{"POSITION", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
				{"TEXCOORD", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
			};

			UINT numElements2D = ARRAYSIZE(layout2D);

			if (!vertexshader_2d.Initialize(graphicsManager->get_device().Get(), L"C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\vs_2d.cso", layout2D, numElements2D)) 
			{

			}

			if (!pixelshader_2d.Initialize(graphicsManager->get_device().Get(), L"C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\ps_2d.cso"))
			{

			}
			cb_vs_vertexshader_2d = new constant_buffer<constant_buffer_2d>();

			cb_vs_vertexshader_2d->Initialize(graphicsManager->get_device().Get(), graphicsManager->get_device_context().Get());
		}

		Texture* m_texture;
		constant_buffer<constant_buffer_2d>* cb_vs_vertexshader_2d = nullptr;
		DirectX::XMMATRIX worldMatrix = DirectX::XMMatrixIdentity();
		ComPtr<ID3D11SamplerState> m_pSampler;

		index_buffer indices;
		vertex_buffer<vertex_2d> vertices;
		VertexShader vertexshader_2d;
		PixelShader pixelshader_2d;
	};

	struct transform_component
	{
		transform_component(
			DirectX::XMFLOAT3 pos, 
			DirectX::XMFLOAT3 rot, 
			DirectX::XMFLOAT2 scale) 
			: m_pos(pos), m_rotation(rot), m_scale(scale)
		{

		}

		DirectX::XMFLOAT3 m_pos;
		DirectX::XMFLOAT3 m_rotation;
		DirectX::XMFLOAT2 m_scale;
	};
}