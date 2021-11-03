#include "Window.h"
#include <ImGui/imgui_impl_win32.h>

namespace SharkSpirit 
{
	LRESULT CALLBACK DefWndProc(
		HWND hwnd,
		UINT msg,
		WPARAM wParam,
		LPARAM lParam)
	{
		if (msg == WM_CREATE)
		{
			SetPropW(hwnd, L"WndPtr", ((tagCREATESTRUCTW*)lParam)->lpCreateParams);
			return true;
		}

		window* wnd = (window*)GetPropW(hwnd, L"WndPtr");

		if (wnd) {
			return wnd->WndProc(hwnd, msg, wParam, lParam);
		}
		else
		{
			return DefWindowProcW(hwnd, msg, wParam, lParam);
		}
	}

	LRESULT window::WndProc(
		HWND   hwnd,
		UINT   msg,
		WPARAM wParam,
		LPARAM lParam)
	{
		if (ImGui_ImplWin32_WndProcHandler(hwnd, msg, wParam, lParam))
		{
			return true;
		}

		switch (msg)
		{
		case WM_CLOSE:
			PostQuitMessage(0);
			return 0;

		case WM_DESTROY:
			PostQuitMessage(0);
			return 0;

		default:
			/*if ((this->m_window_handle) && (m_window_info.m_wndproc_callback)) {
				return m_window_info.m_wndproc_callback(msg, wParam, lParam);
			}
			else
			{*/
				return DefWindowProc(hwnd, msg, wParam, lParam);
			//}
		}
	}

	HWND window::get_window_handle()
	{
		return m_window_handle;
	}
	
	window::window(window_creation_info* createInfo)  
		: m_window_create_info(window_creation_info(0, 0, L"Shark Spirit SANDBOX", L"Shark Spirit SANDBOX", nullptr))
	{
		m_window_create_info = *createInfo;

		initialize(createInfo);
	}

	window::~window()
	{
		UnregisterClass(m_window_create_info.m_class_name, m_window_create_info.m_hInstance);
		DestroyWindow(this->m_window_handle);
	}

	void window::show()
	{
		ShowWindow(this->m_window_handle, SW_SHOW);
	}

	void window::hide()
	{
	}

	void window::set_wnd_proc_callback(wnd_proc_callBack callback)
	{

	}

	void window::initialize(window_creation_info* createInfo)
	{
		WNDCLASSEX wc = { 0 };

		wc.cbSize = sizeof wc;

		wc.lpfnWndProc = DefWndProc;
		wc.cbClsExtra = 0;
		wc.cbWndExtra = 0;
		wc.hInstance = createInfo->m_hInstance;

		wc.style = CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
		wc.hCursor = nullptr;
		wc.hbrBackground = nullptr;
		wc.lpszMenuName = nullptr;
		wc.lpszClassName = createInfo->m_class_name;

		RegisterClassEx(&wc);

		RECT window_rect;

		window_rect.left = 50;
		window_rect.top = 50;
		window_rect.right = createInfo->m_width + window_rect.left;
		window_rect.bottom = createInfo->m_height + window_rect.top;

		AdjustWindowRect(&window_rect, WS_CAPTION | WS_MINIMIZEBOX | WS_SYSMENU, false);

		m_window_handle = CreateWindowEx(
			0,
			createInfo->m_class_name,
			createInfo->m_title,
			WS_CAPTION | WS_MINIMIZEBOX | WS_SYSMENU | WS_MAXIMIZEBOX | WS_OVERLAPPEDWINDOW,
			window_rect.left,
			window_rect.top,
			window_rect.right - window_rect.left,
			window_rect.bottom - window_rect.top,
			nullptr,
			nullptr,
			createInfo->m_hInstance,
			this);
	}
}