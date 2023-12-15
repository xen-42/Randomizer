using UnityEngine;

namespace NHRandomizer;

public static class LocationCreator
{
	/*
	 * Critical locations to progress the plot:
	 * Sunken module -> Ash Twin Project -> Vessel
	 * 
	 * Vessel is too big to fit it into any of the other planets
	 * 
	 * Can switch the contents of the cores of each planet:
	 * Ash Twin - Ash Twin Project
	 * Ember Twin - Nothing
	 * Timber Hearth - Mining Rig
	 * Brittle Hollow - Black hole
	 * Giants Deep - Sunken Module
	 * Interloper - Ghost matter core
	 * 
	 * Interloper can fit the sunken module but thats about it, must be at new Vector3(-1.1964f, -2.8294f, -5.4182f), Quaternion.Euler(58.2396f, 337.1563f, 0)
	 * 
	 */

	public const string MINING_RIG = "MiningRig_Body";
	public const string SUNKEN_MODULE = "GiantsDeep_Body/Sector_GD/Sector_GDInterior/Sector_GDCore/Sector_Module_Sunken";
	public const string TIME_LOOP_RING = "TimeLoopRing_Body";
	public const string TIME_LOOP_CORE = "TowerTwin_Body/Sector_TowerTwin/Sector_TimeLoopInterior";

	public static void RemoveOriginals()
	{
		foreach (var original in new string[] { MINING_RIG, SUNKEN_MODULE, TIME_LOOP_RING, TIME_LOOP_CORE })
		{
			GameObject.Find(original).SetActive(false);
		}
	}

	public static void CreateTrackingModule(GameObject parent, Sector sector, Vector3 position, Quaternion rotation)
	{
		NHRandomizer.NewHorizonsAPI.SpawnObject(parent, sector, SUNKEN_MODULE, position, rotation.eulerAngles, 1f, false);
	}

	public static void CreateMiningRig(GameObject parent, Sector sector, Vector3 position, Quaternion rotation)
	{
		var newMiningRig = new GameObject("MiningRig");
		newMiningRig.transform.parent = parent.transform;
		newMiningRig.transform.localPosition = position;

		foreach (Transform child in GameObject.Find(MINING_RIG).transform)
		{
			var childPath = MINING_RIG + "/" + child.name;
			var childObj = NHRandomizer.NewHorizonsAPI.SpawnObject(parent, sector, childPath,
				position + child.transform.localPosition, (rotation * child.transform.localRotation).eulerAngles, 1f, false);
			childObj.transform.parent = newMiningRig.transform;
		}
		// Do rotation last to properly orient children
		newMiningRig.transform.localRotation = Quaternion.Euler(90, 0, 0);
	}

	public static void CreateTimeLoopCore(GameObject parent, Sector sector, Vector3 position, Quaternion rotation)
	{
		var newTimeLoopCore = NHRandomizer.NewHorizonsAPI.SpawnObject(parent, sector, TIME_LOOP_CORE, position, rotation.eulerAngles, 1f, false);
		newTimeLoopCore.transform.Find("Geometry_TimeLoopInterior").Find("innerShell").gameObject.SetActive(false);

		var timeLoopCoreController = newTimeLoopCore.GetComponentInChildren<TimeLoopCoreController>();

		var timeLoopOffset = new GameObject("TimeLoopOffset");
		timeLoopOffset.transform.parent = newTimeLoopCore.transform;
		timeLoopOffset.transform.localPosition = Vector3.zero;
		timeLoopOffset.transform.localRotation = Quaternion.identity;

		var timeLoopRing = "TimeLoopRing_Body";
		foreach (Transform child in GameObject.Find("TimeLoopRing_Body").transform)
		{
			var childPath = timeLoopRing + "/" + child.name;
			var childObj = NHRandomizer.NewHorizonsAPI.SpawnObject(parent, sector, childPath,
				position + child.transform.localPosition, (rotation * child.transform.localRotation).eulerAngles, 1f, false);
			childObj.transform.parent = timeLoopOffset.transform;
		}
		timeLoopOffset.transform.localRotation = Quaternion.Euler(90, 0, 0);

		var openWarpCoreSlot = timeLoopOffset.transform.Find("Interactibles_TimeLoopRing_Hidden/CoreContainmentInterface/Slots/Slot_OpenWarpCore").GetComponent<NomaiInterfaceSlot>();

		timeLoopCoreController._openSlot.OnSlotActivated -= timeLoopCoreController.OnSlotActivated;
		timeLoopCoreController._openSlot.OnSlotDeactivated -= timeLoopCoreController.OnSlotDeactivated;

		timeLoopCoreController._openSlot = openWarpCoreSlot;
		openWarpCoreSlot.OnSlotActivated += timeLoopCoreController.OnSlotActivated;
		openWarpCoreSlot.OnSlotDeactivated += timeLoopCoreController.OnSlotDeactivated;
	}
}
