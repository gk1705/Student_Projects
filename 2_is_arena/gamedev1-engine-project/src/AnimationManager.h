#pragma once
#include <memory>
#include <unordered_map>
#include "Animationcomponent.h"

using namespace std;

class AnimationManager
{
public:
	static AnimationManager& getInstance();
	static void release();
	void AddComponent(shared_ptr<AnimationComponent> component, const string ownerID);
	shared_ptr<AnimationComponent> GetComponent(const string ownerID) const;

	void reset();

private:
	AnimationManager(void) = default;
	~AnimationManager(void) = default;
	AnimationManager(const AnimationManager& p) = delete;
	AnimationManager& operator=(AnimationManager const&) = delete;

	static AnimationManager *m_instance;
	unordered_map <string, shared_ptr<AnimationComponent>> m_animationComponents;
};