#include "Camera2D.h"

namespace SharkSpirit
{
	camera_2d::camera_2d()
	{
		pos = DirectX::XMFLOAT3(5.0f, 0.0f, 0.0f);
		rot = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);

		UpdateMatrix();
	}

	void camera_2d::SetProjectionValues(float width, float height, float nearZ, float farZ)
	{
		m_ortho_matrix = DirectX::XMMatrixOrthographicOffCenterLH(0.0f, width, height, 0.0f, nearZ, farZ);
	}

	const DirectX::XMMATRIX& camera_2d::GetOrthoMatrix() const
	{
		return m_ortho_matrix;
	}

	const DirectX::XMMATRIX& camera_2d::GetWorldMatrix() const
	{
		return m_world_matrix;
	}

	void camera_2d::SetPosition(const DirectX::XMFLOAT3& pos)
	{
		this->pos = pos;
		UpdateMatrix();
	}

	void camera_2d::UpdateMatrix()
	{
		DirectX::XMMATRIX translationOffsetMatrix = DirectX::XMMatrixTranslation(-pos.x, -pos.y, 0.0f); //z component irrelevant for 2d camera
		DirectX::XMMATRIX camRotationMatrix = DirectX::XMMatrixRotationRollPitchYaw(rot.x, rot.y, rot.z);
		m_world_matrix = camRotationMatrix * translationOffsetMatrix;
	}
}