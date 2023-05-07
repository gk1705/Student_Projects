#pragma once
#include <map>
#include <vector>

using namespace std;
using namespace sf;

class InputManager
{
public:
	static InputManager& getInstance();
	static void release();

	bool IsKeyPressed(const string& action, int playerIdx);
	bool IsButtonPressed(const string& action, int playerIdx);
		
	void bind(string action, Keyboard::Key keyCode, int playerIdx);
	void unbind(const string& action, int playerIdx);

	void bindButton(string action, int button, int playerIdx);

	sf::Vector2f getLeftJoystickWithReset(int playerIdx);
	sf::Vector2f getLeftJoystick(int playerIdx) const;
	sf::Vector2f getRightJoystickWithReset(int playerIdx);
	sf::Vector2f getRightJoystick(int playerIdx) const;

	InputManager(const InputManager& p) = delete;
	InputManager& operator=(InputManager const&) = delete;

private:
	InputManager(void) = default;
	~InputManager(void) = default;

	static InputManager *m_instance;

	vector<map<string, Keyboard::Key>> player;
	vector<map<string, int>> playerControlls;
	vector<bool> m_resetLeftJoystick;
	vector<bool> m_resetRightJoystick;
};