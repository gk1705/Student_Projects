#pragma once
#include <string>
#include <unordered_map>
#include "GameObject.h"

using namespace std;
using namespace sf;

class PlayFieldManager
{
public:
	static PlayFieldManager& getInstance();
	static void release();
	void initFieldArrays();

	void addPlayfield(FloatRect playfield);
	void addObjectToPlayfield(const string& fieldName, const shared_ptr<GameObject>& object);
	void objectSwitchPlayfield(const string& objectID);
	string objectCurrentlyAt(const string& objectID);
	Vector2i returnSpawnPosition(const string& playfieldName, const Vector2f playerPosition) const;

	vector<FloatRect> getPlayfieldsRect() const;
	shared_ptr<GameObject> getPlayerCurrentlyAt(const string& fieldName);

	void reset();
	int& getScore(int idx);

	PlayFieldManager(const PlayFieldManager& p) = delete;
	PlayFieldManager& operator=(PlayFieldManager const&) = delete;

private:
	PlayFieldManager(void) = default;
	~PlayFieldManager(void) = default;

	static PlayFieldManager *m_instance;

	unordered_map<string, unordered_map<string, shared_ptr<GameObject>>> m_controllerObjectsInPlayfield;
	vector<FloatRect> m_playfields;
	vector<int> m_playerscore;
};