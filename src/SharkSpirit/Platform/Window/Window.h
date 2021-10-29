#pragma once

#include <Windows.h>
#include <Windowsx.h>
#include <functional>
#include "Logger/Logger.h"

namespace SharkSpirit 
{
	using wnd_proc_callBack = std::function<LRESULT(UINT, WPARAM, LPARAM)>;

	class window_creation_info
	{
	public:
		window_creation_info(
			int height, 
			int width,
			const wchar_t* title, 
			const wchar_t* className, 
			HINSTANCE h_Instance)
			: m_width(width), m_height(height), m_title(title), m_class_name(className), m_hInstance(h_Instance){}

		int m_width;
		int m_height;
		const wchar_t* m_title;
		const wchar_t* m_class_name;
		HINSTANCE m_hInstance;
	};

	class window
	{
	public:
		window() = default;
		window(window_creation_info* createInfo);
		~window();

		void show();
		void hide();
		void set_wnd_proc_callback(wnd_proc_callBack callback);
		LRESULT CALLBACK WndProc(_In_ HWND hwnd, _In_ UINT msg, _In_ WPARAM wParam, _In_ LPARAM lParam);
		HWND get_window_handle();
	private:
		void initialize(window_creation_info* createInfo);
		HWND m_window_handle;
		window_creation_info m_window_create_info;
	};

	class window_info
	{
	public:
		window_info(
			int height,
			int width,
			const wchar_t* title,
			const wchar_t* className,
			HINSTANCE h_Instance)
			: m_width(width), m_height(height), m_title(title), m_className(className), m_hinstance(h_Instance), m_window_handle(0), m_window(nullptr)
		{
		}

		int m_height;
		int m_width;
		const wchar_t* m_title;
		const wchar_t* m_className;
		HINSTANCE m_hinstance;
		HWND m_window_handle;
		wnd_proc_callBack m_wndproc_callback;
		window* m_window;
	};
	

	class window_factory 
	{
	public:
		static window_info* create_window(window_creation_info* info)
		{
			Logger::LogInfo("Creating Window");
			std::stringstream stream = {};
			stream << "Window configuration: WIDTH- " << info->m_width<< " HEIGHT- " << info->m_height;
			Logger::LogInfo(stream.str());

			auto m_window_info = new window_info(
				info->m_height,
				info->m_width,
				info->m_title,
				info->m_class_name,
				info->m_hInstance);

			auto window_instance = new window(info);

			m_window_info->m_window = window_instance;
			m_window_info->m_window_handle = window_instance->get_window_handle();

			return m_window_info;
		}
	};
}