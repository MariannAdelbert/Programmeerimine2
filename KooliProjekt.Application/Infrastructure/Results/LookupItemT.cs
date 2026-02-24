using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Infrastructure.Results
{
    [ExcludeFromCodeCoverage]
    public class LookupItem<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }
    }
}