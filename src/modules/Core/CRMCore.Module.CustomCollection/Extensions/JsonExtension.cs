using Newtonsoft.Json.Linq;

namespace CRMCore.Module.CustomCollection.Extensions
{
    public static class JsonExtension
    {
        public static bool IsNull(this JToken token)
        {
            if (token == null)
            {
                return true;
            }

            if (token.Type == JTokenType.Null)
            {
                return true;
            }

            if (token is JValue value)
            {
                return value.Value == null;
            }

            return false;
        }
    }
}
