using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GettextMvcLib.HttpContextHelper;

namespace GettextMvcLib
{
    public class GettextModelValidator : ModelValidator
    {
        private readonly ModelValidator implementation;

        public GettextModelValidator(ModelMetadata metadata, ControllerContext controllerContext, ModelValidator implementation) : base(metadata, controllerContext)
        {
            if (implementation == null) throw new ArgumentNullException("implementation");
            this.implementation = implementation;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return implementation.GetClientValidationRules();
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var modelValidationResults = implementation.Validate(container);

            var l = new List<ModelValidationResult>();
            foreach (var modelValidationResult in modelValidationResults)
            {
                l.Add(new ModelValidationResult
                          {
                              MemberName = modelValidationResult.MemberName,
                              Message = S._(modelValidationResult.Message)
                          });
            }

            return l;
        }

        public ControllerBase Controller
        {
            get { return ControllerContext.Controller; }
            set { ControllerContext.Controller = value; }
        }

        public HttpContextBase HttpContext
        {
            get { return ControllerContext.HttpContext; }
            set { ControllerContext.HttpContext = value; }
        }

        public bool IsChildAction
        {
            get { return ControllerContext.IsChildAction; }
        }

        public ViewContext ParentActionViewContext
        {
            get { return ControllerContext.ParentActionViewContext; }
        }

        public RequestContext RequestContext
        {
            get { return ControllerContext.RequestContext; }
            set { ControllerContext.RequestContext = value; }
        }

        public RouteData RouteData
        {
            get { return ControllerContext.RouteData; }
            set { ControllerContext.RouteData = value; }
        }

        public string GetDisplayName()
        {
            return Metadata.GetDisplayName();
        }

        public IEnumerable<ModelValidator> GetValidators(ControllerContext context)
        {
            return Metadata.GetValidators(context);
        }

        public Dictionary<string, object> AdditionalValues
        {
            get { return Metadata.AdditionalValues; }
        }

        public Type ContainerType
        {
            get { return Metadata.ContainerType; }
        }

        public bool ConvertEmptyStringToNull
        {
            get { return Metadata.ConvertEmptyStringToNull; }
            set { Metadata.ConvertEmptyStringToNull = value; }
        }

        public string DataTypeName
        {
            get { return Metadata.DataTypeName; }
            set { Metadata.DataTypeName = value; }
        }

        public string Description
        {
            get { return Metadata.Description; }
            set { Metadata.Description = value; }
        }

        public string DisplayFormatString
        {
            get { return Metadata.DisplayFormatString; }
            set { Metadata.DisplayFormatString = value; }
        }

        public string DisplayName
        {
            get { return Metadata.DisplayName; }
            set { Metadata.DisplayName = value; }
        }

        public string EditFormatString
        {
            get { return Metadata.EditFormatString; }
            set { Metadata.EditFormatString = value; }
        }

        public bool HideSurroundingHtml
        {
            get { return Metadata.HideSurroundingHtml; }
            set { Metadata.HideSurroundingHtml = value; }
        }

        public bool IsComplexType
        {
            get { return Metadata.IsComplexType; }
        }

        public bool IsNullableValueType
        {
            get { return Metadata.IsNullableValueType; }
        }

        public bool IsReadOnly
        {
            get { return Metadata.IsReadOnly; }
            set { Metadata.IsReadOnly = value; }
        }

        public override bool IsRequired
        {
            get { return Metadata.IsRequired; }
        }

        public object Model
        {
            get { return Metadata.Model; }
            set { Metadata.Model = value; }
        }

        public Type ModelType
        {
            get { return Metadata.ModelType; }
        }

        public string NullDisplayText
        {
            get { return Metadata.NullDisplayText; }
            set { Metadata.NullDisplayText = value; }
        }

        public int Order
        {
            get { return Metadata.Order; }
            set { Metadata.Order = value; }
        }

        public IEnumerable<ModelMetadata> Properties
        {
            get { return Metadata.Properties; }
        }

        public string PropertyName
        {
            get { return Metadata.PropertyName; }
        }

        public bool RequestValidationEnabled
        {
            get { return Metadata.RequestValidationEnabled; }
            set { Metadata.RequestValidationEnabled = value; }
        }

        public string ShortDisplayName
        {
            get { return Metadata.ShortDisplayName; }
            set { Metadata.ShortDisplayName = value; }
        }

        public bool ShowForDisplay
        {
            get { return Metadata.ShowForDisplay; }
            set { Metadata.ShowForDisplay = value; }
        }

        public bool ShowForEdit
        {
            get { return Metadata.ShowForEdit; }
            set { Metadata.ShowForEdit = value; }
        }

        public string SimpleDisplayText
        {
            get { return Metadata.SimpleDisplayText; }
            set { Metadata.SimpleDisplayText = value; }
        }

        public string TemplateHint
        {
            get { return Metadata.TemplateHint; }
            set { Metadata.TemplateHint = value; }
        }

        public string Watermark
        {
            get { return Metadata.Watermark; }
            set { Metadata.Watermark = value; }
        }
    }
}