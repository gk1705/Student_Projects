#pragma once
#include "stdafx.h"
#include "AController.h"


class MovementComponent : public IComponent
{
public:
	MovementComponent(GameObject& gameObject, const int id)
		:IComponent(gameObject)
		, m_id(id)
	{

	}

	MovementComponent(GameObject& gameObject)
		:IComponent(gameObject)
		, m_id(0)
	{

	}

	// setter
	// set controller -> state machine
	void setStrategy(shared_ptr<AController> strategy);

	// getter
	AController& getStrategy() const;

	void Update(float fDeltaTime) override;

private:
	shared_ptr<AController> m_controller;
	int m_id;
};