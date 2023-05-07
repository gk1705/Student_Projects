#include "stdafx.h"
#pragma once
#include "AnimationComponent.h"
#include "InputManager.h"
#include "PlayerStatsComponent.h"

AnimationComponent::AnimationComponent(GameObject& gameObject, const shared_ptr<Texture>& textureSheet_, const sf::Vector2u& imageCount_, const float switchTime_)
	: IGraphicsComponent(gameObject)
	, m_textureSheet(textureSheet_)
	, m_imageCount(imageCount_)
	, m_switchTime(switchTime_)
	, m_totalTime(0)
	, m_lockAnimationEnd(false)
	, m_currentImage(Vector2u(0, 0))
{
	m_uvRect.width = m_textureSheet->getSize().x / m_imageCount.x;
	m_uvRect.height = m_textureSheet->getSize().y / m_imageCount.y;

	m_sprite.setTexture(*m_textureSheet);
	m_sprite.setTextureRect(m_uvRect);
}

/// <summary>
/// Switch column if a certain time has passed. -> animation progression
/// we may lock the animation at the last frame, if specified via playAndLockAnimation (eg. Death Animation);
/// </summary>
/// <param name="fDeltaTime"></param>
void AnimationComponent::Update(float fDeltaTime)
{
	m_totalTime += fDeltaTime;

	if (m_totalTime >= m_switchTime)
	{
		m_totalTime -= m_switchTime;

		// check if ought to lock animation at last frame;
		if (!m_lockAnimationEnd || m_currentImage.x != m_imageCount.x - 1)
		{
			m_currentImage.x = (m_currentImage.x + 1) % m_imageCount.x;
		}
	}

	m_uvRect.left = m_currentImage.x * m_uvRect.width;
	m_uvRect.top = m_currentImage.y * m_uvRect.height;

	m_sprite.setTextureRect(m_uvRect);
}

void AnimationComponent::Draw(sf::RenderWindow& window)
{
	m_sprite.setPosition(m_gameObject.GetPosition());
	m_sprite.setColor(Color::White);
	if (m_gameObject.getComponent<PlayerStatsComponent>()->IsDamaged()) {
		m_sprite.setColor(Color(255, 150, 150));
	}
	else if (m_gameObject.getComponent<PlayerStatsComponent>()->IsSlowed()){
		m_sprite.setColor(Color(150, 150, 255));
	}
	window.draw(m_sprite);
}

float AnimationComponent::getWidth() const
{
	return m_uvRect.width;
}

float AnimationComponent::getHeight() const
{
	return m_uvRect.height;
}

void AnimationComponent::addAnimation(const string& animationName, const int row, const int columnCount)
{
	m_animations.insert({ animationName,{ row, columnCount } });
}

// switchTime specifies how much time should pass until the uvRect advances one image on the sprite sheet
// current image is set via previously saved kvps (string, Vector2) in the map m_animations
void AnimationComponent::playAnimation(const string& animationName, float switchTime)
{
	const auto key = m_animations.find(animationName);

	if (key == m_animations.end())
	{
		throw std::invalid_argument("received wrong animationName");
	}

	m_currentImage.y = key->second.first;
	m_currentImage.x = 0;

	m_imageCount.x = key->second.second;
	m_totalTime = 0;

	m_switchTime = switchTime;
}

void AnimationComponent::playAndLockAnimation(const string& animationName, float switchTime)
{
	m_lockAnimationEnd = true;
	playAnimation(animationName, switchTime);
}

void AnimationComponent::playAnimationLoop(const string& animationName, float switchTime)
{
	m_lockAnimationEnd = false;
	playAnimation(animationName, switchTime);
}