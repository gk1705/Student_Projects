#include "stdafx.h"
#include "DirectionalAnimator.h"
#include "AnimationManager.h"

void DirectionalAnimator::playDirectionalAnimation(const AController& controller,
	const Vector2i& currentMovementDirection) const
{
	if (currentMovementDirection == Vector2i(1, 0))
	{
		AnimationManager::getInstance().GetComponent(controller.getGObject().getID())->playAnimation(m_animName + "_right", m_frameDuration);
	}
	else if (currentMovementDirection == Vector2i(-1, 0))
	{
		AnimationManager::getInstance().GetComponent(controller.getGObject().getID())->playAnimation(m_animName + "_left", m_frameDuration);
	}
	else if (currentMovementDirection == Vector2i(0, 1))
	{
		AnimationManager::getInstance().GetComponent(controller.getGObject().getID())->playAnimation(m_animName + "_front", m_frameDuration);
	}
	else if (currentMovementDirection == Vector2i(0, -1))
	{
		AnimationManager::getInstance().GetComponent(controller.getGObject().getID())->playAnimation(m_animName + "_back", m_frameDuration);
	}
	else
		throw std::invalid_argument("received wrong or no direction.");
}
