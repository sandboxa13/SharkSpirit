using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SharkSpirit.Editor.Core.Classes.GameProject
{
    [DataContract]
    public class ProjectTemplate
    {
        public ProjectTemplate(string name)
        {
            Name = name;
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        [DataMember]
        public string Directory { get; set; }
        [DataMember]
        public List<string> Folders { get; set; }
    }
}
