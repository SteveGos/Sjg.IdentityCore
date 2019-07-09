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

        // See if Razor Page...
        if (viewContext.ActionDescriptor is CompiledPageActionDescriptor)
        {
            var modelTypeInfo = ((CompiledPageActionDescriptor)viewContext.ActionDescriptor).ModelTypeInfo;

            if (modelTypeInfo != null)
            {
                layoutAttributeFound = Attribute.GetCustomAttribute(modelTypeInfo, typeof(ViewLayoutAttribute)) as ViewLayoutAttribute;

                return layoutAttributeFound;
            }
        }
        else
        {
            // See if MVC Controller...

            // Property ControllerTypeInfo can be seen on runtime.
            var controllerTypeInfo = (Type)viewContext.ActionDescriptor
                .GetType()
                .GetProperty("ControllerTypeInfo")?
                .GetValue(viewContext.ActionDescriptor);

            if (controllerTypeInfo != null && controllerTypeInfo.IsSubclassOf(typeof(Microsoft.AspNetCore.Mvc.Controller)))
            {
                layoutAttributeFound = Attribute.GetCustomAttribute(controllerTypeInfo, typeof(ViewLayoutAttribute)) as ViewLayoutAttribute;
                if (layoutAttributeFound != null)
                {
                    return layoutAttributeFound;
                }
            }
        }

        return layoutAttributeFound;
    }
}