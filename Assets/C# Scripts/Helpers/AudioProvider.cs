﻿using System;
using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class AudioProvider
{
    private Dictionary<string, AudioClip> cache;
    private AudioProvider()
    { }
    private static AudioProvider instance;
    private AudioSource cachedSource;
	private AudioSource bg;

    private void makeCache()
    {
        if (cache == null)
        {
            cache = new Dictionary<string, AudioClip>();
        }
    }
    public void playAudio(string name)
    {
        if (cachedSource == null)
        {
            GameObject go = GameObject.Find("SoundPlayer");
			if (go == null) {
				return;
			}
            cachedSource = go.GetComponent<AudioSource>();
        }
        playAudio(name, cachedSource, false);
    }

    public void playAudio(string name, AudioSource source, bool loop = false)
    {
		if (PlayerPrefs.GetInt ("audio_mute") != 1) {
			source.clip = getAudio (name);
			source.loop = loop;
			source.Play ();
		}
    }

	public void playBackground(string name){
		GameObject go = GameObject.Find("BackgroundMusic");;
		if (go == null) {
			return;
		}
		bg = go.GetComponent<AudioSource> ();

		bg.clip = getAudio (name);
		bg.loop = true;
		if (PlayerPrefs.GetInt ("audio_mute") != 1) {
			bg.Play ();
		}
	}
	public bool playBackground(){
		if (bg != null) {
			bg.Play ();
			return true;
		}
		return false;
	}
	public bool stopBackground(){
		if (bg != null) {
			bg.Pause ();
			return true;
		}
		return false;
	}

    private AudioClip getAudio(string name)
    {
        makeCache();

        AudioClip gameObj = getCached(name);
        if (gameObj != null)
        {
            return gameObj;
        }

        gameObj = getResource(name);
        cache.Add(name, gameObj);
        return gameObj;
    }

    private AudioClip getCached(string id)
    {

        if (cache.ContainsKey(id))
        {
            return cache[id];
        }

        return null;
    }

    public static AudioProvider getInstance()
    {
        if (instance == null)
        {
            instance = new AudioProvider();
        }
        return instance;
    }
    private AudioClip getResource(string name)
    {
        XmlDocument doc = XmlHelper.getXml();
        XmlNode node = doc.SelectSingleNode("//audio[@name = '" + name + "']");
        if (node == null)
        {
            Debug.LogError("Audio:" + name + " not found!");
            return null;
        }
        string file = node.Attributes["file"].Value;

        return (AudioClip)Resources.Load("Audio/" + file);
    }
}


