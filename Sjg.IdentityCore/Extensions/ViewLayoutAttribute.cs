using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sjg.IdentityCore.Attributes;
using System;

public static class RazorExtensions
{
    /// <summary>
    /// Gets the <see cref="ViewLayoutAttribute"/> from the current calling controller of the <see cref="ViewContext"/>.
    /// </summary>
    public static ViewLayoutAttribute GetLayoutAttribute(this ViewContext viewContext)
    {
        ViewLayoutAttribute layoutAttributeFound = null;

        if (viewContext.ActionDescriptor is CompiledPageActionDescriptor)
        {
            var controllerType = ((CompiledPageActionDescriptor)viewContext.ActionDescriptor).ModelTypeInfo;

            if (controllerType != null)
            {
                layoutAttributeFound = Attribute.GetCustomAttribute(controllerType, typeof(ViewLayoutAttribute)) as ViewLayoutAttribute;
            }
        }

        return layoutAttributeFound;
    }
}