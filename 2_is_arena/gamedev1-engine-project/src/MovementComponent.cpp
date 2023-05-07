#include "stdafx.h"
#include "MovementComponent.h"
#include "AController.h"

void MovementComponent::setStrategy(shared_ptr<AController> strategy)
{
	strategy->setID(m_id);
	m_controller = strategy;
}

AController& MovementComponent::getStrategy() const
{
	return *m_controller;
}

void MovementComponent::Update(float fDeltaTime)
{
	m_controller->Update(fDeltaTime);
}
