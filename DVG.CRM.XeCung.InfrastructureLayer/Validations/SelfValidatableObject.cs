using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Validations
{
    public class SelfValidatableObject
    {
        public IValidator Validator { get; protected set; }

        public ValidationResult ValidationResult => this.Validator == null ? new ValidationResult() : Validator.Validate(this);
        public ValidationResult BusinessRuleViolation { get; protected set; }
        public bool IsValid => this.ValidationResult.IsValid && (this.BusinessRuleViolation == null || this.BusinessRuleViolation.IsValid);
    }
}
