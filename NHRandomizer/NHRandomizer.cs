using NHRandomizer.Utility;
using OWML.ModHelper;
using UnityEngine;

namespace NHRandomizer;

public class NHRandomizer : ModBehaviour
{
	public static INewHorizons NewHorizonsAPI;

	public void Start()
	{
		NewHorizonsAPI = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
		NewHorizonsAPI.LoadConfigs(this);

		NewHorizonsAPI.GetStarSystemLoadedEvent().AddListener(OnStarSystemLoaded);
	}

	private void OnStarSystemLoaded(string systemName)
	{
		if (systemName == "SolarSystem")
		{
			GameObject.Find("Player_Body").AddComponent<DebugCommands>();

			// Testing stuff
			var timberHearth = GameObject.Find("TimberHearth_Body").GetComponent<AstroObject>();
			var interloper = GameObject.Find("Comet_Body").GetComponent<AstroObject>();
			var interloperInterior = interloper.transform.Find("Sector_CO/Sector_CometInterior").GetComponent<Sector>();

			LocationCreator.CreateTrackingModule(timberHearth.gameObject, timberHearth.GetRootSector(), Vector3.up * 300f, Quaternion.identity);
			LocationCreator.CreateTimeLoopCore(timberHearth.gameObject, timberHearth.GetRootSector(), Vector3.up * 300f + Vector3.left * 100f, Quaternion.identity);

			// MiningRig messes up the orbit currently
			//LocationCreator.CreateMiningRig(timberHearth.gameObject, timberHearth.GetRootSector(), Vector3.up * 300f + Vector3.right * 100f, Quaternion.identity);

			LocationCreator.CreateTimeLoopCore(timberHearth.gameObject, timberHearth.GetRootSector(), Vector3.zero, Quaternion.identity);

			LocationCreator.CreateTrackingModule(interloperInterior.gameObject, interloperInterior, new Vector3(-1.1964f, -2.8294f, -5.4182f), Quaternion.Euler(58.2396f, 337.1563f, 0));

			LocationCreator.RemoveOriginals();
		}
	}
}
