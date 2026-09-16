namespace AdapterExportacion.ExternalServices
{
    internal class JsonLibrary
    {
        public string ConvertToJson(string period, int salesCount, decimal total)
        {
            return $"{{\"period\":\"{period}\",\"salesCount\":{salesCount},\"total\":{total:0.00}}}";
        }
    }
}
