﻿namespace SignalRAndWorkerServicePart2
{
    internal enum StringState { Null, Empty, WhiteSpaces, Valid }

    internal static class ValidatorString
    {
        public static string Validate(string propertyName, string propertyValue)
        {
            return (DetermineNullEmptyOrWhiteSpaces(propertyValue)) switch
            {
                StringState.Null => $"The property: \"{propertyName}\" must be a valid {propertyName} and can not be null",
                StringState.Empty => $"The property: \"{propertyName}\" must be a valid {propertyName} and can not be Empty",
                StringState.WhiteSpaces => $"The property: \"{propertyName}\" must be a valid {propertyName} and can not be Whitespaces",
                _ => null,
            };
        }

        public static StringState DetermineNullEmptyOrWhiteSpaces(string data)
        {
            if (data == null)
            {
                return StringState.Null;
            }
            else if (data.Length == 0)
            {
                return StringState.Empty;
            }

            foreach (var chr in data)
            {
                if (!char.IsWhiteSpace(chr))
                {
                    return StringState.Valid;
                }
            }

            return StringState.WhiteSpaces;
        }
    }
}
