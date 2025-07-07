namespace MilkMatrix.Core.Helpers;

public static class PathHelpers
{
    public static string CheckGenerateSubFolderHierarchy(this string filePath, string destinationPathToDirectory = "")
    {
        try
        {
            string destinationPath = string.Empty;
            string getDay = Convert.ToString(DateTime.Now.Day);
            string getMonth = Convert.ToString(DateTime.Now.Month);
            string getYear = Convert.ToString(DateTime.Now.Year);
            destinationPath = filePath + "/" + getYear;
            if (!string.IsNullOrEmpty(destinationPathToDirectory))
            {
                destinationPathToDirectory = destinationPathToDirectory + "/" + destinationPath;
                if (!Directory.Exists(destinationPathToDirectory))
                {
                    DirectoryInfo di = Directory.CreateDirectory(destinationPathToDirectory);
                }
                destinationPathToDirectory = destinationPathToDirectory + "/" + getMonth;
                destinationPath = destinationPath + "/" + getMonth;
                if (!Directory.Exists(destinationPathToDirectory))
                {
                    DirectoryInfo di = Directory.CreateDirectory(destinationPathToDirectory);
                }
                destinationPathToDirectory = destinationPathToDirectory + "/" + getDay;
                destinationPath = destinationPath + "/" + getDay;
                if (!Directory.Exists(destinationPathToDirectory))
                {
                    DirectoryInfo di = Directory.CreateDirectory(destinationPathToDirectory);
                }
            }
            else
            {
                if (!Directory.Exists(destinationPath) && !string.IsNullOrEmpty(filePath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(destinationPath);
                }
                destinationPath = destinationPath + "/" + getMonth;
                if (!Directory.Exists(destinationPath) && !string.IsNullOrEmpty(filePath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(destinationPath);
                }
                destinationPath = destinationPath + "/" + getDay;
                if (!Directory.Exists(destinationPath) && !string.IsNullOrEmpty(filePath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(destinationPath);
                }
            }
            return destinationPath;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
