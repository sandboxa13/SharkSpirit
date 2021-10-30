#include "Input/InputProcessor.h"

namespace SharkSpirit 
{
	bool input_processor::process_input()
	{
		MSG msg = { nullptr };

		if (PeekMessageW(&msg, m_windowInfo->m_window_handle, 0, 0, PM_REMOVE))
		{
			if (msg.message == WM_KILLFOCUS)
			{
				m_keyboard.ClearState();
			}

			if (msg.message == WM_KEYDOWN || msg.message == WM_SYSKEYDOWN)
			{
				if (!(msg.lParam & 0x40000000) || m_keyboard.AutorepeatIsEnabled()) // filter autorepeat
				{
					m_keyboard.OnKeyPressed(static_cast<unsigned char>(msg.wParam));
				}

				if (msg.wParam == VK_ESCAPE)
				{
					return false;
				}

				return true;
			}

			if (msg.message == WM_KEYUP || msg.message == WM_SYSKEYUP)
			{
				m_keyboard.OnKeyReleased(static_cast<unsigned char>(msg.wParam));
			}

			if (msg.message == WM_CHAR)
			{
				m_keyboard.OnChar(static_cast<unsigned char>(msg.wParam));
			}

			if (msg.message == WM_MOUSEMOVE) 
			{
				const POINTS pt = MAKEPOINTS(msg.lParam);

				// in client region -> log move, and log enter + capture mouse (if not previously in window)
				if (pt.x >= 0 && pt.x < m_windowInfo->m_width && pt.y >= 0 && pt.y < m_windowInfo->m_height)
				{
					m_mouse.OnMouseMove(pt.x, pt.y);
					if (!m_mouse.IsInWindow())
					{
						SetCapture(m_windowInfo->m_window_handle);
						m_mouse.OnMouseEnter();
					}
				}
				// not in client -> log move / maintain capture if button down
				else
				{
					if (msg.wParam & (MK_LBUTTON | MK_RBUTTON))
					{
						m_mouse.OnMouseMove(pt.x, pt.y);
					}
					// button up -> release capture / log event for leaving
					else
					{
						ReleaseCapture();
						m_mouse.OnMouseLeave();
					}
				}
			}

			if (msg.message == WM_LBUTTONDOWN) 
			{
				SetForegroundWindow(m_windowInfo->m_window_handle);
				const POINTS pt = MAKEPOINTS(msg.lParam);
				m_mouse.OnLeftPressed(pt.x, pt.y);
			}

			if (msg.message == WM_RBUTTONDOWN) 
			{
				const POINTS pt = MAKEPOINTS(msg.lParam);
				m_mouse.OnRightPressed(pt.x, pt.y);
			}

			if (msg.message == WM_LBUTTONUP) 
			{
				const POINTS pt = MAKEPOINTS(msg.lParam);
				m_mouse.OnLeftReleased(pt.x, pt.y);
				// release mouse if outside of window
				if (pt.x < 0 || pt.x >= m_windowInfo->m_width || pt.y < 0 || pt.y >= m_windowInfo->m_height)
				{
					ReleaseCapture();
					m_mouse.OnMouseLeave();
				}
			}

			if (msg.message == WM_RBUTTONUP) 
			{
				const POINTS pt = MAKEPOINTS(msg.lParam);
				m_mouse.OnRightReleased(pt.x, pt.y);
				// release mouse if outside of window
				if (pt.x < 0 || pt.x >= m_windowInfo->m_width || pt.y < 0 || pt.y >= m_windowInfo->m_height)
				{
					ReleaseCapture();
					m_mouse.OnMouseLeave();
				}
			}

			if (msg.message == WM_MOUSEWHEEL) 
			{
				const POINTS pt = MAKEPOINTS(msg.lParam);
				const int delta = GET_WHEEL_DELTA_WPARAM(msg.wParam);
				m_mouse.OnWheelDelta(pt.x, pt.y, delta);
			}

			if (msg.message == WM_QUIT)
			{
				PostQuitMessage(0);
				return false;
			}
			if (msg.message == WM_CLOSE)
			{
				//DestroyWindow(m_windowInfo->m_window_handle)
				PostQuitMessage(0);
				return false;
			}

			if (msg.message == WM_DESTROY)
			{
				PostQuitMessage(0);
				return false;
			}

			TranslateMessage(&msg);
			DispatchMessage(&msg);

			return true;
		}

		InvalidateRect(m_windowInfo->m_window_handle, nullptr, false);

		return true;
	}
}