#pragma once
#include <unordered_map>
#include "SFML/Audio.hpp"

using namespace std;
using namespace sf;

class SoundManager
{
public:
	static SoundManager& getInstance();
	static void release();
	void AddSound(const string& name, const string& path);
	void PlaySound(const string& name, const string& channel);
	void PlayMusic(const string& path);
	void StopMusic();
	bool SoundIsCurrentlyPlaying() const;
	void reset();

	SoundManager(const SoundManager& p) = delete;
	SoundManager& operator=(SoundManager const&) = delete;

private:
	SoundManager(void) = default;
	~SoundManager(void) = default;

	unordered_map<string, SoundBuffer> m_bufferedSounds;
	Sound m_sound;
	Sound m_soundSecond;
	Music m_music;

	static SoundManager *m_instance;
};