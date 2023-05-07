#pragma once
#include "IComponent.h"

class DespawnComponent : public IComponent {
public:
	DespawnComponent(GameObject& gameObject, float despawnTime);

	void Update(float fDeltaTime) override;

private:
	const float m_despawnTime;
	float m_timer = 0.0f;
};