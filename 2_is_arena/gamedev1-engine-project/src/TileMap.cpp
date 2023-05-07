#include "stdafx.h"
#pragma once
#include "GameObject.h"
#include "TileMap.h"
#include "NLTmxMap.h"
#include <unordered_map>
#include "LayerComponent.h"
#include "RigidbodyComponent.h"
#include "BoxColliderComponent.h"
#include "CollisionResolverObserver.h"
#include "RenderManager.h"
#include "GameObjectManager.h"
#include "PlayFieldManager.h"

using namespace sf;
using namespace std;

namespace {
	int countid = 1; // 0 is for input-types like "changeGameState"

	Color hexToRGB(String hexadezimal) {
		int v[6];
		for (int i = 0; i < 6; i++) {
			switch (hexadezimal[3 + i]) {
			case 'f': v[i] = 15; break;
			case 'e': v[i] = 14; break;
			case 'd': v[i] = 13; break;
			case 'c': v[i] = 12; break;
			case 'b': v[i] = 11; break;
			case 'a': v[i] = 10; break;
			case '9': v[i] = 9; break;
			case '8': v[i] = 8; break;
			case '7': v[i] = 7; break;
			case '6': v[i] = 6; break;
			case '5': v[i] = 5; break;
			case '4': v[i] = 4; break;
			case '3': v[i] = 3; break;
			case '2': v[i] = 2; break;
			case '1': v[i] = 1; break;
			case '0': v[i] = 0; break;
			}
		}

		const int redvalue = 16 * v[0] + 1 * v[1];
		const int greenvalue = 16 * v[2] + 1 * v[3];
		const int bluevalue = 16 * v[4] + 1 * v[5];

		Color color;
		color.r = redvalue;
		color.g = greenvalue;
		color.b = bluevalue;

		return color;
	}
}

vector<shared_ptr<GameObject>> TileMap::LoadMap(const string& filename, const Vector2f& offset, sf::RenderWindow& window) {
	std::vector<shared_ptr<GameObject>> gameObjects;
	srand(time(NULL));
	unordered_map<std::string, shared_ptr<Texture>> tilesetTexture;
	//maybe change the id -> to static counter, 5 as argument is error prone;
	shared_ptr<GameObject> map = make_shared<GameObject>("Map", sf::Vector2f(0, 0));
	int width, height;

	FileInputStream mapStream;
	if (!mapStream.open(filename))
	{
		err() << "loadMap: could not open file " << filename << endl;
		return gameObjects;
	}

	// convert FileInputStream to char* mapBuffer
	char* mapBuffer = new char[mapStream.getSize() + 1];
	mapStream.read(mapBuffer, mapStream.getSize());
	mapBuffer[mapStream.getSize()] = '\0';
	
	// now lets load a NLTmxMap
	NLTmxMap* tilemap = NLLoadTmxMap(mapBuffer);
	//delete mapBuffer;

	err() << "Load tilemap with size: " << tilemap->width << ", "
		<< tilemap->height << " and tilesize: " << tilemap->tileWidth
		<< ", " << tilemap->tileHeight << std::endl;

	// load textures for every tileset
	for (auto tileset : tilemap->tilesets)
	{
		err() << "Load tileset: " << tileset->name << " width filename: "
			<< tileset->filename << " and tilesize: " << tileset->tileWidth
			<< ", " << tileset->tileHeight << std::endl;

		shared_ptr<Texture> texture = make_shared<Texture>();
		if (!texture->loadFromFile("../assets/Levels/" + tileset->filename))
			err() << "Could not load texture " << "../assets/Levels/" + tileset->filename << endl;
		tilesetTexture[tileset->name] = texture;
		m_textures.push_back(texture);
	}

	vector<shared_ptr<LayerComponent>> layers(tilemap->layers.size());

	for (int layerIdx = 0; layerIdx < (int)tilemap->layers.size(); layerIdx++)
	{
		NLTmxMapLayer* layer = tilemap->layers[layerIdx];
		err() << "Load layer: " << layer->name << " with width: "
			<< layer->width << " and height: " << layer->height << std::endl;

		int size = layer->width * layer->height;

		layers[layerIdx] = make_shared<LayerComponent>(*map);
		// go over all elements in the layer
		for (int i = 0; i < size; i++)
		{
			int grid = layer->data[i];

			if (grid == 0)
			{
				continue;
			}

			// get tileset and tileset texture
			NLTmxMapTileset* tileset = tilemap->getTilesetForGrid(grid);
			Vector2i tileSize(tilemap->tileWidth, tilemap->tileHeight);
			shared_ptr<Texture> texture = tilesetTexture[tileset->name];

			// horizontal tile count in tileset texture
			int tileCountX = texture->getSize().x / tileSize.x;

			// calcualte position of tile
			Vector2f position;
			position.x = (i % layer->width) * (float)tileSize.x;
			position.y = (i / layer->width) * (float)tileSize.y;
			position += offset;

			width = layer->width * tileSize.x;
			height = layer->height * tileSize.y;

			// calculate 2d idx of tile in tileset texture
			int idx = grid - tileset->firstGid;
			int idxX = idx % tileCountX;
			int idxY = idx / tileCountX;

			// calculate source area of tile in tileset texture
			IntRect source(idxX * tileSize.x, idxY * tileSize.y, tileSize.x, tileSize.y);

			shared_ptr<Sprite> sprite = make_shared<Sprite>();

			sprite->setTexture(*texture);
			sprite->setTextureRect(source);
			sprite->setPosition(position.x, position.y);

			layers[layerIdx]->Insert(sprite);
		}

		map->AddRenderComponent(layers[layerIdx]);
	}

	RenderManager::getInstance().AddComponent(map->getRenderComponentByIdx(0), map->getID(), 0);
	RenderManager::getInstance().AddComponent(map->getRenderComponentByIdx(1), map->getID(), 0);

	gameObjects.push_back(map);

	// go through all object layers
	for (auto group : tilemap->groups)
	{
		// go over all objects per layer
		for (auto object : group->objects)
		{
			Vector2f position(object->x, object->y);
			position += offset;
			shared_ptr<GameObject> gameObject;

			if (object->type == "Trigger") {
				gameObject = createTrigger(object);
			} else if (object->type == "FixedObject" || object->type == "DestructibleObject") {
				gameObject = createFixedObject(object);	
			}
			else { //not a gameObject
				if (object->type == "Playfield") {//no gameObject, just adding bordercomponent to player
					PlayFieldManager::getInstance().addPlayfield(FloatRect(object->x, object->y, object->width, object->height));
				}
				continue;
			}
			gameObject->setType(object->type);
			gameObjects.push_back(gameObject);
			GameObjectManager::getInstance().addGameObject(gameObject);

		}
	}
	int i = 0;
	delete tilemap;
	delete mapBuffer;
	return gameObjects;
}

