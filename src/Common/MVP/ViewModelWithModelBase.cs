using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Tiveria.Common.MVP
{

    public abstract class ViewModelWithModelBase<TViewModelContract, TViewContract, TModel> : ViewModelBase<TViewModelContract, TViewContract>
        where TViewContract : IView<TViewContract, TViewModelContract>
        where TViewModelContract : IViewModel<TViewModelContract, TViewContract>
        where TModel : class
    {
        public TModel Model
        {
            get;
            set;
        }

        protected override void ExecuteValidation()
        {
            if (Model == null)
                return;

            var results = new List<ValidationResult>();
            ValidationContext c = new ValidationContext(this.Model, null, null);

            if (Model is IValidatableObject)
                results.AddRange((Model as IValidatableObject).Validate(c));
            else
                results.AddRange(OnValidate(c));

            UpdateValidationMessages(results);
            FilterValidationMessages();
        }

        protected virtual void FilterValidationMessages()
        {
        }

        private IEnumerable<ValidationResult> OnValidate(ValidationContext ctx)
        {
            var list = new List<ValidationResult>();
            Validator.TryValidateObject(Model, ctx, list, true);
            return list;
        }

        private void UpdateValidationMessages(IEnumerable<ValidationResult> result)
        {
            if (result != null && result.Any())
            {
                result.ToList().ForEach(r =>
                {
                    if (!ValidationMessages.ContainsKey(r.MemberNames.First()))
                        ValidationMessages.Add(r.MemberNames.First(), r.ErrorMessage);
                    else
                        ValidationMessages[r.MemberNames.First()] += Environment.NewLine + r.ErrorMessage;
                });
            }
        }
    }
}
