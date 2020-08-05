using System.Collections.Generic;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public class ScriptSystem : SystemBase
    {
        private readonly List<ScriptBase> _scripts;

        public ScriptSystem()
        {
            _scripts = new List<ScriptBase>();
        }

        public void AddScript(ScriptBase script)
        {
            _scripts.Add(script);
        }

        public void RemoveScript(ScriptBase script)
        {
            _scripts.Remove(script);
        }

        public void ExecuteScripts()
        {
            _scripts.ForEach(script => script.Execute());
        }
    }
}
