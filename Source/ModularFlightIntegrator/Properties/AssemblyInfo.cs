using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Modular Flight Integrator /L Unleashed")]
[assembly: AssemblyDescription("A VesselModule that allows multiples mods to override or insert code into various call of the stock FlightIntegrator")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(ModularFI.LegalMamboJambo.Company)]
[assembly: AssemblyProduct(ModularFI.LegalMamboJambo.Product)]
[assembly: AssemblyCopyright(ModularFI.LegalMamboJambo.Copyright)]
[assembly: AssemblyTrademark(ModularFI.LegalMamboJambo.Trademark)]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("adbbe337-c3c3-46e5-b8c9-b9f516ca0627")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(ModularFI.Version.Number)]
[assembly: AssemblyFileVersion(ModularFI.Version.Number)]
[assembly: KSPAssembly("ModularFlightIntegrator", ModularFI.Version.major, ModularFI.Version.minor)]
[assembly: KSPAssemblyDependency("KSPe", 2, 0)]
