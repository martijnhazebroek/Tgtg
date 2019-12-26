namespace Hazebroek.Tgtg.Notify
{
    internal sealed class IftttRequest
    {
        public string Token { get; }
        public string Value1 { get; }
        public string Value2 { get; }
        public string Value3 { get; }

        public IftttRequest(string token, string value1, string value2, string value3)
        {
            Token = token;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }
    }
}