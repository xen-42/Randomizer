﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NHRandomizer.Utility;

internal class DebugCommands : MonoBehaviour
{
	private readonly static Key _debugKey = Key.I;
	private readonly static Key _warpKey = Key.O;

	private readonly List<(Key key, Action action)> _debugCommands = new();
	private readonly List<(Key key, Action action)> _debugWarpCommands = new();

	private ScreenPrompt _debugPrompt;
	private readonly List<ScreenPrompt> _debugPromptList = new();

	private ScreenPrompt _warpPrompt;
	private readonly List<ScreenPrompt> _warpPromptList = new();

	public void Start()
	{
		_debugPrompt = PromptUtils.AddPrompt("Debug Commands", PromptPosition.UpperRight, _debugKey);
		_warpPrompt = PromptUtils.AddPrompt("Warp Commands", PromptPosition.UpperRight, _warpKey);

		RegisterDebugWarpCommand(Key.Numpad0, () => WarpUtils.WarpToPlanet(AstroObject.Name.GiantsDeep, Vector3.up * 100f), "Warp to Giants Deep Core");
		RegisterDebugWarpCommand(Key.Numpad1, () => 
			{
				WarpUtils.WarpToPlanet(AstroObject.Name.TowerTwin, Vector3.left * 10f);
				GlobalMessenger<OWRigidbody>.FireEvent("EnterTimeLoopCentral", Locator.GetPlayerBody());
			}, "Warp to Ash Twin Core");
		RegisterDebugWarpCommand(Key.Numpad2, () => WarpUtils.WarpToPlanet(AstroObject.Name.Comet, new Vector3(13.9f, 22.8f, 37.2f)), "Warp to Interloper Core");
	}

	public void Update()
	{
		if (Keyboard.current[_debugKey].wasPressedThisFrame)
		{
			foreach (var prompt in _debugPromptList)
			{
				prompt.SetVisibility(true);
			}
		}
		else if (Keyboard.current[_debugKey].wasReleasedThisFrame)
		{
			foreach (var prompt in _debugPromptList)
			{
				prompt.SetVisibility(false);
			}
		}

		if (Keyboard.current[_debugKey].isPressed)
		{
			foreach (var (key, action) in _debugCommands)
			{
				if (Keyboard.current[key].wasReleasedThisFrame)
				{
					action.Invoke();
				}
			}
		}

		if (Keyboard.current[_warpKey].wasPressedThisFrame)
		{
			foreach (var prompt in _warpPromptList)
			{
				prompt.SetVisibility(true);
			}
		}
		else if (Keyboard.current[_warpKey].wasReleasedThisFrame)
		{
			foreach (var prompt in _warpPromptList)
			{
				prompt.SetVisibility(false);
			}
		}

		if (Keyboard.current[_warpKey].isPressed)
		{
			foreach (var (key, action) in _debugWarpCommands)
			{
				if (Keyboard.current[key].wasReleasedThisFrame)
				{
					action.Invoke();
				}
			}
		}

		var buttonsPressed = Keyboard.current[_debugKey].isPressed || Keyboard.current[_warpKey].isPressed;
		_warpPrompt.SetVisibility(!buttonsPressed);
		_debugPrompt.SetVisibility(!buttonsPressed);
	}

	private void RegisterDebugCommand(Key key, Action action, string description)
	{
		_debugCommands.Add((key, action));
		_debugPromptList.Add(PromptUtils.AddPrompt(description, PromptPosition.UpperRight, key));
	}

	private void RegisterDebugWarpCommand(Key key, Action action, string description)
	{
		_debugWarpCommands.Add((key, action));
		_warpPromptList.Add(PromptUtils.AddPrompt(description, PromptPosition.UpperRight, key));
	}

	public static void OpenSarcophagus()
	{
		var sarcophagusController = GameObject.FindObjectOfType<SarcophagusRingworldController>();
		sarcophagusController.OnFirstSealExtinguish();
		sarcophagusController.OnSecondSealExtinguish();
		sarcophagusController.OnThirdSealExtinguish();
	}
}
