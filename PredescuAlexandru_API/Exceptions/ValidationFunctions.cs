using PredescuAlexandru_API.Helpers.Enum;

namespace PredescuAlexandru_API.Exceptions
{
    public class ValidationFunctions
    {
        public static void ThrowExceptionWhenDateIsNotValid(DateTime? startTime, DateTime? endTime)
        {
            if(startTime.HasValue && endTime.HasValue && startTime > endTime)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.StartEndDatesError);
            }
        }
    }
}
