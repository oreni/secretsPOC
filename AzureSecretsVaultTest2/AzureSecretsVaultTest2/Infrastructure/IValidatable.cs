using System.ComponentModel.DataAnnotations;

namespace AzureSecretsVaultTest2.Infrastructure
{
    public interface IValidatable
    {
        void Validate();
    }
    public class Validatable : IValidatable
    {
        public virtual void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
        }
    }
}
