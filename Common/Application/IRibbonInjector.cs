using Autodesk.Revit.UI;

namespace Common.Application
{
    /// <summary>
    /// Represents a interface to inject elements to Application Ribbon.
    /// </summary>
    public interface IRibbonInjector
    {
        void Inject(UIControlledApplication application);

        int Order { get; }
    }
}
