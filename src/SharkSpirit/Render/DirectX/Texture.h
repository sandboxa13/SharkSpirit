#pragma once
#include <d3d11.h>
#include <wrl/client.h>
#include <Render/DirectX/GraphicsManager.h>
#include <Render/Device.h>

namespace SharkSpirit 
{
	class Color
	{
	public:
		Color();
		Color(unsigned int val);
		Color(BYTE r, BYTE g, BYTE b);
		Color(BYTE r, BYTE g, BYTE b, BYTE a);
		Color(const Color& src);

		Color& operator=(const Color& src);
		bool operator==(const Color& rhs) const;
		bool operator!=(const Color& rhs) const;

		constexpr BYTE GetR() const;
		void SetR(BYTE r);

		constexpr BYTE GetG() const;
		void SetG(BYTE g);

		constexpr BYTE GetB() const;
		void SetB(BYTE b);

		constexpr BYTE GetA() const;
		void SetA(BYTE a);

	private:
		union
		{
			BYTE rgba[4];
			unsigned int color;
		};
	};


	class Texture
	{
	public:
		Texture()
		{

		}
		virtual ~Texture() 
		{
			texture.ReleaseAndGetAddressOf();
			textureView.ReleaseAndGetAddressOf();
		}
		HRESULT initialize(shark_spirit::render::device* device, const std::string& filePath);
		Texture(shark_spirit::render::device* device, const std::string& filePath);
		Texture(shark_spirit::render::device* device, const uint8_t* pData, size_t size);
		ID3D11ShaderResourceView* GetTextureResourceView();
		ID3D11ShaderResourceView** GetTextureResourceViewAddress();

	private:
		Microsoft::WRL::ComPtr<ID3D11Resource> texture = nullptr;
		Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> textureView = nullptr;
	};
}