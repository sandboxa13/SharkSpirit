#include "Input/InputProcessor.h"

namespace SharkSpirit 
{
	bool input_processor::process_input()
	{
		MSG msg = { nullptr };

		if (PeekMessageW(&msg, windowHandle, 0, 0, PM_REMOVE))
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

			if (msg.message == WM_QUIT)
			{
				return false;
			}

			TranslateMessage(&msg);
			DispatchMessage(&msg);

			return true;
		}

		InvalidateRect(windowHandle, nullptr, false);

		return true;
	}
}