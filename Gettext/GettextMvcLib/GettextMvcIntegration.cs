using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GettextLib;

namespace GettextMvcLib
{
    public static class GettextMvcIntegration
    {
        public static GettextMetadataProvider InstallGettextMetadataProviderWrapper(ModelMetadataProvider provider = null)
        {
            var impl = provider ?? ModelMetadataProviders.Current;
            var gettext = new GettextMetadataProvider(impl);
            ModelMetadataProviders.Current = gettext;
            return gettext;
        }

        public static void InstallGettextModelValidatorProviderWrappers()
        {
            var mvp = ModelValidatorProviders.Providers.ToList();
            ModelValidatorProviders.Providers.Clear();
            foreach (var modelValidatorProvider in mvp)
            {
                var gmvp = new GettextModelValidatorProvider(modelValidatorProvider);
                ModelValidatorProviders.Providers.Add(gmvp);
            }
        }
        
        public static void SetAsContextForCurrentRequest(GettextTranslationContext ctx, ControllerBase controllerBase)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");
            if (controllerBase == null) throw new ArgumentNullException("controllerBase");

            controllerBase.ViewData.Add(Consts.GettextContextKey, ctx);
            System.Web.HttpContext.Current.Items[Consts.GettextContextKey] = ctx;
        }
    }
}
