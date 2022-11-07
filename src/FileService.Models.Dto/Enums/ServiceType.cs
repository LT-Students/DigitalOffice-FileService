using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LT.DigitalOffice.FileService.Models.Dto.Enums
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum ServiceType
  {
    Project,
    Wiki
  }
}
