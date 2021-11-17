#include "AssetsManager.h"
#include <system_error>

namespace sharkspirit::assets
{
	void assets_manager::initialize_default_shaders(sharkspirit::render::device* device)
	{
		using namespace sharkspirit::render;

		load_shader(device, "ps_sprite_color", L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\pixel\\ps_sprite_color.hlsl", shader_type::PIXEL);
		load_shader(device, "ps_sprite_light", L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\pixel\\ps_sprite_light.hlsl", shader_type::PIXEL);
		load_shader(device, "ps_sprite_screen_out", L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\pixel\\ps_sprite_screen_out.hlsl", shader_type::PIXEL);

		load_shader(device, "vs_full_screen", L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\vertex\\vs_full_screen.hlsl", shader_type::VERTEX);
		load_shader(device, "vs_simple", L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\vertex\\vs_simple.hlsl", shader_type::VERTEX);

		auto indicies = new index_buffer();
		auto verticies = new vertex_buffer<vertex>();

		std::vector<vertex> vertexData =
		{
			vertex(-0.5f, -0.5f, 0.0f, 0.0f, 0.0f), //TopLeft
			vertex(0.5f, -0.5f, 0.0f, 1.0f, 0.0f), //TopRight
			vertex(-0.5, 0.5, 0.0f, 0.0f, 1.0f), //Bottom Left
			vertex(0.5, 0.5, 0.0f, 1.0f, 1.0f), //Bottom Right
		};

		std::vector<DWORD> indexData =
		{
			0, 1, 2,
			2, 1, 3
		};

		verticies->Initialize(device->get_device().Get(), vertexData.data(), vertexData.size());
		indicies->Initialize(device->get_device().Get(), indexData.data(), indexData.size());

		m_verticies_map.emplace("sprite_vertex", verticies);
		m_indicies_map.emplace("sprite_index", indicies);


		auto indiciesFS = new index_buffer();
		auto verticiesFS = new vertex_buffer<vertex>();

		std::vector<vertex> verticesDataFS =
		{
			vertex(-1.0f, -1.0f, 0.0f, 0.0f, 1.0f), //TopLeft
			vertex(-1.0f, 1.0f, 0.0f, 0.0f, 0.0f), //TopRight
			vertex(1.0f, 1.0f, 0.0f, 1.0f, 0.0f), //Bottom Left
			vertex(1.0f, -1.0f, 0.0f, 1.0f, 1.0f), //Bottom Right
		};

		std::vector<DWORD> indiciesDataFS =
		{
			0, 1, 2,
			0, 2, 3
		};

		verticiesFS->Initialize(device->get_device().Get(), verticesDataFS.data(), verticesDataFS.size());
		indiciesFS->Initialize(device->get_device().Get(), indiciesDataFS.data(), indiciesDataFS.size());

		m_verticies_map.emplace("full_screen_vertex", verticiesFS);
		m_indicies_map.emplace("full_screen_index", indiciesFS);

		verticesDataFS.clear();
		indiciesDataFS.clear();

		indexData.clear();
		vertexData.clear();
	}

	void assets_manager::load_texture(sharkspirit::render::device* device, const std::string name, const std::string& path)
	{
		sharkspirit::log::Logger::LogInfo(std::format("Try to load texture with name [{0}]", name));

		auto pTexture = new sharkspirit::render::Texture();
		HRESULT hr = pTexture->initialize(device, path);

		if (FAILED(hr))
		{
			sharkspirit::log::Logger::LogError(std::format("Error while loading texture with name [{0}], HRESULT - {1}", name, std::system_category().message(hr)));
			pTexture->~Texture();
		}
		else
		{
			m_textures_map.emplace(name, pTexture);
		}
	}
	
	sharkspirit::render::Texture* assets_manager::get_texture(const std::string& name)
	{
		return m_textures_map[name];
	}

	void assets_manager::load_shader(sharkspirit::render::device* device, const std::string name, const std::wstring& path, shader_type type)
	{
		HRESULT hr = { 0 };

		ID3D10Blob* blob = { 0 };

		switch (type)
		{
		case PIXEL:
		{
			hr = D3DCompileFromFile(path.c_str(), 0, D3D_COMPILE_STANDARD_FILE_INCLUDE, "main", "ps_5_0", 0, 0, &blob, 0);

			sharkspirit::log::Logger::LogInfo(std::format("Compile shader with name [{0}], result - {1}", name, std::system_category().message(hr)));

			auto pixelShader = new sharkspirit::render::pixel_shader();
			hr = pixelShader->InitializeFromBlob(device->get_device().Get(), blob);
			m_pixel_shaders_map.emplace(name, pixelShader);
			break;
		}
		case VERTEX:
		{
			hr = D3DCompileFromFile(path.c_str(), 0, D3D_COMPILE_STANDARD_FILE_INCLUDE, "main", "vs_5_0", 0, 0, &blob, 0);

			sharkspirit::log::Logger::LogInfo(std::format("Compile shader with name [{0}], result - {1}", name, std::system_category().message(hr)));

			D3D11_INPUT_ELEMENT_DESC layout2D[] =
			{
				{"POSITION", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
				{"TEXCOORD", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
			};

			UINT numElements2D = ARRAYSIZE(layout2D);

			auto vertexShader = new sharkspirit::render::vertex_shader();
			hr = vertexShader->InitializeFromBlob(device->get_device().Get(), blob, layout2D, numElements2D);
			m_vertex_shaders_map.emplace(name, vertexShader);
		}
		}
	}

	sharkspirit::render::pixel_shader* assets_manager::get_pixel_shader(const std::string& name)
	{
		return m_pixel_shaders_map[name];
	}

	sharkspirit::render::vertex_shader* assets_manager::get_vertex_shader(const std::string& name)
	{
		return m_vertex_shaders_map[name];
	}

	sharkspirit::render::vertex_buffer<sharkspirit::render::vertex>* assets_manager::get_verticies(const std::string& name)
	{
		return m_verticies_map[name];
	}

	sharkspirit::render::index_buffer* assets_manager::get_indicies(const std::string& name)
	{
		return m_indicies_map[name];
	}
}
