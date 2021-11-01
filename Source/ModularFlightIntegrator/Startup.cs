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

*/
using KSPe.Annotations;
using UnityEngine;

namespace ModularFI
{
	[KSPAddon (KSPAddon.Startup.Instantly, true)]
	internal class Startup : MonoBehaviour
	{
		[UsedImplicitly]
		private void Start ()
		{
			Log.force ("Version {0}", Version.Text);

			try {
				KSPe.Util.Installation.Check<Startup> ("ModularFlightIntegrator", Version.Namespace, Version.Vendor);
			} catch (KSPe.Util.InstallmentException e) {
				Log.error (e.ToShortMessage ());
				KSPe.Common.Dialogs.ShowStopperAlertBox.Show (e);
			}
		}
	}
}
