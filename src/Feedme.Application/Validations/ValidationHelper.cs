using System;
using Feedme.Domain.Functional;
using Feedme.Domain.Models;

namespace Feedme.Application.Validations
{
    public static class ValidationHelper
    {
        public static Result<Guid> ConvertToGuid(string textGuid)
        {
            return !Guid.TryParse(textGuid, out var guid) ?
                Result.Fail<Guid>("Id has invalid format") :
                Result.Ok(guid);
        }

        public static Result<SourceType> ConvertToSourceType(string textType)
        {
            return !Enum.TryParse(textType, out SourceType sourceType) ?
                Result.Fail<SourceType>("Type of source has wrong format") :
                Result.Ok(sourceType);
        }
    }
}