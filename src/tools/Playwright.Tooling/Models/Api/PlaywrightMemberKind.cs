using System.Runtime.Serialization;

namespace Playwright.Tooling.Models.Api
{
    public enum PlaywrightMemberKind
    {
        [EnumMember(Value = "method")]
        Method,
        [EnumMember(Value = "event")]
        Event,
        [EnumMember(Value = "property")]
        Property,
    }
}
