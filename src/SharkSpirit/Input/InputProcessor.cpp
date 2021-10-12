#include "Input/InputProcessor.h"

namespace SharkSpirit 
{
	void InputProcessor::ProcessInput()
	{
		MSG msg = { nullptr };

		if (PeekMessageW(&msg, windowHandle, 0, 0, PM_REMOVE))
		{
			if (msg.message == WM_KEYDOWN)
			{
				if (msg.wParam == VK_ESCAPE)
				{
					//Engine->Stop();
				}

				return;
			}

			if (msg.message == WM_QUIT)
			{
				//Engine->Stop();

				return;
			}

			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}

		InvalidateRect(windowHandle, nullptr, false);
	}
}