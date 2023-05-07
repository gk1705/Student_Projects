#pragma once
#include "GameObject.h"
#include "PlayerStatsComponent.h"

class AController
{
public:
	virtual ~AController() = default;

	AController(GameObject& gameObject, shared_ptr<PlayerStatsComponent> stats)
		: m_gameObject(gameObject)
		, m_stats(stats)
	{
	}

	virtual void Update(const float& fDeltaTime) = 0;
	virtual void ChangeState(const string& stateName) = 0;

	void setID(int id);
	GameObject& getGObject() const;
	float getSpeed() const;
	float getID() const;
	bool isHpZero() const;
	void resetHP() const;

protected:
	static Vector2i findNormDirection(const Vector2f& inputVector);

	GameObject & m_gameObject;
	shared_ptr<PlayerStatsComponent> m_stats;
	float m_id;
};