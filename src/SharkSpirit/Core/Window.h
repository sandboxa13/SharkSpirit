#pragma once

#include <Windows.h>
#include <Windowsx.h>
#include <functional>

namespace SSWindow
{
	using WndProcCallBack = std::function<LRESULT(UINT, WPARAM, LPARAM)>;

	class Window
	{
	public:
		Window(const wchar_t* title, const  wchar_t* class_name, int height, int width, HINSTANCE& hinstance);
		virtual ~Window();

		void SetWndProc(WndProcCallBack callback);
		HWND GetHWND();
		LRESULT CALLBACK WndProc(_In_ HWND hwnd, _In_ UINT msg, _In_ WPARAM wParam, _In_ LPARAM lParam);
		bool operator!() const {
			return this != nullptr;
		}
	private:
		int height;
		int width;
		const wchar_t* title;
		const wchar_t* className;
		HINSTANCE m_hinstance;
		HWND m_window_handle;
		WndProcCallBack m_wndproc_callback;
	};
}