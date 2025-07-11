
using OpenQA.Selenium;

namespace AssetManagementTest.Core.Element;
public class WebObject
{
    public By By { get; set; }
    public string Name { get; set; }
    public WebObject(By by, string name)
    {
        this.By = by;
        this.Name = name;
    }
}