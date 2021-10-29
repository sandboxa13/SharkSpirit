#include "Mouse.h"

#pragma once

// target Windows 7 or later
#define _WIN32_WINNT 0x0601
#include <sdkddkver.h>
// The following #defines disable a bunch of unused windows stuff. If you 
// get weird errors when trying to do some windows stuff, try removing some
// (or all) of these defines (it will increase build time though).
#ifndef FULL_WINTARD
#define WIN32_LEAN_AND_MEAN
#define NOGDICAPMASKS
#define NOSYSMETRICS
#define NOMENUS
#define NOICONS
#define NOSYSCOMMANDS
#define NORASTEROPS
#define OEMRESOURCE
#define NOATOM
#define NOCLIPBOARD
#define NOCOLOR
#define NOCTLMGR
#define NODRAWTEXT
#define NOKERNEL
#define NONLS
#define NOMEMMGR
#define NOMETAFILE
#define NOOPENFILE
#define NOSCROLL
#define NOSERVICE
#define NOSOUND
#define NOTEXTMETRIC
#define NOWH
#define NOCOMM
#define NOKANJI
#define NOHELP
#define NOPROFILER
#define NODEFERWINDOWPOS
#define NOMCX
#define NORPC
#define NOPROXYSTUB
#define NOIMAGE
#define NOTAPE
#endif

#define NOMINMAX

#define STRICT

#include <Windows.h>

std::pair<int, int> mouse::GetPos() const noexcept
{
	return { x,y };
}

std::optional<mouse::RawDelta> mouse::ReadRawDelta() noexcept
{
	if (rawDeltaBuffer.empty())
	{
		return std::nullopt;
	}
	const RawDelta d = rawDeltaBuffer.front();
	rawDeltaBuffer.pop();
	return d;
}

int mouse::GetPosX() const noexcept
{
	return x;
}

int mouse::GetPosY() const noexcept
{
	return y;
}

bool mouse::IsInWindow() const noexcept
{
	return isInWindow;
}

bool mouse::LeftIsPressed() const noexcept
{
	return leftIsPressed;
}

bool mouse::RightIsPressed() const noexcept
{
	return rightIsPressed;
}

std::optional<mouse::Event> mouse::Read() noexcept
{
	if (buffer.size() > 0u)
	{
		mouse::Event e = buffer.front();
		buffer.pop();
		return e;
	}
	return {};
}

void mouse::Flush() noexcept
{
	buffer = std::queue<Event>();
}

void mouse::EnableRaw() noexcept
{
	rawEnabled = true;
}

void mouse::DisableRaw() noexcept
{
	rawEnabled = false;
}

bool mouse::RawEnabled() const noexcept
{
	return rawEnabled;
}

void mouse::OnMouseMove(int newx, int newy) noexcept
{
	x = newx;
	y = newy;

	buffer.push(mouse::Event(mouse::Event::Type::Move, *this));
	TrimBuffer();
}

void mouse::OnMouseLeave() noexcept
{
	isInWindow = false;
	buffer.push(mouse::Event(mouse::Event::Type::Leave, *this));
	TrimBuffer();
}

void mouse::OnMouseEnter() noexcept
{
	isInWindow = true;
	buffer.push(mouse::Event(mouse::Event::Type::Enter, *this));
	TrimBuffer();
}

void mouse::OnRawDelta(int dx, int dy) noexcept
{
	rawDeltaBuffer.push({ dx,dy });
	TrimBuffer();
}

void mouse::OnLeftPressed(int x, int y) noexcept
{
	leftIsPressed = true;

	buffer.push(mouse::Event(mouse::Event::Type::LPress, *this));
	TrimBuffer();
}

void mouse::OnLeftReleased(int x, int y) noexcept
{
	leftIsPressed = false;

	buffer.push(mouse::Event(mouse::Event::Type::LRelease, *this));
	TrimBuffer();
}

void mouse::OnRightPressed(int x, int y) noexcept
{
	rightIsPressed = true;

	buffer.push(mouse::Event(mouse::Event::Type::RPress, *this));
	TrimBuffer();
}

void mouse::OnRightReleased(int x, int y) noexcept
{
	rightIsPressed = false;

	buffer.push(mouse::Event(mouse::Event::Type::RRelease, *this));
	TrimBuffer();
}

void mouse::OnWheelUp(int x, int y) noexcept
{
	buffer.push(mouse::Event(mouse::Event::Type::WheelUp, *this));
	TrimBuffer();
}

void mouse::OnWheelDown(int x, int y) noexcept
{
	buffer.push(mouse::Event(mouse::Event::Type::WheelDown, *this));
	TrimBuffer();
}

void mouse::TrimBuffer() noexcept
{
	while (buffer.size() > bufferSize)
	{
		buffer.pop();
	}
}

void mouse::TrimRawInputBuffer() noexcept
{
	while (rawDeltaBuffer.size() > bufferSize)
	{
		rawDeltaBuffer.pop();
	}
}

void mouse::OnWheelDelta(int x, int y, int delta) noexcept
{
	wheelDeltaCarry += delta;
	// generate events for every 120 
	while (wheelDeltaCarry >= WHEEL_DELTA)
	{
		wheelDeltaCarry -= WHEEL_DELTA;
		OnWheelUp(x, y);
	}
	while (wheelDeltaCarry <= -WHEEL_DELTA)
	{
		wheelDeltaCarry += WHEEL_DELTA;
		OnWheelDown(x, y);
	}
}