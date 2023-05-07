#include "stdafx.h"
#pragma once
#include "AnimationManager.h"

AnimationManager& AnimationManager::getInstance()
{
	if (m_instance == nullptr)
		m_instance = new AnimationManager();
	return *m_instance;
}

AnimationManager* AnimationManager::m_instance = nullptr;

void AnimationManager::release()
{
	// resharper: if statement has no effect, deleting nullpointer does nothing;
	if (m_instance != nullptr)
		delete m_instance;
	m_instance = nullptr;
}

void AnimationManager::AddComponent(shared_ptr<AnimationComponent> component, const string ownerID)
{
	const auto keyExists = m_animationComponents.find(ownerID);
	if (keyExists == m_animationComponents.end()) { //does not exist
		m_animationComponents.insert({ ownerID, component });
	}
}

shared_ptr<AnimationComponent> AnimationManager::GetComponent(const string ownerID) const
{
	auto keyExists = m_animationComponents.find(ownerID);
	if (keyExists == m_animationComponents.end()) { //does not exist
		throw std::invalid_argument("received wrong ownerID");
	}

	return keyExists->second;
}

void AnimationManager::reset()
{
	m_animationComponents.clear();
}