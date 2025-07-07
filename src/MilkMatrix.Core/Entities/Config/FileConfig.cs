namespace MilkMatrix.Core.Entities.Config;

public sealed record FileConfig
{
  public const string SectionName = "FileConfiguration";

  public string[] AllowedExtensions { get; set; }

  public long MaximumFileLength { get; set; }

  public string ImageFilePath { get; set; }

  public string CsvFilePath { get; set; }

  public string PdfFilePath { get; set; }

  public string DocsFilePath { get; set; }

  public string FileMediaPath { get; set; }

  public int MaxFilesAllowedToUpload { get; set; }

  public string UploadFileDirectoryPath { get; set; }

  public string UploadFileHost { get; set; }

  public string ProfilePath { get; set; }

  public string InventoryPath { get; set; }

  public string VisitorPath { get; set; }

  public string MobileAppPath { get; set; }

  public string PayRollPath { get; set; }

  public string AccountsPath { get; set; }

  public string PurchasePath { get; set; }

  public string SalesPath { get; set; }

  public string DownloadPDFPath { get; set; }

}
