using System;
using ReflectionSandbox.Mapping;

namespace ReflectionSandbox.Models
{
    public class ModelForDelegates : DelegateMappable<ModelForDelegates>
    {
        public int Id { get; set; }
        public int CustomIdentifier { get; set; }
        public string Label { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string Foo { get; set; }
        public string Bar { get; set; }
        public int? Number { get; set; }
        public decimal? Decimal { get; set; }
        public DateTime? AnotherDate { get; set; }

        public override string ToString()
        {
            return $"{Label}-{CustomIdentifier}: {Value} (created on: {CreatedOn:d}, is deleted: {IsDeleted})";
        }
    }
}
