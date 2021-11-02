#include "Texture.h"
#include <Utils/WICTextureLoader.h>
#include <Utils/StringUtils.h>

namespace SharkSpirit
{
	//Texture::~Texture()
	//{
	//	/*texture.ReleaseAndGetAddressOf();
	//	textureView.ReleaseAndGetAddressOf();*/
	//}

	Texture::Texture(graphics_manager* graphicsManager, const std::string& filePath)
	{
		HRESULT hr = CreateWICTextureFromFile(graphicsManager->get_device().Get(), graphicsManager->get_device_context().Get(), string_to_wide(filePath).c_str(), texture.GetAddressOf(), textureView.GetAddressOf());
	}

	ID3D11ShaderResourceView* Texture::GetTextureResourceView()
	{
		return this->textureView.Get();
	}

	ID3D11ShaderResourceView** Texture::GetTextureResourceViewAddress()
	{
		return this->textureView.GetAddressOf();
	}

	void Texture::Initialize1x1ColorTexture(ID3D11Device* device, const Color& colorData)
	{
		InitializeColorTexture(device, &colorData, 1, 1);
	}

	void Texture::InitializeColorTexture(ID3D11Device* device, const Color* colorData, UINT width, UINT height)
	{
		CD3D11_TEXTURE2D_DESC textureDesc(DXGI_FORMAT_R8G8B8A8_UNORM, width, height);
		ID3D11Texture2D* p2DTexture = nullptr;
		D3D11_SUBRESOURCE_DATA initialData{};
		initialData.pSysMem = colorData;
		initialData.SysMemPitch = width * sizeof(Color);
		HRESULT hr = device->CreateTexture2D(&textureDesc, &initialData, &p2DTexture);
		texture = static_cast<ID3D11Texture2D*>(p2DTexture);
		CD3D11_SHADER_RESOURCE_VIEW_DESC srvDesc(D3D11_SRV_DIMENSION_TEXTURE2D, textureDesc.Format);
		hr = device->CreateShaderResourceView(texture.Get(), &srvDesc, textureView.GetAddressOf());
	}
}