vector<FloatRect> TileMap::GetPlayFields() const
{
	return m_playfields;
}

shared_ptr<GameObject> TileMap::createTrigger(NLTmxMapObject* object) const
{
	sf::Vector2f position(object->x, object->y);
	shared_ptr<GameObject> trigger = make_shared<GameObject>(object->name, position);

	//Add BoxCollider
	trigger->AddCollisionComponent(make_shared<BoxColliderComponent>(*trigger, sf::FloatRect(position, sf::Vector2f(object->width, object->height)), sf::Vector2f(0, 0)));
	trigger->AddRigidbody(make_shared<RigidbodyComponent>(*trigger, 0));
	trigger->getCollisionComponents()[0]->SetTrigger(true);

	return trigger;
}

shared_ptr<GameObject> TileMap::createFixedObject(NLTmxMapObject* object) const
{
	sf::Vector2f position(object->x, object->y);
	shared_ptr<GameObject> gameObject = make_shared<GameObject>(object->name, position);

	//Add BoxCollider
	gameObject->AddCollisionComponent(make_shared<BoxColliderComponent>(*gameObject, sf::FloatRect(position, sf::Vector2f(object->width, object->height))));
	gameObject->AddRigidbody(make_shared<RigidbodyComponent>(*gameObject, 0));

	gameObject->getRigidbody()->registerObserver(make_shared<CollisionResolverObserver>(object->name));

	return gameObject;
}

shared_ptr<GameObject> TileMap::createPlayField(NLTmxMapObject* object) const
{
	sf::Vector2f position(object->x, object->y);
	shared_ptr<GameObject> gameObject = make_shared<GameObject>(object->name, position);
	gameObject->AddCollisionComponent(make_shared<BoxColliderComponent>(*gameObject, sf::FloatRect(position, sf::Vector2f(object->width, object->height)), sf::Vector2f(0, 0)));
	gameObject->getCollisionComponents()[0]->SetTrigger(true);
	return gameObject;
}
