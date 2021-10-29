#include "Window.h"

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
		/*if (ImGui_ImplWin32_WndProcHandler(hwnd, msg, wParam, lParam))
		{
			return true;
		}*/

		switch (msg)
		{
		case WM_CLOSE:
			DestroyWindow(m_window_info.m_window_handle);
			m_window_info.m_window_handle = 0;
			break;

		case WM_DESTROY:
			PostQuitMessage(0);
			return 0;

		default:
			if ((m_window_info.m_window_handle) && (m_window_info.m_wndproc_callback)) {
				return m_window_info.m_wndproc_callback(msg, wParam, lParam);
			}
			else
			{
				return DefWindowProc(hwnd, msg, wParam, lParam);
			}
		}
	}
	
	window::window(window_creation_info* createInfo) : m_window_info(window_info(0, 0, L"Shark Spirit SANDBOX", L"Shark Spirit SANDBOX", nullptr))
	{
		initialize(createInfo);
	}

	window::~window()
	{
		UnregisterClass(m_window_info.m_className, m_window_info.m_hinstance);
		DestroyWindow(m_window_info.m_window_handle);
	}

	void window::show()
	{
		ShowWindow(this->m_window_info.m_window_handle, SW_SHOW);
	}

	void window::hide()
	{
	}

	void window::set_wnd_proc_callback(wnd_proc_callBack callback)
	{
		m_window_info.m_wndproc_callback = callback;
	}

	window_info* window::get_window_info()
	{
		return &this->m_window_info;
	}

	void window::initialize(window_creation_info* createInfo)
	{
		m_window_info = window_info(
			createInfo->m_height, 
			createInfo->m_width, 
			createInfo->m_title,
			createInfo->m_class_name, 
			createInfo->m_hInstance);
	
		WNDCLASSEX wc = { 0 };

		wc.cbSize = sizeof wc;

		wc.lpfnWndProc = DefWndProc;
		wc.cbClsExtra = 0;
		wc.cbWndExtra = 0;
		wc.hInstance = m_window_info.m_hinstance;

		wc.style = CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
		wc.hCursor = nullptr;
		wc.hbrBackground = nullptr;
		wc.lpszMenuName = nullptr;
		wc.lpszClassName = m_window_info.m_className;

		RegisterClassEx(&wc);

		RECT window_rect;

		window_rect.left = 50;
		window_rect.top = 50;
		window_rect.right = m_window_info.m_width + window_rect.left;
		window_rect.bottom = m_window_info.m_height + window_rect.top;

		AdjustWindowRect(&window_rect, WS_CAPTION | WS_MINIMIZEBOX | WS_SYSMENU, false);

		m_window_info.m_window_handle = CreateWindowEx(
			0,
			m_window_info.m_className,
			m_window_info.m_title,
			WS_CAPTION | WS_MINIMIZEBOX | WS_SYSMENU | WS_MAXIMIZEBOX | WS_OVERLAPPEDWINDOW,
			window_rect.left,
			window_rect.top,
			window_rect.right - window_rect.left,
			window_rect.bottom - window_rect.top,
			nullptr,
			nullptr,
			m_window_info.m_hinstance,
			this);
	}
}