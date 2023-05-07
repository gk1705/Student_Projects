#pragma once
#include <TGUI/Gui.hpp>
#include <TGUI/Widgets/Button.hpp>

class GUIVerticalInputHandler
{
public:
	GUIVerticalInputHandler(tgui::Gui& _m_gui)
		: m_gui(_m_gui)
		, m_currentfocus(0)
		, m_buttondown(false)
	{
		
	}

	void Update(const sf::RenderWindow& window)
	{
		const sf::Vector2f scale = sf::Vector2f(window.getSize().x / 1920.0, window.getSize().y / 1080.0);

		trackVerticalInput();
		pollFocusEvents(scale);
		handleClickEvent(scale);
	}

	void AddButton(const tgui::Button::Ptr& button)
	{
		m_buttons.push_back(button);
	}

private:
	void trackVerticalInput();
	void pollFocusEvents(const sf::Vector2f& scale);
	void handleClickEvent(const sf::Vector2f& scale);

	tgui::Gui& m_gui;
	std::vector<tgui::Button::Ptr> m_buttons;
	int m_currentfocus;
	bool m_buttondown;
};