# Modular Flight Integrator Unofficial

ModularFlightIntegrator is a VesselModules that allows multiples mods to override or insert code into various call of the stock FlightIntegrator. Unofficial fork by Lisias.


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

To install, place the GameData folder inside your Kerbal Space Program folder.

**REMOVE ANY OLD VERSIONS OF THE PRODUCT BEFORE INSTALLING**.

### Dependencies
* Hard Dependencies
	* [KSP API Extensions/L](https://github.com/net-lisias-ksp/KSPAPIExtensions) 2.0 or newer

### Licensing

MIT. [See here](./LICENSE).


## UPSTREAM

* [sarbian](https://forum.kerbalspaceprogram.com/index.php?/profile/57146-sarbian/) ROOT / Current Maintainer
	+ [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/106369-122-modularflightintegrator-124-131-march-23th/&)
	+ [GitHub](https://github.com/sarbian/ModularFlightIntegrator)
* [Angel-125](https://forum.kerbalspaceprogram.com/index.php?/profile/106975-angel-125/) Parallel Fork
	+ [GitHub](https://github.com/Angel-125/ModularFlightIntegrator)
