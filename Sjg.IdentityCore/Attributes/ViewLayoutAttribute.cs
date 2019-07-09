using System;

namespace Sjg.IdentityCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewLayoutAttribute : Attribute
    {
        public ViewLayoutAttribute(string layoutName)
        {
            LayoutName = layoutName;
        }

        public string LayoutName { get; }
    }
}
