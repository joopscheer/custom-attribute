using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class RequiredIfOtherPropertyTrueAttribute : RequiredAttribute
{
    public RequiredIfOtherPropertyTrueAttribute(string otherProperty)
    {
        if (otherProperty == null)
        {
            throw new ArgumentNullException("otherProperty");
        }
        OtherProperty = otherProperty;
    }

    public string OtherProperty { get; private set; }

    public override bool RequiresValidationContext => true;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
        if (otherPropertyInfo == null)
        {
            throw new MissingMemberException($"The property {OtherProperty} is missing.");
        }

        // https://github.com/dotnet/aspnetcore/issues/41582
        // At this point all the properties of the IndexModel are not set.
        // The property NameRequired is always false.

        object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
        if (!Equals(true, otherPropertyValue))
        {
            return null; // no validation required if the other property is not true.
        }

        return base.IsValid(value, validationContext);
    }
}
