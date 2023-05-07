#pragma once
#include "IGraphicsComponent.h"
#include <unordered_map>

//mapt.com;

class AnimationComponent : public IGraphicsComponent
{
public:
	//ctor
	AnimationComponent(GameObject& gameObject, const shared_ptr<Texture>& textureSheet_, const sf::Vector2u& imageCount_, const float switchTime_);

	void Update(float fDeltaTime) override;
	void Draw(sf::RenderWindow& window) override;
	sf::IntRect m_uvRect;

	/// <summary>
	/// we need to override getWidth and height because otherwise we'd return the size of the sprite sheet when asking for the texture's size;
	/// </summary>
	/// <returns></returns>
	float getWidth() const override;
	float getHeight() const override;

	void addAnimation(const string& animationName, const int row, const int columnCount);
	void playAnimation(const string& animationName, float switchTime);
	void playAndLockAnimation(const string& animationName, float switchTime);
	void playAnimationLoop(const string& animationName, float switchTime);

private:
	Sprite m_sprite;
	shared_ptr<Texture> m_textureSheet;
	sf::Vector2u m_imageCount;
	sf::Vector2u m_currentImage;
	float m_switchTime;
	float m_totalTime;
	bool m_lockAnimationEnd;

	std::vector<string> m_animationToPlay;
	std::unordered_map<string, pair<int, int>> m_animations;
};