﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System.Waf.Foundation
{
    /// <summary>
    /// Defines a base class for a model that supports validation.
    /// </summary>
    [DataContract]
    public abstract class ValidatableModel : Model, INotifyDataErrorInfo
    {
        private static readonly ValidationResult[] noErrors = new ValidationResult[0];
        
        private readonly Dictionary<string, List<ValidationResult>> errors;
        private IReadOnlyList<ValidationResult> allErrorsCache;
        private bool hasErrors;


        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatableModel"/> class.
        /// </summary>
        protected ValidatableModel()
        {
            errors = new Dictionary<string, List<ValidationResult>>();
            allErrorsCache = noErrors;
        }


        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public bool HasErrors 
        { 
            get { return hasErrors; }
            private set { SetProperty(ref hasErrors, value); }
        }


        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        /// <summary>
        /// Gets the validation errors for the entire entity.
        /// </summary>
        /// <returns>The validation errors for the entity.</returns>
        public IEnumerable<ValidationResult> GetErrors()
        {
            return GetErrors(null);
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; 
        /// or null or String.Empty, to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable<ValidationResult> GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                List<ValidationResult> result;
                if (errors.TryGetValue(propertyName, out result))
                {
                    return result;
                }
                return noErrors;
            }
            else
            {
                return allErrorsCache;
            }
        }
        
        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return GetErrors(propertyName);
        }

        /// <summary>
        /// Validates the object and all its properties. The validation results are stored and can be retrieved by the 
        /// GetErrors method. If the validation results are changing then the ErrorsChanged event will be raised.
        /// </summary>
        /// <returns>True if the object is valid, otherwise false.</returns>
        public bool Validate()
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);
            UpdateErrors(validationResults);
            return !HasErrors;
        }
        
        /// <summary>
        /// Set the property with the specified value and validate the property. If the value is not equal with the field then the field is
        /// set, a PropertyChanged event is raised, the property is validated and it returns true.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Reference to the backing field of the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="propertyName">The property name. This optional parameter can be skipped
        /// because the compiler is able to create it automatically.</param>
        /// <returns>True if the value has changed, false if the old and new value were equal.</returns>
        /// <exception cref="ArgumentException">The argument propertyName must not be null or empty.</exception>
        protected bool SetPropertyAndValidate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException("The argument propertyName must not be null or empty.", nameof(propertyName));
            
            if (SetProperty(ref field, value, propertyName))
            {
                Validate();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Raises the <see cref="E:ErrorsChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.DataErrorsChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        private void RaiseErrorsChanged(string propertyName = "")
        {
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        private void UpdateErrors(IReadOnlyList<ValidationResult> validationResults, string propertyName = null)
        {
            var newErrors = new Dictionary<string, List<ValidationResult>>();
            foreach (var validationResult in validationResults)
            {
                var memberNames = validationResult.MemberNames.Any() ? validationResult.MemberNames : new[] { "" };
                foreach (string memberName in memberNames)
                {
                    if (!newErrors.ContainsKey(memberName))
                    {
                        newErrors.Add(memberName, new List<ValidationResult>() { validationResult });
                    }
                    else
                    {
                        newErrors[memberName].Add(validationResult);
                    }
                }
            }

            var changedProperties = new HashSet<string>();
            var errorKeys = propertyName == null ? errors.Keys : errors.Keys.Where(x => x == propertyName);
            var newErrorKeys = propertyName == null ? newErrors.Keys : newErrors.Keys.Where(x => x == propertyName);
            foreach (var propertyToRemove in errorKeys.Except(newErrorKeys).ToArray())
            {
                changedProperties.Add(propertyToRemove);
                errors.Remove(propertyToRemove);
            }
            foreach (var propertyToUpdate in errorKeys.ToArray())
            {
                if (!errors[propertyToUpdate].SequenceEqual(newErrors[propertyToUpdate], ValidationResultComparer.Default))
                {
                    changedProperties.Add(propertyToUpdate);
                    errors[propertyToUpdate] = newErrors[propertyToUpdate];
                }
            }
            foreach (var propertyToAdd in newErrorKeys.Except(errorKeys).ToArray())
            {
                changedProperties.Add(propertyToAdd);
                errors.Add(propertyToAdd, newErrors[propertyToAdd]);
            }

            if (changedProperties.Any())
            {
                allErrorsCache = errors.Values.SelectMany(x => x).Distinct().ToArray();
                HasErrors = errors.Any();
            }

            foreach (var changedProperty in changedProperties) RaiseErrorsChanged(changedProperty);
        }


        private sealed class ValidationResultComparer : IEqualityComparer<ValidationResult>
        {
            public static ValidationResultComparer Default { get; } = new ValidationResultComparer();

            public bool Equals(ValidationResult x, ValidationResult y)
            {
                if (x == y) return true;
                if (x == null || y == null) return false;
                return Equals(x.ErrorMessage, y.ErrorMessage) && x.MemberNames.SequenceEqual(y.MemberNames);
            }

            public int GetHashCode(ValidationResult obj)
            {
                if (obj == null) return 0;
                return (obj.ErrorMessage?.GetHashCode() ?? 0) ^ obj.MemberNames.Select(x => x?.GetHashCode() ?? 0).Aggregate(0, (current, next) => current ^ next);
            }
        }
    }
}
