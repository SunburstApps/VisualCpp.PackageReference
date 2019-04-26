using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sunburst.VisualCpp.PackageReference
{
    public sealed class CheckPackageRestoreNeeded : Task
    {
        [Required]
        public ITaskItem StampFile { get; set; }

        [Required]
        public ITaskItem[] PackageReferences { get; set; }

        [Output]
        public bool RestoreNeeded { get; set; }

        public override bool Execute()
        {
            Dictionary<string, string> referenceEntries = new Dictionary<string, string>();
            foreach (ITaskItem reference in PackageReferences) referenceEntries.Add(reference.ItemSpec, reference.GetMetadata("Version"));

            Dictionary<string, string> stampEntries = new Dictionary<string, string>();
            foreach (string line in File.ReadAllLines(StampFile.ItemSpec))
            {
                if (line == "") continue;

                string[] parts = line.Split('|');
                if (parts.Length != 2)
                {
                    Log.LogError("Malformed stamp file: Should have exactly two pipe-separated fields");
                    return false;
                }

                stampEntries.Add(parts[0], parts[1]);
            }

            RestoreNeeded = !referenceEntries.Equals(stampEntries);
            return true;
        }
    }
}
