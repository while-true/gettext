using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GettextMvcLib
{
    public class GettextModelValidatorProvider : ModelValidatorProvider
    {
        private readonly ModelValidatorProvider implementation;

        public GettextModelValidatorProvider(ModelValidatorProvider implementation)
        {
            if (implementation == null) throw new ArgumentNullException("implementation");
            this.implementation = implementation;
        }

        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            var modelValidators = implementation.GetValidators(metadata, context);

            var r = new List<ModelValidator>();
            foreach (var modelValidator in modelValidators)
            {
                r.Add(new GettextModelValidator(metadata, context, modelValidator));
            }

            return r;
        }
    }
}