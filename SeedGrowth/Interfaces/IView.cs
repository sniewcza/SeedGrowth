﻿using SeedGrowth.Controllers;
using System.Drawing;

namespace SeedGrowth.Interfaces
{
    public interface IView
    {
        void setController(SeedGrowthController controller);
        void setBitmap(Bitmap bitmap);
        void showExceptionMessage(string message);
        int? getNumber();
        string getImportFilePath();
        string getExportFilePath();
        void showInfo(string info);
    }
}
