#include "stdafx.h"
#pragma once
#include "DirectionalAnimator.h"

class HumanController;

class APlayerState
{
public:
	virtual void handleState(HumanController& humanController, sf::Vector2f tilt, sf::Vector2i dir, float deltaTime) = 0;
	virtual void onEnter(HumanController& humanController, const sf::Vector2i& dirLeft, const sf::Vector2i& dirRight) = 0;

	virtual ~APlayerState() = default;

	APlayerState(const std::string& animName, const float frameDuration)
		: m_dirAnimator(std::make_shared<DirectionalAnimator>(DirectionalAnimator(animName, frameDuration)))
	{
		
	}

protected:
	void playDirectionalAnimation(const AController& controller, const Vector2i& currentMovementDirection) const;

private:
	std::shared_ptr<DirectionalAnimator> m_dirAnimator;
};