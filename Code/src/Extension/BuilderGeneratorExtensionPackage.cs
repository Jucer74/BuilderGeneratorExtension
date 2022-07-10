﻿global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using System.Runtime.InteropServices;
using System.Threading;

namespace BuilderGeneratorExtension
{
   [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
   [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
   [ProvideMenuResource("Menus.ctmenu", 1)]
   [Guid(PackageGuids.BuilderGeneratorExtensionString)]
   [ProvideOptionPage(typeof(OptionsProvider.BuilderGeneratorOptionsOptions), "Builder Generator", "General", 0, 0, true, SupportsProfiles = true)]
   public sealed class BuilderGeneratorExtensionPackage : ToolkitPackage
   {
      protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
      {
         await this.RegisterCommandsAsync();
      }
   }
}