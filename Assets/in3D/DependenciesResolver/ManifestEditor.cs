using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DependenciesResolver
{
    public static class ManifestEditor

    {
        private const string Dependencies = "dependencies";
        private const string ScopedRegistries = "scopedRegistries";
        private const string Scopes = "scopes";

        public static void AddDependencies(string from, string to)
        {
            if (!File.Exists(from) || !File.Exists(to)) return;
            var input = JObject.Parse(File.ReadAllText(from));
            var output = JObject.Parse(File.ReadAllText(to));
            AddDependencies(input, output);
            AddScopedRegistries(input, output);
            File.WriteAllText(to, output.ToString());
        }
        
        private static void AddDependencies(JObject input, JObject output)
        {
            if (input[Dependencies] == null) return;
            var inDep = (JObject)input[Dependencies];

            if (output[Dependencies] == null)
            {
                output[Dependencies] = new JObject();
            }

            var outDep = (JObject)output[Dependencies];

            foreach (var property in inDep.Properties())
            {
                if (outDep.ContainsKey(property.Name)) continue;
                outDep[property.Name] = property.Value;
            }
        }

        private static void AddScopedRegistries(JObject input, JObject output)
        {
            if (input[ScopedRegistries] == null) return;
            var inScR = (JArray)input[ScopedRegistries];

            if (output[ScopedRegistries] == null)
            {
                output[ScopedRegistries] = new JArray();
            }

            var outScR = (JArray)output[ScopedRegistries];
            foreach (JObject inReg in inScR)
            {
                var outRegs = outScR.Where(scr => (string)scr["url"] == (string)inReg["url"]).ToArray();
                if (outRegs.Length == 0)
                {
                    outScR.Add(inReg);
                    continue;
                }

                var allOutScopes = outRegs.SelectMany(t => t[Scopes]).Select(s => (string)s).ToArray();
                foreach (string inScope in (JArray)inReg[Scopes])
                {
                    if (allOutScopes.Contains(inScope)) continue;

                    var outReg = outRegs.First();
                    if (outReg[Scopes] == null)
                    {
                        outReg[Scopes] = new JArray();
                    }

                    var outScopes = (JArray)outReg[Scopes];
                    outScopes.Add(inScope);
                }
            }
        }
    }
}