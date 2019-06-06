using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS.References;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace Sunburst.VisualCpp.PackageReference.Vsix
{
    //[ExportIVsReferenceManagerUserAsync(VsixPackage.PackageGuidString, 0)]
    //[AppliesTo(ProjectCapabilities.VisualC)]
    internal sealed class VCPackageReferenceProvider : IVsReferenceManagerUserAsync
    {
        [Import]
        public JoinableTaskFactory JoinableTaskFactory { get; }
        [Import]
        public ConfiguredProject ConfiguredProject { get; }

        public async Task ChangeReferencesAsync(uint operation, object changedContext)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();

            __VSREFERENCECHANGEOPERATION changeOperation = (__VSREFERENCECHANGEOPERATION)operation;
            IVsReferenceProviderContext context = (IVsReferenceProviderContext)changedContext;

            throw new NotImplementedException();
        }

        public async Task<ExportLifetimeContext<object>> CreateProviderContextAsync()
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();
            throw new NotImplementedException();
        }

        public bool IsApplicable()
        {
            return ConfiguredProject.Capabilities.AppliesTo("Sunburst.VCPackageReference");
        }
    }
}
