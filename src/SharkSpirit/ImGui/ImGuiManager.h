#pragma once
#include <ImGui/imgui.h>
#include <ImGui/imgui_impl_win32.h>
#include <ImGui/imgui_impl_dx11.h>
#include <wrl/client.h>

namespace SharkSpirit
{
	class imgui_manager
	{
	public:
		virtual ~imgui_manager() = default;

		void SetStyle();
		void BeginFrame();
		void EndFrame();
		void InitImgui(HWND hwnd, Microsoft::WRL::ComPtr<ID3D11Device> device, Microsoft::WRL::ComPtr<ID3D11DeviceContext> context);
	};
}