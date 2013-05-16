using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Les informations générales relatives à un assembly dépendent de
// l'ensemble d'attributs suivant. Changez les valeurs de ces attributs pour modifier les informations
// associées à un assembly.
[assembly: AssemblyTitle("Gestion_Ste")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("sophysa")]
[assembly: AssemblyProduct("Gestion_Ste")]
[assembly: AssemblyCopyright("Copyright © sophysa 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// L'affectation de la valeur false à ComVisible rend les types invisibles dans cet assembly
// aux composants COM. Si vous devez accéder à un type dans cet assembly à partir de
// COM, affectez la valeur true à l'attribut ComVisible sur ce type.
[assembly: ComVisible(false)]

// Le GUID suivant est pour l'ID de la typelib si ce projet est exposé à COM
[assembly: Guid("a899d01d-62cf-4a22-9e6e-35528120fdf9")]

// Les informations de version pour un assembly se composent des quatre valeurs suivantes :
//
//      Version principale
//      Version secondaire
//      Numéro de build
//      Révision
//
[assembly: AssemblyVersion("1.0.0.0")]

// L'attribut ci-dessous va supprimer l'avertissement FxCop "CA2232 : Microsoft.Usage : ajoutez STAThreadAttribute à l'assembly"
// car l'application Smart Device ne prend pas en charge le thread STA.
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")]
