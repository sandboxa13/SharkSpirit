#include "Window.h"
#include <string>


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

	Window* wnd = (Window*)GetPropW(hwnd, L"WndPtr");

	if (wnd) {
		return wnd->WndProc(hwnd, msg, wParam, lParam);
	}
	else
	{
		return DefWindowProcW(hwnd, msg, wParam, lParam);
	}
}

LRESULT Window::WndProc(
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
		DestroyWindow(m_window_handle);
		m_window_handle = 0;
		break;

	case WM_DESTROY:
		PostQuitMessage(0);
		return 0;

	default:
		if ((m_window_handle) && (m_wndproc_callback)) {
			return m_wndproc_callback(msg, wParam, lParam);
		}
		else
		{
			return DefWindowProc(hwnd, msg, wParam, lParam);
		}
	}
}


Window::Window(const wchar_t* title, const wchar_t* class_name, int width, int height, HINSTANCE& hinstance)
{
	this->height = height;
	this->width = width;
	this->title = title;
	this->className = class_name;
	this->m_window_handle = { nullptr };
	this->m_hinstance = hinstance;

	WNDCLASSEX wc = { 0 };

	wc.cbSize = sizeof wc;

	wc.lpfnWndProc = DefWndProc;
	wc.cbClsExtra = 0;
	wc.cbWndExtra = 0;
	wc.hInstance = hinstance;

	wc.style = CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
	wc.hCursor = nullptr;
	//wc.hIcon = static_cast<HICON>(LoadImage(hinstance, MAKEINTRESOURCE(IDI_ICON1), IMAGE_ICON, 32, 32, 0));
	wc.hbrBackground = nullptr;
	wc.lpszMenuName = nullptr;
	wc.lpszClassName = className;
	//wc.hIconSm = static_cast<HICON>(LoadImage(hinstance, MAKEINTRESOURCE(IDI_ICON1), IMAGE_ICON, 16, 16, 0));

	RegisterClassEx(&wc);

	RECT window_rect;

	window_rect.left = 50;
	window_rect.top = 50;
	window_rect.right = width + window_rect.left;
	window_rect.bottom = height + window_rect.top;

	AdjustWindowRect(&window_rect, WS_CAPTION | WS_MINIMIZEBOX | WS_SYSMENU, false);

	this->m_window_handle = CreateWindowEx(
		0,
		className,
		title,
		WS_CAPTION | WS_MINIMIZEBOX | WS_SYSMENU | WS_MAXIMIZEBOX | WS_OVERLAPPEDWINDOW,
		window_rect.left,
		window_rect.top,
		window_rect.right - window_rect.left,
		window_rect.bottom - window_rect.top,
		nullptr,
		nullptr,
		hinstance,
		this);
}

Window::~Window()
{
	UnregisterClass(className, m_hinstance);
	DestroyWindow(m_window_handle);
}

void Window::SetWndProc(WndProcCallBack callback)
{
	m_wndproc_callback = callback;
}

HWND Window::GetHWND()
{
	return m_window_handle;
}
