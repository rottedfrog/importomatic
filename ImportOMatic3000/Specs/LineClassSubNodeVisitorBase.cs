using System.Linq;

namespace ImportOMatic3000.Specs
{
    abstract class LineClassSubNodeVisitorBase : YamlNodeVisitorBase
    {
        protected LineClass LineClass
        {
            get
            {
                string name = ActualPath.Split('/')[3];
                return Spec.Import.LineClasses.First(x => x.Name == name);
            }
        }
    }
}
