using System.Collections.Generic;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public class ScriptSystem : SystemBase
    {
        private readonly List<ScriptsBase> _scripts;

        public ScriptSystem()
        {
            _scripts = new List<ScriptsBase>();
        }

        public void AddScript(ScriptsBase script)
        {
            _scripts.Add(script);
        }

        public void RemoveScript(ScriptsBase script)
        {
            _scripts.Remove(script);
        }

        public void ExecuteScripts()
        {
            _scripts.ForEach(script => script.Execute());
        }
    }
}
