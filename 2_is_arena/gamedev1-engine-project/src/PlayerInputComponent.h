#pragma once
#include <string>
#include "IComponent.h"

class GameObject;

class PlayerInputComponent : public IComponent {
public:
	PlayerInputComponent(GameObject& gameObject, int id, float speed)
		: IComponent(gameObject)
		, m_speed(speed)
		, m_id(id)
	{
	}

	virtual void Update(float fDeltaTime) override;

private:
	int m_id;
	float m_speed;
};