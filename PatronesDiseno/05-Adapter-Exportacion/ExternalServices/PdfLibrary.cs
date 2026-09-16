namespace AdapterExportacion.ExternalServices
{
    internal class PdfLibrary
    {
        public string BuildDocument(string title, string body)
        {
            return $"PDF: {title}\n{body}";
        }
    }
}
