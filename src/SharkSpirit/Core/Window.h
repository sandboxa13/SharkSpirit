#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include <Windows.h>
#include <Windowsx.h>
#include <functional>
#include <sstream>
#include "Logger/Logger.h"

namespace SharkSpirit
{
	using WndProcCallBack = std::function<LRESULT(UINT, WPARAM, LPARAM)>;


	class Window
	{
	public:
		Window(const wchar_t* title, const  wchar_t* class_name, int width, int height, HINSTANCE& hinstance);
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

	class WindowConfiguration
	{
	public:
		WindowConfiguration(int width, int height, const wchar_t* title, const wchar_t* class_name, HINSTANCE& hInstance)
			: Width(width), Height(height), Title(title), ClassName(class_name), Hinstance(hInstance)
		{
		};
		virtual ~WindowConfiguration() = default;

		int Width;
		int Height;
		const wchar_t* Title;
		const wchar_t* ClassName;
		HINSTANCE Hinstance;
		std::unique_ptr<Window> Window;
	};

	class WindowFactory
	{
	public:
		static std::unique_ptr<WindowConfiguration> CreateSSWindow(std::unique_ptr<WindowConfiguration> windowConfig)
		{
			Logger::LogInfo("CREATING WINDOW");
			std::stringstream stream = {};
			stream << "Window configuration: WIDTH- " << windowConfig->Width << " HEIGHT- " << windowConfig->Height;
			Logger::LogInfo(stream.str());

			windowConfig->Window = std::make_unique<Window>(windowConfig->Title, windowConfig->ClassName, windowConfig->Width, windowConfig->Height, windowConfig->Hinstance);

			auto& window = windowConfig->Window;

			auto wndProc = [&window](UINT msg, WPARAM wParam, LPARAM lParam) -> LRESULT
			{
				UINT width = {};
				UINT height = {};

				switch (msg)
				{
				case WM_SIZE:

					width = LOWORD(lParam);
					height = HIWORD(lParam);

					break;
				case WM_CLOSE:
					PostQuitMessage(0);
					break;

				case WM_PAINT:
					ValidateRect(window->GetHWND(), nullptr);
					break;
				case WM_MOUSEMOVE:

					break;
				}

				return DefWindowProc(window->GetHWND(), msg, wParam, lParam);
			};

			window->SetWndProc(wndProc);

			if ((windowConfig->Window) == nullptr)
			{
				Logger::LogError("CANNOT CREATE WINDOW");
			}

			return windowConfig;
		}
	};

	class WindowConfigurationFactory
	{
	public:
		static std::unique_ptr<WindowConfiguration> CreateWindowConfiguration(int width, int height, const wchar_t* title, const wchar_t* class_name, HINSTANCE& hInstance)
		{
			return std::make_unique<WindowConfiguration>(width, height, title, class_name, hInstance);
		}
	};
}
