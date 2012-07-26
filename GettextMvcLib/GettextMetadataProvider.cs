using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GettextMvcLib.HttpContextHelper;

namespace GettextMvcLib
{
    public class GettextMetadataProvider : ModelMetadataProvider
    {
        private readonly ModelMetadataProvider implementation;

        public GettextMetadataProvider(ModelMetadataProvider implementation)
        {
            if (implementation == null) throw new ArgumentNullException("implementation");
            this.implementation = implementation;
        }

        public override IEnumerable<ModelMetadata> GetMetadataForProperties(object container, Type containerType)
        {
            var metadataForProperties = implementation.GetMetadataForProperties(container, containerType);
            return metadataForProperties;
        }

        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            var metadataForProperty = implementation.GetMetadataForProperty(modelAccessor, containerType, propertyName);

            //Console.WriteLine(propertyName);
            //Console.WriteLine("display name: " + metadataForProperty.DisplayName);
            
            //metadataForProperty.DisplayName = Utils.PseudoTranslate(metadataForProperty.DisplayName);
            metadataForProperty.DisplayName = S._(metadataForProperty.DisplayName);

            return metadataForProperty;
        }

        public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
        {
            var metadataForType = implementation.GetMetadataForType(modelAccessor, modelType);
            return metadataForType;
        }
    }
}
