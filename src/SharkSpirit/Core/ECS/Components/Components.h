#pragma once

#include <DirectXMath.h>
#include "Render/DirectX/Texture.h"
#include <Render/DirectX/ConstantBuffer.h>
#include <Render/DirectX/IndexBuffer.h>
#include <Render/DirectX/VertexBuffer.h>
#include <Render/DirectX/GraphicsManager.h>
#include <Render/DirectX/Shaders.h>
#include <Render/DirectX/Sampler.h>

namespace SharkSpirit
{
	class base_component 
	{
	public:
		virtual ~base_component()
		{

		}
	};
	class base_render_component : public base_component
	{
	public:
		virtual ~base_render_component()
		{

		}
		sampler* m_sampler;
		Texture* m_texture;
		index_buffer m_indices;
		vertex_shader m_vertex_shader;
		pixel_shader m_pixel_shader;
		DirectX::XMMATRIX m_world_matrix = DirectX::XMMatrixIdentity();
	};

	class sprite_component_create_info
	{
	public :
		sprite_component_create_info(
			const std::string& textureName,
			const std::wstring& pixelShaderPath,
			const std::wstring& vertexShaderPath
			) : 
			m_pixel_shader_path(pixelShaderPath),
			m_vertex_shader_path(vertexShaderPath),
			m_texture_name(textureName)
		{

		}
		const std::string& m_texture_name;
		const std::wstring& m_pixel_shader_path;
		const std::wstring& m_vertex_shader_path;
	};
	class sprite_component : public base_render_component
	{
	public:
		sprite_component(
			assets_manager* assetsManager,
			graphics_manager* graphicsManager, 
			sprite_component_create_info* createInfo)
		{
			HRESULT hr = { 0 };

			m_texture = assetsManager->get_texture(createInfo->m_texture_name);
			m_sampler = new sampler(graphicsManager);

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

			hr = vertices.Initialize(graphicsManager->get_device().Get(), vertexData.data(), vertexData.size());

			hr = m_indices.Initialize(graphicsManager->get_device().Get(), indexData.data(), indexData.size());

			vertexData.clear();
			indexData.clear();

			//2d shaders
			D3D11_INPUT_ELEMENT_DESC layout2D[] =
			{
				{"POSITION", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
				{"TEXCOORD", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
			};

			UINT numElements2D = ARRAYSIZE(layout2D);

			hr = m_vertex_shader.Initialize(graphicsManager->get_device().Get(), createInfo->m_vertex_shader_path, layout2D, numElements2D);
			hr = m_pixel_shader.Initialize(graphicsManager->get_device().Get(), createInfo->m_pixel_shader_path);
			
			cb_vs_vertexshader_2d = new constant_buffer<constant_buffer_2d>();
			hr = cb_vs_vertexshader_2d->Initialize(graphicsManager->get_device().Get(), graphicsManager->get_device_context().Get());
		}

		virtual ~sprite_component()
		{

		}

		vertex_buffer<vertex_2d> vertices;
		constant_buffer<constant_buffer_2d>* cb_vs_vertexshader_2d = nullptr;
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