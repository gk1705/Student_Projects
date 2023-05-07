#pragma once
#include "IGraphicsComponent.h"

class HealthBarComponent : public IGraphicsComponent 
{
public:
	HealthBarComponent(GameObject& gameObject);
	void Draw(sf::RenderWindow &window) override;
	void Update(float fDeltaTime) override {}

	void setScale(float scale);
	
private:
	RectangleShape m_healthbarGreen;
	RectangleShape m_healthbarRed;
};