using System;

namespace ReflectionSandbox
{
    public class Model : Mappable<Model>, IMappable
    {
        public int Id { get; set; }
        [Mappable(PropertyType.Integer, true)]
        public int CustomIdentifier { get; set; }
        [Mappable(PropertyType.String, true)]
        public string Label { get; set; }
        [Mappable(PropertyType.Decimal, true)]
        public decimal Value { get; set; }
        [Mappable(PropertyType.DateTime, true)]
        public DateTime CreatedOn { get; set; }
        [Mappable(PropertyType.Bool, true)]
        public bool IsDeleted { get; set; }
        [Mappable(PropertyType.String)]
        public string Foo { get; set; }
        [Mappable(PropertyType.String)]
        public string Bar { get; set; }
        [Mappable(PropertyType.Integer)]
        public int? Number { get; set; }
        [Mappable(PropertyType.Decimal)]
        public decimal? Decimal { get; set; }
        [Mappable(PropertyType.DateTime)]
        public DateTime? AnotherDate { get; set; }

        public override string ToString()
        {
            return $"{Label}-{CustomIdentifier}: {Value} (created on: {CreatedOn:d}, is deleted: {IsDeleted})";
        }

        public void MapProperty(string target, string value)
        {
            switch (target)
            {
                case nameof(CustomIdentifier):
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ApplicationException($"{target} is required, but no value was provided");
                    if (int.TryParse(value, out var intResult))
                        CustomIdentifier = intResult;
                    else throw new ApplicationException($"{value} is no int32");
                    break;
                case nameof(Label):
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ApplicationException($"{target} is required, but no value was provided");
                    Label = value;
                    break;
                case nameof(Value):
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ApplicationException($"{target} is required, but no value was provided");
                    if (decimal.TryParse(value, out var decimalResult))
                        Value = decimalResult;
                    else throw new ApplicationException($"{value} is no decimal");
                    break;
                case nameof(CreatedOn):
                    if (string.IsNullOrWhiteSpace(value))
                        break;
                    if (DateTime.TryParse(value, out var dateTimeResult))
                        CreatedOn = dateTimeResult;
                    else throw new ApplicationException($"{value} is no DateTime");
                    break;
                case nameof(IsDeleted):
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ApplicationException($"{target} is required, but no value was provided");
                    if (bool.TryParse(value, out var boolResult))
                        IsDeleted = boolResult;
                    else throw new ApplicationException($"{value} is no boolean");
                    break;
                case nameof(Foo):
                    if (string.IsNullOrWhiteSpace(value))
                        break;
                    Foo = value;
                    break;
                case nameof(Bar):
                    if (string.IsNullOrWhiteSpace(value))
                        break;
                    Bar = value;
                    break;
                case nameof(Number):
                    if (string.IsNullOrWhiteSpace(value))
                        break;
                    if (int.TryParse(value, out intResult))
                        Id = intResult;
                    else throw new ApplicationException($"{value} is no int32");
                    break;
                case nameof(Decimal):
                    if (string.IsNullOrWhiteSpace(value))
                        break;
                    if (decimal.TryParse(value, out decimalResult))
                        Decimal = decimalResult;
                    else throw new ApplicationException($"{value} is no decimal");
                    break;
                case nameof(AnotherDate):
                    if (string.IsNullOrWhiteSpace(value))
                        break;
                    if (DateTime.TryParse(value, out dateTimeResult))
                        AnotherDate = dateTimeResult;
                    else throw new ApplicationException($"{value} is no DateTime");
                    break;
            }
        }
    }
}
