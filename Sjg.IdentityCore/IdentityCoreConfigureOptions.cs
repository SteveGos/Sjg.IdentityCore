using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;

namespace Sjg.IdentityCore
{
    internal class IdentityCoreConfigureOptions : IPostConfigureOptions<StaticFileOptions>
    {
        private readonly IHostingEnvironment _environment;

        public IdentityCoreConfigureOptions(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public void PostConfigure(string name, StaticFileOptions options)
        {
            _ = name ?? throw new ArgumentNullException(nameof(name));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            // Basic initialization in case the options weren't initialized by any other component
            options.ContentTypeProvider = options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();

            if (options.FileProvider == null && _environment.WebRootFileProvider == null)
            {
                throw new InvalidOperationException("Missing FileProvider.");
            }

            options.FileProvider = options.FileProvider ?? _environment.WebRootFileProvider;

            // Must be same as in the project file ==> <EmbeddedResource Include = "resources\**\*" />
            var basePath = "resources";

            var filesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, basePath);
            options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
        }
    }
}