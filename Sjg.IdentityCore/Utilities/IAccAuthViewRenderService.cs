using System.Threading.Tasks;

namespace Sjg.IdentityCore.Utilities
{
    /// <summary>
    /// Interface for Rendering View to a string
    /// </summary>
    public interface IAccAuthViewRenderService
    {
        /// <summary>
        ///  Render View Name to String
        /// </summary>
        /// <param name="viewName">Name of view</param>
        /// <param name="model">Model for view</param>
        /// <returns></returns>
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}