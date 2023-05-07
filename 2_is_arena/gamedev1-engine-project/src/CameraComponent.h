#pragma once
#include "IComponent.h"

class GameObject;

class CameraComponent : public IComponent {
public:
	void Init(sf::RenderWindow& window);
	void Update(float deltatime) override;

	CameraComponent(GameObject& gameObject, GameObject* target_, sf::RenderWindow& window);

private:
	const float m_zoom = 0.5f;
	GameObject* m_target;
	sf::RenderWindow& m_window;
	sf::Vector2f m_correctionOffset;
};