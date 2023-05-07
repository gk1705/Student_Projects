#pragma once
#include "GameObject.h"
#include "NLTmxMap.h"
#include "AnimationComponent.h"

class TileMap
{
public:
	std::vector<shared_ptr<GameObject>> LoadMap(const std::string& filename, const sf::Vector2f& offset, sf::RenderWindow& window);
	vector<FloatRect> GetPlayFields() const;
private:
	vector<shared_ptr<Texture>> m_textures;
	vector<FloatRect> m_playfields;

	//Different Objects in Tiled:
	//shared_ptr<GameObject> createPlayerObject(NLTmxMapObject* object);
	shared_ptr<GameObject> createTrigger(NLTmxMapObject* object) const;
	shared_ptr<GameObject> createFixedObject(NLTmxMapObject* object) const;
	shared_ptr<GameObject> createPlayField(NLTmxMapObject* object) const;
};
