using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace XpandTest.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("Settings")]
[XafDisplayName("Sub Category")]
public class NormativeMonitoringSubCategory : AuditableBase
{ 
    public NormativeMonitoringSubCategory(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
    }
    private NormativeMonitoringCategory _Category;
    [XafDisplayName("Category")]
    [Association("NormativeMonitoringCategory-NormativeMonitoringSubCategory")]
    public NormativeMonitoringCategory Category
    {
        get => _Category;
        set => SetPropertyValue(nameof(Category), ref _Category, value);
    }

    private string _Title;
    [RuleUniqueValue]
    [XafDisplayName("Title"), ToolTip("Please enter the title of the normative sub category")]
    [RuleRequiredField(DefaultContexts.Save)]
    public string Title
    {
        get => _Title;
        set => SetPropertyValue(nameof(Title), ref _Title, value);
    }


}