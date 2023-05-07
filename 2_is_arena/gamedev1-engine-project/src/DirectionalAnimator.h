#pragma once
#include <utility>
#include "AController.h"

class DirectionalAnimator
{
public:
	DirectionalAnimator(std::string animName, const float& frameDuration)
		: m_animName(std::move(animName))
		, m_frameDuration(frameDuration)
	{
		
	}

	void playDirectionalAnimation(const AController& controller, const Vector2i& currentMovementDirection) const;

private:
	std::string m_animName;
	float m_frameDuration;
};