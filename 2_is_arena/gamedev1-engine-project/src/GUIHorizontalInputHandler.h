#pragma once
#include <TGUI/Gui.hpp>
#include <TGUI/Widgets/Button.hpp>

class GUIHorizontalInputHandler
{
public:
	GUIHorizontalInputHandler(tgui::Gui& _m_gui)
		: m_gui(_m_gui)
		, m_currentfocus(0)
		, m_buttondown(false)
	{

	}

	void Update(const sf::RenderWindow& window, const int playerIdx)
	{
		m_playerIdx = playerIdx;
		const sf::Vector2f scale = sf::Vector2f(window.getSize().x / 1920.0, window.getSize().y / 1080.0);

		trackHorizontalInput();
		pollFocusEvents(scale);
		handleClickEvent(scale);
	}

	void AddButton(const int idx, const tgui::Button::Ptr& button)
	{
		m_buttons[idx].push_back(button);
	}

	void init();
	void terminate();

private:
	void trackHorizontalInput();
	void pollFocusEvents(const sf::Vector2f& scale);
	void handleClickEvent(const sf::Vector2f& scale);

	tgui::Gui& m_gui;
	std::vector<std::vector<tgui::Button::Ptr>> m_buttons;
	std::vector<int> m_currentfocus;
	std::vector<bool> m_buttondown;
	int m_playerIdx;
};