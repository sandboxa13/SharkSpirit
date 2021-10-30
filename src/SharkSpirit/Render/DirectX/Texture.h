#pragma once
#include <d3d11.h>
#include <wrl/client.h>
#include <string>

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
		Texture(ID3D11Device* device, ID3D11DeviceContext* deviceContext, const std::string& filePath);
		Texture(ID3D11Device* device, const uint8_t* pData, size_t size);
		ID3D11ShaderResourceView* GetTextureResourceView();
		ID3D11ShaderResourceView** GetTextureResourceViewAddress();

	private:
		void Initialize1x1ColorTexture(ID3D11Device* device, const Color& colorData);
		void InitializeColorTexture(ID3D11Device* device, const Color* colorData, UINT width, UINT height);
		Microsoft::WRL::ComPtr<ID3D11Resource> texture = nullptr;
		Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> textureView = nullptr;
	};
}