using System.Diagnostics;

namespace FunkUnions;

public partial class Result
{
    extension<T, TError>(IEnumerable<Result<T, TError>> results)
        where T : notnull
        where TError : notnull
    {
        public Result<IEnumerable<T>, TError> AsResult()
        {
            var result = new List<T>();

            foreach (var item in results)
            {
                switch (item)
                {
                    case T okVal:
                        result.Add(okVal);
                        break;
                    case TError error:
                        return error;
                    default: throw new UnreachableException($"Result object is not expected to have value: '{item.Value}'");
                }
            }

            return result;
        }
    }
}
