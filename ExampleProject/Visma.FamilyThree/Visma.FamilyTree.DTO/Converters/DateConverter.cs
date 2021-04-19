using Newtonsoft.Json.Converters;

namespace Visma.FamilyTree.DTO.Converters
{
    public class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
