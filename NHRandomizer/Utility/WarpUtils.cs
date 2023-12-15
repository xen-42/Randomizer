using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NHRandomizer.Utility;

public static class WarpUtils
{
	public static void WarpToPlanet(AstroObject.Name planetName, Vector3 position)
	{
		var planet = Locator.GetAstroObject(planetName);

		var body = PlayerState.AtFlightConsole() ? Locator.GetShipBody() : Locator.GetPlayerBody();

		var newWorldPos = planet.transform.TransformPoint(position);

		body.WarpToPositionRotation(newWorldPos, Quaternion.identity);
		body.SetVelocity(planet.GetAttachedOWRigidbody().GetPointVelocity(newWorldPos));
	}
}
