using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SharkSpirit.Editor.Core.Classes.Serialization;

namespace SharkSpirit.Editor.Core.Classes.GameProject
{
    public static class BaseProjectTemplatesProvider
    {
        public static IEnumerable<ProjectTemplate> GetDefaultProjectTemplates()
        {
            var resourceDirectory = Environment.CurrentDirectory + "\\Resources\\ProjectTemplates";

            var templates = Directory.GetFiles(resourceDirectory, "projectTemplate.xml", SearchOption.AllDirectories);

            foreach (var pathToTemplate in templates)
            {
                yield return Serializer.Deserialize<ProjectTemplate>(pathToTemplate);
            }
        }
    }
}
