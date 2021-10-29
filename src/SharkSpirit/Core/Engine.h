//#pragma once
//
//#include "Logger/Logger.h"
//#include "Render/RenderEngine.h"
//#include "Input/InputProcessor.h"
//#include "Core/Timer/Timer.h"
//#include "IInitializable.h"
//#include "Platform/Window/Window.h"
//
//namespace SharkSpirit 
//{
//	class EngineConfiguration
//	{
//	public:
//		EngineConfiguration(std::unique_ptr<WindowConfiguration> windowConfiguration) : WindowConfiguration(std::move(windowConfiguration))
//		{
//
//		};
//		virtual ~EngineConfiguration() = default;
//
//		std::unique_ptr<WindowConfiguration> WindowConfiguration;
//	};
//
//	class Engine : public IInitializable
//	{
//	public:
//		Engine() : _IsRunning(false) 
//		{
//		};
//		virtual ~Engine() = default;
//
//	public:
//		std::unique_ptr<EngineConfiguration> EngineConfig;
//		std::unique_ptr<InputProcessor> Input;
//		std::unique_ptr<Timer> Timer;
//
//		static std::unique_ptr<Engine> CreateEngine(std::unique_ptr<EngineConfiguration> engineConfig);
//		bool Run();
//		void Stop();
//		virtual void Initialize()
//		{
//			Logger::LogInfo("Initialize Engine");
//
//			Input = std::make_unique<InputProcessor>(EngineConfig->WindowConfiguration->Window->GetHWND());
//			Input->Initialize();
//
//			Timer = std::make_unique<SharkSpirit::Timer>();
//			Timer->Initialize();
//		}
//	private:
//		bool _IsRunning;
//	};
//}