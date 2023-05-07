#pragma once
#include "IComponent.h"
#include "BoxColliderComponent.h"

class IGraphicsComponent;
class GameObject;

class BorderComponent : public IComponent 
{
public:
	BorderComponent(GameObject& gameObject, sf::FloatRect border);

	void Update(float fDeltaTime) override;

private:
	void horizontalCollisionCheck(std::shared_ptr<BoxColliderComponent> collisioncomp);
	void verticalCollisionCheck(std::shared_ptr<BoxColliderComponent> collisioncomp);

	sf::FloatRect m_border;
};