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

	class WindowConfigurationFactory
	{
	public:
		static std::unique_ptr<WindowConfiguration> CreateWindowConfiguration(int width, int height, const wchar_t* title, const wchar_t* class_name, HINSTANCE& hInstance)
		{
			std::stringstream stream = {};

			stream << "Creating window configuration : WIDTH - " << width << " HEIGHT - " << height;

			Logger::LogInfo(stream.str());

			return std::make_unique<WindowConfiguration>(width, height, title, class_name, hInstance);
		}
	};
}
