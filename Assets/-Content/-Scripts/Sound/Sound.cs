using UnityEngine;

public class Sound
{
	public AudioClip clip;
	public AudioChannel channel;
	public bool doLoop;

	public Sound(AudioClip newClip, AudioChannel newChannel, bool newDoLoop = false)
	{
		clip = newClip;
		channel = newChannel;
		doLoop = newDoLoop;
	}
}