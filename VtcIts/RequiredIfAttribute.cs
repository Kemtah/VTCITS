using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace VtcIts {

    public class RequiredIfAttribute : ValidationAttribute {

        private readonly RequiredAttribute _InnerAttribute = new RequiredAttribute();

        public string DependentUpon { get; set; }

        public object Value { get; set; }



        public RequiredIfAttribute(string dependentUponProperty, object value)
            : base(() => "{0} is required if {1} is {2}.") {
            DependentUpon = dependentUponProperty;
            Value = value;
        }



        public RequiredIfAttribute(string dependentUponProperty)
            : base(() => "{0} is required if {1} exists.") {
            DependentUpon = dependentUponProperty;
            Value = null;
        }



        public string FormatErrorMessage(string name, string dependentName) {
            return string.Format(ErrorMessageString, name, dependentName, Value);
        }



        public override bool IsValid(object value) {
            return _InnerAttribute.IsValid(value);
        }

    }



    public class RequiredIfValidator : DataAnnotationsModelValidator<RequiredIfAttribute> {

        public RequiredIfValidator(ModelMetadata metadata, ControllerContext context, RequiredIfAttribute attribute)
            : base(metadata, context, attribute) {
        }


        protected internal new string ErrorMessage {
            get {
                var field = Metadata.ContainerType.GetProperty(Attribute.DependentUpon);
                var attr = field.GetCustomAttributes(typeof (DisplayAttribute), false);
                var dependentName = attr.Length > 0 ? ((DisplayAttribute) attr[0]).Name : Attribute.DependentUpon;
                return Attribute.FormatErrorMessage(Metadata.GetDisplayName(), dependentName);
            }
        }



        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules() {
            // No client validation yet...
            return base.GetClientValidationRules();
        }



        public override IEnumerable<ModelValidationResult> Validate(object container) {
            // Get a reference to the property this validation depends upon
            var field = Metadata.ContainerType.GetProperty(Attribute.DependentUpon);

            if (field == null) {
                // The dependent property does not exist
                yield break;
            }

            // Get the value of the dependent property
            var dependentValue = field.GetValue(container, null);

            if ((dependentValue == null || string.IsNullOrEmpty(dependentValue.ToString())) // The dependent property is empty
                || (Attribute.Value != null && !dependentValue.Equals(Attribute.Value))) { // The dependent property does not match the required conditional value
                // ...so the current field is not required
                yield break;
            }

            // Proceed with validating the current field
            if (!Attribute.IsValid(Metadata.Model)) {
                yield return new ModelValidationResult {Message = ErrorMessage};
            }
        }

    }
}
