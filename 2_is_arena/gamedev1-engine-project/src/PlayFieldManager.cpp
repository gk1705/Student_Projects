#include "stdafx.h"
#include "Debug.h"
#include "PlayFieldManager.h"

PlayFieldManager* PlayFieldManager::m_instance = nullptr;

PlayFieldManager& PlayFieldManager::getInstance() {
	if (m_instance == nullptr) {
		m_instance = new PlayFieldManager();
	}
	return *m_instance;
}

void PlayFieldManager::release()
{
	if (m_instance != nullptr)
		delete m_instance;
	m_instance = nullptr;
}

// this is where we save a reference to playfield specific objects
void PlayFieldManager::initFieldArrays()
{
	m_controllerObjectsInPlayfield.insert({ "Field1", unordered_map<string, shared_ptr<GameObject>>() });
	m_controllerObjectsInPlayfield.insert({ "Field2", unordered_map<string, shared_ptr<GameObject>>() });
}

// later used to query playfield boundaries
void PlayFieldManager::addPlayfield(FloatRect playfield)
{
	m_playfields.push_back(playfield);
}

// insert into playfield specific map
void PlayFieldManager::addObjectToPlayfield(const string& fieldName, const shared_ptr<GameObject>& object)
{
	const auto keyExists = m_controllerObjectsInPlayfield.find(fieldName);
	if (keyExists == m_controllerObjectsInPlayfield.end()) { //does not exist
		throw std::invalid_argument("received wrong fieldName.");
	}

	// add if found;
	auto fieldMap = keyExists->second;
	const auto keyExists2 = fieldMap.find(object->getID());
	if (keyExists2 != fieldMap.end()) { //already exists
		throw std::invalid_argument("object already exists in this field.");
	}

	m_controllerObjectsInPlayfield[fieldName].insert({ object->getID(), object });
}

// switch playfield map based on what playfield object is currently in
void PlayFieldManager::objectSwitchPlayfield(const string& objectID)
{
	// string -> which field are you currently in?
	string currentlyIn;
	shared_ptr<GameObject> objectToMove = nullptr;

	for (const auto& field : m_controllerObjectsInPlayfield)
	{
		auto fieldMap = field.second;
		const auto keyExists = fieldMap.find(objectID);

		if (keyExists != fieldMap.end())
		{
			objectToMove = keyExists->second;
			currentlyIn = field.first;
			break;
		}
	}

	// has not been found anywhere -> error
	if (!objectToMove)
	{
		throw std::invalid_argument("Object doesn't exist in any playfield.");
	}

	if (currentlyIn == "Field1")
	{
		//switch to field2
		m_controllerObjectsInPlayfield[currentlyIn].erase(objectToMove->getID());
		m_controllerObjectsInPlayfield["Field2"].insert({ objectToMove->getID(), objectToMove });
	}

	if (currentlyIn == "Field2")
	{
		//switch to field1
		m_controllerObjectsInPlayfield[currentlyIn].erase(objectToMove->getID());
		m_controllerObjectsInPlayfield["Field1"].insert({ objectToMove->getID(), objectToMove });
	}
}

// query object by id
string PlayFieldManager::objectCurrentlyAt(const string& objectID)
{
	string currentlyIn = "error";
	for (const auto& field : m_controllerObjectsInPlayfield)
	{
		auto fieldMap = field.second;
		const auto keyExists = fieldMap.find(objectID);

		if (keyExists != fieldMap.end())
		{
			currentlyIn = field.first;
			break;
		}
	}

	if (currentlyIn == "error")
	{
		throw std::invalid_argument("Object could not be found in playfield.");
	}

	return currentlyIn;
}

// TODO: change to a more reliable, resource friendly option
Vector2i PlayFieldManager::returnSpawnPosition(const string& playfieldName, const Vector2f playerPosition) const
{
	// workaround
	FloatRect rectangle;
	if (playfieldName == "Field1") {
		rectangle = getPlayfieldsRect()[0];
	}
	else if (playfieldName == "Field2") 
	{
		rectangle = getPlayfieldsRect()[1];
	}
	else
	{
		throw std::invalid_argument("Rect hasn't been found.");
	}

	//generate random spawn position!
	// magic value -> left top of entity is position, correction needed;
	const auto min_x = rectangle.left + 100;
	const auto max_x = rectangle.left + rectangle.width - 100;

	const auto min_y = rectangle.top + 100;
	const auto max_y = rectangle.top + rectangle.height - 100;
	
	float r1 = rand();
	float r2 = rand();

	int newXPos = rand() % static_cast<int>((max_x - min_x + 1)) + min_x;
	int newYPos = rand() % static_cast<int>((max_y - min_y + 1)) + min_y;

	const auto getDistBetweenVec = [](Vector2f vec1, Vector2i vec2) -> float
	{
		return sqrt((vec1.x - vec2.x) * (vec1.x - vec2.x) + (vec1.y - vec2.y) * (vec1.y - vec2.y));
	};

	// redo if distance between enemy and player is too small;
	while (getDistBetweenVec(playerPosition, Vector2i(newXPos, newYPos)) <= 500)
	{
		newXPos = rand() % static_cast<int>((max_x - min_x + 1)) + min_x;
		newYPos = rand() % static_cast<int>((max_y - min_y + 1)) + min_y;
	}

	return Vector2i(newXPos, newYPos);
}

vector<FloatRect> PlayFieldManager::getPlayfieldsRect() const
{
	return m_playfields;
}

// query player by playfield name
shared_ptr<GameObject> PlayFieldManager::getPlayerCurrentlyAt(const string& fieldName)
{
	shared_ptr<GameObject> player = nullptr;
	for (const auto& gObjectPair : m_controllerObjectsInPlayfield[fieldName])
	{
		if (gObjectPair.second->getType() == "PlayerObject")
		{
			player = gObjectPair.second;
		}
	}

	if (!player)
	{
		throw std::invalid_argument("Player could not be found in playfield.");
	}

	return player;
}

// clearState the two playfield specific maps we use to save references to game objects
void PlayFieldManager::reset()
{
	for (auto& field : m_controllerObjectsInPlayfield)
	{
		field.second.clear();
	}
	
	m_controllerObjectsInPlayfield.clear();
}

int& PlayFieldManager::getScore(int idx)
{
	if (m_playerscore.empty()) {
		m_playerscore.push_back(0);
		m_playerscore.push_back(0);
	}

	return m_playerscore[idx];
}
