/*
	This file is part of Modular Flight Integrator /L Unleashed
		© 2019-2021 Lisias T : http://lisias.net <support@lisias.net>
		© 2014-2018 Sarbian

	Modular Flight Integrator /L is double licensed, as follows:

		* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt
		* GPL 2.0 : https://www.gnu.org/licenses/gpl-2.0.txt

	And you are allowed to choose the License that better suit your needs.

	Modular Flight Integrator /L Unleashed is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with Modular Flight Integrator /L Unleashed.
	If not, see <https://ksp.lisias.net/SKL-1_0.txt>.

	You should have received a copy of the GNU General Public License 2.0
	along with Modular Flight Integrator /L Unleashed.
	If not, see <https://www.gnu.org/licenses/>.

---- To satisfy the previous MIT Licensing terms ----

Copyright (c) 2014 sarbian

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

*/
using UnityEngine;

namespace ModularFI
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class MFIManager: MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
			VesselModuleManager.VesselModuleWrapper fiw = VesselModuleManager.GetWrapper(typeof (FlightIntegrator));
            if (fiw != null && fiw.active)
            {
                print("[MFIManager] FlightIntegrator is active. Deactivating it");

                VesselModuleManager.SetWrapperActive(typeof (FlightIntegrator), false);
            }
            // Should we display this only if we deactivated the stock FI ?
            string msg = "[MFIManager] Current active VesselModule : \n";
            foreach (VesselModuleManager.VesselModuleWrapper vesselModuleWrapper in VesselModuleManager.GetModules(false, false))
            {
                msg += "[MFIManager]  " + vesselModuleWrapper.type.ToString() + " active=" + vesselModuleWrapper.active +
                       " order=" + vesselModuleWrapper.order + "\n";
            }
            print(msg);

            GameEvents.onVesselPrecalcAssign.Add(AddModularPrecalc);
        }

        private void OnDestroy()
        {
            GameEvents.onVesselPrecalcAssign.Remove(AddModularPrecalc);
        }

        private void AddModularPrecalc(Vessel vessel)
        {
            if (!vessel.gameObject.GetComponent<ModularVesselPrecalculate>())
            {
                //print("[MFIManager] Adding ModularVesselPrecalculate");
                vessel.gameObject.AddComponent<ModularVesselPrecalculate>();
            }
        }
        
		private static readonly KSPe.Util.Log.Logger log = KSPe.Util.Log.Logger.CreateForType<MFIManager>(true);
		private static void print(string msg)
		{
			log.info(msg);
		}
    }
}
