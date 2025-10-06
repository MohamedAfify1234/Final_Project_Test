namespace Educational_Platform.AppSettingsImages
{
	public class DeleteImage
	{

		public bool DeleteImg(string fileName)
		{
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");

			string fullPath = Path.Combine(folderPath, fileName);

			if (System.IO.File.Exists(fullPath))
			{

				System.IO.File.Delete(fullPath);

				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
