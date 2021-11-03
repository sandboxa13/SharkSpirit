#pragma once
#include <DirectXMath.h>

namespace SharkSpirit 
{
	class camera_2d
	{
	public:
		camera_2d();

		void SetProjectionValues(float width, float height, float nearZ, float farZ);

		const DirectX::XMMATRIX& GetOrthoMatrix() const;
		const DirectX::XMMATRIX& GetWorldMatrix() const;
		void SetPosition(const DirectX::XMFLOAT3& pos);
	private:
		void UpdateMatrix();

		DirectX::XMMATRIX m_world_matrix;
		DirectX::XMMATRIX m_ortho_matrix;

		DirectX::XMFLOAT3 pos;
		DirectX::XMFLOAT3 rot;

		float m_zoom;
	};
}