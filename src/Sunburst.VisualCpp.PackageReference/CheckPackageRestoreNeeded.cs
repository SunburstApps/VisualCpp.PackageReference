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
            if (!File.Exists(StampFile.ItemSpec))
            {
                Log.LogMessage("Stamp file does not exist, therefore we must restore");
                RestoreNeeded = true;
                return true;
            }

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

            foreach (var pair in referenceEntries)
            {
                if (!stampEntries.ContainsKey(pair.Key))
                {
                    Log.LogMessage("Reference {0} not in stamp file", pair.Key);
                    RestoreNeeded = true;
                }

                if (stampEntries[pair.Key] != pair.Value)
                {
                    Log.LogMessage("Reference {0} has different version in project ({1}) than in stamp file ({2})", pair.Key, pair.Value, stampEntries[pair.Key]);
                    RestoreNeeded = true;
                }
            }

            foreach (var pair in stampEntries)
            {
                if (!referenceEntries.ContainsKey(pair.Key))
                {
                    Log.LogMessage("Reference {0} in stamp file but was removed from project", pair.Key);
                    RestoreNeeded = true;
                }
            }

            return true;
        }
    }
}
