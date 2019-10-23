using Sjg.IdentityCore.Models;

namespace SampleWebApp_1_x_x
{
    /// <summary>
    /// Role Collection
    /// </summary>
    public class AccAuthRoles
    {
        public const string Category = "SampleWebApp_1_x_x";

        /// <summary>
        /// Name of <see cref="Role1"/>
        /// </summary>
        public const string Role1Name = "Sample Role 1";

        /// <summary>
        /// AccAuthRole for <see cref="Role1"/>
        /// </summary>
        public static AccAuthRole Role1 => new AccAuthRole
        {
            Category = Category,
            Name = Role1Name, // Set to the constant.
            Description = "Sample Web Site Role #1",
        };

        /// <summary>
        /// Name of <see cref="Role2"/>
        /// </summary>
        public const string Role2Name = "Sample Role 2";

        /// <summary>
        /// AccAuthRole for <see cref="Role2"/>
        /// </summary>
        public static AccAuthRole Role2 => new AccAuthRole
        {
            Category = Category,
            Name = Role2Name, // Set to the constant.
            Description = "Sample Web Site Role #2",
        };

      
    }
}