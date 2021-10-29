#include "Keyboard.h"

bool keyboard::KeyIsPressed(unsigned char keycode) const noexcept
{
	return keystates[keycode];
}

std::optional<keyboard::Event> keyboard::ReadKey() noexcept
{
	if (keybuffer.size() > 0u)
	{
		keyboard::Event e = keybuffer.front();
		keybuffer.pop();
		return e;
	}
	return {};
}

bool keyboard::KeyIsEmpty() const noexcept
{
	return keybuffer.empty();
}

std::optional<char> keyboard::ReadChar() noexcept
{
	if (charbuffer.size() > 0u)
	{
		unsigned char charcode = charbuffer.front();
		charbuffer.pop();
		return charcode;
	}
	return {};
}

bool keyboard::CharIsEmpty() const noexcept
{
	return charbuffer.empty();
}

void keyboard::FlushKey() noexcept
{
	keybuffer = std::queue<Event>();
}

void keyboard::FlushChar() noexcept
{
	charbuffer = std::queue<char>();
}

void keyboard::Flush() noexcept
{
	FlushKey();
	FlushChar();
}

void keyboard::EnableAutorepeat() noexcept
{
	autorepeatEnabled = true;
}

void keyboard::DisableAutorepeat() noexcept
{
	autorepeatEnabled = false;
}

bool keyboard::AutorepeatIsEnabled() const noexcept
{
	return autorepeatEnabled;
}

void keyboard::OnKeyPressed(unsigned char keycode) noexcept
{
	keystates[keycode] = true;
	keybuffer.push(keyboard::Event(keyboard::Event::Type::Press, keycode));
	TrimBuffer(keybuffer);
}

void keyboard::OnKeyReleased(unsigned char keycode) noexcept
{
	keystates[keycode] = false;
	keybuffer.push(keyboard::Event(keyboard::Event::Type::Release, keycode));
	TrimBuffer(keybuffer);
}

void keyboard::OnChar(char character) noexcept
{
	charbuffer.push(character);
	TrimBuffer(charbuffer);
}

void keyboard::ClearState() noexcept
{
	keystates.reset();
}

template<typename T>
void keyboard::TrimBuffer(std::queue<T>& buffer) noexcept
{
	while (buffer.size() > bufferSize)
	{
		buffer.pop();
	}
}
