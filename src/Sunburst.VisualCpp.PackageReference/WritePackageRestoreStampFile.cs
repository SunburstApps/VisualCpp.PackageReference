using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sunburst.VisualCpp.PackageReference
{
    public sealed class WritePackageRestoreStampFile : Task
    {
        [Required]
        public ITaskItem StampFile { get; set; }

        [Required]
        public ITaskItem[] PackageReferences { get; set; }

        public override bool Execute()
        {
            Dictionary<string, string> referenceEntries = new Dictionary<string, string>();
            foreach (ITaskItem reference in PackageReferences) referenceEntries.Add(reference.ItemSpec, reference.GetMetadata("Version"));

            File.WriteAllLines(StampFile.ItemSpec, referenceEntries.Select(pair => $"{pair.Key}|{pair.Value}"));
            Log.LogMessage("Saved PackageReferences in stamp file {0}", StampFile.ItemSpec);
            return true;
        }
    }
}
