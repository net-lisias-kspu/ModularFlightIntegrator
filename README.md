# Modular Flight Integrator /L Unleashed

ModularFlightIntegrator is a VesselModules that allows multiples mods to override or insert code into various call of the stock FlightIntegrator.

[Unleashed](https://ksp.lisias.net/add-ons-unleashed/) fork by Lisias.



## In a Hurry

* [Latest Release](https://github.com/net-lisias-kspu/ModularFlightIntegrator/releases)
	+ [Binaries](https://github.com/net-lisias-kspu/ModularFlightIntegrator/tree/Archive)
* [Source](https://github.com/net-lisias-kspu/ModularFlightIntegrator)
* [Change Log](./CHANGE_LOG.md)
 

## Description

KSP 1.0 gave us VesselModules and the most important of them : FlightIntegrator.﻿ This module is the one that han﻿dles most of the aerodynamic and heating model.﻿

As with all the VesselModules it is easy to replace it and we could have custom version that change only part of the model (Thanks Mu for the modularity). But a problem rise when more than one mod wants to change something in the FlightIntegrator. We know two ways of handling that properly in code and ModularFlightIntegrator is one of them that should avoid complex debugging later.

ModularFlightIntegrator is a VesselModules that allows multiples mods to override or insert code into various call of the stock FlightIntegrator.﻿﻿ This was written in ﻿collaboration with Ferram4 and Starwaster.

Currently the code is in a rough shape and if multiple mods tries to override the same part then only the first one to try is allowed to. Later I plan to add a more complex system but we needed a first version out quickly.

This plugin is of no direct interest to end user but a least two mods using it should be out soon-ish so here it is.

## Installation

Detailed installation instructions are now on its own file (see the [In a Hurry](#in-a-hurry) section) and on the distribution file.

### Licensing

* Crew Light is double licensed as follows:
	+ [SKL 1.0](https://ksp.lisias.net/SKL-1_0.txt). See [here](./LICENSE.KSPe.SKL-1_0)
		+ You are free to:
			- Use : unpack and use the material in any computer or device
			- Redistribute : redistribute the original package in any medium
		+ Under the following terms:
			- You agree to use the material only on (or to) KSP
			- You don't alter the package in any form or way (but you can embedded it)
			- You don't change the material in any way, and retain any copyright notices
			- You must explicitly state the author's Copyright, as well an Official Site for downloading the original and new versions (the one you used to download is good enough)
	+ [GPL 2.0](https://www.gnu.org/licenses/gpl-2.0.txt). See [here](./LICENSE.KSPe.GPL-2_0)
		+ You are free to:
			- Use : unpack and use the material in any computer or device
			- Redistribute : redistribute the original package in any medium
			- Adapt : Reuse, modify or incorporate source code into your works (and redistribute it!) 
		+ Under the following terms:
			- You retain any copyright notices
			- You recognise and respect any trademarks
			- You don't impersonate the authors, neither redistribute a derivative that could be misrepresented as theirs.
			- You credit the author and republish the copyright notices on your works where the code is used.
			- You relicense (and fully comply) your works using GPL 2.0
			- You don't mix your work with GPL incompatible works.
	* If by some reason the GPL would be invalid for you, rest assured that you still retain the right to Use the Work under SKL 1.0. 

Releases previous to 1.2.10 are still available under the MIT license [here](https://github.com/net-lisias-kspu/ModularFlightIntegrator/tree/Source/MIT).

Please note the copyrights and trademarks in [NOTICE](./NOTICE).


## UPSTREAM

* [sarbian](https://forum.kerbalspaceprogram.com/index.php?/profile/57146-sarbian/) ROOT / Current Maintainer
	+ [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/106369-122-modularflightintegrator-124-131-march-23th/&)
	+ [GitHub](https://github.com/sarbian/ModularFlightIntegrator)
* [Angel-125](https://forum.kerbalspaceprogram.com/index.php?/profile/106975-angel-125/) Parallel Fork / M.I.A
	+ [GitHub](https://github.com/Angel-125/ModularFlightIntegrator)
* [R-T-B](https://forum.kerbalspaceprogram.com/index.php?/profile/200868-r-t-b/) Parallel Fork
	+ [GitHub](https://github.com/R-T-B/ModularFlightIntegrator)
