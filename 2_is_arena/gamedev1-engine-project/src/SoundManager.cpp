#include "stdafx.h"
#include "SoundManager.h"

SoundManager* SoundManager::m_instance = nullptr;

SoundManager& SoundManager::getInstance()
{
	if (m_instance == nullptr)
		m_instance = new SoundManager();
	return *m_instance;
}

void SoundManager::release()
{
	if (m_instance != nullptr)
		delete m_instance;
	m_instance = nullptr;
}

void SoundManager::AddSound(const string& name, const string& path)
{
	SoundBuffer soundBuffer;

	if (!soundBuffer.loadFromFile(path)) 
	{
		throw std::invalid_argument("Can't load file from: " + path);
	}

	m_bufferedSounds.insert({ name, soundBuffer });
}

void SoundManager::PlaySound(const string& name, const string& channel)
{
	const auto key = m_bufferedSounds.find(name);

	if (key == m_bufferedSounds.end()) 
	{
		throw std::invalid_argument("Can't find sound.");
	}

	if (channel == "first") {
		m_sound.setBuffer(key->second);
		m_sound.play();
	}
	else if (channel == "second") {
		m_soundSecond.setBuffer(key->second);
		m_soundSecond.play();
	}
	else {
		throw std::invalid_argument("Channel doesn't exist.");
	}
}

void SoundManager::PlayMusic(const string& path)
{
	if (!m_music.openFromFile(path)) 
	{
		throw std::invalid_argument("Can't find music.");
	}

	m_music.setVolume(20.f);
	m_music.setLoop(true);
	m_music.play();
}

void SoundManager::StopMusic() 
{
	m_music.stop();
}

bool SoundManager::SoundIsCurrentlyPlaying() const
{
	return m_sound.getStatus() == Sound::Playing;
}

void SoundManager::reset()
{
	m_bufferedSounds.clear();
}